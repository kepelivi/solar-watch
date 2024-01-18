using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public interface IGeoCodeProvider
{
    string GetGeoCodeString(string city);
}