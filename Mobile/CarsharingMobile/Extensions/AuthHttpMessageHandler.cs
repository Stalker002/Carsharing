using System.Net;
using System.Net.Http.Headers;

namespace CarsharingMobile.Extensions;

public class AuthHttpMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var isApiRequest = request.RequestUri == null ||
                           !request.RequestUri.IsAbsoluteUri ||
                           request.RequestUri.Port.ToString() == ApiConfig.Port;

        if (isApiRequest)
        {
            var token = await SecureStorage.Default.GetAsync(AuthConstants.TokenKey);

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (isApiRequest && response.StatusCode == HttpStatusCode.Unauthorized)
        {
            SecureStorage.Default.Remove(AuthConstants.TokenKey);

            MainThread.BeginInvokeOnMainThread(() => { Shell.Current?.GoToAsync("//LoginPage"); });
        }

        return response;
    }
}