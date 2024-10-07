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
    public class CategoryDAO
    {
        private readonly TicketResellDbContext _context = new TicketResellDbContext();
        private static CategoryDAO instance;
        private static object instanceLock = new object();

        private CategoryDAO()
        {
        }

        public CategoryDAO(TicketResellDbContext context)
        {
            _context = context;
        }

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }


        public async Task DeleteAsync(int categoryId)
        {
            var savedCategory = await _context.Categories.FindAsync(categoryId);
            if (savedCategory != null)
            {
                savedCategory.IsDeleted = true;
                _context.Entry<Category>(savedCategory).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category?>> Find(Expression<Func<Category,bool>> predicate)
        {
            var result = await _context.Set<Category>().Where(predicate).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task SaveAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            var savedCategory = await _context.Categories.FindAsync(category.Id);
            if (savedCategory != null)
            {
                _context.Entry<Category>(savedCategory).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

        }
    }
}
