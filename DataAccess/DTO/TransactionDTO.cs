using BusinessObject;
using BusinessObject.enums;
using BusinessObject.Enums;

namespace DataAccess.DTO;

public class TransactionDTO
{
    public int Id { get; set; }
    public float? Price { get; set; }
    public DateTime TransactionDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int? Promotion { get; set; }
    public TransactionStatus Status { get; set; }
    
    public PlatformFeeDTO? PlatformFeeDto { get; set; }
    public int? PlatformFeeId { get; set; }
    public int? TicketPostingQuotaId { get; set; }

    public int Quantity { get; set; }
    // public  UserId { get; set; }
}