using SolarWatchAPI.Controllers;
using SolarWatchAPI.Data;
using SolarWatchAPI.Services.SolarWatches;
using SolarWatchAPI.Utilities;
using ILogger = SolarWatchAPI.Utilities.ILogger;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();