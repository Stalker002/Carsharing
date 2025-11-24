using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class ClientsService : IClientsService
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientDocumentRepository _clientDocumentRepository;

    public ClientsService(IClientRepository clientRepository, IClientDocumentRepository clientDocumentRepository)
    {
        _clientDocumentRepository = clientDocumentRepository;
        _clientRepository = clientRepository;
    }

    public async Task<List<Client>> GetClients()
    {
        return await _clientRepository.Get();
    }

    public async Task<List<Client>> GetClientById(int id)
    {
        return await _clientRepository.GetById(id);
    }

    public async Task<List<ClientDocument>> GetClientDocuments(int clientId)
    {
        return await _clientDocumentRepository.GetByClientId(clientId);
    }

    public async Task<int> CreateClient(Client client)
    {
        return await _clientRepository.Create(client);
    }

    public async Task<int> UpdateClient(int id, int userId, string? name, string? surname, string? phoneNumber, string? email)
    {
        return await _clientRepository.Update(id, userId, name, surname, phoneNumber, email);
    }

    public async Task<int> DeleteClient(int id)
    {
        return await _clientRepository.Delete(id);
    }
}