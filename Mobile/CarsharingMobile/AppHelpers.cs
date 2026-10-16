using System.Diagnostics;
using CarsharingMobile.Extensions;

namespace CarsharingMobile;

public class AppHelpers
{
    public static async void CheckAuthAndRedirect()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync(AuthConstants.TokenKey);

            if (!string.IsNullOrEmpty(token)) await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Auth check failed: {ex.Message}");
        }
    }
}