using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class TripHistoryPage : ContentPage
{
    private readonly TripHistoryViewModel _viewModel;

    public TripHistoryPage(TripHistoryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.Trips.Count == 0 && _viewModel.LoadInitialCommand.CanExecute(null))
        {
            await _viewModel.LoadInitialCommand.ExecuteAsync(null);
        }
    }
}
