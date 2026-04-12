namespace CarsharingMobile.Views;

public partial class TipsPage : ContentPage
{
    public TipsPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}