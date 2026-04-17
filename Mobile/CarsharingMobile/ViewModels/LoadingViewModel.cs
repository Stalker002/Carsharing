using CommunityToolkit.Mvvm.ComponentModel;
using CarsharingMobile.Extensions;

namespace CarsharingMobile.ViewModels;

public partial class LoadingViewModel : ObservableObject
{
    private readonly IdentityService _identityService;

    public LoadingViewModel(IdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task CheckAuthAsync()
    {
        var token = await SecureStorage.Default.GetAsync("tasty");

        await Task.Delay(500); 

        if (!string.IsNullOrEmpty(token) && _identityService.IsTokenValid(token))
        {
            var (_, roleId) = await _identityService.GetProfileIdAsync();

            if (roleId == 1 || roleId == 2)
            {
                await Shell.Current.GoToAsync("//MainPage");
                return;
            }
        }

        SecureStorage.Default.Remove("tasty");
        await Shell.Current.GoToAsync("//LoginPage");
    }
}