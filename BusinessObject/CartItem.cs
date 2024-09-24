using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class CartItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public int? Quantity { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? CartId { get; set; }
    public int? TicketId { get; set; }
    public Cart? Cart { get; set; }
    public Ticket? Ticket { get; set; }
}