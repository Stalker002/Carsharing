using CarsharingMobile.ViewModels;
using CarsharingMobile.Views;

namespace CarsharingMobile;

public partial class AppShell : Shell
{
    private static bool _routesRegistered;

    public AppShell(AppShellViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        RegisterRoutesOnce();
    }

    private static void RegisterRoutesOnce()
    {
        if (_routesRegistered)
            return;

        _routesRegistered = true;

        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));

        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(CurrentTripPage), typeof(CurrentTripPage));
        Routing.RegisterRoute(nameof(TripHistoryPage), typeof(TripHistoryPage));
        Routing.RegisterRoute(nameof(BillsPage), typeof(BillsPage));
        Routing.RegisterRoute(nameof(CarDetailsPage), typeof(CarDetailsPage));
        Routing.RegisterRoute(nameof(TripDetailsPage), typeof(TripDetailsPage));
        Routing.RegisterRoute(nameof(TipsPage), typeof(TipsPage));
        Routing.RegisterRoute(nameof(BillPaymentPage), typeof(BillPaymentPage));
    }

    private async void OnCurrentTripClicked(object sender, EventArgs e)
    {
        Current.FlyoutIsPresented = false;
        await Current.GoToAsync(nameof(CurrentTripPage));
    }

    private async void OnTipsClicked(object sender, EventArgs e)
    {
        Current.FlyoutIsPresented = false;
        await Current.GoToAsync(nameof(TipsPage));
    }

    private async void OnCardsClicked(object sender, EventArgs e)
    {
        Current.FlyoutIsPresented = false;
    }

    private async void OnTripsClicked(object sender, EventArgs e)
    {
        Current.FlyoutIsPresented = false;
        await Current.GoToAsync(nameof(TripHistoryPage));
    }

    private async void OnBillsClicked(object sender, EventArgs e)
    {
        Current.FlyoutIsPresented = false;
        await Current.GoToAsync(nameof(BillsPage));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        var answer = await Current.DisplayAlert("Выход", "Вы действительно хотите выйти?", "Да", "Нет");
        if (answer)
        {
            Current.FlyoutIsPresented = false;
            SecureStorage.Default.Remove("tasty");
            await Current.GoToAsync("//LoginPage");
        }
    }
}
