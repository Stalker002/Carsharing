using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using CarsharingMobile.Resources.Fonts;
using CarsharingMobile.Services;
using CarsharingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Trip;

namespace CarsharingMobile.ViewModels;

public partial class TripHistoryViewModel(TripService tripService) : ObservableObject
{
    private const int PageSize = 10;
    private int _currentPage = 1;
    private int _totalCount;

    public ObservableCollection<TripHistoryListItem> Trips { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTrips))]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    [NotifyPropertyChangedFor(nameof(TripsCountText))]
    [NotifyPropertyChangedFor(nameof(TotalDistanceText))]
    [NotifyPropertyChangedFor(nameof(TotalDurationText))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty] public partial bool IsLoadingMore { get; set; }
    [ObservableProperty] public partial string? ErrorMessage { get; set; }

    public bool HasTrips => Trips.Count > 0;
    public bool IsEmptyStateVisible => !IsBusy && !IsRefreshing && string.IsNullOrWhiteSpace(ErrorMessage) && Trips.Count == 0;

    public string TripsCountText => (_totalCount == 0 ? Trips.Count : _totalCount).ToString();
    public string TotalDistanceText => $"{Trips.Sum(x => x.DistanceKm):0.#} км";
    public string TotalDurationText => FormatSummaryDuration(Trips.Sum(x => x.DurationMinutes));

    [RelayCommand]
    private async Task LoadInitialAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            Trips.Clear();
            _currentPage = 1;
            _totalCount = 0;

            await LoadPageAsync();
            OnSummaryChanged();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Не удалось загрузить историю поездок.";
            Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        if (IsBusy)
            return;

        if (!IsRefreshing)
            IsRefreshing = true;

        await LoadInitialAsync();
    }

    [RelayCommand]
    private async Task LoadMoreAsync()
    {
        if (IsBusy || IsLoadingMore || (_totalCount != 0 && Trips.Count >= _totalCount))
            return;

        try
        {
            IsLoadingMore = true;
            await LoadPageAsync();
            OnSummaryChanged();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoadingMore = false;
        }
    }

    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private static async Task OpenTripDetailsAsync(TripHistoryListItem? trip)
    {
        if (trip == null)
            return;

        var route = $"{nameof(TripDetailsPage)}" +
                    $"?TripId={trip.Id.ToString(CultureInfo.InvariantCulture)}" +
                    $"&CarTitle={Uri.EscapeDataString(trip.CarTitle)}";

        if (!string.IsNullOrWhiteSpace(trip.CarImage))
        {
            route += $"&CarImageUrl={Uri.EscapeDataString(trip.CarImage)}";
        }

        await Shell.Current.GoToAsync(route);
    }

    private async Task LoadPageAsync()
    {
        var (items, totalCount) = await tripService.GetHistoryAsync(_currentPage, PageSize);
        _totalCount = totalCount;

        if (items == null)
        {
            ErrorMessage = "Не удалось загрузить историю поездок.";
            return;
        }

        foreach (var trip in items)
        {
            Trips.Add(new TripHistoryListItem(trip, OpenTripDetailsCommand));
        }

        _currentPage++;
    }

    private void OnSummaryChanged()
    {
        OnPropertyChanged(nameof(HasTrips));
        OnPropertyChanged(nameof(IsEmptyStateVisible));
        OnPropertyChanged(nameof(TripsCountText));
        OnPropertyChanged(nameof(TotalDistanceText));
        OnPropertyChanged(nameof(TotalDurationText));
    }

    private static string FormatSummaryDuration(decimal totalMinutes)
    {
        if (totalMinutes <= 0)
            return "0 мин";

        var duration = TimeSpan.FromMinutes((double)totalMinutes);

        if (duration.TotalHours >= 1)
            return $"{(int)duration.TotalHours} ч {duration.Minutes} мин";

        return $"{Math.Max(1, duration.Minutes)} мин";
    }
}

public class TripHistoryListItem
{
    public TripHistoryListItem(TripHistoryDto trip, IAsyncRelayCommand<TripHistoryListItem?> openCommand)
    {
        Id = trip.Id;
        CarTitle = string.Join(" ", new[] { trip.CarBrand, trip.CarModel }.Where(static value => !string.IsNullOrWhiteSpace(value)));
        CarImage = trip.CarImage;
        PeriodText = FormatPeriod(trip.StartTime, trip.EndTime);
        TotalAmountText = $"BYN {trip.TotalAmount.GetValueOrDefault():0.00}";
        DurationText = FormatDuration(trip.Duration.GetValueOrDefault());
        DistanceText = $"{trip.Distance.GetValueOrDefault():0.#} км";
        CalendarGlyph = FluentUI.calendar_ltr_20_regular;
        CalendarDayText = trip.StartTime.ToLocalTime().ToString("dd");
        CalendarMonthText = trip.StartTime.ToLocalTime().ToString("MMM").ToUpperInvariant();
        CarTitle = string.IsNullOrWhiteSpace(CarTitle) ? "Поездка" : CarTitle;
        CarSubtitle = BuildSubtitle(trip);
        StatusText = trip.StatusName;
        DistanceKm = trip.Distance.GetValueOrDefault();
        DurationMinutes = trip.Duration.GetValueOrDefault();
        OpenCommand = openCommand;
    }

    public int Id { get; }
    public string CarTitle { get; }
    public string? CarImage { get; }
    public string CalendarGlyph { get; }
    public string CalendarDayText { get; }
    public string CalendarMonthText { get; }
    public string CarSubtitle { get; }
    public string PeriodText { get; }
    public string TotalAmountText { get; }
    public string DurationText { get; }
    public string DistanceText { get; }
    public string StatusText { get; }
    public decimal DistanceKm { get; }
    public decimal DurationMinutes { get; }
    public IAsyncRelayCommand<TripHistoryListItem?> OpenCommand { get; }

    private static string FormatPeriod(DateTime startTime, DateTime? endTime)
    {
        var localStart = startTime.ToLocalTime();
        if (endTime == null)
            return $"{localStart:dd.MM.yyyy HH:mm} - в пути";

        var localEnd = endTime.Value.ToLocalTime();
        return $"{localStart:dd.MM.yyyy HH:mm} - {localEnd:dd.MM.yyyy HH:mm}";
    }

    private static string FormatDuration(decimal durationMinutes)
    {
        if (durationMinutes <= 0)
            return "0 мин";

        var duration = TimeSpan.FromMinutes((double)durationMinutes);

        if (duration.TotalHours >= 1)
            return $"{(int)duration.TotalHours} ч {duration.Minutes} мин";

        if (duration.TotalMinutes >= 1)
            return $"{Math.Max(1, duration.Minutes)} мин";

        return $"{Math.Max(1, duration.Seconds)} сек";
    }

    private static string BuildSubtitle(TripHistoryDto trip)
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(trip.TariffType))
            parts.Add(TranslateTariff(trip.TariffType));

        if (!string.IsNullOrWhiteSpace(trip.StartLocation))
            parts.Add(trip.StartLocation!);

        return parts.Count == 0 ? "История поездки" : string.Join(" • ", parts);
    }
    private static string TranslateTariff(string? tariffType)
    {
        return tariffType switch
        {
            "per_minute" => "Поминутный",
            "per_km" => "Покилометровый",
            "per_day" => "Посуточный",
            _ => "Стандартный"
        };
    }
}


