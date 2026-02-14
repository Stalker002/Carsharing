using Carsharing.Core.Abstractions;
using Carsharing.DataAccess.Extensions;
using Carsharing.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IBillStatusRepository, BillStatusRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IBookingStatusRepository, BookingStatusRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICarStatusRepository, CarStatusRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IClientDocumentRepository, ClientDocumentRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IFineRepository, FineRepository>();
        services.AddScoped<IFineStatusRepository, FineStatusRepository>();
        services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        services.AddScoped<IInsuranceStatusRepository, InsuranceStatusRepository>();
        services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPromocodeRepository, PromocodeRepository>();
        services.AddScoped<IPromocodeStatusRepository, PromocodeStatusRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<ISpecificationCarRepository, SpecificationCarRepository>();
        services.AddScoped<ITariffRepository, TariffRepository>();
        services.AddScoped<ITripDetailRepository, TripDetailRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<ITripStatusRepository, TripStatusRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}