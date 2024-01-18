using System.Text.Json;
using System.Text.Json.Serialization;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public class JsonProcess : IJsonProcess
{
    public GeoCode Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement lat = json.RootElement.GetProperty("lat");
        JsonElement lon = json.RootElement.GetProperty("lon");
        
        GeoCode geoCode = new GeoCode(lat.GetDecimal(), lon.GetDecimal());

        return geoCode;
    }
}