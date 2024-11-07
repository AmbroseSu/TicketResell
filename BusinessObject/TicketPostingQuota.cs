using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class TicketPostingQuota
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public int Quantity { get; set; }
    
    public int? TransactionId { get; set; }
    
    public virtual Transaction? Transaction { get; set; }
}