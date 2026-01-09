using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITripStatusRepository
{
    Task<List<TripStatus>> Get();
    Task<bool> Exists(int id);
}