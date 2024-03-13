using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public interface IGeoCodeDataProvider
{
    Task<string> GetGeoCodeString(string city);
}