using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Data;

public class SolarWatchContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<Solar> Solars { get; set; }

    public SolarWatchContext (DbContextOptions<SolarWatchContext> options)
        : base(options)
    {
    }

    public SolarWatchContext()
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<City>()
            .Property(p => p.Latitude)
            .HasColumnType("decimal(18,12)");
        
        modelBuilder.Entity<City>()
            .Property(p => p.Longitude)
            .HasColumnType("decimal(18,13)");
    }
}