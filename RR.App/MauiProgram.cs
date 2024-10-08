﻿using Microsoft.Extensions.Logging;

namespace RR.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<Views.App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureLaunchSettings()
            .ConfigureDI();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
