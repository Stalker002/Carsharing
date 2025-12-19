using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface IClientDocumentsService
{
    Task<List<ClientDocument>> GetClientDocuments();
    Task<(int? Id, string Error)> CreateClientDocumentAsync(ClientDocumentsRequest request);
    Task<(bool IsSuccess, string Error)> UpdateClientDocumentAsync(int id, ClientDocumentsRequest request);
    Task<(bool IsSuccess, string Error)> DeleteClientDocumentAsync(int id);
}