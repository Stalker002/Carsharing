using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class CarDetailsPage : ContentPage
{
    public CarDetailsPage(CarDetailsViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}