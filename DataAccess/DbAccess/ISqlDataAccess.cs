namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> LoadDataAsync<T, U>(string pStoredProcedure, U pParameters, string pConnectionId = "Default");
    Task SaveData<T>(string pStoredProcedure, T pParameters, string pConnectionId = "Default");
}