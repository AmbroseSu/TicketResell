namespace DataAccess.DTO;

public class ImageTicketDTO
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? TicketId { get; set; }
}