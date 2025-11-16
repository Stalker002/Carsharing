using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class ClientsService : IClientsService
{
    private readonly IClientRepository _clientRepository;

    public ClientsService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<List<Client>> GetClients()
    {
        return await _clientRepository.Get();
    }

    public async Task<int> CreateClient(Client client)
    {
        return await _clientRepository.Create(client);
    }

    public async Task<int> UpdateClient(int id, string? name, string? surname, string? phoneNumber, string? email)
    {
        return await _clientRepository.Update(id, name, surname, phoneNumber, email);
    }

    public async Task<int> DeleteClient(int id)
    {
        return await _clientRepository.Delete(id);
    }
}