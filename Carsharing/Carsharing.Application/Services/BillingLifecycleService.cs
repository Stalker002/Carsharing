using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Shared.Enums;

namespace Carsharing.Application.Services;

public class BillingLifecycleService(
    IBillRepository billRepository,
    IBookingRepository bookingRepository,
    ICarRepository carRepository,
    IFineRepository fineRepository,
    IPaymentRepository paymentRepository,
    IPromocodeRepository promocodeRepository,
    ISpecificationCarRepository specificationCarRepository,
    ITariffRepository tariffRepository,
    ITripDetailRepository tripDetailRepository,
    ITripRepository tripRepository)
    : IBillingLifecycleService
{
    private const decimal RefuelDiscountPerLiter = 60m;
    private const decimal InsurancePercent = 5m;

    public async Task<int> EnsureBillForTripAsync(int tripId, CancellationToken cancellationToken)
    {
        var existingBill = (await billRepository.GetByTripId([tripId], cancellationToken)).FirstOrDefault();
        if (existingBill != null)
        {
            await RecalculateBillAsync(existingBill.Id, cancellationToken);
            return existingBill.Id;
        }

        var (bill, error) = Bill.Create(
            0,
            tripId,
            null,
            (int)BillStatusEnum.Unpaid,
            DateTime.UtcNow,
            0m,
            0m);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Bill create exception: {error}");

        var billId = await billRepository.Create(bill, cancellationToken);
        await RecalculateBillAsync(billId, cancellationToken);

        return billId;
    }

    public async Task RecalculateBillAsync(int billId, CancellationToken cancellationToken)
    {
        var bill = await billRepository.GetById(billId, cancellationToken)
                   ?? throw new NotFoundException("Bill not found");

        var calculation = await BuildBillCalculationAsync(bill.TripId, bill.PromocodeId, cancellationToken);
        var payments = await paymentRepository.GetByBillId(billId, cancellationToken);
        var totalPaid = payments.Sum(static payment => payment.Sum);
        var remainingAmount = decimal.Round(Math.Max(0m, calculation.Amount - totalPaid), 2,
            MidpointRounding.AwayFromZero);

        var statusId = remainingAmount switch
        {
            0m => (int)BillStatusEnum.Paid,
            _ when remainingAmount < calculation.Amount => (int)BillStatusEnum.PartiallyPaid,
            _ => (int)BillStatusEnum.Unpaid
        };

        await billRepository.Update(
            billId,
            null,
            bill.PromocodeId,
            statusId,
            null,
            calculation.Amount,
            remainingAmount,
            cancellationToken);
    }

    public async Task EnsureNoOverpaymentOnCreateAsync(Payment payment, CancellationToken cancellationToken)
    {
        await RecalculateBillAsync(payment.BillId, cancellationToken);

        var bill = await billRepository.GetById(payment.BillId, cancellationToken)
                   ?? throw new NotFoundException("Bill not found");

        if (payment.Sum > bill.RemainingAmount.GetValueOrDefault())
            throw new ConflictException(
                $"Сумма платежа ({payment.Sum:0.00}) превышает остаток по счёту ({bill.RemainingAmount.GetValueOrDefault():0.00})");
    }

    public async Task EnsureNoOverpaymentOnUpdateAsync(int paymentId, int targetBillId, decimal targetSum,
        CancellationToken cancellationToken)
    {
        var existingPayment = (await paymentRepository.GetById(paymentId, cancellationToken)).FirstOrDefault()
                              ?? throw new NotFoundException("Payment not found");

        await RecalculateBillAsync(targetBillId, cancellationToken);

        var bill = await billRepository.GetById(targetBillId, cancellationToken)
                   ?? throw new NotFoundException("Bill not found");

        var payments = await paymentRepository.GetByBillId(targetBillId, cancellationToken);
        var currentPaidWithoutEditedPayment = payments
            .Where(payment => payment.Id != paymentId)
            .Sum(static payment => payment.Sum);

        var newTotalPaid = currentPaidWithoutEditedPayment + targetSum;
        if (newTotalPaid > bill.Amount.GetValueOrDefault())
            throw new ConflictException(
                $"Сумма платежей ({newTotalPaid:0.00}) превышает сумму счёта ({bill.Amount.GetValueOrDefault():0.00})");

        if (existingPayment.BillId != targetBillId)
            await RecalculateBillAsync(existingPayment.BillId, cancellationToken);
    }

    public async Task<TripDetail> NormalizeTripDetailAsync(TripDetail tripDetail, CancellationToken cancellationToken)
    {
        var trip = (await tripRepository.GetById(tripDetail.TripId, cancellationToken)).FirstOrDefault()
                   ?? throw new NotFoundException("Trip not found");

        var booking = (await bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");

        var car = (await carRepository.GetById(booking.CarId, cancellationToken)).FirstOrDefault()
                  ?? throw new NotFoundException("Car not found");

        var specification = (await specificationCarRepository.GetById(car.SpecificationId, cancellationToken))
            .FirstOrDefault() ?? throw new NotFoundException("Specification not found");

        var calculatedFuelUsed = tripDetail.FuelUsed.GetValueOrDefault() > 0
            ? tripDetail.FuelUsed!.Value
            : decimal.Round(specification.FuelPerKm * trip.Distance.GetValueOrDefault(), 3,
                MidpointRounding.AwayFromZero);

        var (normalizedTripDetail, error) = TripDetail.Create(
            tripDetail.Id,
            tripDetail.TripId,
            tripDetail.StartLocation,
            tripDetail.EndLocation,
            tripDetail.InsuranceActive,
            calculatedFuelUsed,
            tripDetail.Refueled);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Trip detail create exception: {error}");

        return normalizedTripDetail;
    }

    public async Task SyncCarFuelLevelAsync(int tripId, CancellationToken cancellationToken)
    {
        var tripDetail = await tripDetailRepository.GetByTripId(tripId, cancellationToken)
                         ?? throw new NotFoundException("Trip detail not found");
        var trip = (await tripRepository.GetById(tripId, cancellationToken)).FirstOrDefault()
                   ?? throw new NotFoundException("Trip not found");
        var booking = (await bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");
        var car = (await carRepository.GetById(booking.CarId, cancellationToken)).FirstOrDefault()
                  ?? throw new NotFoundException("Car not found");
        var specification = (await specificationCarRepository.GetById(car.SpecificationId, cancellationToken))
            .FirstOrDefault() ?? throw new NotFoundException("Specification not found");

        var fuelUsed = tripDetail.FuelUsed.GetValueOrDefault() > 0
            ? tripDetail.FuelUsed!.Value
            : decimal.Round(specification.FuelPerKm * trip.Distance.GetValueOrDefault(), 3,
                MidpointRounding.AwayFromZero);

        var refueled = tripDetail.Refueled.GetValueOrDefault();
        var newFuelLevel = car.FuelLevel - fuelUsed + refueled;
        newFuelLevel = Math.Min(Math.Max(0m, newFuelLevel), specification.MaxFuel);

        await carRepository.Update(
            car.Id,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            decimal.Round(newFuelLevel, 2, MidpointRounding.AwayFromZero),
            null,
            cancellationToken);
    }

    private async Task<BillCalculationSnapshot> BuildBillCalculationAsync(int tripId, int? promocodeId,
        CancellationToken cancellationToken)
    {
        var trip = (await tripRepository.GetById(tripId, cancellationToken)).FirstOrDefault()
                   ?? throw new NotFoundException("Trip not found");
        var booking = (await bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");
        var car = (await carRepository.GetById(booking.CarId, cancellationToken)).FirstOrDefault()
                  ?? throw new NotFoundException("Car not found");
        var tariff = (await tariffRepository.GetById(car.TariffId, cancellationToken)).FirstOrDefault()
                     ?? throw new NotFoundException("Tariff not found");
        var tripDetail = await tripDetailRepository.GetByTripId(tripId, cancellationToken);
        var fines = await fineRepository.GetByTripId(tripId, cancellationToken);

        var tripDurationMinutes = trip.Duration
                                  ?? decimal.Round((decimal)((trip.EndTime ?? DateTime.UtcNow) - trip.StartTime)
                                      .TotalMinutes, 2, MidpointRounding.AwayFromZero);
        if (tripDurationMinutes < 0)
            tripDurationMinutes = 0;

        var tripDistance = trip.Distance.GetValueOrDefault();
        var tripCost = trip.TariffType switch
        {
            "per_km" => tripDistance * tariff.PricePerKm,
            "per_day" => Math.Max(1m, decimal.Ceiling(tripDurationMinutes / (60m * 24m))) * tariff.PricePerDay,
            _ => tripDurationMinutes * tariff.PricePerMinute
        };

        var fineTotal = fines
            .Where(fine => fine.Amount > 0)
            .Sum(static fine => fine.Amount);

        var refuelDiscount = tripDetail?.Refueled is > 0
            ? tripDetail.Refueled.Value * RefuelDiscountPerLiter
            : 0m;
        var insuranceCost = tripDetail?.InsuranceActive == true
            ? tripCost * InsurancePercent / 100m
            : 0m;

        var discount = 0m;
        if (promocodeId.HasValue)
        {
            var promocode = (await promocodeRepository.GetById(promocodeId.Value, cancellationToken)).FirstOrDefault();
            if (promocode is
                {
                    StartDate: var startDate,
                    EndDate: var endDate
                } && startDate <= DateOnly.FromDateTime(DateTime.UtcNow)
                    && endDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                discount = promocode.Discount;
            }
        }

        var amount = decimal.Round(Math.Max(0m,
            tripCost + fineTotal + insuranceCost - (tripCost * discount / 100m) - refuelDiscount), 2,
            MidpointRounding.AwayFromZero);

        return new BillCalculationSnapshot(amount);
    }

    private sealed record BillCalculationSnapshot(decimal Amount);
}
