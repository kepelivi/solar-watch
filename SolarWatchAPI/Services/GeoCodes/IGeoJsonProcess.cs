using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public interface IGeoJsonProcess
{
    City Process(string data);
}