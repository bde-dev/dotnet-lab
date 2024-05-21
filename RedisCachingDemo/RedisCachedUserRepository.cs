using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryPatternEntityFramework.Models;
using RepositoryPatternEntityFramework.Repository;

namespace RedisCachingDemo;

public class RedisCachedUserRepository : IUserRepository
{
    private readonly IUserRepository _decorated;
    private readonly IDistributedCache _distributedCache;

    public RedisCachedUserRepository(IUserRepository decorated, IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetById(int id)
    {
        var key = $"user-{id}";

        var cachedUser = await _distributedCache.GetStringAsync(key);

        User? user;
        if (string.IsNullOrEmpty(cachedUser))
        {
            user = await _decorated.GetById(id);

            if (user == null)
            {
                return user;
            }

            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user));

            return user;
        }

        user = JsonConvert.DeserializeObject<User>(cachedUser);

        return user;
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