using Shared.Contracts.Bookings;
using Shared.Contracts.Cars;

namespace CarsharingMobile.Services;

public class BookingStateService
{
    private const string DefaultTariffType = "per_minute";
    private BookingsResponse? _activeBooking;
    private CarWithInfoDto? _bookedCarDetails;
    private string _selectedTariffType = DefaultTariffType;

    public bool TryGet(out BookingsResponse? activeBooking, out CarWithInfoDto? bookedCarDetails,
        out string selectedTariffType)
    {
        activeBooking = _activeBooking;
        bookedCarDetails = _bookedCarDetails;
        selectedTariffType = _selectedTariffType;
        return _activeBooking != null && _bookedCarDetails != null;
    }

    public void Set(BookingsResponse activeBooking, CarWithInfoDto bookedCarDetails, string? selectedTariffType)
    {
        _activeBooking = activeBooking;
        _bookedCarDetails = bookedCarDetails;
        _selectedTariffType = string.IsNullOrWhiteSpace(selectedTariffType)
            ? DefaultTariffType
            : selectedTariffType;
    }

    public void Clear()
    {
        _activeBooking = null;
        _bookedCarDetails = null;
        _selectedTariffType = DefaultTariffType;
    }
}
