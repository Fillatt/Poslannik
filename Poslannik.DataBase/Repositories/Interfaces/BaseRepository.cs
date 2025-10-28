using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories
{
    public abstract class BaseRepository<TEntity, TModel> : IRepository<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return entities.Select(MapToModel);
        }

        public virtual async Task AddAsync(TModel model)
        {
            var entity = MapToEntity(model);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            var entity = MapToEntity(model);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // Абстрактные методы для маппинга, которые должны быть реализованы в наследниках
        protected abstract TModel MapToModel(TEntity entity);
        protected abstract TEntity MapToEntity(TModel model);
    }
}