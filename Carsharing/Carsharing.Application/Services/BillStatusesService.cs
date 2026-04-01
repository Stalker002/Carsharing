using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class BillStatusesService : IBillStatusesService
{
    private readonly IBillStatusRepository _billStatusRepository;

    public BillStatusesService(
        IBillStatusRepository billStatusRepository)
    {
        _billStatusRepository = billStatusRepository;
    }

    public async Task<List<BillStatus>> GetBillStatuses()
    {
        var billStatuses = await _billStatusRepository.Get();

        return billStatuses;
    }
}