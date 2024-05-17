using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data;
public class UserData : IUserData
{
    private readonly ISqlDataAccess _db;

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<UserModel>> GetUsers() =>
        _db.LoadDataAsync<UserModel, dynamic>("spUser_GetAll", new { });

    public async Task<UserModel?> GetUser(int id)
    {
        var results = await _db.LoadDataAsync<UserModel, dynamic>(
            "spUser_Get",
            new { p_Id = id });

        return results.FirstOrDefault();
    }

    public Task InsertUser(UserModel user) =>
        _db.SaveData("spUser_Insert", new { p_FirstName = user.FirstName, p_LastName = user.LastName });

    public Task UpdateUser(UserModel user) =>
        _db.SaveData("spUser_Update", new { p_Id = user.Id, p_FirstName = user.FirstName, p_LastName = user.LastName });

    public Task DeleteUser(int id) =>
        _db.SaveData("spUser_Delete", new { p_Id = id });
}