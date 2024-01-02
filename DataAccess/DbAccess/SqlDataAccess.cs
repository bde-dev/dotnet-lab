using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DataAccess.DbAccess;
public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        this._config = config;
    }

    public async Task<IEnumerable<T>> LoadDataAsync<T, U>(
        string pStoredProcedure,
        U pParameters,
        string pConnectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(pConnectionId));

        return await connection.QueryAsync<T>(pStoredProcedure, pParameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task SaveData<T>(
        string pStoredProcedure,
        T pParameters,
        string pConnectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(pConnectionId));

        await connection.ExecuteAsync(pStoredProcedure, pParameters,
            commandType: CommandType.StoredProcedure);
    }
}