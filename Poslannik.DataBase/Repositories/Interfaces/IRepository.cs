namespace Poslannik.DataBase.Repositories;

public interface IRepository<TEntity, TModel>
    where TEntity : class
    where TModel : class
{
    Task<IEnumerable<TModel>> GetAllAsync();
    Task AddAsync(TModel model);
    Task UpdateAsync(TModel model);
    Task DeleteAsync(Guid id);
}