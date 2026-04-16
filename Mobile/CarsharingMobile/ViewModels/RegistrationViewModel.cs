using System.Net.Mail;
using System.Text.RegularExpressions;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Shared.Contracts.Clients;
using Shared.Contracts.Users;

namespace CarsharingMobile.ViewModels;

public partial class RegistrationViewModel(AuthService authService) : ObservableObject
{
    [ObservableProperty] public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsStep1))]
    [NotifyPropertyChangedFor(nameof(IsStep2))]
    [NotifyPropertyChangedFor(nameof(IsStep3))]
    public partial int CurrentStep { get; set; } = 1;

    public bool IsStep1 => CurrentStep == 1;
    public bool IsStep2 => CurrentStep == 2;
    public bool IsStep3 => CurrentStep == 3;

    [ObservableProperty] public partial string? PhoneNumber { get; set; }
    [ObservableProperty] public partial string? PhoneNumberError { get; set; }

    [ObservableProperty] public partial string? SmsCode { get; set; }
    [ObservableProperty] public partial string? SmsCodeError { get; set; }
    [ObservableProperty] public partial string? Name { get; set; }
    [ObservableProperty] public partial string? NameError { get; set; }

    [ObservableProperty] public partial string? Surname { get; set; }
    [ObservableProperty] public partial string? SurnameError { get; set; }
    [ObservableProperty] public partial string? Email { get; set; }
    [ObservableProperty] public partial string? EmailError { get; set; }

    [ObservableProperty] public partial string? UserPassword { get; set; }
    [ObservableProperty] public partial string? UserPasswordError { get; set; }

    [ObservableProperty] public partial bool IsPasswordHidden { get; set; } = true;
    [ObservableProperty] public partial string PasswordIcon { get; set; } = "eye_hide.png";

    [GeneratedRegex(@"^(\+375|80)(29|44|33|25)\d{7}$")]
    private static partial Regex MyRegex { get; }

    partial void OnPhoneNumberChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            PhoneNumberError = "Введите телефон.";
            return;
        }

        var phoneRegex = MyRegex;
        if (!phoneRegex.IsMatch(value.Trim()))
            PhoneNumberError = "Неверный формат телефона. Пример: (+375/80)(29/44/33/25)XXX-XX-XX";
        else
            PhoneNumberError = null;
    }

    partial void OnSmsCodeChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 4)
            SmsCodeError = "Введите 4-значный код.";
        else
            SmsCodeError = null;
    }

    partial void OnNameChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            NameError = "Введите имя.";
        else
            NameError = null;
    }

    partial void OnSurnameChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            SurnameError = "Введите фамилию.";
        else
            SurnameError = null;
    }

    partial void OnEmailChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            EmailError = "Введите почту.";
            return;
        }

        try
        {
            _ = new MailAddress(value);
            EmailError = null;
        }
        catch
        {
            EmailError = "Неверный формат e-mail.";
        }
    }

    partial void OnUserPasswordChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            UserPasswordError = "Введите пароль.";
        else if (value.Length < 6)
            UserPasswordError = "Пароль должен быть от 6 символов.";
        else
            UserPasswordError = null;
    }

    [RelayCommand]
    private void TogglePassword()
    {
        IsPasswordHidden = !IsPasswordHidden;
        PasswordIcon = IsPasswordHidden ? "eye_hide.png" : "eye_visible.png";
    }

    [RelayCommand]
    private async Task SendSmsAsync()
    {
        OnPhoneNumberChanged(PhoneNumber);
        if (PhoneNumberError != null) return;

        IsBusy = true;
        await Task.Delay(1000);
        IsBusy = false;

        CurrentStep = 2;
    }

    [RelayCommand]
    private async Task VerifySmsAsync()
    {
        OnSmsCodeChanged(SmsCode);
        if (SmsCodeError != null) return;

        if (SmsCode != "0000")
        {
            SmsCodeError = "Неверный код (для теста введите 0000)";
            return;
        }

        IsBusy = true;
        await Task.Delay(500);
        IsBusy = false;

        CurrentStep = 3;
    }

    [RelayCommand]
    private async Task CompleteRegistrationAsync()
    {
        OnNameChanged(Name);
        OnSurnameChanged(Surname);
        OnEmailChanged(Email);
        OnUserPasswordChanged(UserPassword);

        if (NameError != null || EmailError != null || UserPasswordError != null)
            return;

        IsBusy = true;

        var login = PhoneNumber!.Trim();

        var request = new ClientRegistrationRequest(
            Name!.Trim(),
            Surname?.Trim() ?? "",
            PhoneNumber.Trim(),
            Email!.Trim(),
            login,
            UserPassword!);

        var errorMessage = await authService.RegisterAsync(request);

        if (errorMessage == null)
        {
            var loginError = await authService.LoginAsync(new LoginRequest(login, UserPassword!));
            IsBusy = false;

            if (loginError == null)
            {
                WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
        else
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Ошибка регистрации", errorMessage, "ОК");
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (CurrentStep > 1)
            CurrentStep--;
        else
            await Shell.Current.GoToAsync("..");
    }
}