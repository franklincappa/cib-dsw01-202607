using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VentasAPI.Data;
using VentasAPI.Repositories.Interfaces;

namespace VentasAPI.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return false;
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(object id) => await _dbSet.FindAsync(id) is not null;

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();
}
