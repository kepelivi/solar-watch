using System.Text.Json;
using System.Text.Json.Serialization;
using SolarWatchAPI.Data;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Controllers;

public class GeoJsonProcess : IGeoJsonProcess
{
    public GeoCode Process(string data)
    {
        using var dbContext = new SolarWatchContext();
        
        JsonDocument json = JsonDocument.Parse(data);
        var lat = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("lat").GetDecimal();
        var lon = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("lon").GetDecimal();
        var name = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("name").GetString();
        var country = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("country").GetString();
        if (json.RootElement.EnumerateArray().FirstOrDefault().TryGetProperty("state", out var state))
        {
            dbContext.Add(new City(name, lat, lon, state.GetString(), country));
        }
        else
        {
            dbContext.Add(new City(name, lat, lon, null, country));
        }

        dbContext.SaveChanges();
        
        GeoCode geoCode = new(lon, lat);

        return geoCode;
    }
}