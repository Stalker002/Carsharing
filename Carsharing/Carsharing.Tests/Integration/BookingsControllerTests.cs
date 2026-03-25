using System.Net.Http.Headers;
using System.Net.Http.Json;
using Carsharing.Contracts;
using Carsharing.Core.Enum;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Entites;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.Tests.Integration;

public class BookingsControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateBooking_ValidRequest_ReturnsOkAndBookingId()
    {
        const int testUserId = 1;
        const int testClientId = 10;
        const int testCarId = 5;
        
        var token = factory.GenerateTestToken(testUserId, 2);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();
            
            db.Client.Add(new ClientEntity { Id = testClientId, UserId = testUserId, Name = "Test", PhoneNumber = "123", Email = "test@test.com" });
            
            db.Car.Add(new CarEntity { Id = testCarId, StatusId = (int)CarStatusEnum.Available, Location = "Center" });
            
            await db.SaveChangesAsync();
        }

        var request = new BookingsRequest(
            StatusId: (int)BookingStatusEnum.Active,
            CarId: testCarId,
            ClientId: testClientId,
            StartTime: DateTime.UtcNow.AddMinutes(-5),
            EndTime: DateTime.UtcNow.AddHours(2)
        );

        var response = await _client.PostAsJsonAsync("/Bookings", request);

        var responseString = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"Запрос упал со статусом {response.StatusCode}. Детали: {responseString}");

        Assert.True(int.TryParse(responseString, out var bookingId));
        Assert.True(bookingId > 0);

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();
            var updatedCar = await db.Car.FindAsync(testCarId);
            Assert.Equal((int)CarStatusEnum.Reserved, updatedCar!.StatusId);
        }
    }
}