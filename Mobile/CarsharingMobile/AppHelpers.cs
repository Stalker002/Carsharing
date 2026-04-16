using System.Diagnostics;

namespace CarsharingMobile;

public class AppHelpers
{
    public static async void CheckAuthAndRedirect()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync("jwt_token");

            if (!string.IsNullOrEmpty(token)) await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Auth check failed: {ex.Message}");
        }
    }
}