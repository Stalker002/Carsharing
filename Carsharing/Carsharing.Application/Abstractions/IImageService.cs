using Microsoft.AspNetCore.Http;

namespace Carsharing.Application.Abstractions;

public interface IImageService
{
    Task<string> SaveCarImageAsync(IFormFile file);
    Task<string> SaveDocumentImageAsync(IFormFile file);
    void DeleteFile(string webPath);
}