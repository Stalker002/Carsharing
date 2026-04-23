using System.Collections.ObjectModel;
using System.Globalization;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Bills;
using Shared.Contracts.Payments;

namespace CarsharingMobile.ViewModels;

[QueryProperty(nameof(BillId), "BillId")]
[QueryProperty(nameof(ReturnRoute), "ReturnRoute")]
public partial class BillPaymentViewModel(BillService billService, PaymentService paymentService) : ObservableObject
{
    private static readonly string[] PaymentMethods = ["Картой", "ЕРИП", "Наличными"];

    [ObservableProperty] public partial int BillId { get; set; }
    [ObservableProperty] public partial bool IsBusy { get; set; }
    [ObservableProperty] public partial bool IsPaying { get; set; }
    [ObservableProperty] public partial string? ErrorMessage { get; set; }
    [ObservableProperty] public partial string? ReturnRoute { get; set; }
    [ObservableProperty] public partial string? PromoCode { get; set; }
    [ObservableProperty] public partial string? PaymentAmount { get; set; }
    [ObservableProperty] public partial string SelectedPaymentMethod { get; set; } = PaymentMethods[0];
    [ObservableProperty] public partial BillWithInfoDto? Bill { get; set; }

    public ObservableCollection<string> AvailablePaymentMethods { get; } = [.. PaymentMethods];
    public ObservableCollection<PaymentsResponse> Payments { get; } = [];

    public string BillTitle => Bill == null ? "Счет" : $"Счет #{Bill.Id}";
    public string StatusText => Bill?.StatusName ?? "Статус уточняется";
    public string AmountText => $"{Bill?.Amount.GetValueOrDefault():0.00} BYN";
    public string RemainingText => $"{Bill?.RemainingAmount.GetValueOrDefault():0.00} BYN";
    public string TripInfoText => Bill == null
        ? "Данные о поездке загружаются"
        : $"{TranslateTariff(Bill.TariffType)} • {Bill.Distance.GetValueOrDefault():0.##} км • {FormatDuration(Bill.Duration)}";
    public bool HasPayments => Payments.Count > 0;
    public bool IsFullyPaid => Bill?.RemainingAmount.GetValueOrDefault() <= 0m;
    public bool CanEditPayment => Bill != null && !IsFullyPaid;
    public bool CanPay => Bill != null && !IsFullyPaid && !IsBusy && !IsPaying && TryGetPaymentAmount(out _);
    public string PaymentHintText
    {
        get
        {
            if (Bill == null)
                return "Счет загружается";

            var remainingAmount = Bill.RemainingAmount.GetValueOrDefault();
            if (remainingAmount <= 0m)
                return "Счет полностью оплачен";

            if (!TryGetPaymentAmount(out var amount))
                return $"Введите сумму от 0.01 до {remainingAmount:0.00} BYN";

            return $"Будет списано: {amount:0.00} BYN";
        }
    }

    public async Task InitializeAsync()
    {
        if (BillId <= 0)
        {
            ErrorMessage = "Не удалось определить счет для оплаты.";
            return;
        }

        await LoadAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadAsync();
    }

    [RelayCommand]
    private async Task ApplyPromocodeAsync()
    {
        if (BillId <= 0 || string.IsNullOrWhiteSpace(PromoCode))
            return;

        IsBusy = true;
        ErrorMessage = null;

        var error = await billService.ApplyPromocodeAsync(BillId, PromoCode.Trim());
        IsBusy = false;

        if (error == null)
        {
            PromoCode = string.Empty;
            await LoadAsync();
            await Shell.Current.DisplayAlert("Промокод применен", "Счет пересчитан.", "ОК");
            return;
        }

        ErrorMessage = error;
    }

