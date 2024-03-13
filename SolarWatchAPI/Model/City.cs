namespace SolarWatchAPI.Model;

public class City
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
    public string? State { get; init; }
    public string Country { get; init; }

    public City(string name, decimal latitude, decimal longitude, string state, string country)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        State = state;
        Country = country;
    }
}