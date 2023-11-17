using ToDo.Domain.Entities;

namespace ToDo.Domain.Contracts.Repository;

public interface IBaseRepository<T> where T : Base
{
    void Create(T entity);
    Task<T?> GetById(int? id);
    Task<List<T>> GetAll();
    void Update(T entity);
    void Delete(T entity);
    

}