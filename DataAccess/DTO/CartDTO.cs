namespace DataAccess.DTO;

public class CartDTO
{
    public int Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<int>? CartItemIds { get; set; }
    public int? UserId { get; set; }
}