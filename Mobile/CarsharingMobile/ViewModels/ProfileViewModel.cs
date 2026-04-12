using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarsharingMobile.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [RelayCommand]
    private async Task GoBackd()
    {
            await Shell.Current.GoToAsync("..");
    }
}
