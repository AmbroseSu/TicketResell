using BusinessObject;

namespace DataAccess.DAO;

public class AddressDAO : BaseDAO<Address>
{
    private static AddressDAO instance;
    private static object instanceLock = new object();
     
     
    public static AddressDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new AddressDAO();
                }
            }
            return instance;
        }
    }
    
    public async Task SaveAsync(Address address)
    {
        try
        {
            using var context = new TicketResellDbContext();
            await context.Addresses.AddAsync(address);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(Address address)
    {
        try
        {
            using var context = new TicketResellDbContext();
            context.Entry<Address>(address).State
                = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();
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
            using var context = new TicketResellDbContext();
            var address = await context.Addresses.FindAsync(id);
            if (address != null)
            {
                address.IsDeleted = true;
                context.Entry<Address>(address).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await context.SaveChangesAsync(); 
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<Address?> FindByIdAsync(long id)
    {
        throw new NotImplementedException();
    }
}