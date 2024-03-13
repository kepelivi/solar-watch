using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.SolarWatches;

public interface ISolarWatchDataProvider
{
    Task<string> GetCurrentSolarWatch(GeoCode geoCode, string date);
}