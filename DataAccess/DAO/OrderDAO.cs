using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO;

public class OrderDAO : IBaseDAO<Order>
{
    
    private readonly TicketResellDbContext _context = new TicketResellDbContext();
    private static OrderDAO instance;
    private static object instanceLock = new object();
    
    public OrderDAO(TicketResellDbContext context)
    {
        _context = context;
    }
    public OrderDAO()
    {
            
    }
    
    public static OrderDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new OrderDAO();
                }
            }
            return instance;
        }
    }
    
    public async Task SaveAsync(Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(Order order)
    {
        try
        {
            Order? savedRequest = await _context.Orders.FindAsync(order.Id);
            if (savedRequest != null)
            {
                _context.Entry<Order>(order).State
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
            var savedRequest = await _context.Orders.FindAsync(id);
            if (savedRequest != null)
            {
                savedRequest.IsDeleted = true;
                _context.Entry<Order>(savedRequest).State
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

    public async Task<Order?> FindByIdAsync(long id)
    {
        try
        {
            return await _context.Orders.FindAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Order>> GetAllOrdersByUserId(long userId)
    {
        try
        {
            return await _context.Orders.Include(o => o.OrderStatuses).Where(o => o.UserId == userId).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}