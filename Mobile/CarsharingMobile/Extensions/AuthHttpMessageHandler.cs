using System.Net;
using System.Net.Http.Headers;

namespace CarsharingMobile.Extensions;

public class AuthHttpMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await SecureStorage.Default.GetAsync("tasty");

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        SecureStorage.Default.Remove("tasty");

        MainThread.BeginInvokeOnMainThread(() => { Shell.Current.GoToAsync("//LoginPage"); });

        return response;
    }
}