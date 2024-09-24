using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Address
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(1000)]
    public string? AddressName { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? UserId { get; set; }
    public User? User { get; set; }
}