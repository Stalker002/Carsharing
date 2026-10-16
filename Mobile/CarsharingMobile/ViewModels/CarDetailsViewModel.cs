using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Shared.Contracts.Bookings;
using Shared.Contracts.Cars;
using Shared.Enums;

namespace CarsharingMobile.ViewModels;

[QueryProperty(nameof(Car), "Car")]
public partial class CarDetailsViewModel(
    BookingService bookingService,
    BookingStateService bookingStateService,
    CarsharingMobile.Extensions.IdentityService identityService) : ObservableObject, IQueryAttributable
{
    private const string PerMinuteTariffType = "per_minute";
    private const string PerKmTariffType = "per_km";
    private const string PerDayTariffType = "per_day";

    [ObservableProperty]
    public partial CarWithMinInfoDto? Car { get; set; }

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    public partial string SelectedTariffType { get; set; } = PerMinuteTariffType;

    public bool IsPerMinuteTariffSelected => SelectedTariffType == PerMinuteTariffType;
    public bool IsPerKmTariffSelected => SelectedTariffType == PerKmTariffType;
    public bool IsPerDayTariffSelected => SelectedTariffType == PerDayTariffType;

    public string SelectedTariffTitle => SelectedTariffType switch
    {
        PerKmTariffType => "Покилометровый тариф",
        PerDayTariffType => "Посуточный тариф",
        _ => "Поминутный тариф"
    };

    partial void OnSelectedTariffTypeChanged(string value)
    {
        OnPropertyChanged(nameof(IsPerMinuteTariffSelected));
        OnPropertyChanged(nameof(IsPerKmTariffSelected));
        OnPropertyChanged(nameof(IsPerDayTariffSelected));
        OnPropertyChanged(nameof(SelectedTariffTitle));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Car", out var carObj) && carObj is CarWithMinInfoDto selectedCar)
        {
            Car = selectedCar;
            SelectedTariffType = PerMinuteTariffType;
        }
    }

    [RelayCommand]
    private void SelectTariff(string tariffType)
    {
        if (string.IsNullOrWhiteSpace(tariffType))
            return;

        SelectedTariffType = tariffType;
    }

    [RelayCommand]
    private async Task ConfirmBookingAsync()
    {
        if (Car == null)
            return;

        try
        {
            IsBusy = true;

            var (profileId, _) = await identityService.GetProfileIdAsync();
            var clientId = (int)profileId;

            var request = new BookingsRequest(
                StatusId: (int)BookingStatusEnum.Active,
                CarId: Car.Id,
                ClientId: clientId,
                StartTime: DateTime.UtcNow.AddMinutes(-1),
                EndTime: DateTime.UtcNow.AddMinutes(20)
            );

            var (bookingId, error) = await bookingService.CreateBookingAsync(request);

            if (error == null && bookingId.HasValue)
            {
                var booking = new BookingsResponse(
                    bookingId.Value,
                    (int)BookingStatusEnum.Active,
                    Car.Id,
                    clientId,
                    request.StartTime,
                    request.EndTime);

                var carDetails = new CarWithInfoDto(
                    Car.Id,
                    Car.StatusName,
                    Car.PricePerMinute,
                    Car.PricePerKm,
                    Car.PricePerDay,
                    Car.CategoryName,
                    Car.FuelType,
                    Car.Brand,
                    Car.Model,
                    Car.Transmission,
                    0,
                    Car.StateNumber ?? string.Empty,
                    Car.MaxFuel,
                    null,
                    Car.Latitude,
                    Car.Longitude,
                    Car.FuelLevel,
                    Car.ImagePath);

                bookingStateService.Set(booking, carDetails, SelectedTariffType);
                WeakReferenceMessenger.Default.Send(
                    new BookingCreatedMessage(booking, carDetails, SelectedTariffType));

                await Shell.Current.DisplayAlert(
                    "Машина забронирована!",
                    "У вас есть 20 бесплатных минут, чтобы дойти до автомобиля и осмотреть его.",
                    "Отлично");

                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка бронирования", error, "ОК");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось забронировать: {ex.Message}", "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}

public sealed class BookingCreatedMessage(BookingsResponse activeBooking, CarWithInfoDto bookedCarDetails,
    string selectedTariffType)
{
    public BookingsResponse ActiveBooking { get; } = activeBooking;
    public CarWithInfoDto BookedCarDetails { get; } = bookedCarDetails;
    public string SelectedTariffType { get; } = selectedTariffType;
}
