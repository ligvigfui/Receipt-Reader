using RR.API.Configuration.InitialDataSeeders;

namespace RR.API.Configuration;

public static class DataSeeder
{
    public static async Task SeedEssentialData(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var seeders = services.GetServices<ISeedable>();
        foreach (var seeder in seeders)
        {
            if (await seeder.ShouldSeed())
            {
                await seeder.Seed();
            }
        }
    }
}
