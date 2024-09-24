namespace DataAccess.DTO;

public class PostDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool Status { get; set; }
    
    public int? UserId { get; set; }
    public int? TicketId { get; set; }
}