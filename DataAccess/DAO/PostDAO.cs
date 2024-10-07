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
    public class PostDAO
    {
        private readonly TicketResellDbContext _context = new TicketResellDbContext();
        private static PostDAO instance;
        private static object instanceLock = new object();

        public PostDAO(TicketResellDbContext context)
        {
            _context = context;
        }

        private PostDAO()
        {
        }

        public static PostDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PostDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task DeleteAsync(int postId)
        {
            var savedPost = await _context.Posts.FindAsync(postId);
            if (savedPost != null)
            {
                savedPost.IsDeleted = true;
                _context.Entry<Post>(savedPost).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Post?>> Find(Expression<Func<Post,bool>> predicate)
        {
            var result = await _context.Set<Post>().Where(predicate).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task SaveAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            var savedPost = await _context.Posts.FindAsync(post.Id);
            if (savedPost != null)
            {
                _context.Entry<Post>(savedPost).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

        }

    }
}
