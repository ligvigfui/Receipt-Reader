using System.Reflection;

namespace RR.API.Configuration;

public static class DependencyIncejtionConfiguration
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
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

        return services;
    }
}
