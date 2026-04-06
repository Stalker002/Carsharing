using System.Diagnostics;
using System.Net.Http.Json;
using CarsharingMobile.Extensions;
using Shared.Contracts.Cars;

namespace CarsharingMobile.Services;

public class CarService(HttpClient httpClient)
{
    public async Task<(List<CarsResponse>? Items, int TotalCount)> GetCarsAsync(int page = 1, int limit = 25)
    {
        var url = $"Cars?_page={page}&_limit={limit}";
        return await httpClient.GetPagedAsync<CarsResponse>(url);
    }
    
    public async Task<(List<CarWithMinInfoDto>? Items, int TotalCount)> GetAvailableCarsAsync(int page = 1, int limit = 15)
    {
        var url = $"Cars/pagedByCategory?_page={page}&_limit={limit}";
        return await httpClient.GetPagedAsync<CarWithMinInfoDto>(url);
    }

    public async Task<CarWithInfoDto?> GetCarDetailsAsync(int carId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<CarWithInfoDto>>($"Cars/with-info/{carId}");
            return response?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }
}