namespace CarsharingMobile.Extensions;

public static class ApiConfig
{
    public const string Port = "5078";

    public const string LocalIpAddress = "172.23.201.129";

    public static string HostIp =>
        DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.DeviceType == DeviceType.Virtual
            ? "10.0.2.2"
            : LocalIpAddress;

    public static string BaseUrl => $"http://{HostIp}:{Port}/";
}