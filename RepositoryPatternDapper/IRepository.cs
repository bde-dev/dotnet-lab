namespace RepositoryPatternDapper;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll(string pStoredProcedure);
    Task<IEnumerable<T?>> GetById(string pStoredProcedure, object id);
    Task Add<U>(string pStoredProcedure, U parameters);
    Task Update<U>(string pStoredProcedure, U parameters);
    Task Delete(string pStoredProcedure, object id);
}