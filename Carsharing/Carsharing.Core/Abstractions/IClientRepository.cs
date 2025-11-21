using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IClientRepository
{
    Task<List<Client>> Get();
    Task<List<Client>> GetById(int id);
    Task<List<Client>> GetClientByUserId(int userId);
    Task<int> GetCount();
    Task<int> Create(Client client);
    Task<int> Update(int id, int? userId, string? name, string? surname, string? phoneNumber, string? email);
    Task<int> Delete(int id);
}