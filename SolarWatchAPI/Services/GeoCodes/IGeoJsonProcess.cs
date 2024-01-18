using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public interface IGeoJsonProcess
{
    GeoCode Process(string data);
}