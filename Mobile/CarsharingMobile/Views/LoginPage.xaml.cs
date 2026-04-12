using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class LoginPage : ContentPage 
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnLoginCompleted(object sender, EventArgs e)
    {
        PasswordEntry.Focus();
    }
}