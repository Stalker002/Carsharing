using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPromocodeStatusRepository
{
    Task<List<PromocodeStatus>> Get();
    Task<bool> Exists(int id);
}