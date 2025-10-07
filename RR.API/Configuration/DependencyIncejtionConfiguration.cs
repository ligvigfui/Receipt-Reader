using Azure.Storage.Blobs;
using System.Reflection;

namespace RR.API.Configuration;

public static class DependencyIncejtionConfiguration
{
    public static IHostApplicationBuilder ConfigureDependencyInjection(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var assembliesToScan = new[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.GetAssembly(typeof(ISecurityService)),
            Assembly.GetAssembly(typeof(IUserRepository))
        };

        /*
         * [RegisterAsSingleton]
         * [RegisterAsTransient]
         * [RegisterAsScoped]
         * [DoNotAutoRegister]
         */
        services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .AsPublicImplementedInterfaces();

        services.AddHttpContextAccessor();
        services.AddTransient<AutoRefreshTokenMiddleware>();
        services.AddSingleton(x =>
        {
            var settings = x.GetRequiredService<IOptions<AzureBlobStorageSettings>>().Value;
            return new BlobServiceClient(settings.ConnectionString);
        });
        return builder;
    }
}
