using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("tariffs")]
public class TariffEntity
{
    [Column("tariff_id")]
    public int Id { get; set; }

    [Column("tariff_name")]
    public string Name { get; set; }

    [Column("tariff_price_per_minute")]
    public static decimal PricePerMinute { get; set; }

    [Column("tariff_price_per_km")]
    public static decimal PricePerKm { get; set; }

    [Column("tariff_price_per_day")]
    public static decimal PricePerDay { get; set; }

    public ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();
}