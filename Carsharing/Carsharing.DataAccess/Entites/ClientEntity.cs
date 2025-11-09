using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("clients")]
public class ClientEntity
{
    [Column("client_id")]
    public int Id { get; set; }
    [Column("client_user_id")]
    public int UserId { get; set; }
    [Column("client_name")]
    public string Name { get; set; } = string.Empty;
    [Column("client_surname")]
    public string Surname { get; set; } = string.Empty;
    [Column("client_phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Column("client_email")]
    public string Email { get; set; } = string.Empty;

    public UserEntity? User { get; set; }

}