using CarsharingMobile.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Maps.Handlers;
#if ANDROID
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Object = Java.Lang.Object;
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
                fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentUI");
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

#if ANDROID
        ConfigureAndroidMapHandlers();
#endif

        return builder.Build();
    }

#if ANDROID
    private static void ConfigureAndroidMapHandlers()
    {
        MapHandler.Mapper.AppendToMapping("HideNativeControls",
            (handler, _) => { handler.PlatformView.GetMapAsync(new MapCallback()); });

        MapPinHandler.Mapper.AppendToMapping("CustomPinIcon", (handler, pin) =>
        {
            if (pin is not CarPin carPin || string.IsNullOrWhiteSpace(carPin.ImageUrl)) return;

            var markerOptions = handler.PlatformView;
            try
            {
                Bitmap? bitmap = null;

                if (carPin.ImageUrl.StartsWith("/") && File.Exists(carPin.ImageUrl))
                    bitmap = BitmapFactory.DecodeFile(carPin.ImageUrl);

                if (bitmap != null)
                {
                    var targetWidth = 180;
                    var ratio = (float)bitmap.Height / bitmap.Width;
                    var targetHeight = (int)(targetWidth * ratio);

                    var scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, targetWidth, targetHeight, false);
                    var icon = BitmapDescriptorFactory.FromBitmap(scaledBitmap);

                    markerOptions.SetIcon(icon);
                    markerOptions.Anchor(0.5f, 0.5f);
                }
            }
            catch
            {
            }
        });
    }
#endif
}

#if ANDROID
internal sealed class MapCallback : Object, IOnMapReadyCallback
{
    public void OnMapReady(GoogleMap? googleMap)
    {
        if (googleMap == null)
            return;

        googleMap.UiSettings.MyLocationButtonEnabled = false;
        googleMap.UiSettings.ZoomControlsEnabled = false;
        googleMap.UiSettings.MapToolbarEnabled = false;
    }
}
#endif
