namespace Carsharing.Application.Services;

public class MinioStorageOptions
{
    public string ServiceURL { get; set; } = string.Empty;

    public string PublicURL { get; set; } = string.Empty;

    public string AccessKey { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public string BucketName { get; set; } = "carsharing-images";
}
