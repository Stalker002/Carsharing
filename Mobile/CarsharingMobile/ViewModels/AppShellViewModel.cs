using CarsharingMobile.Services;
using CarsharingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace CarsharingMobile.ViewModels;

public class UserLoggedInMessage
{
}

public partial class AppShellViewModel : ObservableObject, IRecipient<UserLoggedInMessage>
{
    private readonly ClientService _clientService;

    public AppShellViewModel(ClientService clientService)
    {
        _clientService = clientService;

        WeakReferenceMessenger.Default.Register(this);

        Task.Run(LoadProfileAsync);
    }

    [ObservableProperty] public partial string Name { get; set; } = "Загрузка...";
    [ObservableProperty] public partial string Surname { get; set; } = "";

    public void Receive(UserLoggedInMessage message)
    {
        Task.Run(LoadProfileAsync);
    }

    [RelayCommand]
    public async Task LoadProfileAsync()
    {
        var profile = await _clientService.GetMyProfileAsync();

        if (profile != null)
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Name = string.IsNullOrWhiteSpace(profile.Name) ? "Без имени" : profile.Name;
                Surname = profile.Surname ?? "";
            });
    }

    [RelayCommand]
    private async Task GoToProfileAsync()
    {
        Shell.Current.FlyoutIsPresented = false;

        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }
}