using BusinessObject.Enums;

namespace DataAccess.DTO;

public class TicketRequestDTO
{
    public int Id { get; set; }
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    public DateTime TicketRequestDate { get; set; }
    public string? Address { get; set; }
    public TicketRequestStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? UserId { get; set; }
    public int? TicketId { get; set; }
}