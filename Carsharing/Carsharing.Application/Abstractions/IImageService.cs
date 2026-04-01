using Microsoft.AspNetCore.Http;

namespace Carsharing.Application.Abstractions;

public interface IImageService
{
    Task<string> SaveCarImageAsync(IFormFile file, CancellationToken cancellationToken);

    Task<string> SaveDocumentImageAsync(IFormFile file, CancellationToken cancellationToken);

    void DeleteFile(string webPath, CancellationToken cancellationToken);
}