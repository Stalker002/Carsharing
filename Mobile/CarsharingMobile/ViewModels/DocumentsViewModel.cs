using System.Collections.ObjectModel;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarsharingMobile.ViewModels;

public partial class DocumentsViewModel(ClientDocumentsService clientDocumentsService) : ObservableObject
{
    public ObservableCollection<DocumentListItem> Documents { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmptyStateVisible))]
    public partial string? ErrorMessage { get; set; }

    public bool IsEmptyStateVisible => !IsBusy && !IsRefreshing && string.IsNullOrWhiteSpace(ErrorMessage) && Documents.Count == 0;

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            Documents.Clear();

            var items = await clientDocumentsService.GetMyDocumentsAsync();
            if (items == null)
            {
                ErrorMessage = "Не удалось загрузить документы.";
                return;
            }

            foreach (var document in items.OrderBy(static x => x.ExpiryDate))
                Documents.Add(new DocumentListItem(document));

            OnPropertyChanged(nameof(IsEmptyStateVisible));
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
            OnPropertyChanged(nameof(IsEmptyStateVisible));
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

public class DocumentListItem
{
    public DocumentListItem(ClientDocumentSummaryResponse document)
    {
        Title = string.IsNullOrWhiteSpace(document.Type) ? "Документ" : document.Type!;
        CategoryText = string.IsNullOrWhiteSpace(document.LicenseCategory) ? "Категория не указана" : $"Категория {document.LicenseCategory}";
        IssueDateText = document.IssueDate.ToString("dd.MM.yyyy");
        ExpiryDateText = document.ExpiryDate.ToString("dd.MM.yyyy");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var daysLeft = document.ExpiryDate.DayNumber - today.DayNumber;

        if (daysLeft < 0)
        {
            StatusText = "Истек";
            StatusColor = "#B42318";
            StatusBackground = "#FEE4E2";
        }
        else if (daysLeft <= 30)
        {
            StatusText = "Скоро истекает";
            StatusColor = "#B54708";
            StatusBackground = "#FFF3E0";
        }
        else
        {
            StatusText = "Действует";
            StatusColor = "#027A48";
            StatusBackground = "#ECFDF3";
        }
    }

    public string Title { get; }
    public string CategoryText { get; }
    public string IssueDateText { get; }
    public string ExpiryDateText { get; }
    public string StatusText { get; }
    public string StatusColor { get; }
    public string StatusBackground { get; }
}
