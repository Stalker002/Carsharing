using Shared.Contracts.Bookings;
using System.Net.Http.Json;

namespace CarsharingMobile.Services;

public class BookingService(HttpClient httpClient)
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

    public async Task<BookingsResponse?> GetMyActiveBookingAsync()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<BookingsResponse>>("Bookings/byClient");

            return response?.FirstOrDefault(b => b.StatusId == 5);
        }
        catch
        {
            return null;
        }
    }
}
