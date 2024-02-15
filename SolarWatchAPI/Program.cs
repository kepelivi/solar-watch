using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Data;
using SolarWatchAPI.Services.SolarWatches;
using SolarWatchAPI.Utilities;
using ILogger = SolarWatchAPI.Utilities.ILogger;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILogger, ConsoleLogger>();

builder.Services.AddSingleton<IGeoCodeDataProvider, GeoCodeDataProvider>();
builder.Services.AddSingleton<IGeoJsonProcess, GeoJsonProcess>();

builder.Services.AddSingleton<ISolarWatchDataProvider, SolarWatchDataProvider>();
builder.Services.AddSingleton<ISolarJsonProcess, SolarJsonProcess>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["ValidIssuer"],
            ValidAudience = config["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["IssuerSigningKey"])
            ),
        };
    });

builder.Services.AddDbContext<UsersContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();