using Microsoft.EntityFrameworkCore;

namespace RepositoryPatternEntityFramework.Repository;

public abstract class EntityFrameworkRepository<T> : IRepository<T> where T : class
{
    private readonly RepositoryPatternEfContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public EntityFrameworkRepository(RepositoryPatternEfContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();  
    }

    public async Task<T> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public void Add(T entity)
    {
        _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}