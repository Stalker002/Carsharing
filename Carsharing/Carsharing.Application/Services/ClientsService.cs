using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;

namespace Carsharing.Application.Services;

public class ClientsService : IClientsService
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientDocumentRepository _clientDocumentRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly CarsharingDbContext _context;

    private ClientsService(IClientRepository clientRepository, IClientDocumentRepository clientDocumentRepository, IUsersRepository usersRepository, CarsharingDbContext context)
    {
        _context = context;
        _usersRepository = usersRepository;
        _clientDocumentRepository = clientDocumentRepository;
        _clientRepository = clientRepository;
    }

    public async Task<List<Client>> GetClients()
    {
        return await _clientRepository.Get();
    }

    public async Task<List<Client>> GetPagedClients(int page, int limit)
    {
        return await _clientRepository.GetPaged(page, limit);
    }

    public async Task<int> GetCountClients()
    {
        return await _clientRepository.GetCount();
    }

    public async Task<List<Client>> GetClientById(int id)
    {
        return await _clientRepository.GetById(id);
    }

    public async Task<List<Client>> GetClientByUserId(int userId)
    {
        return await _clientRepository.GetClientByUserId(userId);
    }

    public async Task<List<ClientDocument>> GetMyDocuments(int userId)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _clientDocumentRepository.GetByClientId(clientId);
    }

    public async Task<List<ClientDocument>> GetClientDocuments(int clientId)
    {
        return await _clientDocumentRepository.GetByClientId(clientId);
    }

    public async Task<int> CreateClient(Client client)
    {
        return await _clientRepository.Create(client);
    }

    public async Task<int> CreateClientWithUser(Client client, User user)
    {
        var userExists = await _usersRepository.GetByLogin(user.Login);
        if (userExists != null)
            throw new ConflictException($"Пользователь с таким логином уже существует");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            client.UserId = await _usersRepository.CreateUser(user);
            var clientId = await _clientRepository.Create(client);

            await transaction.CommitAsync();
            return await _clientRepository.Create(client);

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new ConflictException($"Клиент не создан");
        }
    }

    public async Task<int> UpdateClient(int id, int userId, string? name, string? surname, string? phoneNumber, string? email)
    {
        return await _clientRepository.Update(id, userId, name, surname, phoneNumber, email);
    }

    public async Task<int> DeleteClient(int id)
    {
        var client = await _clientRepository.GetById(id);
        var userId = client.Select(c => c.UserId).FirstOrDefault();

        var user = await _usersRepository.DeleteUser(userId);

        return await _clientRepository.Delete(id);
    }
}