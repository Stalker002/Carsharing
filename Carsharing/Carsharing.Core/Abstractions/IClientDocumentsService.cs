using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IClientDocumentsService
{
    Task<List<ClientDocument>> GetClientDocuments();
    Task<int> CreateClientDocument(ClientDocument clientDocument);

    Task<int> UpdateClientDocument(int id, int? clientId, string? type, string? number,
        DateOnly? issueDate, DateOnly? expiryDate, string? filePath);

    Task<int> DeleteClientDocument(int id);
}