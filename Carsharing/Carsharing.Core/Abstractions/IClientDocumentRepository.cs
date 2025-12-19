using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IClientDocumentRepository
{
    Task<List<ClientDocument>> Get();
    Task<List<ClientDocument>> GetByClientId(int clientId);
    Task<List<ClientDocument>> GetById(int id);
    Task<int> Create(ClientDocument document);

    Task<int> Update(int id, int? clientId, string? type, string? licenseCategory, string? number,
        DateOnly? issueDate, DateOnly? expiryDate, string? filePath);

    Task<int> Delete(int id);
}