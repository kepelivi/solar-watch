using Moq;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Data;
using SolarWatchAPI.Model;
using SolarWatchAPI.Services.Repositories;

namespace SolarWatchTest;

public class GeoJsonProcessTest
{
    private readonly ICityRepository _cityRepository = new CityRepository(new SolarWatchContext());

    [Test]
    public void Should_Throw_Key_Not_Found_Exception_When_Given_Json_Input_Without_Lat_Or_Lon_Properties()
    {
        string data = "[{\"lat\": 123.456}]";

        GeoJsonProcess geoJsonProcess = new GeoJsonProcess(_cityRepository);

        Assert.Throws<KeyNotFoundException>(() => geoJsonProcess.Process(data));
    }
}