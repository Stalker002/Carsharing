using CarsharingMobile.ViewModels;

namespace CarsharingMobile.Views;

public partial class TripDetailsPage : ContentPage, IQueryAttributable
{
    private readonly TripDetailsViewModel _viewModel;

    public TripDetailsPage(TripDetailsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TripContext", out var context) && context is TripDetailsNavigationContext tripContext)
        {
            _viewModel.SetContext(tripContext);
            return;
        }

        if (!query.TryGetValue("TripId", out var tripIdValue))
            return;

        if (!TryParseTripId(tripIdValue, out var tripId))
            return;

        query.TryGetValue("BillId", out var billIdValue);
        query.TryGetValue("CarTitle", out var carTitleValue);
        query.TryGetValue("CarImageUrl", out var carImageUrlValue);
        query.TryGetValue("RegistrationNumber", out var registrationNumberValue);
        query.TryGetValue("Transmission", out var transmissionValue);

        var contextFromQuery = new TripDetailsNavigationContext(
            tripId,
            ParseNullableInt(billIdValue),
            carTitleValue?.ToString(),
            carImageUrlValue?.ToString(),
            registrationNumberValue?.ToString(),
            transmissionValue?.ToString());

        _viewModel.SetContext(contextFromQuery);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.HasDetails && !_viewModel.IsBusy && _viewModel.LoadCommand.CanExecute(null))
        {
            await _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }

    private static bool TryParseTripId(object value, out int tripId)
    {
        return value switch
        {
            int intValue => (tripId = intValue) > 0,
            string stringValue when int.TryParse(stringValue, out var parsedId) => (tripId = parsedId) > 0,
            _ => (tripId = 0) > 0
        };
    }

    private static int? ParseNullableInt(object? value)
    {
        return value switch
        {
            int intValue when intValue > 0 => intValue,
            string stringValue when int.TryParse(stringValue, out var parsedValue) && parsedValue > 0 => parsedValue,
            _ => null
        };
    }
}
