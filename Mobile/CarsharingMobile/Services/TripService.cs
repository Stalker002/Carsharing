using System.Diagnostics;
using System.Net.Http.Json;
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
                return await response.Content.ReadFromJsonAsync<CurrentTripDto>();
            }
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
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
}