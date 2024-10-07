using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BaseDAO<T> where T : class
    {
        private readonly TicketResellDbContext _context;
        private static BaseDAO<T> instance;
        private static object instanceLock = new object();

        public static BaseDAO<T> Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BaseDAO<T>();
                    }
                }
                return instance;
            }
        }

        public BaseDAO()
        {
        }

    

        public async Task SaveAsync(T T)
        {
            using var context = new TicketResellDbContext();
            context.Set<T>().Add(T);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T T)
        {
            using var context = new TicketResellDbContext();
            context.Set<T>().Update(T);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            using var context = new TicketResellDbContext();
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T?>> Find(Expression<Func<T, bool>> predicate)
        {

            using var context = new TicketResellDbContext();
            var result = await context.Set<T>().Where(predicate).ToListAsync();
            return result;
        }
    }
}
