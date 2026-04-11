using CarsharingMobile.Extensions;

namespace CarsharingMobile;

public partial class App
{
    private readonly AppShell _shell;
    private readonly IdentityService _identityService;

    public App(AppShell shell, IdentityService identityService)
    {
        InitializeComponent();
        
        _shell = shell;
        _identityService = identityService;
        
        _ = CheckAutoLogin();
    }

    private async Task CheckAutoLogin()
    {
        var token = await SecureStorage.Default.GetAsync("tasty");

        if (!string.IsNullOrEmpty(token) && _identityService.IsTokenValid(token))
        {
            var (_, roleId) = await _identityService.GetProfileIdAsync();

            if (roleId is 1 or 2)
            {
                await Shell.Current.GoToAsync("//MainPage");
                return;
            }
        }
        
        SecureStorage.Default.Remove("tasty");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_shell);
    }
}