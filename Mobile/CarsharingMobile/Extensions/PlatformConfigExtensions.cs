#if ANDROID
using Android.Content.Res;
using Android.OS;
using AndroidX.Core.View;
using Color = Android.Graphics.Color;
#endif
#if IOS
using UIKit;
#endif
using Microsoft.Maui.Handlers;
using Microsoft.Maui.LifecycleEvents;

namespace CarsharingMobile.Extensions;

public static class PlatformConfigExtensions
{
    public static MauiAppBuilder ConfigurePlatformLifecycle(this MauiAppBuilder builder)
    {
#if ANDROID
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddAndroid(android => android.OnCreate(static (activity, _) =>
            {
                var window = activity.Window;
                if (window == null) return;

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    window.SetStatusBarColor(Color.ParseColor("#112347"));

                var controller = WindowCompat.GetInsetsController(window, window.DecorView);

                if (controller == null) return;
                controller.AppearanceLightStatusBars = false;
            }));
        });
#endif
#if IOS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddiOS(iOS => iOS.FinishedLaunching((application, launchOptions) =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

                return true;
            }));
        });
#endif
        return builder;
    }

    public static MauiAppBuilder ConfigureCustomHandlers(this MauiAppBuilder builder)
    {
        EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, _) =>
        {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Color.Transparent);
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
#endif
        });
        return builder;
    }
}
