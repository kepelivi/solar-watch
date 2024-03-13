using System.Net;
using SolarWatchAPI.Model;
using ILogger = SolarWatchAPI.Utilities.ILogger;

namespace SolarWatchAPI.Controllers;

public class GeoCodeDataProvider : IGeoCodeDataProvider
{
    private readonly ILogger _logger;

    public GeoCodeDataProvider(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetGeoCodeString(string city)
    {
        var apiKey = "b3ad07ee0b9dac795e637580a6260751";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}";

        using var client = new HttpClient();
        _logger.LogInfo($"Calling GeoCode API with url: {url}");

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}