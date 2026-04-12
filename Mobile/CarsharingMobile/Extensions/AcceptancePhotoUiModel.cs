namespace CarsharingMobile.Extensions;

public class AcceptancePhotoUiModel
{
    public long Id { get; set; }
    public required ImageSource ImageSource { get; set; }
    public string? Description { get; set; }
}