﻿namespace RR.API.Configuration;

public static class ConfigureServices
{
    public static void ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=RRDB;Trusted_Connection=True;MultipleActiveResultSets=true;"));
    }
}