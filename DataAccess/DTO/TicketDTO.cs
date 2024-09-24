using BusinessObject.Enums;

namespace DataAccess.DTO;

public class TicketDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public float? Price { get; set; }
    public int? Quantity { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string? Venue { get; set; }
    public TicketStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<int>? FeedBackIds { get; set; }
    public List<int>? CartItemIds { get; set; }
    public List<int>? PostIds { get; set; }
    public List<int>? ImageTicketIds { get; set; }
    public List<int>? OrderIds { get; set; }
    public int? TicketRequestId { get; set; }
    public int? CategoryId { get; set; }
}