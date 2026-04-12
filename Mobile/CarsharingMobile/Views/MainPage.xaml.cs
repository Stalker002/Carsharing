using CarsharingMobile.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Shared.Contracts.Cars;

namespace CarsharingMobile.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        DrawServiceArea();

        await MoveMapToUserLocationAsync();

        if (_viewModel.Cars.Count == 0 && _viewModel.LoadInitialCommand.CanExecute(null))
        {
            await _viewModel.LoadInitialCommand.ExecuteAsync(null);
        }
    }

    private void OnCarPinClicked(object sender, Microsoft.Maui.Controls.Maps.PinClickedEventArgs e)
    {
        e.HideInfoWindow = true;

        if (sender is Pin pin)
        {
            if (pin.BindingContext is CarWithMinInfoDto clickedCar)
            {
                _viewModel.SelectCarCommand.Execute(clickedCar);
            }
        }
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        if (_viewModel.IsCardVisible)
        {
            _viewModel.CloseCardCommand.Execute(null);
        }
    }

    private void DrawServiceArea()
    {
        CarMap.MapElements.Clear();

        var minskZone = new Polygon
        {
            StrokeWidth = 4,
            StrokeColor = Color.FromArgb("#9B59B6"), 
            FillColor = Color.FromArgb("#339B59B6")
        };

        var mkadPoints = new List<Location>
        {
            new(53.9632, 27.6245),
            new(53.9610, 27.6380),
            new(53.9585, 27.6490),
            
            // --- СЕВЕРО-ВОСТОК (Выступ Уручье / Копище) ---
            new(53.9555, 27.6650),
            new(53.9560, 27.6850),
            new(53.9530, 27.7050),
            new(53.9480, 27.7200),
            new(53.9380, 27.7250),
            new(53.9300, 27.7120),
            new(53.9230, 27.6980),

            // --- ВОСТОК (Степянка / Дражня) ---
            new(53.9150, 27.6900),
            new(53.9050, 27.6860),
            new(53.8950, 27.6810),
            new(53.8850, 27.6760),
            new(53.8750, 27.6740),

            // --- ЮГО-ВОСТОК (Выступ Шабаны) ---
            new(53.8650, 27.6850),
            new(53.8580, 27.7000),
            new(53.8480, 27.7100),
            new(53.8400, 27.7000),
            new(53.8350, 27.6850),
            new(53.8380, 27.6650),

            // --- ЮГО-ВОСТОК (МКАД Чижовка) ---
            new(53.8350, 27.6520),
            new(53.8300, 27.6350),
            new(53.8260, 27.6150),
            new(53.8210, 27.5950),

            // --- ЮГ (Лошица) ---
            new(53.8180, 27.5750),
            new(53.8180, 27.5550),
            new(53.8220, 27.5350),
            new(53.8260, 27.5150),

            // --- ЮГО-ЗАПАД (Курасовщина / Малиновка) ---
            new(53.8320, 27.4950),
            new(53.8360, 27.4750),
            new(53.8400, 27.4550),
            new(53.8450, 27.4400),
            new(53.8520, 27.4300),

            // --- ЗАПАД (Сухарево / Каменная горка) ---
            new(53.8620, 27.4240),
            new(53.8720, 27.4190),
            new(53.8820, 27.4150),
            new(53.8920, 27.4120),
            new(53.9020, 27.4120),
            new(53.9120, 27.4140),
            new(53.9220, 27.4180),

            // --- СЕВЕРО-ЗАПАД (Ждановичи / Лебяжий) ---
            new(53.9320, 27.4260),
            new(53.9400, 27.4360),
            new(53.9470, 27.4500),
            new(53.9530, 27.4680),

            // --- СЕВЕР (Дрозды / Новинки / Цнянка) ---
            new(53.9580, 27.4880),
            new(53.9620, 27.5100),
            new(53.9640, 27.5300),
            new(53.9650, 27.5500),
            new(53.9650, 27.5700),
            new(53.9650, 27.5900),
            new(53.9640, 27.6100)
        };

        foreach (var point in mkadPoints)
        {
            minskZone.Geopath.Add(point);
        }

        CarMap.MapElements.Add(minskZone);
    }

    private async Task MoveMapToUserLocationAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                var location = await Geolocation.Default.GetLastKnownLocationAsync() ?? await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5)));

                if (location != null)
                {
                    CarMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка геолокации: {ex.Message}");
        }
    }

    private async void OnMyLocationClicked(object sender, EventArgs e)
    {
        await MoveMapToUserLocationAsync();
    }

    private void OnZoomInClicked(object sender, EventArgs e)
    {
        var region = CarMap.VisibleRegion;
        if (region != null)
        {
            CarMap.MoveToRegion(MapSpan.FromCenterAndRadius(region.Center, Distance.FromKilometers(region.Radius.Kilometers * 0.5)));
        }
    }

    private void OnZoomOutClicked(object sender, EventArgs e)
    {
        var region = CarMap.VisibleRegion;
        if (region != null)
        {
            CarMap.MoveToRegion(MapSpan.FromCenterAndRadius(region.Center, Distance.FromKilometers(region.Radius.Kilometers * 2)));
        }
    }
}