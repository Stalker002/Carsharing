using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("reviews")]
public class ReviewEntity
{
    [Column("review_id")]
    public int Id { get; set; }

    [Column("review_client_id")]
    public int ClientId { get; set; }

    [Column("review_car_id")]
    public int CarId { get; set; }

    [Column("review_rating")]
    public short Rating { get; set; }

    [Column("review_comment")]
    public string Comment { get; set; }

    [Column("review_date")]
    public DateTime Date { get; set; }

    public ClientEntity? Client { get; set; }

    public CarEntity? Car { get; set; }
}