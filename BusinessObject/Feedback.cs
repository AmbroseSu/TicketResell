using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Feedback
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public int? Rating { get; set; }
    [StringLength(2000)]
    public string? Context { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? TicketId { get; set; }
    public int? UserId { get; set; }
    public Ticket? Ticket { get; set; }
    public User? User { get; set; }
    
}