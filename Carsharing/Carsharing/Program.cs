using Amazon.S3;
using Carsharing.Application.Abstractions;
using Carsharing.Application.Extensions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Repositories;
using Carsharing.Extension;
using Carsharing.Middleware;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;

namespace Carsharing;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var minioConfig = builder.Configuration.GetSection("Minio");

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .WithExposedHeaders("x-total-count");
            });
            options.AddPolicy("AllowAllOriginsDevelopment",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("x-total-count");
                });
        });
        
        builder.Services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = minioConfig["ServiceURL"],
                ForcePathStyle = true
            };

            return new AmazonS3Client(
                minioConfig["AccessKey"],
                minioConfig["SecretKey"],
                config
            );
        });

        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<CarsharingDbContext>();

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();

        builder.Services.AddScoped<IBillRepository, BillRepository>();
        builder.Services.AddScoped<IBillsService, BillsService>();
        builder.Services.AddScoped<IBillStatusRepository, BillStatusRepository>();
        builder.Services.AddScoped<IBillStatusesService, BillStatusesService>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IBookingsService, BookingsService>();
        builder.Services.AddScoped<IBookingStatusRepository, BookingStatusRepository>();
        builder.Services.AddScoped<IBookingStatusesService, BookingStatusesService>();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<ICarsService, CarsService>();
        builder.Services.AddScoped<ICarStatusRepository, CarStatusRepository>();
        builder.Services.AddScoped<ICarStatusesService, CarStatusesService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoriesService, CategoriesService>();
        builder.Services.AddScoped<IClientDocumentRepository, ClientDocumentRepository>();
        builder.Services.AddScoped<IClientDocumentsService, ClientDocumentsService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IClientsService, ClientsService>();
        builder.Services.AddScoped<IFineRepository, FineRepository>();
        builder.Services.AddScoped<IFinesService, FinesService>();
        builder.Services.AddScoped<IFineStatusRepository, FineStatusRepository>();
        builder.Services.AddScoped<IFineStatusesService, FineStatusesService>();
        builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();
        builder.Services.AddScoped<IInsurancesService, InsurancesService>();
        builder.Services.AddScoped<IInsuranceStatusRepository, InsuranceStatusRepository>();
        builder.Services.AddScoped<IInsuranceStatusesService, InsuranceStatusesService>();
        builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
        builder.Services.AddScoped<IMaintenancesService, MaintenancesService>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IPromocodeRepository, PromocodeRepository>();
        builder.Services.AddScoped<IPromocodesService, PromocodesService>();
        builder.Services.AddScoped<IPromocodeStatusRepository, PromocodeStatusRepository>();
        builder.Services.AddScoped<IPromocodeStatusesService, PromocodeStatusesService>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IReviewsService, ReviewsService>();
        builder.Services.AddScoped<ISpecificationCarRepository, SpecificationCarRepository>();
        builder.Services.AddScoped<ISpecificationsCarService, SpecificationsCarService>();
        builder.Services.AddScoped<ITariffRepository, TariffRepository>();
        builder.Services.AddScoped<ITariffsService, TariffsService>();
        builder.Services.AddScoped<ITripDetailRepository, TripDetailRepository>();
        builder.Services.AddScoped<ITripDetailsService, TripDetailsService>();
        builder.Services.AddScoped<ITripRepository, TripRepository>();
        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddScoped<ITripStatusRepository, TripStatusRepository>();
        builder.Services.AddScoped<ITripStatusesService, TripStatusesService>();
        builder.Services.AddScoped<IUsersService, UsersService>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IImageService, ImageService>();

        builder.Services.AddProblemDetails();

        builder.Services.AddApiAuthentication(builder.Configuration);

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
            .SetApplicationName("carsharing-app");

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));

        
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
            app.UseCors("Frontend");
        }
        else
        {
            app.UseCors("Frontend");
        }

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

        app.UseCustomException();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();

        app.MapControllers();

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}