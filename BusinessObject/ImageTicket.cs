using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class ImageTicket
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(2000)]
    public string? ImageUrl { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? TicketId { get; set; }
    public Ticket? Ticket { get; set; }
}