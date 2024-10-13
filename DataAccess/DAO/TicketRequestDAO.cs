using BusinessObject;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class TicketRequestDAO : IBaseDAO<TicketRequest>
{
    
    private readonly TicketResellDbContext _context = new TicketResellDbContext();
    private static TicketRequestDAO instance;
    private static object instanceLock = new object();
    
    public TicketRequestDAO(TicketResellDbContext context)
    {
        _context = context;
    }
    public TicketRequestDAO()
    {
            
    }
    
    public static TicketRequestDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new TicketRequestDAO();
                }
            }
            return instance;
        }
    }
    
    public async Task SaveAsync(TicketRequest ticketRequest)
    {
        try
        {
            await _context.TicketRequests.AddAsync(ticketRequest);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(TicketRequest ticketRequest)
    {
        try
        {
            TicketRequest? savedRequest = await _context.TicketRequests.FindAsync(ticketRequest.Id);
            if (savedRequest != null)
            {
                _context.Entry<TicketRequest>(ticketRequest).State
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
    

    public async Task DeleteAsync(long requestId)
    {
        try
        {
            var savedRequest = await _context.TicketRequests.FindAsync(requestId);
            if (savedRequest != null)
            {
                savedRequest.IdDeleted = true;
                _context.Entry<TicketRequest>(savedRequest).State
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

    public async Task<TicketRequest?> FindByIdAsync(long id)
    {
        try
        {
            return await _context.TicketRequests.FindAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<TicketRequest>> FindAllTicketRequestsByTicketIdAsync(int ticketId)
    {
        try
        {
            return await _context.TicketRequests.Where(x => x.TicketId == ticketId).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}