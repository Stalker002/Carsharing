using System.Collections.ObjectModel;
using CarsharingMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Contracts.Cars;

namespace CarsharingMobile.ViewModels;

public partial class MainViewModel(CarService carService) : ObservableObject
{
    public ObservableCollection<CarWithMinInfoDto> Cars { get; } = [];

    [ObservableProperty] public partial bool IsBusy { get; set; }
    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial bool IsLoadingMore { get; set; }

    private const int _currentPage = 1;
    private int _totalItems;
    private const int PageSize = 15;

    [ObservableProperty]
    public partial CarWithMinInfoDto? SelectedCar { get; set; }

    [ObservableProperty]
    public partial bool IsCardVisible { get; set; } // Показывает/скрывает нижнюю карточку

    // Имитация координат для пинов (в реальном проекте надо добавить Lat/Lng в БД авто)
    public Location MapCenter { get; set; } = new Location(53.9006, 27.5590); // Центр Минска

    [RelayCommand]
    private void SelectCar(CarWithMinInfoDto car)
    {
        SelectedCar = car;
        IsCardVisible = true; // Показываем карточку снизу
    }

    [RelayCommand]
    private void CloseCard()
    {
        IsCardVisible = false; // Прячем карточку
        SelectedCar = null;
    }

    [RelayCommand]
    private void OpenMenu()
    {
        Shell.Current.FlyoutIsPresented = true; // Открываем боковую шторку
    }

    [RelayCommand]
    private async Task BookCarAsync()
    {
        if (SelectedCar != null)
        {
            // Переход на страницу бронирования с передачей машины
            var navParam = new Dictionary<string, object> { { "Car", SelectedCar } };
            await Shell.Current.GoToAsync("CarDetailsPage", navParam);
        }
    }
}