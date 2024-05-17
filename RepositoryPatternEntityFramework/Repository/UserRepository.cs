using RepositoryPatternEntityFramework.Models;

namespace RepositoryPatternEntityFramework.Repository;

public class UserRepository : EntityFrameworkRepository<User>, IUserRepository
{
    public UserRepository(RepositoryPatternEfContext dbContext) : base(dbContext)
    {
        
    }
}