using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Services.SolarWatches;
using SolarWatchAPI.Utilities;

namespace SolarWatchTest;

public class SolarWatchControllerTest
{
    private readonly Mock<ILogger> _logger = new();
    private readonly Mock<GeoCodeDataProvider> geoProv;
    private readonly Mock<GeoJsonProcess> geoJsonProc = new();
    private readonly Mock<SolarWatchDataProvider> solarProv;
    private readonly Mock<SolarJsonProcess> solarJsonProc = new();
    private readonly SolarWatchController controller;

    public SolarWatchControllerTest()
    {
        geoProv = new Mock<GeoCodeDataProvider>(_logger.Object);
        solarProv = new Mock<SolarWatchDataProvider>(_logger.Object);
        controller = new SolarWatchController(
            _logger.Object,
            geoProv.Object,
            geoJsonProc.Object,
            solarProv.Object,
            solarJsonProc.Object);
    }

    [Test]
    public void Proper_Processing_of_GeoCode_and_SolarWatch()
    {
        var city = "Budapest";
        var date = "2022-12-13";

        var geoString = geoProv.Object.GetGeoCodeString(city);
        var geoCode = geoJsonProc.Object.Process(geoString);

        var solarString = solarProv.Object.GetCurrentSolarWatch(geoCode, date);
        var expected = solarJsonProc.Object.Process(solarString);
        var result = controller.GetSolarWatch(city, date) as OkObjectResult;
        
        Assert.That(result.Value, Is.EqualTo(expected));
    }

    [Test]
    public void NonExistent_City_Throws_Exception()
    {
        var city = "Non Existent City";
        var date = "2022-01-01";
        
        var result = controller.GetSolarWatch(city, date) as NotFoundObjectResult;

        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(404));
    }
}