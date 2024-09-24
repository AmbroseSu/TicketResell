namespace DataAccess.DTO;

public class FeedbackDTO
{
    public int Id { get; set; }
    public int? Rating { get; set; }
    public string? Context { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? TicketId { get; set; }
    public int? UserId { get; set; }
}