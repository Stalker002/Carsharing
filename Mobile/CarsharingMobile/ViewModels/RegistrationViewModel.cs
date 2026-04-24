using System.Net.Mail;
using System.Text.RegularExpressions;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;
using Shared.Contracts.Clients;
using Shared.Contracts.Users;

namespace CarsharingMobile.ViewModels;

public partial class RegistrationViewModel(AuthService authService, ClientDocumentsService clientDocumentsService) : ObservableObject
{
    [ObservableProperty] public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsStep1))]
    [NotifyPropertyChangedFor(nameof(IsStep2))]
    [NotifyPropertyChangedFor(nameof(IsStep3))]
    [NotifyPropertyChangedFor(nameof(IsStep4))]
    public partial int CurrentStep { get; set; } = 1;

    public bool IsStep1 => CurrentStep == 1;
    public bool IsStep2 => CurrentStep == 2;
    public bool IsStep3 => CurrentStep == 3;
    public bool IsStep4 => CurrentStep == 4;

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
    [ObservableProperty] public partial string? DriverLicenseNumber { get; set; }
    [ObservableProperty] public partial string? DriverLicenseNumberError { get; set; }
    [ObservableProperty] public partial string? DriverLicenseCategory { get; set; }
    [ObservableProperty] public partial string? DriverLicenseCategoryError { get; set; }
    [ObservableProperty] public partial DateTime DriverLicenseIssueDate { get; set; } = DateTime.Today.AddYears(-1);
    [ObservableProperty] public partial DateTime DriverLicenseExpiryDate { get; set; } = DateTime.Today.AddYears(9);
    [ObservableProperty] public partial string? DriverLicenseExpiryDateError { get; set; }
    [ObservableProperty] public partial string? DriverLicenseFileName { get; set; }
    [ObservableProperty] public partial string? DriverLicenseFileError { get; set; }

    [ObservableProperty] public partial bool IsPasswordHidden { get; set; } = true;
    [ObservableProperty] public partial string PasswordIcon { get; set; } = "eye_hide.png";

    private FileResult? _driverLicenseFile;

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

    partial void OnDriverLicenseNumberChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            DriverLicenseNumberError = "Введите номер водительских прав.";
        else
            DriverLicenseNumberError = null;
    }

    partial void OnDriverLicenseCategoryChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            DriverLicenseCategoryError = "Укажите категорию прав.";
        else
            DriverLicenseCategoryError = null;
    }

    partial void OnDriverLicenseExpiryDateChanged(DateTime value)
    {
        DriverLicenseExpiryDateError = value.Date <= DriverLicenseIssueDate.Date
            ? "Срок действия должен быть позже даты выдачи."
            : null;
    }

    partial void OnDriverLicenseIssueDateChanged(DateTime value)
    {
        DriverLicenseExpiryDateError = DriverLicenseExpiryDate.Date <= value.Date
            ? "Срок действия должен быть позже даты выдачи."
            : null;
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

        var cleanPhone = PhoneNumber!.Replace(" ", "").Replace("-", "");

        var error = await authService.SendSmsAsync(cleanPhone);

        IsBusy = false;

        if (error == null)
        {
            CurrentStep = 2;
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", error, "ОК");
        }
    }

    [RelayCommand]
    private async Task VerifySmsAsync()
    {
        OnSmsCodeChanged(SmsCode);
        if (SmsCodeError != null) return;

        IsBusy = true;

        var cleanPhone = PhoneNumber!.Replace(" ", "").Replace("-", "");

        var error = await authService.VerifySmsAsync(cleanPhone, SmsCode!);

        IsBusy = false;

        if (error == null)
        {
            CurrentStep = 3;
        }
        else
        {
            SmsCodeError = "Неверный код подтверждения";
        }
    }

    [RelayCommand]
    private void ContinueToLicenseStep()
    {
        OnNameChanged(Name);
        OnSurnameChanged(Surname);
        OnEmailChanged(Email);
        OnUserPasswordChanged(UserPassword);

        if (NameError != null || SurnameError != null || EmailError != null || UserPasswordError != null)
            return;

        CurrentStep = 4;
    }

    [RelayCommand]
    private async Task PickDriverLicenseFileAsync()
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите фото или PDF водительских прав",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, ["public.image", "com.adobe.pdf"] },
                    { DevicePlatform.Android, ["image/*", "application/pdf"] },
                    { DevicePlatform.WinUI, [".jpg", ".jpeg", ".png", ".pdf"] },
                    { DevicePlatform.macOS, ["public.image", "com.adobe.pdf"] },
                    { DevicePlatform.MacCatalyst, ["public.image", "com.adobe.pdf"] }
                })
            });

            if (file == null)
                return;

            _driverLicenseFile = file;
            DriverLicenseFileName = file.FileName;
            DriverLicenseFileError = null;
        }
        catch (Exception)
        {
            DriverLicenseFileError = "Не удалось выбрать файл.";
        }
    }

    [RelayCommand]
    private async Task CompleteRegistrationAsync()
    {
        OnNameChanged(Name);
        OnSurnameChanged(Surname);
        OnEmailChanged(Email);
        OnUserPasswordChanged(UserPassword);
        OnDriverLicenseNumberChanged(DriverLicenseNumber);
        OnDriverLicenseCategoryChanged(DriverLicenseCategory);
        OnDriverLicenseIssueDateChanged(DriverLicenseIssueDate);

        if (_driverLicenseFile == null)
            DriverLicenseFileError = "Добавьте файл водительских прав.";

        if (NameError != null || SurnameError != null || EmailError != null || UserPasswordError != null ||
            DriverLicenseNumberError != null || DriverLicenseCategoryError != null ||
            DriverLicenseExpiryDateError != null || DriverLicenseFileError != null)
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

            if (loginError == null)
            {
                var licenseError = await clientDocumentsService.CreateDriverLicenseAsync(
                    _driverLicenseFile!,
                    DriverLicenseNumber!.Trim(),
                    DriverLicenseCategory!.Trim().ToUpperInvariant(),
                    DateOnly.FromDateTime(DriverLicenseIssueDate),
                    DateOnly.FromDateTime(DriverLicenseExpiryDate));

                IsBusy = false;

                if (licenseError != null)
                {
                    await Shell.Current.DisplayAlert("Не удалось добавить права", licenseError, "ОК");
                    return;
                }

                WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Ошибка входа", loginError, "ОК");
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
