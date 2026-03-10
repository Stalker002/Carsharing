using System.Net.Http.Headers;
using System.Net.Http.Json;
using Carsharing.Application.DTOs;
using Carsharing.Core.Enum;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Entites;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.Tests.Integration;

public class TripsControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task FinishTrip_ActiveTrip_CompletesSuccessfully()
    {
        const int testUserId = 1;
        const int testTripId = 100;
        const int testBookingId = 50;
        const int testCarId = 10;
        
        var token = factory.GenerateTestToken(testUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();
            
            db.Tariff.Add(new TariffEntity { Id = 1, PricePerMinute = 5, Name = "Basic" });
            
            var car = new CarEntity { Id = testCarId, StatusId = (int)CarStatusEnum.Reserved, Location = "Old Location", FuelLevel = 50 };
            db.Car.Add(car);

            var booking = new BookingEntity { Id = testBookingId, CarId = testCarId, ClientId = 1, StatusId = (int)BookingStatusEnum.Active };
            db.Booking.Add(booking);

            var trip = new TripEntity 
            { 
                Id = testTripId, 
                BookingId = testBookingId, 
                StatusId = (int)TripStatusEnum.EnRoute, 
                StartTime = DateTime.UtcNow.AddMinutes(-30),
                TariffType = "per_minute"
            };
            db.Trip.Add(trip);
            
            await db.SaveChangesAsync();
        }

        var finishRequest = new FinishTripRequest(
            TripId: testTripId,
            Distance: 15.5m,
            EndLocation: "New Location",
            FuelLevel: 45m
        );

        var response = await _client.PostAsJsonAsync("/Trips/finish", finishRequest);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync();
            throw new Exception($"Сервер вернул 500. Детали: {errorText}");
        }
        
        var result = await response.Content.ReadFromJsonAsync<TripFinishResult>();
        Assert.NotNull(result);
        Assert.Equal("Поездка успешно завершена", result.Message);

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();
            
            var updatedTrip = await db.Trip.FindAsync(testTripId);
            Assert.NotNull(updatedTrip!.EndTime);
            Assert.Equal((int)TripStatusEnum.Finished, updatedTrip.StatusId);
            Assert.Equal(15.5m, updatedTrip.Distance);
            
            var updatedBooking = await db.Booking.FindAsync(testBookingId);
            Assert.Equal((int)BookingStatusEnum.Completed, updatedBooking!.StatusId);
            
            var updatedCar = await db.Car.FindAsync(testCarId);
            Assert.Equal((int)CarStatusEnum.Available, updatedCar!.StatusId);
            Assert.Equal("New Location", updatedCar.Location);
            Assert.Equal(45m, updatedCar.FuelLevel);
        }
    }
}