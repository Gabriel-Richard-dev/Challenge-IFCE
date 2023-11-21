using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;


namespace ToDo.Infra.Data.Repository;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : Base
{
    public BaseRepository(ToDoContext context)
    {
        _context = context;
        _dbset = context.Set<T>();
    }
    
    protected readonly ToDoContext _context;
    private readonly DbSet<T> _dbset;
    
    public virtual async Task<T> Create(T entity)
    {
        throw new NotImplementedException();
    }


    public virtual async Task<List<T>> GetAll()
    {
        throw new NotImplementedException();
    }
    public virtual async Task<T?> GetById(long id)
    {
        throw new NotImplementedException();
    }    
    public virtual async Task<T> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

  
}