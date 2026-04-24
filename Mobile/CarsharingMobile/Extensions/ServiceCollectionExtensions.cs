using CarsharingMobile.Services;

namespace CarsharingMobile.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<AuthHttpMessageHandler>();
        services.AddSingleton<IdentityService>();
        services.AddSingleton<BookingStateService>();

        services.AddApiClient<AuthService>();
        services.AddApiClient<CarService>();
        services.AddApiClient<TripService>();
        services.AddApiClient<BookingService>();
        services.AddApiClient<ClientService>();
        services.AddApiClient<ClientDocumentsService>();
        services.AddApiClient<BillService>();
        services.AddApiClient<PaymentService>();

        return services;
    }

    private static IServiceCollection AddApiClient<TService>(this IServiceCollection services)
        where TService : class
    {
        services.AddHttpClient<TService>(client => { client.BaseAddress = new Uri(ApiConfig.BaseUrl); })
            .AddHttpMessageHandler<AuthHttpMessageHandler>();

        return services;
    }
}
