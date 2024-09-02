using NetCore.AutoRegisterDi;
using System.Reflection;

namespace RR.API.Configuration;

public static class ConfigureDI
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        var assembliesToScan = new[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.GetAssembly(typeof(IVendorRepository)),
            //Assembly.GetAssembly(typeof(IUserRepository))
        };

        services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
            //.Where(c => !c.Namespace.StartsWith("RR.Common.Messaging"))
            //.IgnoreThisInterface<IMessage>()
            .AsPublicImplementedInterfaces();

        return services;
    }
}
