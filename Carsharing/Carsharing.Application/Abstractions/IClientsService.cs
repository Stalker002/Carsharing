using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IClientsService
{
    Task<List<Client>> GetClients(CancellationToken cancellationToken);

    Task<List<Client>> GetPagedClients(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountClients(CancellationToken cancellationToken);

    Task<List<Client>> GetClientById(int id, CancellationToken cancellationToken);

    Task<List<Client>> GetClientByUserId(int userId, CancellationToken cancellationToken);

    Task<List<ClientDocument>> GetMyDocuments(int userId, CancellationToken cancellationToken);

    Task<List<ClientDocument>> GetClientDocuments(int clientId, CancellationToken cancellationToken);

    Task<int> CreateClient(Client client, CancellationToken cancellationToken);

    Task<int> CreateClientWithUser(Client client, User user, CancellationToken cancellationToken);

    Task<int> UpdateClient(int id, int userId, string? name, string? surname, string? phoneNumber, string? email, CancellationToken cancellationToken);

    Task<int> DeleteClient(int id, CancellationToken cancellationToken);
}