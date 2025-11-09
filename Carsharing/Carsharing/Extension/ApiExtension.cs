using Carsharing.Application.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;

namespace Carsharing.Extension;

public static class ApiExtension
{
    public static void AddApiAuthentication(
        this IServiceCollection service, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection(nameof(JwtOptions));
        var jwtOptions = jwtSection.Get<JwtOptions>();

        if (jwtOptions == null || string.IsNullOrEmpty(jwtOptions.SecretKey))
            throw new Exception("JWT configuration missing or invalid");

        service.Configure<JwtOptions>(jwtSection);
        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["tasty"];

                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });
        service.AddAuthorization();
    }
}