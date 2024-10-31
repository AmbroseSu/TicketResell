using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.enums;

namespace BusinessObject;

public class Notification
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public String? Title { get; set; }
    public String? Content { get; set; }
    public int? TicketRequestId { get; set; }
    public DateTime? DateCreated { get; set; }
    public NotificationType Type { get; set; }
    public bool Status { get; set; }
    public bool IsClick { get; set; }
    
    public int? UserReceivedId { get; set; }
    
    public User? User { get; set; }
}