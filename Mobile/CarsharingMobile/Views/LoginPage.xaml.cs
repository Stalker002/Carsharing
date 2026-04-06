using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class LoginPage : ContentPage 
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}