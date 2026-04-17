using System.Diagnostics;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Bills;
using Shared.Contracts.Payments;
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

            var trip = await tripService.GetTripDetailsAsync(_context.TripId);
            if (trip == null)
            {
                Details = null;
                ErrorMessage = "Не удалось загрузить детали поездки.";
                return;
            }

            BillWithInfoDto? bill = null;
            IReadOnlyList<PaymentsResponse> payments = [];

            if (_context.BillId is int billId and > 0)
            {
                bill = await tripService.GetBillInfoAsync(billId);
                if (bill != null)
                {
                    payments = await tripService.GetPaymentsByBillAsync(bill.Id);
                }
            }

            Details = TripDetailsUiModelFactory.Create(trip, bill, payments, _context);
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
        TripWithInfoDto trip,
        BillWithInfoDto? bill,
        IReadOnlyList<PaymentsResponse> payments,
        TripDetailsNavigationContext context)
    {
        var carCard = new TripCarCardUiModel(
            context.CarTitle ?? $"Поездка #{trip.Id}",
            context.Transmission ?? "КПП не указана",
            context.RegistrationNumber ?? "Госномер не указан",
            context.CarImageUrl);

        var overview = new TripOverviewUiModel(
            FormatDistance(trip.Distance),
            FormatDuration(trip.Duration),
            FormatAmount(bill?.Amount));

        var summaryRows = BuildSummaryRows(trip, bill);
        var paymentRows = payments.Select(BuildPayment).ToList();

        return new TripDetailsUiModel(carCard, overview, summaryRows, paymentRows);
    }

    private static IReadOnlyList<TripSummaryRowUiModel> BuildSummaryRows(TripWithInfoDto trip, BillWithInfoDto? bill)
    {
        var rows = new List<TripSummaryRowUiModel>
        {
            new("Период", FormatPeriod(trip.StartTime, trip.EndTime)),
            new("Маршрут", FormatRoute(trip.StartLocation, trip.EndLocation)),
            new("Тариф", string.IsNullOrWhiteSpace(trip.TariffType) ? "Не указан" : trip.TariffType!),
            new("Пробег", FormatDistance(trip.Distance)),
            new("Длительность", FormatDuration(trip.Duration)),
            new("Страхование", trip.InsuranceActive ? "Подключено" : "Не подключено")
        };

        if (trip.FuelUsed is > 0)
            rows.Add(new TripSummaryRowUiModel("Расход топлива", FormatLiters(trip.FuelUsed)));

        if (trip.Refueled is > 0)
            rows.Add(new TripSummaryRowUiModel("Дозаправка", FormatLiters(trip.Refueled)));

        if (bill?.Amount is not null)
            rows.Add(new TripSummaryRowUiModel("Сумма счета", FormatAmount(bill.Amount)));

        if (bill?.RemainingAmount is not null)
            rows.Add(new TripSummaryRowUiModel("Остаток к оплате", FormatAmount(bill.RemainingAmount)));

        return rows;
    }

    private static TripPaymentUiModel BuildPayment(PaymentsResponse payment)
    {
        return new TripPaymentUiModel(
            "Списание",
            payment.Date.ToLocalTime().ToString("dd.MM.yyyy HH:mm"),
            $"-{payment.Sum:0.00} BYN",
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
}
