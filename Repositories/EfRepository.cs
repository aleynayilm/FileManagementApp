using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _table;

        public EfRepository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task AddAsync(T entity)=>await _table.AddAsync(entity);

        public void Delete(T entity) => _table.Remove(entity);

        public async Task<List<T>> GetAllAsync()=>await _table.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)=>await _table.FindAsync(id);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
