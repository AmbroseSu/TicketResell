using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.enums;

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
    public DateTime? CreatedDate { get; set; } = DateTime.Now.ToUniversalTime();
    public bool IsDeleted { get; set; } = false;
    public PostStatus Status { get; set; }
    public int? TicketId { get; set; }
    public Ticket? Ticket { get; set; }
}