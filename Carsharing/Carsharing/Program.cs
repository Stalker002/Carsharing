using Amazon.S3;
using Carsharing.Application;
using Carsharing.Application.Abstractions;
using Carsharing.Application.Extensions;
using Carsharing.DataAccess;
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
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

        builder.Services.AddRepositories();
        builder.Services.AddApplication();

        builder.Services.AddProblemDetails();

        builder.Services.AddApiAuthentication(builder.Configuration);

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
            .SetApplicationName("carsharing-app");

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