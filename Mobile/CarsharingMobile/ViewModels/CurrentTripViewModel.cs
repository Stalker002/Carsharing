using System.Diagnostics;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Shared.Contracts.Trip;

namespace CarsharingMobile.ViewModels;

public partial class CurrentTripViewModel(TripService tripService) : ObservableObject
{
    private static readonly TimeSpan TrackingInterval = TimeSpan.FromSeconds(12);
    private CancellationTokenSource? _trackingCts;
    private bool _isInitialized;

    [ObservableProperty] public partial bool IsBusy { get; set; }
    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial bool IsFinishing { get; set; }
    [ObservableProperty] public partial string? ErrorMessage { get; set; }
    [ObservableProperty] public partial string Title { get; set; } = "Активная поездка";
    [ObservableProperty] public partial string CarTitle { get; set; } = "Автомобиль не найден";
    [ObservableProperty] public partial string TariffText { get; set; } = "Тариф не указан";
    [ObservableProperty] public partial string StartedAtText { get; set; } = "-";
    [ObservableProperty] public partial string PricePerMinuteText { get; set; } = "-";
    [ObservableProperty] public partial string PricePerKmText { get; set; } = "-";
    [ObservableProperty] public partial string PricePerDayText { get; set; } = "-";
    [ObservableProperty] public partial string CurrentLocationText { get; set; } = "Ожидание геопозиции";
    [ObservableProperty] public partial string CoordinatesText { get; set; } = "-";
    [ObservableProperty] public partial string LastSyncText { get; set; } = "Ещё не синхронизировано";
    [ObservableProperty] public partial string TrackingStatusText { get; set; } = "Трекинг не запущен";
    [ObservableProperty] public partial bool HasActiveTrip { get; set; }

    public CurrentTripDto? CurrentTrip { get; private set; }

    public async Task InitializeAsync()
    {
        if (_isInitialized)
        {
            StartTracking();
            return;
        }

        _isInitialized = true;
        await LoadCurrentTripAsync();
        StartTracking();
    }

