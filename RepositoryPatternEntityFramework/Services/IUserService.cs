using RepositoryPatternEntityFramework.Models;

namespace RepositoryPatternEntityFramework.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User?> GetUserById(int id);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(User user);
}