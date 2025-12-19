using Microsoft.Extensions.Configuration;

namespace RR.Tests;

internal static class Get
{
    public static IConfigurationRoot Configuration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.secrets.json")
            .Build();
        return configuration;
    }
    public static ApplicationDbContext ApplicationDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new ApplicationDbContext(options);
        return context;
    }
}
