namespace DataAccess.DTO;

public class OrderDTO
{
    public int Id { get; set; }
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    public string? Address { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<int>? OrderStatusIds { get; set; }
    public int? TicketId { get; set; }
    public int? UserId { get; set; }
}