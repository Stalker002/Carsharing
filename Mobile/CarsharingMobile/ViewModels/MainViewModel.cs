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

    private int _currentPage = 1;
    private int _totalItems;
    private const int PageSize = 15;

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
        {
            return;
        }

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
        var (items, total) = await carService.GetAvailableCarsAsync(_currentPage);
        _totalItems = total;

        if (items != null)
        {
            foreach (var item in items)
            {
                Cars.Add(item);
            }
        }

        _currentPage++;
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(CarWithMinInfoDto? car)
    {
        if (car == null) return;

        var navParam = new Dictionary<string, object>
        {
            { "Car", car }
        };

        await Shell.Current.GoToAsync("CarDetailsPage", navParam);
    }
}