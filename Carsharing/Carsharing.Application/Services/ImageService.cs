using Carsharing.Core.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Carsharing.Application.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _env;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

    public ImageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveCarImageAsync(IFormFile file)
    {
        return await SaveFileInternalAsync(file, "cars");
    }

    public async Task<string> SaveDocumentImageAsync(IFormFile file)
    {
        return await SaveFileInternalAsync(file, "documents");
    }

    private async Task<string> SaveFileInternalAsync(IFormFile file, string subFolder)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(ext))
            throw new ArgumentException($"Неподдерживаемый формат файла. Разрешены: {string.Join(", ", _allowedExtensions)}");

        var fileName = $"{Guid.NewGuid()}{ext}";

        var rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var folder = Path.Combine(rootPath, "images", subFolder);

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        var fullPath = Path.Combine(folder, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/{subFolder}/{fileName}";
    }

    public void DeleteFile(string webPath)
    {
        if (string.IsNullOrEmpty(webPath)) return;

        var rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var systemPath = Path.Combine(rootPath, webPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(systemPath)) return;

        try
        {
            File.Delete(systemPath);
        }
        catch
        {
            // Логирование ошибки удаления
        }
    }
}