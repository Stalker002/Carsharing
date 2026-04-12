using Shared.Contracts.Clients;
using System.Diagnostics;
using System.Net.Http.Json;

namespace CarsharingMobile.Services;

public class ClientService(HttpClient httpClient)
{
    public async Task<ClientsResponse?> GetMyProfileAsync()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<ClientsResponse>>("Clients/My");
            return response?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
            return null;
        }
    }
}