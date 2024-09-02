namespace RR.API.Configuration;

public static class ConfigureDB
{
    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=RRDB;Trusted_Connection=True;MultipleActiveResultSets=true;"));

        return builder;
    }
}
