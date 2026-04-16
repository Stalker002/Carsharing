using System.Diagnostics;
using System.Net.Http.Json;
using Shared.Contracts.Clients;
using Shared.Contracts.Users;

namespace CarsharingMobile.Services;

public record LoginResponse(string Token);

public class AuthService(HttpClient httpClient)
{
    public async Task<string?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Users/login", request);

            if (!response.IsSuccessStatusCode) return "Неверный логин или пароль";

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (string.IsNullOrEmpty(result?.Token)) return "Сервер не вернул токен";
            await SecureStorage.Default.SetAsync("tasty", result.Token);
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return "Ошибка подключения к серверу";
        }
    }

    public async Task<string?> RegisterAsync(ClientRegistrationRequest request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Clients/with-user", request);

            if (response.IsSuccessStatusCode) return null;

            var errorContent = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(errorContent) ? "Ошибка регистрации" : errorContent;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return "Ошибка подключения к серверу";
        }
    }
}