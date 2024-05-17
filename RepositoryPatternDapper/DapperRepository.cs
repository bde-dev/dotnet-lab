using System.Data;
using Dapper;

namespace RepositoryPatternDapper;

public class DapperRepository<T> : IRepository<T> where T : class
{
    private readonly ILogger<T> _logger;
    private readonly IDbConnection _dbConnection;

    public DapperRepository(ILogger<T> logger, IDbConnection dbConnection)
    {
        _logger = logger;
        _dbConnection = dbConnection;
    }
    
    public async Task<IEnumerable<T>> GetAll(string pStoredProcedure)
    {
        _logger.LogInformation("Database Connection details: {0}", _dbConnection.ConnectionString);

        return await _dbConnection.QueryAsync<T>(pStoredProcedure, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<T>> GetById(string pStoredProcedure, object id)
    {
        _logger.LogInformation("Database Connection details: {0}", _dbConnection.ConnectionString);

        return await _dbConnection.QueryAsync<T>(pStoredProcedure, id, commandType: CommandType.StoredProcedure);
    }

    public async Task Add<U>(string pStoredProcedure, U parameters)
    {
        _logger.LogInformation("Database Connection details: {0}", _dbConnection.ConnectionString);

        await _dbConnection.QueryAsync<T>(pStoredProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task Update<U>(string pStoredProcedure, U parameters)
    {
        _logger.LogInformation("Database Connection details: {0}", _dbConnection.ConnectionString);
        
        await _dbConnection.QueryAsync<T>(pStoredProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task Delete(string pStoredProcedure, object id)
    {
        _logger.LogInformation("Database Connection details: {0}", _dbConnection.ConnectionString);
        
        await _dbConnection.QueryAsync<T>(pStoredProcedure, id, commandType: CommandType.StoredProcedure);
    }
}