using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripDetailsService : ITripDetailsService
{
    private readonly ITripDetailRepository _tripDetailRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ICarRepository _carRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly ITripRepository _tripRepository;

    public TripDetailsService(ITripDetailRepository tripDetailRepository, IBookingRepository bookingRepository, ICarRepository carRepository, IInsuranceRepository insuranceRepository, ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
        _insuranceRepository = insuranceRepository;
        _carRepository = carRepository;
        _bookingRepository = bookingRepository;
        _tripDetailRepository = tripDetailRepository;
    }

    public async Task<List<TripDetail>> GetTripDetails()
    {
        return await _tripDetailRepository.Get();
    }

    public async Task<int> CreateTripDetail(TripDetail tripDetail)
    {
        var trip = await _tripRepository.GetById(tripDetail.TripId);
        var bookingId = trip.Select(b => b.BookingId).FirstOrDefault();

        var booking = await _bookingRepository.GetById(bookingId);
        if (booking == null)
            throw new Exception("Бронирование не найдено");
        var carId = booking.Select(b => b.CarId).FirstOrDefault();

        var car = await _carRepository.GetById(carId);
        if (car == null)
            throw new Exception("Автомобиль не найден");

        if (tripDetail.InsuranceActive)
        {
            var insurance = await _insuranceRepository.GetActiveByCarId(carId);

            if (insurance.Count == 0)
                throw new Exception("Нельзя начать поездку: у автомобиля нет активной страховки");
        }
        
        return await _tripDetailRepository.Create(tripDetail);
    }

    public async Task<int> UpdateTripDetail(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled)
    {
        return await _tripDetailRepository.Update(id, tripId, startLocation, endLocation, insuranceActive, fuelUsed,
            refueled);
    }

    public async Task<int> DeleteTripDetail(int id)
    {
        return await _tripDetailRepository.Delete(id);
    }
}