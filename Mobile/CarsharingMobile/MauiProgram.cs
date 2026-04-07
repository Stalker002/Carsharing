using Microsoft.Extensions.Logging;
using CarsharingMobile.Extensions;
using CommunityToolkit.Maui;

namespace CarsharingMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit()
            .ConfigurePlatformLifecycle()
            .ConfigureCustomHandlers();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddApplicationServices()
            .AddViewModels()
            .AddViews();

        return builder.Build();
    }
}