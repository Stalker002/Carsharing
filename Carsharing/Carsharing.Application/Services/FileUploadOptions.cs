namespace Carsharing.Application.Services;

public class FileUploadOptions
{
    public long MaxCarImageBytes { get; set; } = 5 * 1024 * 1024;

    public long MaxDocumentImageBytes { get; set; } = 10 * 1024 * 1024;
}