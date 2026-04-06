using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IClientDocumentRepository
{
    Task<List<ClientDocument>> Get(CancellationToken cancellationToken);

    Task<List<ClientDocument>> GetByClientId(int clientId, CancellationToken cancellationToken);

    Task<List<ClientDocument>> GetById(int id, CancellationToken cancellationToken);

    Task<int> Create(ClientDocument document, CancellationToken cancellationToken);

    Task<int> Update(int id, int? clientId, string? type, string? licenseCategory, string? number,
        DateOnly? issueDate, DateOnly? expiryDate, string? filePath, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}