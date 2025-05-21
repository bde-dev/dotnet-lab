using Microsoft.EntityFrameworkCore;

namespace TestcontainersDemoSource;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _dbContext.users.ToListAsync();
    }

    public void AddUser(User user)
    {
        _dbContext.users.Add(user);
        _dbContext.SaveChanges();
    }
}