    [RelayCommand]
    private async Task PayAsync()
    {
        if (Bill == null || IsFullyPaid)
            return;

        if (!TryGetPaymentAmount(out var amount))
        {
            ErrorMessage = $"Введите сумму от 0.01 до {Bill.RemainingAmount.GetValueOrDefault():0.00} BYN";
            return;
        }

        IsPaying = true;
        OnPropertyChanged(nameof(CanPay));
        ErrorMessage = null;

        var request = new PaymentsRequest(
            Bill.Id,
            amount,
            SelectedPaymentMethod,
            DateTime.UtcNow);

        var (_, error) = await paymentService.CreatePaymentAsync(request);
        IsPaying = false;
        OnPropertyChanged(nameof(CanPay));

        if (error == null)
        {
            await LoadAsync();
            await Shell.Current.DisplayAlert("Оплата прошла", "Счет успешно оплачен.", "ОК");
            if (IsFullyPaid && !string.IsNullOrWhiteSpace(ReturnRoute))
                await Shell.Current.GoToAsync(ReturnRoute);

            return;
        }

        ErrorMessage = error;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (!string.IsNullOrWhiteSpace(ReturnRoute))
        {
            await Shell.Current.GoToAsync(ReturnRoute);
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var bill = await billService.GetBillInfoAsync(BillId);
            if (bill == null)
            {
                ErrorMessage = "Не удалось загрузить счет.";
                return;
            }

            Bill = bill;
            PaymentAmount = FormatAmountForInput(bill.RemainingAmount.GetValueOrDefault());

            Payments.Clear();
            var payments = await paymentService.GetPaymentsByBillAsync(BillId);
            foreach (var payment in payments.OrderByDescending(p => p.Date))
                Payments.Add(payment);

            OnPropertyChanged(nameof(HasPayments));
            OnPropertyChanged(nameof(CanPay));
            OnPropertyChanged(nameof(CanEditPayment));
            OnPropertyChanged(nameof(PaymentHintText));
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(CanPay));
        }
    }

    partial void OnIsBusyChanged(bool value)
    {
        OnPropertyChanged(nameof(CanPay));
    }

    partial void OnIsPayingChanged(bool value)
    {
        OnPropertyChanged(nameof(CanPay));
    }

    partial void OnPaymentAmountChanged(string? value)
    {
        OnPropertyChanged(nameof(CanPay));
        OnPropertyChanged(nameof(PaymentHintText));
    }

    partial void OnBillChanged(BillWithInfoDto? value)
    {
        OnPropertyChanged(nameof(BillTitle));
        OnPropertyChanged(nameof(StatusText));
        OnPropertyChanged(nameof(AmountText));
        OnPropertyChanged(nameof(RemainingText));
        OnPropertyChanged(nameof(TripInfoText));
        OnPropertyChanged(nameof(IsFullyPaid));
        OnPropertyChanged(nameof(CanEditPayment));
        OnPropertyChanged(nameof(CanPay));
        OnPropertyChanged(nameof(PaymentHintText));
    }

    private static string TranslateTariff(string? tariffType)
    {
        return tariffType switch
        {
            "per_minute" => "Поминутный тариф",
            "per_km" => "Покилометровый тариф",
            "per_day" => "Посуточный тариф",
            _ => "Тариф поездки"
        };
    }

    private static string FormatDuration(decimal? durationMinutes)
    {
        if (!durationMinutes.HasValue || durationMinutes <= 0)
            return "0 мин";

        var duration = TimeSpan.FromMinutes((double)durationMinutes.Value);
        if (duration.TotalHours >= 1)
            return $"{(int)duration.TotalHours} ч {duration.Minutes} мин";

        return $"{Math.Max(1, duration.Minutes)} мин";
    }

    private bool TryGetPaymentAmount(out decimal amount)
    {
        amount = 0m;

        var remainingAmount = Bill?.RemainingAmount.GetValueOrDefault() ?? 0m;
        if (remainingAmount <= 0m)
            return false;

        if (string.IsNullOrWhiteSpace(PaymentAmount))
            return false;

        var normalized = PaymentAmount.Trim().Replace(',', '.');
        if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
            return false;

        amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);

        return amount > 0m && amount <= remainingAmount;
    }

    private static string FormatAmountForInput(decimal amount)
    {
        return amount <= 0m ? string.Empty : amount.ToString("0.00", CultureInfo.InvariantCulture);
    }
}
