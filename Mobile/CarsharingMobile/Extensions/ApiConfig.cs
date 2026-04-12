namespace CarsharingMobile.Extensions;

public static class ApiConfig
{
    public const string Port = "5078";

    public const string LocalIpAddress = "192.168.1.12";

    public static string HostIp =>
        (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.DeviceType == DeviceType.Virtual)
            ? "10.0.2.2"
            : LocalIpAddress;

    public static string BaseUrl => $"http://{HostIp}:{Port}/";
}