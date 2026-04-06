using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (_viewModel.Cars.Count == 0 && _viewModel.LoadInitialCommand.CanExecute(null))
        {
            await _viewModel.LoadInitialCommand.ExecuteAsync(null);
        }
    }
}