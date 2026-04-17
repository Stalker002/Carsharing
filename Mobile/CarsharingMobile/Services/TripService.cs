using System.Diagnostics;
using System.Net.Http.Json;
using CarsharingMobile.Extensions;
using Shared.Contracts.Bills;
using Shared.Contracts.Bookings;
using Shared.Contracts.Payments;
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
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<CurrentTripDto>();
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

    public async Task<TripWithInfoDto?> GetTripDetailsAsync(int tripId)
    {
        try
        {
            var response = await httpClient.GetAsync($"Trips/{tripId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var trips = await response.Content.ReadFromJsonAsync<List<TripWithInfoDto>>();
            return trips?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return null;
        }
    }

    public async Task<BillWithInfoDto?> GetBillInfoAsync(int billId)
    {
        try
        {
            var response = await httpClient.GetAsync($"Bills/info/{billId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var bills = await response.Content.ReadFromJsonAsync<List<BillWithInfoDto>>();
            return bills?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return null;
        }
    }

    public async Task<IReadOnlyList<PaymentsResponse>> GetPaymentsByBillAsync(int billId)
    {
        try
        {
            var response = await httpClient.GetAsync($"Payments/byBill/{billId}");
            if (!response.IsSuccessStatusCode)
                return [];

            return await response.Content.ReadFromJsonAsync<List<PaymentsResponse>>() ?? [];
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return [];
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

    private static string? NormalizeImageUrl(string? imageUrl)
    {
        return imageUrl?
            .Replace("http://localhost:9000", $"http://{ApiConfig.HostIp}:9000")
            .Replace("http://minio:9000", $"http://{ApiConfig.HostIp}:9000");
    }
}
