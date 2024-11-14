using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.enums;
using BusinessObject.Enums;

namespace BusinessObject;

public class Transaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public float? Price { get; set; }
    public long? OrderCode {get; set;}
    public DateTime? TransactionDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int? Promotion { get; set; }
    public TransactionStatus Status { get; set; }
    public int? Number {get; set;}

    public int? PlatformFeeId { get; set; }
    public int? UserId { get; set; }
    public int? TicketPostingQuotaId { get; set; }
    public PlatformFee? PlatformFee { get; set; }
    public User? User { get; set; }
    public TicketPostingQuota? TicketPostingQuota { get; set; }
}