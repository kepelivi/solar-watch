using SolarWatchAPI.Model;
using SolarWatchAPI.Services.SolarWatches;

namespace SolarWatchTest;

public class SolarJsonProcessTest
{
    [Test]
    public void Should_Parse_Valid_Json_With_Sunrise_And_Sunset_Properties()
    {
        string data = "{\"results\":{\"sunrise\":\"6:00 AM\",\"sunset\":\"6:00 PM\"}}";
        SolarJsonProcess solarJsonProcess = new SolarJsonProcess();

        SolarWatch solarWatch = solarJsonProcess.Process(data);

        Assert.AreEqual("6:00 AM", solarWatch.Sunrise);
        Assert.AreEqual("6:00 PM", solarWatch.Sunset);
    }

    [Test]
    public void Should_Handle_Json_With_Additional_Properties()
    {
        string data = "{\"results\":{\"sunrise\":\"8:00 AM\",\"sunset\":\"8:00 PM\",\"additional\":\"value\"}}";
        SolarJsonProcess solarJsonProcess = new SolarJsonProcess();

        SolarWatch solarWatch = solarJsonProcess.Process(data);

        Assert.AreEqual("8:00 AM", solarWatch.Sunrise);
        Assert.AreEqual("8:00 PM", solarWatch.Sunset);
    }

    [Test]
    public void Should_Throw_Key_Not_Found_Exception_When_Sunrise_Or_Sunset_Property_Missing()
    {
        string data = "{\"results\":{}}";
        SolarJsonProcess solarJsonProcess = new SolarJsonProcess();

        Assert.Throws<KeyNotFoundException>(() => solarJsonProcess.Process(data));
    }
}