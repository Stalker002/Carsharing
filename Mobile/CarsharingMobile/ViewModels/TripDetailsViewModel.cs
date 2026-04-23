using System.Diagnostics;
using CarsharingMobile.Extensions;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Trip;

namespace CarsharingMobile.ViewModels;

public partial class TripDetailsViewModel(TripService tripService) : ObservableObject
{
    private TripDetailsNavigationContext? _context;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasDetails))]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    [NotifyPropertyChangedFor(nameof(HasPayments))]
    public partial TripDetailsUiModel? Details { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial string? ErrorMessage { get; set; }

    public bool HasDetails => Details != null;

    public bool HasPayments => Details?.Payments.Count > 0;

    public bool IsEmptyStateVisible =>
        !IsBusy && !IsRefreshing && string.IsNullOrWhiteSpace(ErrorMessage) && Details == null;

    public void SetContext(TripDetailsNavigationContext context)
    {
        _context = context;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (_context == null || _context.TripId <= 0)
        {
            ErrorMessage = "Не передан идентификатор поездки.";
            Details = null;
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var details = await tripService.GetTripFullDetailsAsync(_context.TripId);
            if (details == null)
            {
                Details = null;
                ErrorMessage = "Не удалось загрузить детали поездки.";
                return;
            }

            Details = TripDetailsUiModelFactory.Create(details, _context);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            Details = null;
            ErrorMessage = "Не удалось загрузить детали поездки.";
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
        if (IsBusy || IsRefreshing)
            return;

        IsRefreshing = true;
        await LoadAsync();
    }

    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}

internal static class TripDetailsUiModelFactory
{
    public static TripDetailsUiModel Create(
        TripDetailsDto trip,
        TripDetailsNavigationContext context)
    {
        var header = trip.Header;
        var summary = trip.Summary;

        var carTitle = FirstNotBlank(context.CarTitle, header.CarTitle) ?? $"Поездка #{header.TripId}";
        var imageUrl = NormalizeImageUrl(FirstNotBlank(context.CarImageUrl, header.CarImage));

        var carCard = new TripCarCardUiModel(
            carTitle,
            context.Transmission ?? header.Transmission ?? "КПП не указана",
            context.RegistrationNumber ?? header.RegistrationNumber ?? "Госномер не указан",
            imageUrl);

        var overview = new TripOverviewUiModel(
            FormatDistance(summary.Distance),
            FormatDuration(summary.Duration),
            FormatAmount(summary.TotalAmount));

        var summaryRows = BuildSummaryRows(trip);
        var paymentRows = trip.Payments.Select(BuildPayment).ToList();

        return new TripDetailsUiModel(carCard, overview, summaryRows, paymentRows);
    }

    private static List<TripSummaryRowUiModel> BuildSummaryRows(TripDetailsDto trip)
    {
        var header = trip.Header;
        var summary = trip.Summary;
        var rows = new List<TripSummaryRowUiModel>
        {
            new("Период", FormatPeriod(header.StartTime, header.EndTime)),
            new("Маршрут", FormatRoute(header.StartLocation, header.EndLocation)),
            new("Тариф", TranslateTariff(header.TariffType)),
            new("Пробег", FormatDistance(summary.Distance)),
            new("Длительность", FormatDuration(summary.Duration)),
            new("Страхование", summary.InsuranceActive ? "Подключено" : "Не подключено")
        };

        if (summary.FuelUsed is > 0)
            rows.Add(new TripSummaryRowUiModel("Расход топлива", FormatLiters(summary.FuelUsed)));

        if (summary.Refueled is > 0)
            rows.Add(new TripSummaryRowUiModel("Дозаправка", FormatLiters(summary.Refueled)));

        if (summary.TotalAmount is not null)
            rows.Add(new TripSummaryRowUiModel("Сумма счета", FormatAmount(summary.TotalAmount)));

        if (summary.RemainingAmount is not null)
            rows.Add(new TripSummaryRowUiModel("Остаток к оплате", FormatAmount(summary.RemainingAmount)));

        return rows;
    }

    private static TripPaymentUiModel BuildPayment(TripPaymentDto payment)
    {
        return new TripPaymentUiModel(
            payment.Title,
            payment.Date.ToLocalTime().ToString("dd.MM.yyyy HH:mm"),
            $"-{payment.Amount:0.00} BYN",
            FormatPaymentMethod(payment.Method));
    }

    private static string FormatDistance(decimal? distanceKm)
    {
        return distanceKm is > 0 ? $"{distanceKm.Value:0.#} км" : "0 км";
    }

    private static string FormatDuration(decimal? durationMinutes)
    {
        if (durationMinutes is null or <= 0)
            return "0 мин";

        var duration = TimeSpan.FromMinutes((double)durationMinutes.Value);

        if (duration.TotalHours >= 1)
            return $"{(int)duration.TotalHours} ч {duration.Minutes} мин";

        if (duration.TotalMinutes >= 1)
            return $"{Math.Max(1, duration.Minutes)} мин";

        return $"{Math.Max(1, duration.Seconds)} сек";
    }

    private static string FormatAmount(decimal? amount)
    {
        return amount.HasValue ? $"{amount.Value:0.00} BYN" : "Не рассчитано";
    }

    private static string FormatLiters(decimal? liters)
    {
        return liters is > 0 ? $"{liters.Value:0.#} л" : "0 л";
    }

    private static string FormatLocation(string? location)
    {
        return string.IsNullOrWhiteSpace(location) ? "Не указано" : location!;
    }

    private static string FormatRoute(string? startLocation, string? endLocation)
    {
        var start = FormatLocation(startLocation);
        var end = string.IsNullOrWhiteSpace(endLocation) ? "В пути" : endLocation!;

        return $"{start} - {end}";
    }

    private static string FormatPeriod(DateTime startTime, DateTime? endTime)
    {
        var localStart = startTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        if (endTime == null)
            return $"{localStart} - в пути";

        return $"{localStart} - {endTime.Value.ToLocalTime():dd.MM.yyyy HH:mm}";
    }

    private static string FormatPaymentMethod(string? method)
    {
        return method switch
        {
            "1" => "Картой",
            "2" => "Наличными",
            "3" => "ЕРИП",
            _ when string.IsNullOrWhiteSpace(method) => "Способ не указан",
            _ => method
        };
    }

    private static string? FirstNotBlank(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return Uri.UnescapeDataString(value);
        }

        return null;
    }

    private static string? NormalizeImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return null;

        return imageUrl
            .Replace("http://localhost:9000", $"http://{ApiConfig.HostIp}:9000")
            .Replace("http://minio:9000", $"http://{ApiConfig.HostIp}:9000");
    }

    private static string TranslateTariff(string? tariffType) => tariffType switch
    {
        "per_minute" => "Поминутный",
        "per_km" => "Покилометровый",
        "per_day" => "Посуточный",
        _ => "Стандартный"
    };
}
