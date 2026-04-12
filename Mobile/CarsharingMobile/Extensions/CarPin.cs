using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;

namespace CarsharingMobile.Extensions;

public class CarPin : Pin
{
    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(CarPin), null);

    public static readonly BindableProperty LatitudeProperty =
        BindableProperty.Create(nameof(Latitude), typeof(double?), typeof(CarPin), null, propertyChanged: OnCoordinateChanged);

    public static readonly BindableProperty LongitudeProperty =
        BindableProperty.Create(nameof(Longitude), typeof(double?), typeof(CarPin), null, propertyChanged: OnCoordinateChanged);

    public string? ImageUrl
    {
        get => (string?)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public double? Latitude
    {
        get => (double?)GetValue(LatitudeProperty);
        set => SetValue(LatitudeProperty, value);
    }

    public double? Longitude
    {
        get => (double?)GetValue(LongitudeProperty);
        set => SetValue(LongitudeProperty, value);
    }

    private static void OnCoordinateChanged(BindableObject bindable, object? _, object? __)
    {
        if (bindable is not CarPin pin || pin.Latitude is null || pin.Longitude is null)
            return;

        pin.Location = new Location(pin.Latitude.Value, pin.Longitude.Value);
    }
}
