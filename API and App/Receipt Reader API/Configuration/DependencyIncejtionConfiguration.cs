using NetCore.AutoRegisterDi;
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

        List<Type> typesToExclude =
        [
            typeof(AuthorizeRolesAttribute)
        ];

        services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => !typesToExclude.Contains(c))
                //.IgnoreThisInterface<IMessage>()
                .AsPublicImplementedInterfaces();

        services.AddHttpContextAccessor();

        return services;
    }
}
