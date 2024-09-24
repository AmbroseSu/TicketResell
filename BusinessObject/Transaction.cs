using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class Transaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public float? Price { get; set; }
    public DateTime? TransactionDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int? Promotion { get; set; }
    public bool Status { get; set; }

    public int? PlatformFeeId { get; set; }
    public int? UserId { get; set; }
    public PlatformFee? PlatformFee { get; set; }
    public User? User { get; set; }
}