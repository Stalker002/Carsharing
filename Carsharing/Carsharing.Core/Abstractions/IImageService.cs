using Microsoft.AspNetCore.Http;

namespace Carsharing.Core.Abstractions;

public interface IImageService
{
    Task<string> SaveCarImageAsync(IFormFile file);
    Task<string> SaveDocumentImageAsync(IFormFile file);
    void DeleteFile(string webPath);
}