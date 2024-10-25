using BusinessObject;

namespace DataAccess.DAO;

public class OrderStatusDAO : IBaseDAO<OrderStatus>
{
    
    private readonly TicketResellDbContext _context = new TicketResellDbContext();
    private static OrderStatusDAO instance;
    private static object instanceLock = new object();
    
    public OrderStatusDAO(TicketResellDbContext context)
    {
        _context = context;
    }
    public OrderStatusDAO()
    {
            
    }
    
    public static OrderStatusDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new OrderStatusDAO();
                }
            }
            return instance;
        }
    }
    
    public async Task SaveAsync(OrderStatus orderStatus)
    {
        try
        {
            await _context.OrderStatuses.AddAsync(orderStatus);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(OrderStatus orderStatus)
    {
        try
        {
            OrderStatus? savedRequest = await _context.OrderStatuses.FindAsync(orderStatus.Id);
            if (savedRequest != null)
            {
                _context.Entry<OrderStatus>(orderStatus).State
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
            var savedRequest = await _context.OrderStatuses.FindAsync(id);
            if (savedRequest != null)
            {
                //savedRequest.IsDeleted = true;
                _context.Entry<OrderStatus>(savedRequest).State
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

    public async Task<OrderStatus?> FindByIdAsync(long id)
    {
        try
        {
            return await _context.OrderStatuses.FindAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}