using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTO;

public class FeedbackDTO
{
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }
    [StringLength(1000, ErrorMessage = "FullName must be less than 1000 characters")]
    [Required(ErrorMessage = "FullName is required")]
    [RegularExpression(@"^[a-zA-Z 1-9]+$", ErrorMessage = "Context must contain only letters or number")]
    public string Context { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public List<ImageFeedbackDTO> ImageFeedbacks { get; set; }
}