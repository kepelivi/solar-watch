namespace SolarWatchAPI.Model;

public class Solar
{
    public int Id { get; init; }
    public int CityId { get; init; }
    
    public string Date { get; init; }
    public string Sunrise { get; init; }
    public string Sunset { get; init; }

    public Solar(int cityId, string date, string sunrise, string sunset)
    {
        CityId = cityId;
        Date = date;
        Sunrise = sunrise;
        Sunset = sunset;
    }
}