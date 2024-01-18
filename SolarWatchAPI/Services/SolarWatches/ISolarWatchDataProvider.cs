using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.SolarWatches;

public interface ISolarWatchDataProvider
{
    string GetCurrentSolarWatch(GeoCode geoCode, string date);
}