using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IFineStatusRepository
{
    Task<List<FineStatus>> Get();
    Task<bool> Exists(int id);
}