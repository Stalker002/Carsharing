using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class CurrentTripPage : ContentPage
{
    private readonly CurrentTripViewModel _viewModel;

    public CurrentTripPage(CurrentTripViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        _viewModel.StopTracking();
        base.OnDisappearing();
    }
}
