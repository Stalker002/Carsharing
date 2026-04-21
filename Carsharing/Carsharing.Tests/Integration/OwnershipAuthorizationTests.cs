using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Entites;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Clients;
using Shared.Contracts.Payments;
using Shared.Contracts.Trip;
using Shared.Enums;

namespace Carsharing.Tests.Integration;

public class OwnershipAuthorizationTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateClient_OtherUsersClient_ReturnsForbidden()
    {
        const int currentUserId = 1001;
        const int currentClientId = 2001;
        const int foreignClientId = 2002;

        SeedOwnershipData(factory, currentUserId, currentClientId, foreignClientId);

        var token = factory.GenerateTestToken(currentUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new ClientsRequest(currentUserId, "Hacker", "User", "+375291112244", "hack@test.com");

        var response = await _client.PutAsJsonAsync($"/Clients/{foreignClientId}", request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetBillInfo_ForeignBill_ReturnsNotFound()
    {
        const int currentUserId = 1101;
        const int currentClientId = 2101;
        const int foreignClientId = 2102;
        const int foreignBillId = 4101;

        SeedOwnershipData(factory, currentUserId, currentClientId, foreignClientId, foreignBillId);

        var token = factory.GenerateTestToken(currentUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/Bills/info/{foreignBillId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreatePayment_ForForeignBill_ReturnsNotFound()
    {
        const int currentUserId = 1201;
        const int currentClientId = 2201;
        const int foreignClientId = 2202;
        const int foreignBillId = 4201;

        SeedOwnershipData(factory, currentUserId, currentClientId, foreignClientId, foreignBillId);

        var token = factory.GenerateTestToken(currentUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new PaymentsRequest(foreignBillId, 15m, "Картой", DateTime.UtcNow);

        var response = await _client.PostAsJsonAsync("/Payments", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTripWithInfo_ForeignTrip_ReturnsUnauthorized()
    {
        const int currentUserId = 1301;
        const int currentClientId = 2301;
        const int foreignClientId = 2302;
        const int foreignTripId = 4301;

        SeedOwnershipData(factory, currentUserId, currentClientId, foreignClientId, foreignTripId: foreignTripId);

        var token = factory.GenerateTestToken(currentUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/Trips/{foreignTripId}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetBookingWithInfo_ForeignBooking_ReturnsUnauthorized()
    {
        const int currentUserId = 1401;
        const int currentClientId = 2401;
        const int foreignClientId = 2402;

        var foreignBookingId = SeedOwnershipData(factory, currentUserId, currentClientId, foreignClientId);

        var token = factory.GenerateTestToken(currentUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/Bookings/withInfo/{foreignBookingId}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTripLocation_ForeignTrip_ReturnsUnauthorized()
    {
        const int currentUserId = 1601;
        const int currentClientId = 2601;
        const int foreignClientId = 2602;

        var (_, foreignTripId, _) = SeedOwnershipData(
            factory,
            currentUserId,
            currentClientId,
            foreignClientId,
            null,
            null,
            false);

        var token = factory.GenerateTestToken(currentUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync(
            $"/Trips/{foreignTripId}/location",
            new UpdateTripLocationRequest("Remote", 53.9, 27.56));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetFinesByTrip_ForeignTrip_ReturnsUnauthorized()
    {
        const int currentUserId = 1501;
        const int currentClientId = 2501;
        const int foreignClientId = 2502;

        var (_, foreignTripId, _) = SeedOwnershipData(
            factory,
            currentUserId,
            currentClientId,
            foreignClientId,
            null,
            null,
            true);

        var token = factory.GenerateTestToken(currentUserId, 2);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/Fines/byTrip/{foreignTripId}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static int SeedOwnershipData(
        CustomWebApplicationFactory factory,
        int currentUserId,
        int currentClientId,
        int foreignClientId,
        int? foreignBillId = null,
        int? foreignTripId = null)
    {
        var (foreignBookingId, _, _) = SeedOwnershipData(factory, currentUserId, currentClientId, foreignClientId,
            foreignBillId, foreignTripId, false);
        return foreignBookingId;
    }

    private static (int ForeignBookingId, int ForeignTripId, int ForeignBillId) SeedOwnershipData(
        CustomWebApplicationFactory factory,
        int currentUserId,
        int currentClientId,
        int foreignClientId,
        int? foreignBillId,
        int? foreignTripId,
        bool withFine)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();

        SeedReferenceData(db);

        db.Client.AddRange(
            new ClientEntity
            {
                Id = currentClientId,
                UserId = currentUserId,
                Name = "Current",
                Surname = "Client",
                PhoneNumber = "+375291111111",
                Email = "current@test.com"
            },
            new ClientEntity
            {
                Id = foreignClientId,
                UserId = currentUserId + 1,
                Name = "Foreign",
                Surname = "Client",
                PhoneNumber = "+375292222222",
                Email = "foreign@test.com"
            });

        var foreignCarId = foreignClientId + 5000;
        var foreignBookingId = foreignClientId + 6000;
        var resolvedForeignTripId = foreignTripId ?? foreignClientId + 7000;
        var resolvedForeignBillId = foreignBillId ?? foreignClientId + 8000;

        db.Car.Add(new CarEntity
        {
            Id = foreignCarId,
            StatusId = (int)CarStatusEnum.Reserved,
            Location = "Remote",
            FuelLevel = 40
        });

        db.Booking.Add(new BookingEntity
        {
            Id = foreignBookingId,
            StatusId = (int)BookingStatusEnum.Active,
            CarId = foreignCarId,
            ClientId = foreignClientId,
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow.AddHours(1)
        });

        db.Trip.Add(new TripEntity
        {
            Id = resolvedForeignTripId,
            BookingId = foreignBookingId,
            StatusId = (int)TripStatusEnum.EnRoute,
            TariffType = "per_minute",
            StartTime = DateTime.UtcNow.AddMinutes(-30),
            Distance = 10
        });

        db.Bill.Add(new BillEntity
        {
            Id = resolvedForeignBillId,
            TripId = resolvedForeignTripId,
            StatusId = (int)BillStatusEnum.Unpaid,
            IssueDate = DateTime.UtcNow,
            Amount = 50,
            RemainingAmount = 50
        });

        if (withFine)
            db.Fine.Add(new FineEntity
            {
                Id = resolvedForeignBillId + 1000,
                TripId = resolvedForeignTripId,
                StatusId = 1,
                Type = "speeding",
                Amount = 25,
                Date = DateTime.UtcNow
            });

        db.SaveChanges();

        return (foreignBookingId, resolvedForeignTripId, resolvedForeignBillId);
    }

    private static void SeedReferenceData(CarsharingDbContext db)
    {
        if (!db.CarStatus.Any(s => s.Id == (int)CarStatusEnum.Reserved))
            db.CarStatus.Add(new CarStatusEntity
            {
                Id = (int)CarStatusEnum.Reserved,
                Name = nameof(CarStatusEnum.Reserved)
            });

        if (!db.BookingStatus.Any(s => s.Id == (int)BookingStatusEnum.Active))
            db.BookingStatus.Add(new BookingStatusEntity
            {
                Id = (int)BookingStatusEnum.Active,
                Name = nameof(BookingStatusEnum.Active)
            });

        if (!db.TripStatus.Any(s => s.Id == (int)TripStatusEnum.EnRoute))
            db.TripStatus.Add(new TripStatusEntity
            {
                Id = (int)TripStatusEnum.EnRoute,
                Name = nameof(TripStatusEnum.EnRoute)
            });

        if (!db.BillStatus.Any(s => s.Id == (int)BillStatusEnum.Unpaid))
            db.BillStatus.Add(new BillStatusEntity
            {
                Id = (int)BillStatusEnum.Unpaid,
                Name = nameof(BillStatusEnum.Unpaid)
            });
    }
}
