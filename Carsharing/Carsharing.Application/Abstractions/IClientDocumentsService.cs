using Carsharing.Application.DTOs;
using Carsharing.Core.Models;
using Shared.Contracts.ClientDocuments;

namespace Carsharing.Application.Abstractions;

public interface IClientDocumentsService
{
    Task<List<ClientDocument>> GetClientDocuments(CancellationToken cancellationToken);

    Task<(int? Id, string Error)> CreateClientDocumentAsync(int userId, ClientDocumentsRequest request, CancellationToken cancellationToken);

    Task<(bool IsSuccess, string Error)> UpdateClientDocumentAsync(int userId, int id, ClientDocumentsRequest request, CancellationToken cancellationToken);

    Task<(bool IsSuccess, string Error)> DeleteClientDocumentAsync(int userId, int id, CancellationToken cancellationToken);
}
