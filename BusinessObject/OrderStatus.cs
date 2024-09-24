using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class OrderStatus
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(500)]
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    
    public int OrderId { get; set; }
    public Order? Order { get; set; }
}