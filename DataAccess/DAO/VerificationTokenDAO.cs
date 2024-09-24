using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class VerificationTokenDAO
{
    private static VerificationTokenDAO? instance;
    private static object instanceLock = new object();

    public static VerificationTokenDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new VerificationTokenDAO();
                }
            }
            return instance;
        }
    }
    
    public VerificationTokenDAO()
    {
    }
    
    public async Task SaveAsync(VerificationToken verificationToken)
    {
        using var context = new TicketResellDbContext();
        if (verificationToken.User == null && verificationToken.UserId != 0)
        {
            verificationToken.User = await context.Users.FindAsync(verificationToken.UserId);
        }
        await context.VerificationTokens.AddAsync(verificationToken);
        await context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int verificationTokenId)
    {
        using var context = new TicketResellDbContext();
        var verificationToken = await context.VerificationTokens.FindAsync(verificationTokenId);
        if (verificationToken != null)
        {
            context.VerificationTokens.Remove(verificationToken);
            await context.SaveChangesAsync(); 
        }
    }
    
    public async Task UpdateAsync(VerificationToken verificationToken)
    {
        using var context = new TicketResellDbContext();
        context.Entry<VerificationToken>(verificationToken).State
            = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<VerificationToken?> FindByTokenAsync(string token)
    {
        using var context = new TicketResellDbContext();
        VerificationToken? verificationToken = await context.Set<VerificationToken>()
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.Token == token);
        return verificationToken;
    }

    public async Task<VerificationToken?> FindByUserIdAsync(int userId)
    {
        using var context = new TicketResellDbContext();
        return await context.Set<VerificationToken>()
            .FirstOrDefaultAsync(v => v.User.Id == userId);
    }
}