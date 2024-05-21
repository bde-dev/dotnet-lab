namespace RepositoryPatternEntityFramework.Repository;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}