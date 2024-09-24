using BusinessObject;
using BusinessObject.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class UserDAO
{
    
     private static UserDAO instance;
     private static object instanceLock = new object();
     
     
     public static UserDAO Instance
     {
         get
         {
             lock (instanceLock)
             {
                 if (instance == null)
                 {
                     instance = new UserDAO();
                 }
             }
             return instance;
         }
     }
     

    public UserDAO()
    {
        
    }
    
    // Save (Thêm mới một User)
    public async Task SaveAsync(User user)
    {
        using var context = new TicketResellDbContext();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    // Delete (Xóa một User)
    public async Task DeleteAsync(long userId)
    {
        using var context = new TicketResellDbContext();
        var user = await context.Users.FindAsync(userId);
        if (user != null)
        {
            user.IsDeleted = true;
            context.Entry<User>(user).State
                = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync(); 
        }
    }
    
    public async Task UpdateAsync(User user)
    {
        using var context = new TicketResellDbContext();
        context.Entry<User>(user).State
            = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<User?> FindByLoginAsync(string email)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .Where(u => EF.Functions.Like(u.Email,email))
            .FirstOrDefaultAsync();
    }

    public async Task<User?> FindByPhoneAsync(string phone)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .Where(u => u.PhoneNumber == phone)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email) != null;
    }

    public async Task<bool> ExistsByPhoneAsync(string phone)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .AnyAsync(u => u.PhoneNumber == phone);
    }

    public async Task<User?> FindByRoleAsync(Role role)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .Where(u => u.Role == role)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .Where(u => EF.Functions.Like(u.Email, email))
            .FirstOrDefaultAsync();
    }

    public async Task<User?> FindUserByIdAsync(long id)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> FindUserByPhoneAsync(string phone)
    {
        using var context = new TicketResellDbContext();
        return await context.Users
            .Where(u => EF.Functions.Like(u.PhoneNumber, phone))
            .FirstOrDefaultAsync();
    }
}