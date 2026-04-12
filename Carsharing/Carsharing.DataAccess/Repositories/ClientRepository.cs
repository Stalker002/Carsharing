using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly CarsharingDbContext _context;

    public ClientRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> Get(CancellationToken cancellationToken)
    {
        var clientEntities = await _context.Client
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var clients = clientEntities
            .Select(c => Client.Create(
                c.Id,
                c.UserId,
                c.Name,
                c.Surname,
                c.PhoneNumber,
                c.Email
            ).client)
            .ToList();

        return clients;
    }

    public async Task<List<Client>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var clientEntities = await _context.Client
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .OrderBy(cl => cl.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var clients = clientEntities
            .Select(c => Client.Create(
                c.Id,
                c.UserId,
                c.Name,
                c.Surname,
                c.PhoneNumber,
                c.Email
            ).client)
            .ToList();

        return clients;
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _context.Client.CountAsync(cancellationToken);
    }

    public async Task<List<Client>> GetById(int id, CancellationToken cancellationToken)
    {
        var clientEntities = await _context.Client
            .Where(c => c.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var clients = clientEntities
            .Select(c => Client.Create(
                c.Id,
                c.UserId,
                c.Name,
                c.Surname,
                c.PhoneNumber,
                c.Email
            ).client)
            .ToList();

        return clients;
    }

    public async Task<List<Client>> GetClientByUserId(int userId, CancellationToken cancellationToken)
    {
        var clientEntities = await _context.Client
            .Where(c => c.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var clients = clientEntities
            .Select(c => Client.Create(
                c.Id,
                c.UserId,
                c.Name,
                c.Surname,
                c.PhoneNumber,
                c.Email
            ).client)
            .ToList();

        return clients;
    }

    public async Task<int> Create(Client client, CancellationToken cancellationToken)
    {
        var (_, error) = Client.Create(
            0,
            client.UserId,
            client.Name,
            client.Surname,
            client.PhoneNumber,
            client.Email);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception Client: {error}");

        var clientEntities = new ClientEntity
        {
            UserId = client.UserId,
            Name = client.Name,
            Surname = client.Surname,
            PhoneNumber = client.PhoneNumber,
            Email = client.Email
        };

        await _context.Client.AddAsync(clientEntities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return clientEntities.Id;
    }

    public async Task<int> Update(int id, int? userId, string? name, string? surname,
        string? phoneNumber, string? email, CancellationToken cancellationToken)
    {
        var client = await _context.Client.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken)
                     ?? throw new Exception("Client not found");

        if (userId.HasValue)
            client.UserId = userId.Value;

        if (!string.IsNullOrWhiteSpace(name))
            client.Name = name;

        if (!string.IsNullOrWhiteSpace(surname))
            client.Surname = surname;

        if (!string.IsNullOrWhiteSpace(phoneNumber))
            client.PhoneNumber = phoneNumber;

        if (!string.IsNullOrWhiteSpace(email))
            client.Email = email;

        var (_, error) = Client.Create(
            0,
            client.UserId,
            client.Name,
            client.Surname,
            client.PhoneNumber,
            client.Email);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception Client: {error}");

        await _context.SaveChangesAsync(cancellationToken);

        return client.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        var clientEntity = await _context.Client
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return id;
    }
}
