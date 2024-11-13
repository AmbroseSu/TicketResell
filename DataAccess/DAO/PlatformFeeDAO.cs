using BusinessObject;

namespace DataAccess.DAO;

public class PlatformFeeDAO
{
     private readonly TicketResellDbContext _context = new TicketResellDbContext();
    private static PlatformFeeDAO instance;
    private static object instanceLock = new object();
    
    public PlatformFeeDAO(TicketResellDbContext context)
    {
        _context = context;
    }
    public PlatformFeeDAO()
    {
            
    }
    
    public static PlatformFeeDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new PlatformFeeDAO();
                }
            }
            return instance;
        }
    }
    
    public async Task SaveAsync(PlatformFee PlatformFee)
    {
        try
        {
            await _context.PlatformFees.AddAsync(PlatformFee);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(PlatformFee PlatformFee)
    {
        try
        {
            PlatformFee? savedRequest = await _context.PlatformFees.FindAsync(PlatformFee.Id);
            if (savedRequest != null)
            {
                _context.Entry<PlatformFee>(PlatformFee).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        try
        {
            var savedRequest = await _context.PlatformFees.FindAsync(id);
            if (savedRequest != null)
            {
                //savedRequest.IsDeleted = true;
                _context.Entry<PlatformFee>(savedRequest).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PlatformFee?> FindByIdAsync(long id)
    {
        try
        {
            return await _context.PlatformFees.FindAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}