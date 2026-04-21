using CarsharingMobile.Services;
using CarsharingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Bookings;
using Shared.Contracts.Cars;
using Shared.Contracts.Trip;
using System.Collections.ObjectModel;

namespace CarsharingMobile.ViewModels;

public partial class MainViewModel(CarService carService, BookingService bookingService, TripService tripService) : ObservableObject
{

    private const int PageSize = 50;

    private int _currentPage = 1;
    private int _totalItems;
    public ObservableCollection<CarWithMinInfoDto> Cars { get; } = [];

    [ObservableProperty] public partial bool IsBusy { get; set; }
    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial bool IsLoadingMore { get; set; }

    [ObservableProperty] public partial CarWithMinInfoDto? SelectedCar { get; set; }
    [ObservableProperty] public partial bool IsCardVisible { get; set; }

    [ObservableProperty] public partial bool IsBookingCardVisible { get; set; }
    [ObservableProperty] public partial BookingsResponse? ActiveBooking { get; set; }
    [ObservableProperty] public partial CarWithInfoDto? BookedCarDetails { get; set; }

    [RelayCommand]
    private async Task LoadInitialAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Cars.Clear();
            _currentPage = 1;
            _totalItems = 0;

            await LoadDataInternalAsync();

            ActiveBooking = await bookingService.GetMyActiveBookingAsync();

            if (ActiveBooking != null)
            {
                // Если есть бронь, скачиваем детали забронированной машины
                BookedCarDetails = await carService.GetCarDetailsAsync(ActiveBooking.CarId);

                if (BookedCarDetails != null)
                {
                    // Показываем карточку "Вы идете к машине"
                    IsBookingCardVisible = true;
                }
            }

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить авто: {ex.Message}", "ОК");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task StartTripAsync()
    {
        if (ActiveBooking == null || BookedCarDetails == null) return;

        IsBusy = true;

        // Формируем запрос на начало поездки (статус 9 - В пути)
        var request = new TripCreateRequest(
            BookingId: ActiveBooking.Id,
            StatusId: 9,
            CarId: ActiveBooking.CarId,
            StartLocation: BookedCarDetails.Location ?? "Неизвестно",
            EndLocation: "",
            InsuranceActive: true, // Включаем базовую страховку
            FuelUsed: 0,
            Refueled: 0,
            TariffType: "per_minute", // По умолчанию минутный тариф
            StartTime: DateTime.UtcNow.AddMinutes(-1), // Отнимаем минуту, чтобы сервер не ругался на "будущее"
            EndTime: null,
            Duration: 0,
            Distance: 0
        );

        var (tripId, error) = await tripService.StartTripAsync(request);

        IsBusy = false;

        if (error == null)
        {
            // Успех! Закрываем карточку брони
            IsBookingCardVisible = false;
            ActiveBooking = null;

            await Shell.Current.DisplayAlert("Поехали!", "Двери открыты. Хорошей поездки!", "ОК");

            await Shell.Current.GoToAsync(nameof(CurrentTripPage));
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", error, "ОК");
        }
    }

    [RelayCommand]
    private async Task LoadNextPageAsync()
    {
        if (IsLoadingMore || IsBusy || (Cars.Count >= _totalItems && _totalItems != 0))
            return;

        try
        {
            IsLoadingMore = true;
            await LoadDataInternalAsync();
        }
        finally
        {
            IsLoadingMore = false;
        }
    }

    private async Task LoadDataInternalAsync()
    {
        var (items, total) = await carService.GetAvailableCarsAsync(_currentPage, PageSize);
        _totalItems = total;

        if (items != null)
        {
            var tasks = items.Select(async car =>
            {
                var localPath = await carService.DownloadAndCacheImageAsync(car.ImagePath, car.Id);
                return car with { ImagePath = localPath };
            }).ToList();

            var processedCars = await Task.WhenAll(tasks);

            foreach (var car in processedCars) Cars.Add(car);
        }

        _currentPage++;
    }

    [RelayCommand]
    private void SelectCar(CarWithMinInfoDto? car)
    {
        if (car == null) return;

        SelectedCar = car;
        IsCardVisible = true;
    }

    [RelayCommand]
    private void CloseCard()
    {
        IsCardVisible = false;
        SelectedCar = null;
    }

    [RelayCommand]
    private void OpenMenu()
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    [RelayCommand]
    private async Task BookCarAsync()
    {
        if (SelectedCar == null) return;

        IsCardVisible = false;

        var navParam = new Dictionary<string, object>
        {
            { "Car", SelectedCar }
        };

        await Shell.Current.GoToAsync("CarDetailsPage", navParam);
    }
}
