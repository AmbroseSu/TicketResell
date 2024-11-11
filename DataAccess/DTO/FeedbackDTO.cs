using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTO;

public class FeedbackDTO
{
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int? Rating { get; set; }
    public string? Context { get; set; }
    public int? TicketId { get; set; }
    public int? UserId { get; set; }
}