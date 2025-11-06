using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Negotiate;
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

        builder.Services.AddScoped<IUsersService, UsersService>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();

        // controllers in future epta :)

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }
        app.UseHttpsRedirection();

        // app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}