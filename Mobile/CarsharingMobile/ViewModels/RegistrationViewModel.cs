using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarsharingMobile.Services;
using Shared.Contracts.Clients;
using Shared.Contracts.Users;

namespace CarsharingMobile.ViewModels;

public partial class RegistrationViewModel(AuthService authService) : ObservableObject
{
    [ObservableProperty] public partial string? PhoneNumber { get; set; }
    [ObservableProperty] public partial string? SmsCode { get; set; }
    [ObservableProperty] public partial string? Name { get; set; }
    [ObservableProperty] public partial string? Surname { get; set; }
    [ObservableProperty] public partial string? Email { get; set; }[ObservableProperty] public partial string? UserPassword { get; set; }

    [ObservableProperty] public partial bool IsPasswordHidden { get; set; } = true;
    [ObservableProperty] public partial string PasswordIcon { get; set; } = "eye_hide.png";
    [ObservableProperty] public partial bool IsBusy { get; set; }

    [NotifyPropertyChangedFor(nameof(IsStep1))][NotifyPropertyChangedFor(nameof(IsStep2))]
    [NotifyPropertyChangedFor(nameof(IsStep3))]
    [ObservableProperty] public partial int CurrentStep { get; set; } = 1;

    public bool IsStep1 => CurrentStep == 1;
    public bool IsStep2 => CurrentStep == 2;
    public bool IsStep3 => CurrentStep == 3;

    [RelayCommand]
    private async Task SendSmsAsync()
    {
        if (string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length < 9)
        {
            await Shell.Current.DisplayAlert("Внимание", "Введите корректный номер телефона", "ОК");
            return;
        }

        IsBusy = true;
        
        // В реальном проекте здесь был бы вызов API интеграции с Twilio / SMS.RU
        await Task.Delay(1000); // Имитация запроса на сервер
        
        IsBusy = false;
        CurrentStep = 2;
    }

    [RelayCommand]
    private async Task VerifySmsAsync()
    {
        if (SmsCode != "0000")
        {
            await Shell.Current.DisplayAlert("Ошибка", "Неверный код (введите 0000 для теста)", "ОК");
            return;
        }

        IsBusy = true;
        await Task.Delay(500); // Имитация
        IsBusy = false;

        CurrentStep = 3;
    }

    [RelayCommand]
    private async Task CompleteRegistrationAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(UserPassword))
        {
            await Shell.Current.DisplayAlert("Внимание", "Заполните обязательные поля", "ОК");
            return;
        }

        IsBusy = true;

        var login = PhoneNumber!.Trim(); 

        var request = new ClientRegistrationRequest(
            Name.Trim(), 
            Surname?.Trim() ?? "", 
            PhoneNumber.Trim(), 
            Email.Trim(), 
            login,
            UserPassword);

        var errorMessage = await authService.RegisterAsync(request);

        if (errorMessage == null)
        {
            var loginError = await authService.LoginAsync(new LoginRequest(login, UserPassword));

            IsBusy = false;

            if (loginError == null)
            {
                await Shell.Current.GoToAsync("//DocumentsPage"); 
            }
        }
        else
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Ошибка", errorMessage, "ОК");
        }
    }

    [RelayCommand]
    private void TogglePassword()
    {
        IsPasswordHidden = !IsPasswordHidden;
        PasswordIcon = IsPasswordHidden ? "eye_hide.png" : "eye_visible.png";
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (CurrentStep > 1)
            CurrentStep--; // Возвращаемся на предыдущий шаг WIzard'а
        else
            await Shell.Current.GoToAsync("..");
    }
}