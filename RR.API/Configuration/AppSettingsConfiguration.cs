namespace RR.API.Configuration;

public static class AppSettingsConfiguration
{
    public static WebApplicationBuilder ConfigureAppSettings(this WebApplicationBuilder builder)
    {
        var environment = /*builder.Configuration.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??*/ builder.Environment.EnvironmentName;
        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddJsonFile("appsettings.secrets.json", optional: true)
            .AddEnvironmentVariables();

        void Configure<T>(string section) where T : class =>
            builder.Services.Configure<T>(builder.Configuration.GetSection(section));

        Configure<AzureBlobStorageSettings>("AzureBlobStorage");
        Configure<AzureDocumentIntelligenceAPISettings>("AzureDocumentIntelligenceAPI");
        Configure<JWTSettings>("JWT");
        Configure<SwaggerSettings>("Swagger");

        return builder;
    }
}
