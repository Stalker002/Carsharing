using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBillStatusRepository
{
    Task<List<BillStatus>> Get();
    Task<bool> Exists(int id);
}