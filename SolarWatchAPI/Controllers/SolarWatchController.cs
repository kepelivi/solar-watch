using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Model;
using SolarWatchAPI.Services.SolarWatches;
using ILogger = SolarWatchAPI.Utilities.ILogger;

namespace SolarWatchAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IGeoCodeDataProvider _geoCodeDataProvider;
    private readonly IGeoJsonProcess _geoJsonProcess;
    private readonly ISolarWatchDataProvider _solarWatchDataProvider;
    private readonly ISolarJsonProcess _solarJsonProcess;

    public SolarWatchController(
        ILogger logger,
        IGeoCodeDataProvider geoProv,
        IGeoJsonProcess geoJsonProc,
        ISolarWatchDataProvider solarProv,
        ISolarJsonProcess solarJsonProc
        )
    {
        _logger = logger;
        _geoCodeDataProvider = geoProv;
        _geoJsonProcess = geoJsonProc;
        _solarWatchDataProvider = solarProv;
        _solarJsonProcess = solarJsonProc;
    }

    [HttpGet("GetSolarWatch")]
    public async Task<ActionResult<SolarWatch>> GetSolarWatch([Required] string city, [Required] string date)
    {
        try
        {
            var geoCodeString = await _geoCodeDataProvider.GetGeoCodeString(city);
            var geoCode = _geoJsonProcess.Process(geoCodeString);

            var solarString = await _solarWatchDataProvider.GetCurrentSolarWatch(geoCode, date);

            var solarWatch = _solarJsonProcess.Process(solarString);
            
            return Ok(solarWatch);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error getting solar data: {e}");
            return NotFound($"Error getting solar data: {e}");
        }
    }
}