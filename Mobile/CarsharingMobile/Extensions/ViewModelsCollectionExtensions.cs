using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Extensions;

public static class ViewModelsCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<AppShellViewModel>();

        services.AddTransient<LoadingViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<CurrentTripViewModel>();
        services.AddTransient<TripHistoryViewModel>();

        return services;
    }
}
