using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Cars;
using System.Collections.ObjectModel;

namespace CarsharingMobile.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly CarService _carService;

    public MainViewModel(CarService carService)
    {
        _carService = carService;
    }

    public ObservableCollection<CarWithMinInfoDto> Cars { get; } = [];

    [ObservableProperty] public partial bool IsBusy { get; set; }
    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial bool IsLoadingMore { get; set; }

    [ObservableProperty] public partial CarWithMinInfoDto? SelectedCar { get; set; }
    [ObservableProperty] public partial bool IsCardVisible { get; set; }

    private int _currentPage = 1;
    private int _totalItems;
    private const int PageSize = 50;

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
        var (items, total) = await _carService.GetAvailableCarsAsync(_currentPage, PageSize);
        _totalItems = total;

        if (items != null)
        {
            var tasks = items.Select(async car =>
            {
                var localPath = await _carService.DownloadAndCacheImageAsync(car.ImagePath, car.Id);
                return car with { ImagePath = localPath };
            }).ToList();

            var processedCars = await Task.WhenAll(tasks);

            foreach (var car in processedCars)
            {
                Cars.Add(car);
            }
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