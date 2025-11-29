using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IClientsService
{
    Task<List<Client>> GetClients();
    Task<List<Client>> GetPagedClients(int page, int limit);
    Task<int> GetCountClients();
    Task<List<Client>> GetClientById(int id);
    Task<List<Client>> GetClientByUserId(int userId);
    Task<List<ClientDocument>> GetClientDocuments(int clientId);
    Task<int> CreateClient(Client client);
    Task<int> UpdateClient(int id, int userId, string? name, string? surname, string? phoneNumber, string? email);
    Task<int> DeleteClient(int id);
}