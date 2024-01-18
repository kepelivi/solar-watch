namespace SolarWatchAPI.Model;

public record SolarWatch(TimeOnly Sunrise, TimeOnly Sunset, DateTime Date, string City);