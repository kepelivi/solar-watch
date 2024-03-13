using System.Text.Json;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.SolarWatches;

public class SolarJsonProcess : ISolarJsonProcess
{
    public SolarWatch Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        var sunrise = results.GetProperty("sunrise").GetString();
        var sunset = results.GetProperty("sunset").GetString();

        SolarWatch solarWatch = new(sunrise, sunset);

        return solarWatch;
    }
}