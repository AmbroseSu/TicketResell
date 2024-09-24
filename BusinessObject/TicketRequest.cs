using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class TicketRequest
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    public DateTime? TicketRequestDate { get; set; }
    [StringLength(2000)]
    public string? Address { get; set; }
    public TicketRequestStatus Status { get; set; }
    public bool IdDeleted { get; set; }
    
    
    public int? UserId { get; set; }
    public int? TicketId { get; set; }
    public User? User { get; set; }
    public Ticket? Ticket { get; set; }
    
    
}