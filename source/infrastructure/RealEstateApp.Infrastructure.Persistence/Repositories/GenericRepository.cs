using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Infrastructure.Persistence.Context;

namespace RealEstateApp.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly RealEstateAppContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(RealEstateAppContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties)
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var property in properties)
        {
            query = query.Include(property);
        }
        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<TEntity?> GetByIdWithIncludeAsync(int id, List<string> properties)
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var property in properties)
        {
            query = query.Include(property);
        }

        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty is null) return null;

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public virtual async Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(int id, TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity is not null;
    }
}