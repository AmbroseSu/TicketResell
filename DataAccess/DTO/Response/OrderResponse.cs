namespace DataAccess.DTO.Response;

public class OrderResponse
{
    public int Id { get; set; }
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    public string? Address { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<OrderStatusDTO> OrderStatuses { get; set; }
    public string? TicketName { get; set; }
    public string? BuyerName { get; set; }
    
}