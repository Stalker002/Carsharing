using System.Diagnostics;
using System.Net.Http.Json;
using CarsharingMobile.Extensions;
using Shared.Contracts.Bills;
using Shared.Contracts.Promocodes;

namespace CarsharingMobile.Services;

public class BillService(HttpClient httpClient)
{
    public async Task<(List<BillWithMinInfoDto>? Items, int TotalCount)> GetMyBillsAsync(int page = 1, int limit = 20)
    {
        return await httpClient.GetPagedAsync<BillWithMinInfoDto>($"Bills/pagedByUser?_page={page}&_limit={limit}");
    }

    public async Task<BillWithInfoDto?> GetBillInfoAsync(int billId)
    {
        try
        {
            var response = await httpClient.GetAsync($"Bills/info/{billId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var items = await response.Content.ReadFromJsonAsync<List<BillWithInfoDto>>();
            return items?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return null;
        }
    }

    public async Task<string?> ApplyPromocodeAsync(int billId, string code)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"Bills/{billId}/promocode", new ApplyPromocodeRequest(code));
            if (response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return "Ошибка подключения к серверу";
        }
    }
}
