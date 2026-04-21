using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Bookings;
using Shared.Contracts.Cars;

namespace CarsharingMobile.ViewModels;

[QueryProperty(nameof(Car), "Car")]
public partial class CarDetailsViewModel(BookingService bookingService) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public partial CarWithMinInfoDto? Car { get; set; }

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Car", out var carObj) && carObj is CarWithMinInfoDto selectedCar)
        {
            Car = selectedCar;
        }
    }

    [RelayCommand]
    private async Task ConfirmBookingAsync()
    {
        if (Car == null) return;

        IsBusy = true;

        var request = new BookingsRequest(
            StatusId: 5,
            CarId: Car.Id,
            ClientId: 0,
            StartTime: DateTime.UtcNow.AddMinutes(-1),
            EndTime: DateTime.UtcNow.AddMinutes(20)
        );

        var (bookingId, error) = await bookingService.CreateBookingAsync(request);

        IsBusy = false;

        if (error == null)
        {
            await Shell.Current.DisplayAlert(
                "Машина забронирована!",
                "У вас есть 20 бесплатных минут, чтобы дойти до автомобиля и осмотреть его.",
                "Отлично");

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка бронирования", error, "ОК");
        }
    }

    [RelayCommand]
    private async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
}