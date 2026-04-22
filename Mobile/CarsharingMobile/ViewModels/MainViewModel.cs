using CarsharingMobile.Services;
using CarsharingMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Shared.Contracts.Bookings;
using Shared.Contracts.Cars;
using Shared.Contracts.Trip;
using Shared.Enums;
using System.Collections.ObjectModel;

namespace CarsharingMobile.ViewModels;

public partial class MainViewModel : ObservableObject, IRecipient<BookingCreatedMessage>
{
    private const int PageSize = 50;
    private static readonly TimeSpan CarsPollingInterval = TimeSpan.FromSeconds(30);
    private const string DefaultTariffType = "per_minute";

    private readonly CarService carService;
    private readonly BookingService bookingService;
    private readonly TripService tripService;
    private readonly BookingStateService bookingStateService;
    private int _currentPage = 1;
    private int _totalItems;
    private CancellationTokenSource? _carsPollingCts;
    public ObservableCollection<CarWithMinInfoDto> Cars { get; } = [];

    [ObservableProperty] public partial bool IsBusy { get; set; }
    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial bool IsLoadingMore { get; set; }

    [ObservableProperty] public partial CarWithMinInfoDto? SelectedCar { get; set; }
    [ObservableProperty] public partial bool IsCardVisible { get; set; }

    [ObservableProperty] public partial bool IsBookingCardVisible { get; set; }
    [ObservableProperty] public partial BookingsResponse? ActiveBooking { get; set; }
    [ObservableProperty] public partial CarWithInfoDto? BookedCarDetails { get; set; }
    [ObservableProperty] public partial string SelectedTariffType { get; set; } = DefaultTariffType;

    public string SelectedTariffTitle => SelectedTariffType switch
    {
        "per_km" => "Покилометровый",
        "per_day" => "Посуточный",
        _ => "Поминутный"
    };

    public string SelectedTariffPriceText => BookedCarDetails == null
        ? "-"
        : SelectedTariffType switch
        {
            "per_km" => $"{BookedCarDetails.PricePerKm:0.00} BYN / км",
            "per_day" => $"{BookedCarDetails.PricePerDay:0.00} BYN / сутки",
            _ => $"{BookedCarDetails.PricePerMinute:0.00} BYN / мин"
        };

    partial void OnSelectedTariffTypeChanged(string value)
    {
        OnPropertyChanged(nameof(SelectedTariffTitle));
        OnPropertyChanged(nameof(SelectedTariffPriceText));
    }

    partial void OnBookedCarDetailsChanged(CarWithInfoDto? value)
    {
        OnPropertyChanged(nameof(SelectedTariffPriceText));
    }

    public MainViewModel(CarService carService, BookingService bookingService, TripService tripService,
        BookingStateService bookingStateService)
    {
        this.carService = carService;
        this.bookingService = bookingService;
        this.tripService = tripService;
        this.bookingStateService = bookingStateService;

        WeakReferenceMessenger.Default.Register(this);
    }

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
            await RefreshBookingStateAsync();
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

    public void StartCarsPolling()
    {
        if (_carsPollingCts != null)
            return;

        _carsPollingCts = new CancellationTokenSource();
        _ = RunCarsPollingAsync(_carsPollingCts.Token);
    }

    public void StopCarsPolling()
    {
        _carsPollingCts?.Cancel();
        _carsPollingCts?.Dispose();
        _carsPollingCts = null;
    }

