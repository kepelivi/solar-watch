﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetSolarWatch([Required] string city, [Required] string date)
    {
        try
        {
            var geoCodeString = _geoCodeDataProvider.GetGeoCodeString(city);
            var geoCode = _geoJsonProcess.Process(geoCodeString);

            var solarString = _solarWatchDataProvider.GetCurrentSolarWatch(geoCode, date);
            
            return Ok(_solarJsonProcess.Process(solarString));
        }
        catch (Exception e)
        {
            _logger.LogError($"Error getting solar data: {e}");
            return NotFound($"Error getting solar data: {e}");
        }
    }
}