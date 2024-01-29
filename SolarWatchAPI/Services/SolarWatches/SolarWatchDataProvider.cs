using System.Net;
using SolarWatchAPI.Model;
using ILogger = SolarWatchAPI.Utilities.ILogger;

namespace SolarWatchAPI.Services.SolarWatches;

public class SolarWatchDataProvider : ISolarWatchDataProvider
{
    private readonly ILogger _logger;

    public SolarWatchDataProvider(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<string> GetCurrentSolarWatch(GeoCode geoCode, string date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={geoCode.Latitude}&lng={geoCode.Longitude}&date={date}";
        using var client = new HttpClient();
        
        _logger.LogInfo($"Calling GeoCode API with url: {url}");

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}