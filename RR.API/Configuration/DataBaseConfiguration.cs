namespace RR.API.Configuration;

public static class DataBaseConfiguration
{
    public static IHostApplicationBuilder ConfigureDatabase(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.Get<ConnectionStringsSettings>()!.ReceiptReaderDatabase;
        builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies()
                .UseSqlServer(connectionString, options =>
                {
					options.MigrationsAssembly("RR.Data");
                    options.EnableRetryOnFailure();
                    options.CommandTimeout(300);
                })
        );
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddIdentity<UserDBO, RoleDBO>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
        })
            .AddRoles<RoleDBO>()
            .AddRoleManager<RoleManager<RoleDBO>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        return builder;
    }
}
