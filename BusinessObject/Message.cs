using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;

namespace BusinessObject;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [StringLength(3000)]
    public string? Content { get; set; }
    public DateTime SendAt { get; set; }
    public MessageStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? ChatId { get; set; }
    public int? UserId { get; set; }
    public Chat? Chat { get; set; }
    public User? User { get; set; }
    
    
}