    public void StopTracking()
    {
        _trackingCts?.Cancel();
        _trackingCts?.Dispose();
        _trackingCts = null;
        TrackingStatusText = HasActiveTrip ? "Трекинг остановлен" : "Нет активной поездки";
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        if (IsBusy || IsRefreshing)
            return;

        IsRefreshing = true;
        try
        {
            await LoadCurrentTripAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    [RelayCommand]
    private async Task FinishTripAsync()
    {
        if (CurrentTrip == null || IsFinishing)
            return;

        try
        {
            IsFinishing = true;
            ErrorMessage = null;

            var currentLocation = await GetPreciseLocationAsync();
            if (currentLocation == null)
            {
                ErrorMessage = "Не удалось получить геопозицию для завершения поездки.";
                return;
            }

            var locationLabel = await ResolveLocationLabelAsync(currentLocation.Latitude, currentLocation.Longitude);
            var request = new FinishTripRequest(
                CurrentTrip.Id,
                0,
                locationLabel,
                currentLocation.Latitude,
                currentLocation.Longitude,
                0);

            var (result, error) = await tripService.FinishTripAsync(request);
            if (result == null)
            {
                ErrorMessage = string.IsNullOrWhiteSpace(error)
                    ? "Не удалось завершить поездку."
                    : error;
                return;
            }

            StopTracking();
            HasActiveTrip = false;
            TrackingStatusText = "Поездка завершена";
            await Shell.Current.DisplayAlert("Поездка завершена",
                $"{result.Message}\nСумма: {result.TotalAmount.GetValueOrDefault():0.00} BYN", "ОК");
            await Shell.Current.GoToAsync("//MainPage");
        }
        finally
        {
            IsFinishing = false;
        }
    }

    private async Task LoadCurrentTripAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            CurrentTrip = await tripService.GetCurrentTripAsync();
            HasActiveTrip = CurrentTrip != null;

            if (CurrentTrip == null)
            {
                Title = "Активной поездки нет";
                CarTitle = "Поездка не найдена";
                TariffText = "Вернитесь на карту и начните новую поездку.";
                TrackingStatusText = "Нет активной поездки";
                LastSyncText = "Синхронизация не требуется";
                return;
            }

            Title = $"Поездка #{CurrentTrip.Id}";
            CarTitle = string.Join(" ", new[] { CurrentTrip.CarBrand, CurrentTrip.CarModel }
                .Where(static value => !string.IsNullOrWhiteSpace(value)));
            CarTitle = string.IsNullOrWhiteSpace(CarTitle) ? $"Автомобиль #{CurrentTrip.CarId}" : CarTitle;
            TariffText = string.IsNullOrWhiteSpace(CurrentTrip.TariffType)
                ? "Тариф не указан"
                : $"Тариф: {CurrentTrip.TariffType}";
            StartedAtText = CurrentTrip.StartTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            PricePerMinuteText = $"{CurrentTrip.PricePerMinute:0.00} BYN/мин";
            PricePerKmText = $"{CurrentTrip.PricePerKm:0.00} BYN/км";
            PricePerDayText = $"{CurrentTrip.PricePerDay:0.00} BYN/сутки";
            CurrentLocationText = string.IsNullOrWhiteSpace(CurrentTrip.CarLocation)
                ? "Местоположение уточняется"
                : CurrentTrip.CarLocation!;
            CoordinatesText = FormatCoordinates(CurrentTrip.CarLatitude, CurrentTrip.CarLongitude);
            TrackingStatusText = "Трекинг готов к запуску";
        }
        catch (Exception ex)
        {
            ErrorMessage = "Не удалось загрузить текущую поездку.";
            Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void StartTracking()
    {
        if (!HasActiveTrip || CurrentTrip == null || _trackingCts != null)
            return;

        _trackingCts = new CancellationTokenSource();
        _ = RunTrackingLoopAsync(_trackingCts.Token);
    }

    private async Task RunTrackingLoopAsync(CancellationToken cancellationToken)
    {
        TrackingStatusText = "Трекинг геопозиции запущен";

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await SyncLocationOnceAsync(cancellationToken);
                await Task.Delay(TrackingInterval, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            TrackingStatusText = "Ошибка трекинга";
        }
        finally
        {
            _trackingCts?.Dispose();
            _trackingCts = null;
        }
    }

    private async Task SyncLocationOnceAsync(CancellationToken cancellationToken)
    {
        if (CurrentTrip == null)
            return;

        var location = await GetPreciseLocationAsync();
        if (location == null)
        {
            TrackingStatusText = "Нет доступа к геопозиции";
            return;
        }

        var locationLabel = await ResolveLocationLabelAsync(location.Latitude, location.Longitude);
        var error = await tripService.UpdateTripLocationAsync(CurrentTrip.Id,
            new UpdateTripLocationRequest(locationLabel, location.Latitude, location.Longitude));

        if (!string.IsNullOrWhiteSpace(error))
        {
            TrackingStatusText = "Сервер не принял геопозицию";
            ErrorMessage = error;
            return;
        }

        ErrorMessage = null;
        CurrentLocationText = locationLabel;
        CoordinatesText = FormatCoordinates(location.Latitude, location.Longitude);
        LastSyncText = $"Последняя синхронизация: {DateTime.Now:HH:mm:ss}";
        TrackingStatusText = "Геопозиция обновляется";

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            if (CurrentTrip != null)
            {
                CurrentTrip = CurrentTrip with
                {
                    CarLocation = locationLabel,
                    CarLatitude = location.Latitude,
                    CarLongitude = location.Longitude
                };
            }
        });
    }

    private static async Task<Location?> GetPreciseLocationAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
            return null;

        return await Geolocation.Default.GetLastKnownLocationAsync()
               ?? await Geolocation.Default.GetLocationAsync(
                   new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10)));
    }

    private static async Task<string> ResolveLocationLabelAsync(double latitude, double longitude)
    {
        try
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
            var place = placemarks?.FirstOrDefault();
            if (place != null)
            {
                var parts = new[]
                {
                    place.Thoroughfare,
                    place.SubThoroughfare,
                    place.Locality
                };

                var label = string.Join(", ", parts.Where(static value => !string.IsNullOrWhiteSpace(value)));
                if (!string.IsNullOrWhiteSpace(label))
                    return label;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return $"{latitude:0.000000}, {longitude:0.000000}";
    }

    private static string FormatCoordinates(double? latitude, double? longitude)
    {
        if (!latitude.HasValue || !longitude.HasValue)
            return "Координаты недоступны";

        return $"{latitude.Value:0.000000}, {longitude.Value:0.000000}";
    }
}
