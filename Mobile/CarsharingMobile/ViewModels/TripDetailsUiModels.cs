namespace CarsharingMobile.ViewModels;

public sealed record TripDetailsNavigationContext(
    int TripId,
    int? BillId = null,
    string? CarTitle = null,
    string? CarImageUrl = null,
    string? RegistrationNumber = null,
    string? Transmission = null);

public sealed record TripDetailsUiModel(
    TripCarCardUiModel CarCard,
    TripOverviewUiModel Overview,
    IReadOnlyList<TripSummaryRowUiModel> SummaryRows,
    IReadOnlyList<TripPaymentUiModel> Payments);

public sealed record TripCarCardUiModel(
    string Title,
    string Transmission,
    string RegistrationNumber,
    string? ImageUrl);

public sealed record TripOverviewUiModel(
    string DistanceText,
    string DurationText,
    string AmountText);

public sealed record TripSummaryRowUiModel(
    string Title,
    string Value);

public sealed record TripPaymentUiModel(
    string Title,
    string DateText,
    string AmountText,
    string MethodText);
