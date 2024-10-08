﻿namespace RR.App.Configurations;

public static class DependencyInjectionConfiguration
{
    public static MauiAppBuilder ConfigureDI(this MauiAppBuilder builder)
    {
        //var assembliesToScan = new[]
        //{
        //    Assembly.GetExecutingAssembly(),
        //    Assembly.GetAssembly(typeof(MainViewModel)),
        //    Assembly.GetAssembly(typeof(MainPage))
        //};

        //builder.Services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
        //        .AsPublicImplementedInterfaces();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();
        
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoginViewModel>();

        var apiHttpClientSettings = builder.Configuration.GetSection("ApiHttpClient").Get<ApiHttpClientSettings>()!;
        builder.Services.AddHttpClient(HttpClientConstants.ApiClient, client =>
        {
            client.BaseAddress = new Uri(apiHttpClientSettings.BaseAddress);
        });

        return builder;
    }
}
