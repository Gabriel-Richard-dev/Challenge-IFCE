using ToDo.Domain.Entities;

namespace ToDo.Domain.Contracts.Repository;

public interface IBaseRepository<T> where T : Base
{
    Task<T> Create(T entity);
    Task<T?> GetById(long id);
    Task<List<T>?> GetAll();
    Task<T> Update(T entity);
    Task Delete(long id);
    

}