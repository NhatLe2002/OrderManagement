using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Repositories
{
    public class RepoBase<T> : IRepoBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public RepoBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public async Task<bool> AddRangeAsync(ICollection<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return true;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, int entry = 1, int page = 1, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                        string includeProperties = "")
        {
            IQueryable<T> query = _dbSet.Where(expression);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.Skip((page - 1) * entry).Take(entry).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> IsAny(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            else { return false; }
        }
        public async Task<bool> RemoveEntityAsync(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }
        public async Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return true;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }
    }
}
