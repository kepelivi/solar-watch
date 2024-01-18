using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public interface IGeoCodeDataProvider
{
    string GetGeoCodeString(string city);
}