namespace DataAccess.DTO;

public class CartItemDTO
{
    public int Id { get; set; }
    public int? Quantity { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? CartId { get; set; }
    public int? TicketId { get; set; }
}