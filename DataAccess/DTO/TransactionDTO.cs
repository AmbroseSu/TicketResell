using BusinessObject.Enums;

namespace DataAccess.DTO;

public class TransactionDTO
{
    public int Id { get; set; }
    public float? Price { get; set; }
    public DateTime TransactionDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int? Promotion { get; set; }
    public bool Status { get; set; }
    
    public int? PlatformFeeId { get; set; }
    public int? UserId { get; set; }
}