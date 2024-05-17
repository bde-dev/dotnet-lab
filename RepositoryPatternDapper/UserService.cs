namespace RepositoryPatternDapper;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<User>> GetAllUsers() =>
        _userRepository.GetAll("spUser_GetAll");

    public async Task<User?> GetUserById(int id)
    {
        var results = await _userRepository.GetById("spUser_Get", new {p_Id = id});

        return results.FirstOrDefault();
    }

    public Task CreateUser(User user) =>
        _userRepository.Add("spUser_Insert", new { p_FirstName = user.FirstName, p_LastName = user.LastName });

    public Task UpdateUser(User user) =>
        _userRepository.Update("spUser_Update", new { p_Id = user.Id, p_FirstName = user.FirstName, p_LastName = user.LastName });

    public Task DeleteUser(int id) =>
        _userRepository.Delete("spUser_Delete", new {p_Id = id});
}