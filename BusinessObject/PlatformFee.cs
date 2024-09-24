using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class PlatformFee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(500)]
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<Transaction>? Transactions { get; set; }

}