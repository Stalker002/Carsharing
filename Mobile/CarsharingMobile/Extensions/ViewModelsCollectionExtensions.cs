using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Extensions;

public static class ViewModelsCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<MainViewModel>();

        return services;
    }
}