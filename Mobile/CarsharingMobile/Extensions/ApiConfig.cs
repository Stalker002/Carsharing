namespace CarsharingMobile.Extensions;

public static class ApiConfig
{
    public const string DefaultPort = "5078";
    public const string DefaultLocalIpAddress = "10.252.11.227";

    public static string Port =>
        Preferences.Default.Get("ApiPort", DefaultPort);

    public static string HostIp
    {
        get
        {
            var customHost = Preferences.Default.Get("ApiHost", string.Empty);
            if (!string.IsNullOrWhiteSpace(customHost))
                return customHost;

            if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.DeviceType == DeviceType.Virtual)
                return "10.0.2.2";

            if (DeviceInfo.Platform == DevicePlatform.WinUI ||
                (DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.DeviceType == DeviceType.Virtual) ||
                DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                return "localhost";

            return DefaultLocalIpAddress;
        }
    }

    public static string BaseUrl => $"http://{HostIp}:{Port}/";

    public static string? NormalizeMediaUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return url;
        return url
            .Replace("http://localhost:9000", $"http://{HostIp}:9000")
            .Replace("http://127.0.0.1:9000", $"http://{HostIp}:9000")
            .Replace("http://minio:9000", $"http://{HostIp}:9000");
    }
}