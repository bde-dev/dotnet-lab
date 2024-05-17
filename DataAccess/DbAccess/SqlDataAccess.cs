using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;

namespace DataAccess.DbAccess;
public class SqlDataAccess : ISqlDataAccess
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config, ILogger logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> LoadDataAsync<T, U>(
        string pStoredProcedure,
        U pParameters,
        string pConnectionId)
    {
        pConnectionId = "MariaDB";
        using IDbConnection connection = new MySqlConnection(_config.GetConnectionString(pConnectionId));
        _logger.Information("Load Data: DB Connection details: {0}", connection.ConnectionString);
        
        return await connection.QueryAsync<T>(pStoredProcedure, pParameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task SaveData<T>(
        string pStoredProcedure,
        T pParameters,
        string pConnectionId)
    {
        pConnectionId = "MariaDB";
        using IDbConnection connection = new MySqlConnection(_config.GetConnectionString(pConnectionId));
        _logger.Information("Save Data: DB Connection details: {0}", connection);
        
        await connection.ExecuteAsync(pStoredProcedure, pParameters,
            commandType: CommandType.StoredProcedure);
    }
}