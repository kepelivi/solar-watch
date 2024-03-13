using Microsoft.EntityFrameworkCore;
using SolarWatchAPI.Data;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Services.Repositories;

public class SolarRepository : ISolarRepository
{
    private readonly SolarWatchContext _context;
    private readonly IConfigurationRoot _config;
    private readonly DbContextOptionsBuilder<SolarWatchContext> _optionsBuilder;

    public SolarRepository(SolarWatchContext context)
    {
        _config = new ConfigurationBuilder().AddUserSecrets<SolarRepository>().Build();
        _optionsBuilder = new DbContextOptionsBuilder<SolarWatchContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
        _context = context;
    }
    
    public Solar GetSolar(int cityId)
    {
        return _context.Solars.FirstOrDefault(s => s.CityId == cityId);
    }

    public void AddSolar(Solar solar)
    {
        _context.Add(solar);
        _context.SaveChanges();
    }

    public void DeleteSolar(int id)
    {
        var solarToDelete = _context.Solars.Find(id);
        _context.Solars.Remove(solarToDelete);
        _context.SaveChanges();
    }

    public void UpdateSolar(Solar solar)
    {
        _context.Solars.Update(solar);
        _context.SaveChanges();
    }
}