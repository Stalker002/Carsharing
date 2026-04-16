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

    public async Task<(List<CarWithMinInfoDto>? Items, int TotalCount)> GetAvailableCarsAsync(int page = 1,
        int limit = 15)
    {
        var url = $"Cars/pagedByCategory?_page={page}&_limit={limit}";
        var (items, count) = await httpClient.GetPagedAsync<CarWithMinInfoDto>(url);
        if (items != null)
            items =
            [
                .. items.Select(car =>
                {
                    var safeUrl = car.ImagePath?
                        .Replace("http://localhost:9000", $"http://{ApiConfig.HostIp}:9000")
                        .Replace("http://minio:9000", $"http://{ApiConfig.HostIp}:9000");

                    return car with { ImagePath = safeUrl };
                })
            ];

        return (items, count);
    }

    public async Task<CarWithInfoDto?> GetCarDetailsAsync(int carId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<CarWithInfoDto>>($"Cars/with-info/{carId}");
            var car = response?.FirstOrDefault();

            if (car != null)
                return car with
                {
                    ImagePath = car.ImagePath?
                        .Replace("http://localhost:9000", $"http://{ApiConfig.HostIp}:9000")
                        .Replace("http://minio:9000", $"http://{ApiConfig.HostIp}:9000")
                };

            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }

    public async Task<string?> DownloadAndCacheImageAsync(string? imageUrl, int carId)
    {
        if (string.IsNullOrWhiteSpace(imageUrl)) return null;

        var fileName = $"car_icon_{carId}.png";
        var localPath = Path.Combine(FileSystem.CacheDirectory, fileName);

        if (File.Exists(localPath))
            return localPath;

        try
        {
            var bytes = await httpClient.GetByteArrayAsync(imageUrl);

            await File.WriteAllBytesAsync(localPath, bytes);

            return localPath;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка скачивания фото авто: {ex.Message}");
            return imageUrl;
        }
    }
}