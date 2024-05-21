using Microsoft.Extensions.Caching.Memory;
using RepositoryPatternEntityFramework.Models;
using RepositoryPatternEntityFramework.Repository;

namespace CachedDecoratorPattern;

public class CachedUserRepository : IUserRepository
{
    private readonly UserRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedUserRepository(UserRepository decorated, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetById(int id)
    {
        var key = $"user-{id}";

        return await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                return _decorated.GetById(id);
            });
    }

    public void Add(User entity)
    {
        throw new NotImplementedException();
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }
}