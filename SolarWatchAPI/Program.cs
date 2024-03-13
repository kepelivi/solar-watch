using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Data;
using SolarWatchAPI.Services.Authentication;
using SolarWatchAPI.Services.Repositories;
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
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddSingleton<ILogger, ConsoleLogger>();

builder.Services.AddSingleton<IGeoCodeDataProvider, GeoCodeDataProvider>();
builder.Services.AddTransient<IGeoJsonProcess, GeoJsonProcess>();

builder.Services.AddSingleton<ISolarWatchDataProvider, SolarWatchDataProvider>();
builder.Services.AddSingleton<ISolarJsonProcess, SolarJsonProcess>();

builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<ISolarRepository, SolarRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<AuthenticationSeeder>();
builder.Services.AddDbContext<SolarWatchContext>((container, options) =>
{
    options.UseSqlServer(config["ConnectionString"]);
});

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
            ValidIssuer = config["ValidateIssuer"],
            ValidAudience = config["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["IssuerSigningKey"])
            ),
        };
    });

AddIdentity();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        policy  =>
        {
            policy.WithOrigins("*").AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("MyAllowSpecificOrigins");

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

using var scope = app.Services.CreateScope();
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();

authenticationSeeder.AddRoles();

authenticationSeeder.AddAdmin();

app.Run();

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<SolarWatchContext>();
}

