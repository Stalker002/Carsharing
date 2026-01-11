using Carsharing.Application.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
        service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["tasty"];

                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;
                        if (context.Request.Cookies.ContainsKey("tasty"))
                            context.Token = context.Request.Cookies["tasty"];

                        return Task.CompletedTask;
                    }
                };
            });

        service.AddAuthorization(options =>
        {
            options.AddPolicy("AdminClientPolicy", policy =>
            {
                policy.RequireClaim("userRoleId", "1", "2");
            });

            options.AddPolicy("ClientPolicy", policy =>
            {
                policy.RequireClaim("userRoleId", "2");
            });

            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireClaim("userRoleId", "1");
            });
        });
    }
}