using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarWatchAPI.Controllers;
using SolarWatchAPI.Model;
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
    public async Task Proper_Processing_of_GeoCode_and_SolarWatch()
    {
        var city = "Budapest";
        var date = "2022-12-13";

        var solarWatch = new SolarWatch("6:21:17 AM", "2:54:38 PM");

        // Act
        var response = await controller.GetSolarWatch(city, date);
        var result = response.Result as OkObjectResult;
        // Assert
        Assert.IsInstanceOf<ActionResult<SolarWatch>>(response);
        Assert.AreEqual(solarWatch,result.Value);
    }

    [Test]
    public async Task NonExistent_City_Throws_Exception()
    {
        var city = "Non Existent City";
        var date = "2022-01-01";

        var result = await controller.GetSolarWatch(city, date);
        
        Assert.That(result.Value, Is.EqualTo(null));
    }
}