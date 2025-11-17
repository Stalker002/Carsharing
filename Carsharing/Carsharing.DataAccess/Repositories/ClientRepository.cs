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

    public async Task<List<Client>> Get()
    {
        var clientEntities = await _context.Client
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Client>> GetClientByUserId(int userId)
    {
        var clientEntities = await _context.Client
            .Where(c => c.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<int> Create(Client client)
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

        await _context.Client.AddAsync(clientEntities);
        await _context.SaveChangesAsync();

        return clientEntities.Id;
    }

    public async Task<int> Update(int id, int? userId, string? name, string? surname,
        string? phoneNumber, string? email)
    {
        var client = await _context.Client.FirstOrDefaultAsync(c => c.Id == id)
                     ?? throw new Exception("Client not found");

        if(userId.HasValue)
            client.UserId = userId.Value;
        
        if (!string.IsNullOrWhiteSpace(name))
            client.Name = name;

        if (!string.IsNullOrWhiteSpace(surname))
            client.Surname = surname;

        if (!string.IsNullOrWhiteSpace(phoneNumber))
            client.PhoneNumber = phoneNumber;

        if (!string.IsNullOrWhiteSpace(email))
            client.Email = email;

        await _context.SaveChangesAsync();

        return client.Id;
    }

    public async Task<int> Delete(int id)
    {
        var clientEntity = await _context.Client
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}