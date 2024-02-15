using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Data;
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

    [HttpGet("GetSolarWatch"), Authorize]
    public async Task<ActionResult<SolarWatch>> GetSolarWatch([Required] string cityName, [Required] string date)
    {
        await using var dbContext = new SolarWatchContext();
        var city = dbContext.Cities.FirstOrDefault(c => c.Name == cityName);

        if (city != null)
        {
            var solar = dbContext.Solars.FirstOrDefault(s => s.CityId == city.Id);

            if (solar != null)
            {
                try
                {
                    _logger.LogInfo($"Found and retrieved solar data from database.");
                    return Ok(new SolarWatch(solar.Sunrise, solar.Sunset));
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error getting solar data: {e}");
                    return NotFound($"Error getting solar data: {e}");
                }
            }

            try
            {
                var geoCode = new GeoCode(city.Longitude, city.Latitude);
                var solarString = await _solarWatchDataProvider.GetCurrentSolarWatch(geoCode, date);
                var solarWatch = _solarJsonProcess.Process(solarString);

                dbContext.Add(new Solar(city.Id, solarWatch.Sunrise, solarWatch.Sunset));
                await dbContext.SaveChangesAsync();

                _logger.LogInfo($"Found and retrieved city {city.Name} from database.");
                _logger.LogInfo("Added new solar data.");
                return Ok(solarWatch);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting solar data: {e}");
                return NotFound($"Error getting solar data: {e}");
            }
        }

        try
        {
            var geoCodeString = await _geoCodeDataProvider.GetGeoCodeString(cityName);
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