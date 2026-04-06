using Carsharing.Core.Models;
using Shared.Enums;

namespace Carsharing.Tests.Core;

public class PromocodeTests
{
    [Fact]
    public void Create_ValidData_ReturnsPromocode()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var tomorrow = today.AddDays(1);

        var (promo, error) = Promocode.Create(1, (int)PromocodeStatusEnum.Active, "SUMMER20", 20m, today, tomorrow);

        Assert.NotNull(promo);
        Assert.Equal(string.Empty, error);
        Assert.Equal("SUMMER20", promo.Code);
    }

    [Fact]
    public void Create_NegativeDiscount_ReturnsError()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var tomorrow = today.AddDays(1);

        var (promo, error) = Promocode.Create(1, (int)PromocodeStatusEnum.Active, "SUMMER20", -5m, today, tomorrow);

        Assert.Null(promo);
        Assert.Equal("Discount must be positive", error);
    }

    [Fact]
    public void Create_StartDateAfterEndDate_ReturnsError()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        var (promo, error) = Promocode.Create(1, (int)PromocodeStatusEnum.Active, "TEST", 10m, today, yesterday);

        Assert.Null(promo);
        Assert.Equal("Start date can not exceed end date ", error);
    }
}