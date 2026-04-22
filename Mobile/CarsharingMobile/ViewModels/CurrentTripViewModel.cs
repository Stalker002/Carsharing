using System.Diagnostics;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Shared.Contracts.Trip;
using CarsharingMobile.Views;

namespace CarsharingMobile.ViewModels;

public partial class CurrentTripViewModel(TripService tripService) : ObservableObject
{
    private static readonly TimeSpan TrackingInterval = TimeSpan.FromSeconds(12);
    private const double MinDistanceForSyncMeters = 15;
    private CancellationTokenSource? _trackingCts;
    private IDispatcherTimer? _dashboardTimer;
    private Location? _lastSentLocation;

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
        await LoadCurrentTripAsync();
        StartTracking();
        StartDashboardTimer();
    }

    public void StopTracking()
    {
        StopDashboardTimer();
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
                CurrentTrip.Distance,
                locationLabel,
                currentLocation.Latitude,
                currentLocation.Longitude,
                CurrentTrip.FuelLevel);

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

            if (result.BillId > 0)
            {
                await Shell.Current.GoToAsync(nameof(BillPaymentPage),
                    new Dictionary<string, object>
                    {
                        { "BillId", result.BillId },
                        { "ReturnRoute", "//MainPage" }
                    });
                return;
            }

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
                _lastSentLocation = null;
                Title = "Активной поездки нет";
                CarTitle = "Поездка не найдена";
                TariffText = "Вернитесь на карту и начните новую поездку.";
                StartedAtText = "-";
                PricePerMinuteText = "-";
                PricePerKmText = "-";
                PricePerDayText = "-";
                CurrentLocationText = "Ожидание геопозиции";
                CoordinatesText = "Координаты недоступны";
                ElapsedTimeText = "00:00:00";
                CurrentCostText = "0.00 BYN";
                DistanceText = "0.00 км";
                TrackingStatusText = "Нет активной поездки";
                LastSyncText = "Синхронизация не требуется";
                OnPropertyChanged(nameof(TranslatedTariffType));
                OnPropertyChanged(nameof(ActiveTariffPriceText));
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
            _lastSentLocation = CurrentTrip.CarLatitude.HasValue && CurrentTrip.CarLongitude.HasValue
                ? new Location(CurrentTrip.CarLatitude.Value, CurrentTrip.CarLongitude.Value)
                : null;

            UpdateDashboard();
            OnPropertyChanged(nameof(TranslatedTariffType));
            OnPropertyChanged(nameof(ActiveTariffPriceText));
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
        _trackingCts?.Cancel();
        _trackingCts?.Dispose();
        _trackingCts = null;

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

        if (ShouldSkipSync(location))
        {
            UpdateCurrentTripLocation(location, null, 0m);
            TrackingStatusText = "Ожидание заметного смещения";
            CoordinatesText = FormatCoordinates(location.Latitude, location.Longitude);
            return;
        }

        var travelledDistanceKm = CalculateIncrementalDistance(location);
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
        _lastSentLocation = location;
        CurrentLocationText = locationLabel;
        CoordinatesText = FormatCoordinates(location.Latitude, location.Longitude);
        LastSyncText = $"Последняя синхронизация: {DateTime.Now:HH:mm:ss}";
        TrackingStatusText = "Геопозиция обновляется";

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            UpdateCurrentTripLocation(location, locationLabel, travelledDistanceKm);
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

    private void UpdateDashboard()
    {
        if (CurrentTrip == null) return;

        var timePassed = DateTime.UtcNow - CurrentTrip.StartTime;
        if (timePassed < TimeSpan.Zero)
            timePassed = TimeSpan.Zero;

        ElapsedTimeText = $"{(int)timePassed.TotalHours:00}:{timePassed.Minutes:00}:{timePassed.Seconds:00}";
        DistanceText = $"{CurrentTrip.Distance:0.00} км";
        CurrentCostText = $"{CalculateCurrentCost(CurrentTrip, timePassed):0.00} BYN";
    }

    private async Task<string> ResolveLocationLabelAsync(double latitude, double longitude)
    {
        try
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                var parts = new[]
                {
                    placemark.Thoroughfare,
                    placemark.SubThoroughfare,
                    placemark.Locality
                };

                var label = string.Join(", ", parts.Where(static value => !string.IsNullOrWhiteSpace(value)));
                if (!string.IsNullOrWhiteSpace(label))
                    return label;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка геокодера: {ex.Message}");
        }

        return $"{latitude:0.0000}, {longitude:0.0000}";
    }

    private bool ShouldSkipSync(Location location)
    {
        if (_lastSentLocation == null)
            return false;

        return Location.CalculateDistance(_lastSentLocation, location, DistanceUnits.Kilometers) * 1000
               < MinDistanceForSyncMeters;
    }

    private decimal CalculateIncrementalDistance(Location location)
    {
        if (_lastSentLocation == null)
            return 0m;

        var distanceKm = Location.CalculateDistance(_lastSentLocation, location, DistanceUnits.Kilometers);
        return distanceKm <= 0 ? 0m : decimal.Round((decimal)distanceKm, 3, MidpointRounding.AwayFromZero);
    }

    private void UpdateCurrentTripLocation(Location location, string? locationLabel, decimal travelledDistanceKm)
    {
        if (CurrentTrip == null)
            return;

        CurrentTrip = CurrentTrip with
        {
            CarLocation = locationLabel ?? CurrentTrip.CarLocation,
            CarLatitude = location.Latitude,
            CarLongitude = location.Longitude,
            Distance = CurrentTrip.Distance + travelledDistanceKm
        };

        UpdateDashboard();
    }

    private void StartDashboardTimer()
    {
        if (!HasActiveTrip || CurrentTrip == null)
        {
            StopDashboardTimer();
            return;
        }

        if (_dashboardTimer != null)
            return;

        _dashboardTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_dashboardTimer == null)
            return;

        _dashboardTimer.Interval = TimeSpan.FromSeconds(1);
        _dashboardTimer.Tick += OnDashboardTimerTick;
        _dashboardTimer.Start();
    }

    private void StopDashboardTimer()
    {
        if (_dashboardTimer == null)
            return;

        _dashboardTimer.Stop();
        _dashboardTimer.Tick -= OnDashboardTimerTick;
        _dashboardTimer = null;
    }

    private void OnDashboardTimerTick(object? sender, EventArgs e)
    {
        UpdateDashboard();
    }

    private static decimal CalculateCurrentCost(CurrentTripDto trip, TimeSpan timePassed)
    {
        return trip.TariffType switch
        {
            "per_km" => decimal.Round(trip.Distance * trip.PricePerKm, 2, MidpointRounding.AwayFromZero),
            "per_day" => decimal.Round(Math.Max(1m, (decimal)Math.Ceiling(timePassed.TotalDays)) * trip.PricePerDay,
                2, MidpointRounding.AwayFromZero),
            _ => decimal.Round((decimal)timePassed.TotalMinutes * trip.PricePerMinute, 2,
                MidpointRounding.AwayFromZero)
        };
    }

    private static string FormatCoordinates(double? latitude, double? longitude)
    {
        if (!latitude.HasValue || !longitude.HasValue)
            return "Координаты недоступны";

        return $"{latitude.Value:0.000000}, {longitude.Value:0.000000}";
    }

    public string TranslatedTariffType => CurrentTrip?.TariffType switch
    {
        "per_minute" => "Поминутный",
        "per_km" => "Покилометровый",
        "per_day" => "Посуточный",
        _ => "Стандартный"
    };

    public string ActiveTariffPriceText => CurrentTrip?.TariffType switch
    {
        "per_minute" => $"{CurrentTrip?.PricePerMinute:0.00} BYN / мин",
        "per_km" => $"{CurrentTrip?.PricePerKm:0.00} BYN / км",
        "per_day" => $"{CurrentTrip?.PricePerDay:0.00} BYN / сутки",
        _ => ""
    };

    [ObservableProperty] public partial string ElapsedTimeText { get; set; } = "00:00:00";
    [ObservableProperty] public partial string CurrentCostText { get; set; } = "0.00 BYN";
    [ObservableProperty] public partial string DistanceText { get; set; } = "0.00 км";
}
