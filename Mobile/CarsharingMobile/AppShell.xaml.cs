using CarsharingMobile.Views;

namespace CarsharingMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(CurrentTripPage), typeof(CurrentTripPage));
        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        //Routing.RegisterRoute(nameof(CarDetailsPage), typeof(CarDetailsPage));
        //Routing.RegisterRoute(nameof(DocumentsPage), typeof(DocumentsPage));
    }
}