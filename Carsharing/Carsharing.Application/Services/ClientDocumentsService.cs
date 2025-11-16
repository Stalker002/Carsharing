using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class ClientDocumentsService : IClientDocumentsService
{
    private readonly IClientDocumentRepository _clientDocumentRepository;

    public ClientDocumentsService(IClientDocumentRepository clientDocumentRepository)
    {
        _clientDocumentRepository = clientDocumentRepository;
    }

    public async Task<List<ClientDocument>> GetClientDocuments()
    {
        return await _clientDocumentRepository.Get();
    }

    public async Task<int> CreateClientDocument(ClientDocument clientDocument)
    {
        return await _clientDocumentRepository.Create(clientDocument);
    }

    public async Task<int> UpdateClientDocument(int id, int? clientId, string? type, string? number,
        DateOnly? issueDate, DateOnly? expiryDate, string? filePath)
    {
        return await _clientDocumentRepository.Update(id, clientId, type, number, issueDate, expiryDate, filePath);
    }

    public async Task<int> DeleteClientDocument(int id)
    {
        return await _clientDocumentRepository.Delete(id);
    }

}