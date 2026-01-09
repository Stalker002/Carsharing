using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICarStatusRepository
{
    Task<List<CarStatus>> Get();
    Task<List<CarStatus>> GetById(int id);
    Task<bool> Exists(int id);
}