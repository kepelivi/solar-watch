using System.Text.Json;
using System.Text.Json.Serialization;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public class GeoJsonProcess : IGeoJsonProcess
{
    public GeoCode Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement lat = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("lat");
        JsonElement lon = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("lon");
        
        GeoCode geoCode = new(lon.GetDecimal(), lat.GetDecimal());

        return geoCode;
    }
}