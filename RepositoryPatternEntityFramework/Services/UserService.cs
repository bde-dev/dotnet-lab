using RepositoryPatternEntityFramework.Models;
using RepositoryPatternEntityFramework.Repository;

namespace RepositoryPatternEntityFramework.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<User>> GetAllUsers() => _userRepository.GetAll();

    public async Task<User?> GetUserById(int id) => await _userRepository.GetById(id);

    public void CreateUser(User user) => _userRepository.Add(user);

    public void UpdateUser(User user) => _userRepository.Update(user);

    public void DeleteUser(User user) => _userRepository.Delete(user);
}