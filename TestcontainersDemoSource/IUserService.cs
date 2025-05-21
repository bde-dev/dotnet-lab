namespace TestcontainersDemoSource;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    void AddUser(User user);
}