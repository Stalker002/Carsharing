using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IClientRepository
{
    Task<List<Client>> Get(CancellationToken cancellationToken);

    Task<List<Client>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<List<Client>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Client>> GetClientByUserId(int userId, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<int> Create(Client client, CancellationToken cancellationToken);

    Task<int> Update(int id, int? userId, string? name, string? surname, string? phoneNumber, string? email, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}