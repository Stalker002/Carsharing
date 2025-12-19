using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class ClientDocumentRepository : IClientDocumentRepository
{
    private readonly CarsharingDbContext _context;

    public ClientDocumentRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClientDocument>> Get()
    {
        var clientDocumentEntities = await _context.ClientDocument
            .AsNoTracking()
            .ToListAsync();

        var documents = clientDocumentEntities
            .Select(c => ClientDocument.Create(
                c.Id,
                c.ClientId,
                c.Type,
                c.LicenseCategory,
                c.Number,
                c.IssueDate,
                c.ExpiryDate,
                c.FilePath).clientDocument)
            .ToList();

        return documents;
    }

    public async Task<List<ClientDocument>> GetById(int id)
    {
        var clientDocumentEntities = await _context.ClientDocument
            .Where(d => d.Id == id)
            .OrderBy(cl => cl.Id)
            .AsNoTracking()
            .ToListAsync();

        var documents = clientDocumentEntities
            .Select(c => ClientDocument.Create(
                c.Id,
                c.ClientId,
                c.Type,
                c.LicenseCategory,
                c.Number,
                c.IssueDate,
                c.ExpiryDate,
                c.FilePath).clientDocument)
            .ToList();

        return documents;
    }

    public async Task<List<ClientDocument>> GetByClientId(int clientId)
    {
        var clientDocumentEntities = await _context.ClientDocument
            .Where(d => d.ClientId == clientId)
            .OrderBy(cl => cl.Id)
            .AsNoTracking()
            .ToListAsync();

        var documents = clientDocumentEntities
            .Select(c => ClientDocument.Create(
                c.Id,
                c.ClientId,
                c.Type,
                c.LicenseCategory,
                c.Number,
                c.IssueDate,
                c.ExpiryDate,
                c.FilePath).clientDocument)
            .ToList();

        return documents;
    }

    public async Task<int> GetCount()
    {
        return await _context.ClientDocument.CountAsync();
    }

    public async Task<int> Create(ClientDocument document)
    {
        var (_, error) = ClientDocument.Create(
            0,
            document.ClientId,
            document.Type,
            document.LicenseCategory,
            document.Number,
            document.IssueDate,
            document.ExpiryDate,
            document.FilePath);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception document Client: {error}");

        var clientDocumentEntities = new ClientDocumentEntity
        {
            ClientId = document.ClientId,
            Type = document.Type,
            LicenseCategory = document.LicenseCategory,
            Number = document.Number,
            IssueDate = document.IssueDate,
            ExpiryDate = document.ExpiryDate,
            FilePath = document.FilePath
        };

        await _context.ClientDocument.AddAsync(clientDocumentEntities);
        await _context.SaveChangesAsync();

        return clientDocumentEntities.Id;
    }

    public async Task<int> Update(int id, int? clientId, string? licenseCategory, string? type, string? number,
        DateOnly? issueDate, DateOnly? expiryDate, string? filePath)
    {
        var document = await _context.ClientDocument.FirstOrDefaultAsync(d => d.Id == id)
                       ?? throw new Exception("Client document not found");

        if (clientId.HasValue)
            document.ClientId = clientId.Value;

        if (!string.IsNullOrWhiteSpace(type))
            document.Type = type;

        if (!string.IsNullOrWhiteSpace(licenseCategory))
            document.LicenseCategory = licenseCategory;

        if (!string.IsNullOrWhiteSpace(number))
            document.Number = number;

        if (issueDate.HasValue)
            document.IssueDate = issueDate.Value;

        if (expiryDate.HasValue)
            document.ExpiryDate = expiryDate.Value;

        if (!string.IsNullOrWhiteSpace(filePath))
            document.FilePath = filePath;

        var (_, error) = ClientDocument.Create(
            0,
            document.ClientId,
            document.Type,
            document.LicenseCategory,
            document.Number,
            document.IssueDate,
            document.ExpiryDate,
            document.FilePath);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception document Client: {error}");

        await _context.SaveChangesAsync();

        return document.Id;
    }

    public async Task<int> Delete(int id)
    {
        var clientDocumentEntity = await _context.ClientDocument
            .Where(d => d.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}