using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IBillStatusesService
{
    Task<List<BillStatus>> GetBillStatuses();
}