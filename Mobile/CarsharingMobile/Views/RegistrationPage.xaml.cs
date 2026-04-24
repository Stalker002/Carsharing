using System.ComponentModel;
using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage(RegistrationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(RegistrationViewModel.CurrentStep))
        {
            var vm = (RegistrationViewModel)sender!;

            if (vm.CurrentStep == 2)
                Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), () => SmsCodeEntry.Focus());
            else if (vm.CurrentStep == 3)
                Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), () => NameEntry.Focus());
        }
    }

    private void OnNameCompleted(object sender, EventArgs e)
    {
        SurnameEntry.Focus();
    }

    private void OnSurnameCompleted(object sender, EventArgs e)
    {
        EmailEntry.Focus();
    }

    private void OnEmailCompleted(object sender, EventArgs e)
    {
        PasswordEntry.Focus();
    }
}
