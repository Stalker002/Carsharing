using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IClientsService
{
    Task<List<Client>> GetClients();
    Task<int> CreateClient(Client client);
    Task<int> UpdateClient(int id, string? name, string? surname, string? phoneNumber, string? email);
    Task<int> DeleteClient(int id);
}