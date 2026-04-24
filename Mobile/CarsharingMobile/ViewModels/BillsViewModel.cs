using System.Collections.ObjectModel;
using System.Diagnostics;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Bills;

namespace CarsharingMobile.ViewModels;

public partial class BillsViewModel(BillService billService) : ObservableObject
{
    private const int PageSize = 20;
    private int _currentPage = 1;
    private int _totalCount;

    public ObservableCollection<BillListItem> Bills { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasBills))]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    [NotifyPropertyChangedFor(nameof(UnpaidCountText))]
    [NotifyPropertyChangedFor(nameof(PaidCountText))]
    [NotifyPropertyChangedFor(nameof(TotalOutstandingText))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty] public partial bool IsLoadingMore { get; set; }
    [ObservableProperty] public partial string? ErrorMessage { get; set; }

    public bool HasBills => Bills.Count > 0;
    public bool IsEmptyStateVisible => !IsBusy && !IsRefreshing && string.IsNullOrWhiteSpace(ErrorMessage) && Bills.Count == 0;
    public string UnpaidCountText => Bills.Count(static x => !x.IsPaid).ToString();
    public string PaidCountText => Bills.Count(static x => x.IsPaid).ToString();
    public string TotalOutstandingText => $"{Bills.Where(static x => !x.IsPaid).Sum(static x => x.RemainingAmount):0.00} BYN";

    [RelayCommand]
    private async Task LoadInitialAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            Bills.Clear();
            _currentPage = 1;
            _totalCount = 0;

            await LoadPageAsync();
            OnSummaryChanged();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Не удалось загрузить счета.";
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
        if (IsBusy || IsRefreshing)
            return;

        IsRefreshing = true;
        await LoadInitialAsync();
    }

    [RelayCommand]
    private async Task LoadMoreAsync()
    {
        if (IsBusy || IsLoadingMore || (_totalCount != 0 && Bills.Count >= _totalCount))
            return;

        try
        {
            IsLoadingMore = true;
            await LoadPageAsync();
            OnSummaryChanged();
        }
        finally
        {
            IsLoadingMore = false;
        }
    }

    [RelayCommand]
    private static async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    [RelayCommand]
    private static async Task OpenBillAsync(BillListItem? bill)
    {
        if (bill == null)
            return;

        await Shell.Current.GoToAsync(nameof(Views.BillPaymentPage), new Dictionary<string, object>
        {
            { "BillId", bill.Id }
        });
    }

    private async Task LoadPageAsync()
    {
        var (items, totalCount) = await billService.GetMyBillsAsync(_currentPage, PageSize);
        _totalCount = totalCount;

        if (items == null)
        {
            ErrorMessage = "Не удалось загрузить счета.";
            return;
        }

        foreach (var bill in items)
            Bills.Add(new BillListItem(bill, OpenBillCommand));

        _currentPage++;
    }

    private void OnSummaryChanged()
    {
        OnPropertyChanged(nameof(HasBills));
        OnPropertyChanged(nameof(IsEmptyStateVisible));
        OnPropertyChanged(nameof(UnpaidCountText));
        OnPropertyChanged(nameof(PaidCountText));
        OnPropertyChanged(nameof(TotalOutstandingText));
    }
}

public class BillListItem
{
    public BillListItem(BillWithMinInfoDto bill, IAsyncRelayCommand<BillListItem?> openCommand)
    {
        Id = bill.Id;
        StatusText = bill.StatusName;
        IssueDateText = bill.IssueDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        AmountText = $"{bill.Amount.GetValueOrDefault():0.00} BYN";
        RemainingText = $"{bill.RemainingAmount.GetValueOrDefault():0.00} BYN";
        TariffText = bill.TariffType switch
        {
            "per_minute" => "Поминутный тариф",
            "per_km" => "Покилометровый тариф",
            "per_day" => "Посуточный тариф",
            _ => "Тариф поездки"
        };
        IsPaid = bill.RemainingAmount.GetValueOrDefault() <= 0m;
        RemainingAmount = bill.RemainingAmount.GetValueOrDefault();
        StatusColor = IsPaid ? "#2F855A" : "#D97706";
        AccentColor = IsPaid ? "#EDFDF3" : "#FFF7ED";
        ActionText = IsPaid ? "ОТКРЫТЬ" : "ОПЛАТИТЬ";
        OpenCommand = openCommand;
    }

    public int Id { get; }
    public string StatusText { get; }
    public string IssueDateText { get; }
    public string AmountText { get; }
    public string RemainingText { get; }
    public string TariffText { get; }
    public bool IsPaid { get; }
    public decimal RemainingAmount { get; }
    public string StatusColor { get; }
    public string AccentColor { get; }
    public string ActionText { get; }
    public IAsyncRelayCommand<BillListItem?> OpenCommand { get; }
}
