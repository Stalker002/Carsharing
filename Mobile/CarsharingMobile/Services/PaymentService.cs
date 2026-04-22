using System.Diagnostics;
using System.Net.Http.Json;
using Shared.Contracts.Payments;

namespace CarsharingMobile.Services;

public class PaymentService(HttpClient httpClient)
{
    public async Task<List<PaymentsResponse>> GetPaymentsByBillAsync(int billId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<PaymentsResponse>>($"Payments/byBill/{billId}");
            return response ?? [];
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return [];
        }
    }

    public async Task<(int? PaymentId, string? Error)> CreatePaymentAsync(PaymentsRequest request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Payments", request);
            if (response.IsSuccessStatusCode)
            {
                var paymentId = await response.Content.ReadFromJsonAsync<int>();
                return (paymentId, null);
            }

            return (null, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return (null, "Ошибка подключения к серверу");
        }
    }
}
