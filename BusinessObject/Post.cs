using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class Post
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(2000)]
    public string? Title { get; set; }
    [StringLength(3000)]
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool Status { get; set; }
    public int? UserId { get; set; }
    public int? TicketId { get; set; }
    public User? User { get; set; }
    public Ticket? Ticket { get; set; }
}