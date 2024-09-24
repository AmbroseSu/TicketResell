using BusinessObject.Enums;

namespace DataAccess.DTO;

public class MessageDTO
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime SendAt { get; set; }
    public MessageStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? ChatId { get; set; }
    public int? UserId { get; set; }
}