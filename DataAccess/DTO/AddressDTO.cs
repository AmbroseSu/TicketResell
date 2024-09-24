namespace DataAccess.DTO;

public class AddressDTO
{
    public int Id { get; set; }
    public string? AddressName { get; set; }
    public bool IsDeleted { get; set; }
    
    public int? UserId { get; set; }
}