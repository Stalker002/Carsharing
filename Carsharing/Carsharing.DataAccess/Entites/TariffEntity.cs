namespace Carsharing.DataAccess.Entites;

public class TariffEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal PricePerMinute { get; set; }

    public decimal PricePerKm { get; set; }

    public decimal PricePerDay { get; set; }

    public ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();
}