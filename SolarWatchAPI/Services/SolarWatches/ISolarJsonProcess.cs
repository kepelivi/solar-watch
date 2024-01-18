using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.SolarWatches;

public interface ISolarJsonProcess
{
    SolarWatch Process(string data);
}