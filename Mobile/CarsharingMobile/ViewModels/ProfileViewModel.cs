using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarsharingMobile.ViewModels;

public partial class ProfileViewModel(ClientService clientService, ClientDocumentsService clientDocumentsService)
    : ObservableObject
{
    [ObservableProperty] public partial bool IsBusy { get; set; }
    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial string? ErrorMessage { get; set; }
    [ObservableProperty] public partial string FullName { get; set; } = "Загрузка...";
    [ObservableProperty] public partial string PhoneNumber { get; set; } = "Не указан";
    [ObservableProperty] public partial string Email { get; set; } = "Не указан";
    [ObservableProperty] public partial string DocumentsCountText { get; set; } = "0";
    [ObservableProperty] public partial string ActiveDocumentsCountText { get; set; } = "0";
    [ObservableProperty] public partial string ExpiringDocumentsCountText { get; set; } = "0";
    [ObservableProperty] public partial string DocumentsSubtitle { get; set; } = "Документы еще не загружены";
    [ObservableProperty] public partial string Initials { get; set; } = "?";

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var profileTask = clientService.GetMyProfileAsync();
            var documentsTask = clientDocumentsService.GetMyDocumentsAsync();

            await Task.WhenAll(profileTask, documentsTask);

            var profile = profileTask.Result;
            var documents = documentsTask.Result;

            if (profile == null)
            {
                ErrorMessage = "Не удалось загрузить профиль.";
                return;
            }

            var name = string.Join(" ", new[] { profile.Name, profile.Surname }.Where(static x => !string.IsNullOrWhiteSpace(x)));
            FullName = string.IsNullOrWhiteSpace(name) ? "Без имени" : name;
            PhoneNumber = string.IsNullOrWhiteSpace(profile.PhoneNumber) ? "Не указан" : profile.PhoneNumber;
            Email = string.IsNullOrWhiteSpace(profile.Email) ? "Не указан" : profile.Email;
            Initials = BuildInitials(profile.Name, profile.Surname);

            var allDocuments = documents ?? [];
            var activeCount = allDocuments.Count(static x => x.ExpiryDate >= DateOnly.FromDateTime(DateTime.Today));
            var expiringCount = allDocuments.Count(static x =>
                x.ExpiryDate >= DateOnly.FromDateTime(DateTime.Today) &&
                x.ExpiryDate <= DateOnly.FromDateTime(DateTime.Today.AddDays(30)));

            DocumentsCountText = allDocuments.Count.ToString();
            ActiveDocumentsCountText = activeCount.ToString();
            ExpiringDocumentsCountText = expiringCount.ToString();
            DocumentsSubtitle = allDocuments.Count switch
            {
                0 => "Документы еще не загружены",
                _ when expiringCount > 0 => $"Проверьте {expiringCount} документ(а) с истекающим сроком",
                _ => "Все документы выглядят актуальными"
            };
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

    [RelayCommand]
    private static async Task OpenDocumentsAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.DocumentsPage));
    }

    private static string BuildInitials(string? name, string? surname)
    {
        var parts = new[] { name, surname }
            .Where(static x => !string.IsNullOrWhiteSpace(x))
            .Select(static x => x!.Trim()[0].ToString().ToUpperInvariant())
            .Take(2)
            .ToArray();

        return parts.Length == 0 ? "?" : string.Join(string.Empty, parts);
    }
}
