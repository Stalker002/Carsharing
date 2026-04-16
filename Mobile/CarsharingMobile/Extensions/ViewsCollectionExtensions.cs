using CarsharingMobile.Views;

namespace CarsharingMobile.Extensions;

public static class ViewsCollectionExtensions
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();
        services.AddSingleton<App>();

        services.AddTransient<LoginPage>();
        services.AddTransient<RegistrationPage>();
        services.AddTransient<CurrentTripPage>();
        services.AddTransient<MainPage>();
        services.AddTransient<ProfilePage>();

        return services;
    }
}