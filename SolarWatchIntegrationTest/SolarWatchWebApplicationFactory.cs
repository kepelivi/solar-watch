using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarWatchAPI.Data;

namespace SolarWatchIntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<SolarWatchContext>));

            services.Remove(dbContextDescriptor);
            
            var config = new ConfigurationBuilder()
                .AddUserSecrets<SolarWatchWebApplicationFactory>()
                .Build();

            services.AddDbContext<SolarWatchContext>((container, options) =>
            {
                options.UseSqlServer(config["TestConnectionString"] != null
                    ? config["TestConnectionString"]:Environment.GetEnvironmentVariable("TESTCONNECTIONSTRING"));
            });
            
            services.AddAuthorization(opt =>
            {
                opt.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("User")
                    .Combine(opt.DefaultPolicy)
                    .Build();
            });

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
            var authSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            authSeeder.AddRoles();
            authSeeder.AddAdmin();
        });
    }
}