using System.Collections.Generic;
using System.Threading.Tasks;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories
{
    public interface IRepository<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        Task<TModel?> GetByIdAsync(long id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task AddAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(long id);
    }
}