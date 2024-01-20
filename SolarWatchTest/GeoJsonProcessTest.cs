using SolarWatchAPI.Controllers;
using SolarWatchAPI.Model;

namespace SolarWatchTest;

public class GeoJsonProcessTest
{
    [Test]
    public void Should_Parse_Valid_Json_With_Lat_And_Lon_Properties()
    {
        string data = "[{\"lat\": 123.456, \"lon\": 789.012}]";

        GeoJsonProcess geoJsonProcess = new GeoJsonProcess();
        GeoCode result = geoJsonProcess.Process(data);

        Assert.AreEqual(123.456, result.Latitude);
        Assert.AreEqual(789.012, result.Longitude);
    }

    [Test]
    public void Should_Throw_Key_Not_Found_Exception_When_Given_Json_Input_Without_Lat_Or_Lon_Properties()
    {
        string data = "[{\"lat\": 123.456}]";

        GeoJsonProcess geoJsonProcess = new GeoJsonProcess();

        Assert.Throws<KeyNotFoundException>(() => geoJsonProcess.Process(data));
    }
}