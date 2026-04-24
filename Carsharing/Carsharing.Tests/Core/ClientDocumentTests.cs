using Carsharing.Core.Models;

namespace Carsharing.Tests.Core;

public class ClientDocumentTests
{
    [Fact]
    public void Create_AcceptsDriverLicenseAlias_AndNormalizesType()
    {
        var (document, error) = ClientDocument.Create(
            1,
            1,
            "Водительское удостоверение",
            "B",
            "AB1234567",
            DateOnly.FromDateTime(DateTime.Today.AddYears(-1)),
            DateOnly.FromDateTime(DateTime.Today.AddYears(5)),
            "docs/license.png");

        Assert.Equal(string.Empty, error);
        Assert.NotNull(document);
        Assert.Equal("Водительские права", document.Type);
    }
}
