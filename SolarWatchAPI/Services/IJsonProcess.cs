using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public interface IJsonProcess
{
    GeoCode Process(string data);
}