using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class Ticket
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(500)]
    public string? Name { get; set; }
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    public DateTime? ExpirationDate { get; set; }
    [StringLength(1000)]
    public string? Venue { get; set; }
    public TicketStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    
    //public int? TicketRequestId { get; set; }
    public int? CategoryId { get; set; }
    public List<Feedback>? Feedbacks { get; set; }
    public List<CartItem>? CartItems { get; set; }
    public List<Post>? Posts { get; set; }
    public List<ImageTicket>? ImageTickets { get; set; }
    public List<Order>? Orders { get; set; }
    public List<TicketRequest>? TicketRequest { get; set; }
    public Category? Category { get; set; }
    
    
}