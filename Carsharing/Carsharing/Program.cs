using Carsharing.Application.Extensions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Repositories;
using Carsharing.Extension;
using Microsoft.AspNetCore.CookiePolicy;

namespace Carsharing;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<CarsharingDbContext>();

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();

        builder.Services.AddScoped<IBillRepository, BillRepository>();
        builder.Services.AddScoped<IBillsService, BillsService>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IBookingsService, BookingsService>();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<Application.Services.ICarsService, CarsService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoriesService, CategoriesService>();
        builder.Services.AddScoped<IClientDocumentRepository, ClientDocumentRepository>();
        builder.Services.AddScoped<IClientDocumentsService, ClientDocumentsService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IClientsService, ClientsService>();
        builder.Services.AddScoped<IFineRepository, FineRepository>();
        builder.Services.AddScoped<IFinesService, FinesService>();
        builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        builder.Services.AddScoped<IInsurancesService, InsurancesService>();
        builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
        builder.Services.AddScoped<IMaintenancesService, MaintenancesService>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IPromocodeRepository, PromocodeRepository>();
        builder.Services.AddScoped<IPromocodesService, PromocodesService>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IReviewsService, ReviewsService>();
        builder.Services.AddScoped<ISpecificationCarRepository, SpecificationCarRepository>();
        builder.Services.AddScoped<ISpecificationsCarService, SpecificationsCarService>();
        builder.Services.AddScoped<IStatusRepository, StatusRepository>();
        builder.Services.AddScoped<IStatusesService, StatusesService>();
        builder.Services.AddScoped<ITariffRepository, TariffRepository>();
        builder.Services.AddScoped<ITariffsService, TariffsService>();
        builder.Services.AddScoped<ITripDetailRepository, TripDetailRepository>();
        builder.Services.AddScoped<ITripDetailsService, TripDetailsService>();
        builder.Services.AddScoped<ITripRepository, TripRepository>();
        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddScoped<IUsersService, UsersService>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


        builder.Services.AddApiAuthentication(builder.Configuration);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();
            dbContext.Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}