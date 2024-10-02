using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public class VerificationToken
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public string? Token { get; set; }
    public DateTime ExpirationTime { get; set; }
    
    private const int EXPIRATION_TIME_MINUTES = 1;
    
    public int UserId { get; set; }
    public virtual User? User { get; set; } 
    
    public VerificationToken()
    {
        // Default constructor
    }
    
    public VerificationToken(string token, int userId)
    {
        this.Token = token;
        this.UserId = userId;
        this.ExpirationTime = GetTokenExpirationTime();
    }
    
    private DateTime GetTokenExpirationTime()
    {
        DateTime now = DateTime.UtcNow; // Sử dụng UtcNow thay vì Now
        DateTime expirationTime = now.AddMinutes(EXPIRATION_TIME_MINUTES);
        return expirationTime;
    }
}