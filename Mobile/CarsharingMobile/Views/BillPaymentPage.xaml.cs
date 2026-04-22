using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class BillPaymentPage : ContentPage
{
    private readonly BillPaymentViewModel _viewModel;

    public BillPaymentPage(BillPaymentViewModel viewModel)
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
}
