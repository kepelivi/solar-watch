﻿namespace SolarWatchAPI.Utilities;

public interface ILogger
{
    public void LogInfo(string message);
    public void LogError(string message);
}