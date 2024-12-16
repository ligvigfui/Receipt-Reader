namespace RR.API.Configuration;

public static class AppSettingsConfiguration
{
    public static WebApplicationBuilder ConfigureAppSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json")
            //.AddJsonFile($"appsettings.{builder.Configuration.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
            .AddEnvironmentVariables();

        void Configure<T>(string section) where T : class =>
            builder.Services.Configure<T>(builder.Configuration.GetSection(section));

        Configure<JWTSettings>("JWT");

        return builder;
    }
}
