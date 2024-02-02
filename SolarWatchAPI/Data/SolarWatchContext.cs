using Microsoft.EntityFrameworkCore;
using SolarWatchAPI.Model;

namespace SolarWatchAPI.Data;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<Solar> Solars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=CluelessRick2002!;Encrypt=false;");
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