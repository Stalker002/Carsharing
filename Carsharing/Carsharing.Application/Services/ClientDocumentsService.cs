using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class ClientDocumentsService : IClientDocumentsService
{
    private readonly IClientDocumentRepository _clientDocumentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public ClientDocumentsService(IClientDocumentRepository clientDocumentRepository, IClientRepository clientRepository, IUnitOfWork unitOfWork, IImageService imageService)
    {
        _clientRepository = clientRepository;
        _clientDocumentRepository = clientDocumentRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public async Task<List<ClientDocument>> GetClientDocuments(CancellationToken cancellationToken)
    {
        return await _clientDocumentRepository.Get(cancellationToken);
    }

    public async Task<(int? Id, string Error)> CreateClientDocumentAsync(int userId, ClientDocumentsRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        string? savedFilePath = null;

        try
        {
            if (request.File is { Length: > 0 })
            {
                savedFilePath = await _imageService.SaveDocumentImageAsync(request.File, cancellationToken);
            }
            else
            {
                return (null, "Файл документа обязателен");
            }

            var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
            var clientId = client.Select(c => c.Id).FirstOrDefault();

            if (clientId == 0)
                return (null, "Клиент не найден");

            var (document, errorDocument) = ClientDocument.Create(
                0,
                clientId,
                request.Type,
                request.LicenseCategory,
                request.Number,
                request.IssueDate,
                request.ExpiryDate,
                savedFilePath);

            if (!string.IsNullOrEmpty(errorDocument))
            {
                _imageService.DeleteFile(savedFilePath, cancellationToken);
                return (null, errorDocument);
            }

            var documentId = await _clientDocumentRepository.Create(document, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return (documentId, string.Empty);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            if (savedFilePath != null) _imageService.DeleteFile(savedFilePath, cancellationToken);

            if (ex.InnerException?.Message.Contains("23505") == true)
            {
                return (null, "Документ с таким номером уже существует.");
            }

            return (null, $"Internal error: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Error)> UpdateClientDocumentAsync(int userId, int id, ClientDocumentsRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        string? newFilePathSystem = null;

        try
        {
            var existingDoc = await _clientDocumentRepository.GetById(id, cancellationToken);
            var docEntity = existingDoc.FirstOrDefault();

            if (docEntity == null) return (false, "Document not found");

            var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
            var clientId = client.Select(c => c.Id).FirstOrDefault();

            if (clientId == 0) return (false, "Клиент не найден");
            if (docEntity.ClientId != clientId) return (false, "Document not found");

            string? filePathToUpdate = null;

            if (request.File is { Length: > 0 })
            {
                filePathToUpdate = await _imageService.SaveDocumentImageAsync(request.File, cancellationToken);
                newFilePathSystem = filePathToUpdate;
            }

            await _clientDocumentRepository.Update(
                id,
                clientId,
                request.Type,
                request.LicenseCategory,
                request.Number,
                request.IssueDate,
                request.ExpiryDate,
                filePathToUpdate,
                cancellationToken
            );

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            if (newFilePathSystem != null && !string.IsNullOrEmpty(docEntity.FilePath))
            {
                _imageService.DeleteFile(docEntity.FilePath, cancellationToken);
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            if (newFilePathSystem != null)
            {
                _imageService.DeleteFile(newFilePathSystem, cancellationToken);
            }

            return ex.InnerException?.Message.Contains("23505") == true
                ? (false, "Дубликат данных: Документ с таким номером уже существует.")
                : (false, $"Internal error: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Error)> DeleteClientDocumentAsync(int userId, int id, CancellationToken cancellationToken)
    {
        try
        {
            var documents = await _clientDocumentRepository.GetById(id, cancellationToken);
            var doc = documents.FirstOrDefault();
            if (doc == null) return (true, string.Empty);

            var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
            var clientId = client.Select(c => c.Id).FirstOrDefault();

            if (clientId == 0 || doc.ClientId != clientId) return (false, "Document not found");

            var filePath = doc.FilePath;

            await _clientDocumentRepository.Delete(id, cancellationToken);

            if (!string.IsNullOrEmpty(filePath))
            {
                _imageService.DeleteFile(filePath, cancellationToken);
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Internal error: {ex.Message}");
        }
    }
}
