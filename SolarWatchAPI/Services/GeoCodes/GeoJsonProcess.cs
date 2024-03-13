using System.Text.Json;
using System.Text.Json.Serialization;
using SolarWatchAPI.Data;
using SolarWatchAPI.Model;
using SolarWatchAPI.Services.Repositories;

namespace SolarWatchAPI.Controllers;

public class GeoJsonProcess : IGeoJsonProcess
{
    private readonly ICityRepository _cityRepository;

    public GeoJsonProcess(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    public City Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        var lat = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("lat").GetDecimal();
        var lon = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("lon").GetDecimal();
        var name = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("name").GetString();
        var country = json.RootElement.EnumerateArray().FirstOrDefault().GetProperty("country").GetString();
        if (json.RootElement.EnumerateArray().FirstOrDefault().TryGetProperty("state", out var state))
        {
            var city = new City(name, lat, lon, state.GetString(), country);
            _cityRepository.AddCity(city);
            return city;
        }
        else
        {
            var city = new City(name, lat, lon, null, country);
            _cityRepository.AddCity(city);
            return city;
        }
    }
}