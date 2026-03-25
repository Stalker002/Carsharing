using Carsharing.Application.Abstractions;
using Carsharing.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // 2. Сюда же можно перенести регистрацию Валидаторов (FluentValidation)
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IBillsService, BillsService>();
        services.AddScoped<IBillStatusesService, BillStatusesService>();
        services.AddScoped<IBookingsService, BookingsService>();
        services.AddScoped<IBookingStatusesService, BookingStatusesService>();
        services.AddScoped<ICarsService, CarsService>();
        services.AddScoped<ICarStatusesService, CarStatusesService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IClientDocumentsService, ClientDocumentsService>();
        services.AddScoped<IClientsService, ClientsService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IFinesService, FinesService>();
        services.AddScoped<IFineStatusesService, FineStatusesService>();
        services.AddScoped<IInsurancesService, InsurancesService>();
        services.AddScoped<IInsuranceStatusesService, InsuranceStatusesService>();
        services.AddScoped<IMaintenancesService, MaintenancesService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPromocodesService, PromocodesService>();
        services.AddScoped<IPromocodeStatusesService, PromocodeStatusesService>();
        services.AddScoped<IReviewsService, ReviewsService>();
        services.AddScoped<ISpecificationsCarService, SpecificationsCarService>();
        services.AddScoped<ITariffsService, TariffsService>();
        services.AddScoped<ITripDetailsService, TripDetailsService>();
        services.AddScoped<ITripStatusesService, TripStatusesService>();
        services.AddScoped<ITripService, TripService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
}