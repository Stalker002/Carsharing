using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class BillsPage : ContentPage
{
    private readonly BillsViewModel _viewModel;

    public BillsPage(BillsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.Bills.Count == 0 && _viewModel.LoadInitialCommand.CanExecute(null))
            await _viewModel.LoadInitialCommand.ExecuteAsync(null);
    }
}
