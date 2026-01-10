namespace Carsharing.DataAccess.Entites;

public class InsuranceStatusEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<InsuranceEntity> Insurances { get; set; } = new List<InsuranceEntity>();
}