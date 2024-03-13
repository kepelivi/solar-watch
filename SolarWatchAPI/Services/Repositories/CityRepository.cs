using Microsoft.EntityFrameworkCore;
using SolarWatchAPI.Data;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.Repositories;

public class CityRepository : ICityRepository
{
    private readonly SolarWatchContext _context;
    private readonly IConfigurationRoot _config;
    private readonly DbContextOptionsBuilder<SolarWatchContext> _optionsBuilder;

    public CityRepository(SolarWatchContext context)
    {
        _config = new ConfigurationBuilder().AddUserSecrets<CityRepository>().Build();
        _optionsBuilder = new DbContextOptionsBuilder<SolarWatchContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
        _context = context;
    }
    
    public City GetCity(string name)
    {
        return _context.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void AddCity(City city)
    {
        _context.Add(city);
        _context.SaveChanges();
    }
}