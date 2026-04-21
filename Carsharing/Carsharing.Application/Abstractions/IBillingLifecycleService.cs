using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IBillingLifecycleService
{
    Task<int> EnsureBillForTripAsync(int tripId, CancellationToken cancellationToken);

    Task RecalculateBillAsync(int billId, CancellationToken cancellationToken);

    Task EnsureNoOverpaymentOnCreateAsync(Payment payment, CancellationToken cancellationToken);

    Task EnsureNoOverpaymentOnUpdateAsync(int paymentId, int targetBillId, decimal targetSum,
        CancellationToken cancellationToken);

    Task<TripDetail> NormalizeTripDetailAsync(TripDetail tripDetail, CancellationToken cancellationToken);

    Task SyncCarFuelLevelAsync(int tripId, CancellationToken cancellationToken);
}
