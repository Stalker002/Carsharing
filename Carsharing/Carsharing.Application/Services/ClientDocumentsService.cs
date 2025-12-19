using System.Diagnostics;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;

namespace Carsharing.Application.Services;

public class ClientDocumentsService : IClientDocumentsService
{
    private readonly IClientDocumentRepository _clientDocumentRepository;
    private readonly CarsharingDbContext _context;
    private readonly IImageService _imageService;

    public ClientDocumentsService(IClientDocumentRepository clientDocumentRepository, CarsharingDbContext context, IImageService imageService)
    {
        _clientDocumentRepository = clientDocumentRepository;
        _context = context;
        _imageService = imageService;
    }

    public async Task<List<ClientDocument>> GetClientDocuments()
    {
        return await _clientDocumentRepository.Get();
    }

    public async Task<(int? Id, string Error)> CreateClientDocumentAsync(ClientDocumentsRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        string? savedFilePath = null;

        try
        {
            if (request.File is { Length: > 0 })
            {
                savedFilePath = await _imageService.SaveDocumentImageAsync(request.File);
            }
            else
            {
                return (null, "Файл документа обязателен");
            }

            var (document, errorDocument) = ClientDocument.Create(
                0,
                request.ClientId,
                request.Type,
                request.LicenseCategory,
                request.Number,
                request.IssueDate,
                request.ExpiryDate,
                savedFilePath);

            if (!string.IsNullOrEmpty(errorDocument))
            {
                if (savedFilePath != null) _imageService.DeleteFile(savedFilePath);
                return (null, errorDocument);
            }

            var documentId = await _clientDocumentRepository.Create(document);

            await transaction.CommitAsync();
            return (documentId, null)!;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (savedFilePath != null) _imageService.DeleteFile(savedFilePath);

            if (ex.InnerException?.Message.Contains("23505") == true)
            {
                return (null, "Документ с таким номером уже существует.");
            }

            return (null, $"Internal error: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Error)> UpdateClientDocumentAsync(int id, ClientDocumentsRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        string? newFilePathSystem = null;

        try
        {
            var existingDoc = await _clientDocumentRepository.GetById(id);
            var docEntity = existingDoc.FirstOrDefault();

            if (docEntity == null) return (false, "Document not found");

            string? filePathToUpdate = null;

            if (request.File is { Length: > 0 })
            {
                filePathToUpdate = await _imageService.SaveDocumentImageAsync(request.File);
                newFilePathSystem = filePathToUpdate;
            }

            await _clientDocumentRepository.Update(
                id,
                request.ClientId,
                request.Type,
                request.LicenseCategory,
                request.Number,
                request.IssueDate,
                request.ExpiryDate,
                filePathToUpdate
            );

            await transaction.CommitAsync();

            if (newFilePathSystem != null && !string.IsNullOrEmpty(docEntity.FilePath))
            {
                _imageService.DeleteFile(docEntity.FilePath);
            }

            return (true, null)!;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (newFilePathSystem != null)
            {
                _imageService.DeleteFile(newFilePathSystem);
            }

            return ex.InnerException?.Message.Contains("23505") == true
                ? (false, "Дубликат данных: Документ с таким номером уже существует.")
                : (false, $"Internal error: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Error)> DeleteClientDocumentAsync(int id)
    {
        try
        {
            var documents = await _clientDocumentRepository.GetById(id);
            var doc = documents.FirstOrDefault();
            if (doc == null) return (true, null)!;
            var filePath = doc.FilePath;

            await _clientDocumentRepository.Delete(id);

            if (!string.IsNullOrEmpty(filePath))
            {
                _imageService.DeleteFile(filePath);
            }

            return (true, null)!;
        }
        catch (Exception ex)
        {
            return (false, $"Internal error: {ex.Message}");
        }
    }
}