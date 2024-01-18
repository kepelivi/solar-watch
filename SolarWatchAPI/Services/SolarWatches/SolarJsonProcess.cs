using System.Text.Json;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.SolarWatches;

public class SolarJsonProcess : ISolarJsonProcess
{
    public SolarWatch Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        Console.WriteLine(data);
        JsonElement results = json.RootElement.GetProperty("results");
        JsonElement sunrise = results.GetProperty("sunrise");
        JsonElement sunset = results.GetProperty("sunset");

        SolarWatch solarWatch = new SolarWatch(sunrise.ToString(), sunset.GetString());

        return solarWatch;
    }
}