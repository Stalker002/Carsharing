using Carsharing.Application.Extensions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Repositories;
using Carsharing.Extension;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;


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

        builder.Services.AddDbContext<CarsharingDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(CarsharingDbContext)));
        });

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();

        builder.Services.AddScoped<IUsersService, UsersService>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


        builder.Services.AddApiAuthentication(builder.Configuration);

        // controllers in future epta :)

        var app = builder.Build();

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