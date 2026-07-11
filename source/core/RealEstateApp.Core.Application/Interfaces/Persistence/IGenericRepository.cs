using System.Linq.Expressions;

namespace RealEstateApp.Core.Application.Interfaces.Persistence;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> GetByIdWithIncludeAsync(int id, List<string> properties);
    Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(int id, TEntity entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}