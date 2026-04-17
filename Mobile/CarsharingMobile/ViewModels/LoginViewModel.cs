using CarsharingMobile.Extensions;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Shared.Contracts.Users;

namespace CarsharingMobile.ViewModels;

public partial class LoginViewModel(AuthService authService, IdentityService identityService) : ObservableObject
{
    [ObservableProperty] public partial string? UserLogin { get; set; }

    [ObservableProperty] public partial string? UserPassword { get; set; }

    [ObservableProperty] public partial bool IsPasswordHidden { get; set; } = true;

    [ObservableProperty] public partial string PasswordIcon { get; set; } = "eye_hide.png";

    [ObservableProperty] public partial bool IsBusy { get; set; }

    [RelayCommand]
    private void TogglePassword()
    {
        IsPasswordHidden = !IsPasswordHidden;
        PasswordIcon = IsPasswordHidden ? "eye_hide.png" : "eye_visible.png";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(UserLogin) || string.IsNullOrWhiteSpace(UserPassword))
        {
            await Shell.Current.DisplayAlert("Внимание", "Заполните все поля", "ОК");
            return;
        }

        IsBusy = true;

        var request = new LoginRequest(UserLogin, UserPassword);

        var errorMessage = await authService.LoginAsync(request);

        IsBusy = false;

        if (errorMessage == null)
        {
            var (_, roleId) = await identityService.GetProfileIdAsync();

            switch (roleId)
            {
                case 2:
                    WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());
                    await Shell.Current.GoToAsync("//MainPage");
                    break;
                case 1:
                    WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());
                    await Shell.Current.DisplayAlert("Привет, Админ!", "Добро пожаловать в систему управления", "ОК");
                    await Shell.Current.GoToAsync("//MainPage");
                    break;
                default:
                    await Shell.Current.DisplayAlert("Ошибка", "Неизвестная роль пользователя", "ОК");
                    break;
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка входа", errorMessage, "ОК");
        }
    }

    [RelayCommand]
    private static async Task GoToRegistrationAsync()
    {
        await Shell.Current.GoToAsync("RegistrationPage");
    }
}