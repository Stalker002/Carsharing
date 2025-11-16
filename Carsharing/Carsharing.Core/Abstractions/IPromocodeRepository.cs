using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPromocodeRepository
{
    Task<List<Promocode>> Get();
    Task<int> Create(Promocode promocode);

    Task<int> Update(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate);

    Task<int> Delete(int id);
}