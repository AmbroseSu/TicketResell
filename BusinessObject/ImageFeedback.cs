using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class ImageFeedback
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    public int? FeedbackId { get; set; }
    public Feedback? Feedback { get; set; }
}