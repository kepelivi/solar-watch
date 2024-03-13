using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.Repositories;

public interface ICityRepository
{
    public City GetCity(string name);
    public void AddCity(City city);
}