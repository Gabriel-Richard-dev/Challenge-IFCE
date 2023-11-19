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
    
    private readonly ToDoContext _context;
    private readonly DbSet<T> _dbset;
    
    public async Task<T> Create(T entity)
    {
        _dbset.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public Task<T?> GetById(int? id)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetAll()
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