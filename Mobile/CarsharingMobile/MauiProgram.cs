using Microsoft.Extensions.Logging;
using CarsharingMobile.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Maui.Maps.Handlers;
#if ANDROID
using Android.Gms.Maps;
#endif

namespace CarsharingMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            
            .ConfigurePlatformLifecycle()
            .ConfigureCustomHandlers();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddApplicationServices()
            .AddViewModels()
            .AddViews();

        MapHandler.Mapper.AppendToMapping("HideNativeControls", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.GetMapAsync(new MapCallback());
#endif
        });

        return builder.Build();
    }
}

#if ANDROID
class MapCallback : Java.Lang.Object, IOnMapReadyCallback
{
    public void OnMapReady(GoogleMap googleMap)
    {
        if (googleMap != null)
        {
            googleMap.UiSettings.MyLocationButtonEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            googleMap.UiSettings.MapToolbarEnabled = false;
        }
    }
}
#endif