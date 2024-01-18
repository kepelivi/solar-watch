using System.Net;
using SolarWatchAPI.Model;
using ILogger = SolarWatchAPI.Utilities.ILogger;

namespace SolarWatchAPI.Controllers;

public class GeoCodeProvider : IGeoCodeProvider
{
    private readonly ILogger _logger;

    public GeoCodeProvider(ILogger logger)
    {
        _logger = logger;
    }
    
    public string GetGeoCodeString(string city)
    {
        var apiKey = "b3ad07ee0b9dac795e637580a6260751";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=5&appid={apiKey}";

        var client = new WebClient();
        _logger.LogInfo($"Calling GeoCode API with url: {url}");

        return client.DownloadString(url);
    }
}