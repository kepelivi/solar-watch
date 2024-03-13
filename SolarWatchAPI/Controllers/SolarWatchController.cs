using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatchAPI.Data;
using SolarWatchAPI.Model;
using SolarWatchAPI.Services.Repositories;
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
    private readonly ICityRepository _cityRepository;
    private readonly ISolarRepository _solarRepository;

    public SolarWatchController(
        ILogger logger,
        IGeoCodeDataProvider geoProv,
        IGeoJsonProcess geoJsonProc,
        ISolarWatchDataProvider solarProv,
        ISolarJsonProcess solarJsonProc,
        ICityRepository cityRepository,
        ISolarRepository solarRepository
    )
    {
        _logger = logger;
        _geoCodeDataProvider = geoProv;
        _geoJsonProcess = geoJsonProc;
        _solarWatchDataProvider = solarProv;
        _solarJsonProcess = solarJsonProc;
        _cityRepository = cityRepository;
        _solarRepository = solarRepository;
    }

    [HttpGet("GetSolarWatch"), Authorize(Roles="User, Admin")]
    public async Task<ActionResult<SolarWatch>> GetSolarWatch([Required] string cityName, [Required] string date)
    {
        var city = _cityRepository.GetCity(cityName);

        if (city != null)
        {
            var solar = _solarRepository.GetSolar(date);

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

                _solarRepository.AddSolar(new Solar(city.Id, date, solarWatch.Sunrise, solarWatch.Sunset));

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
            var cityFromApi = _geoJsonProcess.Process(geoCodeString);

            var geoCode = new GeoCode(cityFromApi.Longitude, cityFromApi.Latitude);

            var solarString = await _solarWatchDataProvider.GetCurrentSolarWatch(geoCode, date);
            var solarWatch = _solarJsonProcess.Process(solarString);
            
            _solarRepository.AddSolar(new Solar(cityFromApi.Id, date, solarWatch.Sunrise, solarWatch.Sunset));

            return Ok(solarWatch);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error getting solar data: {e}");
            return NotFound($"Error getting solar data: {e}");
        }
    }

    [HttpPatch("EditSolar"), Authorize(Roles = ("Admin"))]
    public async Task<ActionResult> EditSolar(Solar solar)
    {
        try
        {
            _solarRepository.UpdateSolar(solar);
            return Ok("Successfully updated sunrise and sunset time.");
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while updating solar data: {e}");
            return NotFound($"Error while updating solar data: {e}");
        }
    }
    
    [HttpDelete("DeleteSolar"), Authorize(Roles = ("Admin"))]
    public async Task<ActionResult> DeleteSolar([Required] int id)
    {
        try
        {
            _solarRepository.DeleteSolar(id);
            return Ok("Successfully deleted sunrise and sunset times.");
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while deleting solar data: {e}");
            return NotFound($"Error while deleting solar data: {e}");
        }
    }
}