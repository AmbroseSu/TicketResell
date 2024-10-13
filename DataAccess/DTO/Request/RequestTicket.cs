namespace DataAccess.DTO.Request;

public class RequestTicket
{
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    public string? Address { get; set; }
    
    public int? UserId { get; set; }
    public int? TicketId { get; set; }
}