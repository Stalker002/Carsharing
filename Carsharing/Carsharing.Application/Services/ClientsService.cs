using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class ClientsService : IClientsService
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientDocumentRepository _clientDocumentRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientsService(IClientRepository clientRepository, IClientDocumentRepository clientDocumentRepository,
        IUsersRepository usersRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _clientDocumentRepository = clientDocumentRepository;
        _clientRepository = clientRepository;
    }

    public async Task<List<Client>> GetClients(CancellationToken cancellationToken)
    {
        return await _clientRepository.Get(cancellationToken);
    }

    public async Task<List<Client>> GetPagedClients(int page, int limit, CancellationToken cancellationToken)
    {
        return await _clientRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetCountClients(CancellationToken cancellationToken)
    {
        return await _clientRepository.GetCount(cancellationToken);
    }

    public async Task<List<Client>> GetClientById(int id, CancellationToken cancellationToken)
    {
        return await _clientRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Client>> GetClientByUserId(int userId, CancellationToken cancellationToken)
    {
        return await _clientRepository.GetClientByUserId(userId, cancellationToken);
    }

    public async Task<List<ClientDocument>> GetMyDocuments(int userId, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _clientDocumentRepository.GetByClientId(clientId, cancellationToken);
    }

    public async Task<List<ClientDocument>> GetClientDocuments(int clientId, CancellationToken cancellationToken)
    {
        return await _clientDocumentRepository.GetByClientId(clientId, cancellationToken);
    }

    public async Task<int> CreateClient(Client client, CancellationToken cancellationToken)
    {
        return await _clientRepository.Create(client, cancellationToken);
    }

    public async Task<int> CreateClientWithUser(Client client, User user, CancellationToken cancellationToken)
    {
        var userExists = await _usersRepository.GetByLogin(user.Login, cancellationToken);
        if (userExists != null)
            throw new ConflictException($"Пользователь с таким логином уже существует");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var hashedPassword = _passwordHasher.Generate(user.Password);
            var userToCreate = User.Restore(0, user.RoleId, user.Login, hashedPassword);

            client.UserId = await _usersRepository.CreateUser(userToCreate, cancellationToken);
            var clientId = await _clientRepository.Create(client, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return clientId;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<int> UpdateClient(int id, int userId, string? name, string? surname, string? phoneNumber, string? email, CancellationToken cancellationToken)
    {
        return await _clientRepository.Update(id, userId, name, surname, phoneNumber, email, cancellationToken);
    }

    public async Task<int> UpdateOwnClient(int userId, string? name, string? surname, string? phoneNumber, string? email, CancellationToken cancellationToken)
    {
        var client = (await _clientRepository.GetClientByUserId(userId, cancellationToken)).SingleOrDefault()
            ?? throw new NotFoundException("Client not found");

        return await _clientRepository.Update(client.Id, null, name, surname, phoneNumber, email, cancellationToken);
    }

    public async Task<int> DeleteClient(int id, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetById(id, cancellationToken);
        var userId = client.Select(c => c.UserId).FirstOrDefault();

        await _usersRepository.DeleteUser(userId, cancellationToken);

        return await _clientRepository.Delete(id, cancellationToken);
    }
}
