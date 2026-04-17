using System.Diagnostics;
using System.Net.Http.Json;
using CarsharingMobile.Extensions;
using Shared.Contracts.Bookings;
using Shared.Contracts.Trip;

namespace CarsharingMobile.Services;

public class TripService(HttpClient httpClient)
{
    public async Task<(int? BookingId, string? Error)> CreateBookingAsync(BookingsRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("Bookings", request);

        if (response.IsSuccessStatusCode)
        {
            var id = await response.Content.ReadFromJsonAsync<int>();
            return (id, null);
        }

        var error = await response.Content.ReadAsStringAsync();
        return (null, error);
    }

    public async Task<CurrentTripDto?> GetCurrentTripAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("Trips/current");
            if (response.IsSuccessStatusCode)
            {
                var trip = await response.Content.ReadFromJsonAsync<CurrentTripDto>();
                return trip is null ? null : NormalizeCurrentTrip(trip);
            }

            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }

    public async Task<(List<TripHistoryDto>? Items, int TotalCount)> GetHistoryAsync(int page = 1, int limit = 10)
    {
        var url = $"Trips/history?page={page}&limit={limit}";
        var (items, totalCount) = await httpClient.GetPagedAsync<TripHistoryDto>(url);

        if (items != null)
        {
            items = [.. items.Select(trip => trip with
            {
                CarImage = NormalizeImageUrl(trip.CarImage)
            })];
        }

        return (items, totalCount);
    }

    public async Task<string?> UpdateTripLocationAsync(int tripId, UpdateTripLocationRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"Trips/{tripId}/location", request);
        if (response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<(TripFinishResult? Result, string? Error)> FinishTripAsync(FinishTripRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("Trips/finish", request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TripFinishResult>();
            return (result, null);
        }

        var error = await response.Content.ReadAsStringAsync();
        return (null, error);
    }

    private static CurrentTripDto NormalizeCurrentTrip(CurrentTripDto trip)
    {
        return trip with
        {
            CarImage = NormalizeImageUrl(trip.CarImage)
        };
    }

    private static string? NormalizeImageUrl(string? imageUrl)
    {
        return imageUrl?
            .Replace("http://localhost:9000", $"http://{ApiConfig.HostIp}:9000")
            .Replace("http://minio:9000", $"http://{ApiConfig.HostIp}:9000");
    }
}

public record UpdateTripLocationRequest(
    string Location,
    double CarLatitude,
    double CarLongitude
);