    private async Task RunCarsPollingAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var timer = new PeriodicTimer(CarsPollingInterval);
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                await RefreshCarsAsync(cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task RefreshCarsAsync(CancellationToken cancellationToken)
    {
        if (IsBusy || IsLoadingMore)
            return;

        try
        {
            var (items, total) = await carService.GetAvailableCarsAsync(1, PageSize);
            _totalItems = total;

            if (items == null)
                return;

            var processedCars = await ProcessCarsAsync(items, cancellationToken);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                SyncCarsCollection(processedCars);
            });
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task LoadBookingStateAsync()
    {
        if (bookingStateService.TryGet(out var cachedBooking, out var cachedCarDetails, out var cachedTariffType) &&
            cachedBooking != null && cachedCarDetails != null)
        {
            ActiveBooking = cachedBooking;
            BookedCarDetails = cachedCarDetails;
            SelectedTariffType = cachedTariffType;
            IsBookingCardVisible = true;
            return;
        }

        ActiveBooking = await bookingService.GetMyActiveBookingAsync();

        if (ActiveBooking == null)
        {
            bookingStateService.Clear();
            BookedCarDetails = null;
            SelectedTariffType = DefaultTariffType;
            IsBookingCardVisible = false;
            return;
        }

        BookedCarDetails = await carService.GetCarDetailsAsync(ActiveBooking.CarId);
        SelectedTariffType = DefaultTariffType;
        IsBookingCardVisible = BookedCarDetails != null;

        if (BookedCarDetails != null)
            bookingStateService.Set(ActiveBooking, BookedCarDetails, SelectedTariffType);
    }

    public async Task RefreshBookingStateAsync()
    {
        await LoadBookingStateAsync();
    }

    public void Receive(BookingCreatedMessage message)
    {
        ActiveBooking = message.ActiveBooking;
        BookedCarDetails = message.BookedCarDetails;
        SelectedTariffType = message.SelectedTariffType;
        IsBookingCardVisible = true;
    }

    [RelayCommand]
    private async Task StartTripAsync()
    {
        if (ActiveBooking == null || BookedCarDetails == null) return;

        IsBusy = true;

        // Поездка создается в состоянии ожидания старта и становится активной
        // после первой успешной отправки геопозиции.
        var request = new TripCreateRequest(
            BookingId: ActiveBooking.Id,
            StatusId: (int)TripStatusEnum.WaitingStart,
            CarId: ActiveBooking.CarId,
            StartLocation: BookedCarDetails.Location ?? "Неизвестно",
            EndLocation: "",
            InsuranceActive: true, // Включаем базовую страховку
            FuelUsed: 0,
            Refueled: 0,
            TariffType: SelectedTariffType,
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
            bookingStateService.Clear();
            IsBookingCardVisible = false;
            ActiveBooking = null;
            BookedCarDetails = null;
            SelectedTariffType = DefaultTariffType;

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

        if (items == null)
            return;

        var processedCars = await ProcessCarsAsync(items, CancellationToken.None);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var car in processedCars)
            {
                Cars.Add(car);
            }
        });

        _currentPage++;
    }

    private async Task<IReadOnlyList<CarWithMinInfoDto>> ProcessCarsAsync(IEnumerable<CarWithMinInfoDto> items,
        CancellationToken cancellationToken)
    {
        var tasks = items.Select(async car =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var localPath = await carService.DownloadAndCacheImageAsync(car.ImagePath, car.Id);
            return car with { ImagePath = localPath };
        }).ToList();

        return await Task.WhenAll(tasks);
    }

    private void SyncCarsCollection(IReadOnlyList<CarWithMinInfoDto> updatedCars)
    {
        var selectedCarId = SelectedCar?.Id;
        var updatedById = updatedCars.ToDictionary(static car => car.Id);

        for (var index = Cars.Count - 1; index >= 0; index--)
        {
            if (!updatedById.ContainsKey(Cars[index].Id))
                Cars.RemoveAt(index);
        }

        for (var index = 0; index < updatedCars.Count; index++)
        {
            var updatedCar = updatedCars[index];
            var existingIndex = Cars.ToList().FindIndex(car => car.Id == updatedCar.Id);

            if (existingIndex < 0)
            {
                if (index <= Cars.Count)
                    Cars.Insert(index, updatedCar);
                else
                    Cars.Add(updatedCar);
                continue;
            }

            if (existingIndex != index)
                Cars.Move(existingIndex, index);

            Cars[index] = updatedCar;
        }

        if (selectedCarId.HasValue && updatedById.TryGetValue(selectedCarId.Value, out var selectedCar))
        {
            SelectedCar = selectedCar;
            IsCardVisible = true;
        }
        else if (selectedCarId.HasValue)
        {
            SelectedCar = null;
            IsCardVisible = false;
        }
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
