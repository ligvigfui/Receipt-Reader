namespace RR.App.Configurations;

public static class AppSettingsConfiguration
{
    public static MauiAppBuilder ConfigureLaunchSettings(this MauiAppBuilder builder)
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("RR.App.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

        builder.Configuration.AddConfiguration(config);

        void Configure<T>(string section) where T : class =>
            builder.Services.Configure<T>(builder.Configuration.GetSection(section));

        Configure<ApiHttpClientSettings>("ApiHttpClient");

        return builder;
    }
}

public class ApiHttpClientSettings
{
    public string BaseAddress { get; set; }
}