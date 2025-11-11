using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("categories")]
public class CategoryEntity
{
    [Column("category_id")]
    public int Id { get; set; }

    [Column("category_name")]
    public string Name { get; set; }

    public ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();
}