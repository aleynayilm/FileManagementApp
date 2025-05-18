using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync (int id);
        Task AddAsync(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}
