using System.Diagnostics;
using System.Net.Http.Json;
using Shared.Contracts.Bookings;

namespace CarsharingMobile.Services;

public class BookingService(HttpClient httpClient)
{
    public async Task<(int? BookingId, string? Error)> CreateBookingAsync(BookingsRequest request)
    {
        try
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
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return (null, "Ошибка подключения к серверу");
        }
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
