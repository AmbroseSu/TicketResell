using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusinessObject;

public class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    [StringLength(2000)]
    public string? Address { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsDeleted { get; set; }
    
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public List<OrderStatus>? OrderStatuses { get; set; }
    public Ticket? Ticket { get; set; }
    public User? User { get; set; }
    
}