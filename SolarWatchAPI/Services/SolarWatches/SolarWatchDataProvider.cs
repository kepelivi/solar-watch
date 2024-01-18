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

    public string GetCurrentSolarWatch(GeoCode geoCode, DateTime date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={geoCode.Latitude}&lng={geoCode.Longitude}&date={date.ToShortDateString()}";
        var client = new WebClient();
        
        _logger.LogInfo($"Calling GeoCode API with url: {url}");

        return client.DownloadString(url);
    }
}