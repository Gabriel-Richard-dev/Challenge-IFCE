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
        _dbset.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public virtual async Task<List<T>> GetAll()
    {
        return await _dbset.ToListAsync();
    }
    public virtual async Task<T?> GetById(long id)
    {
        var list = await _dbset.Where(o => o.Id == id)
            .ToListAsync();

        return list.FirstOrDefault();
    }    
    
    public async virtual Task<T> Update(T entity)
    {
        var PreviousEntity = await GetById(entity.Id);
        var entityEntry = _dbset.Entry(PreviousEntity).State;
        entityEntry = EntityState.Detached;
        _dbset.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task Delete(long id)
    {
        var entity = await GetById(id);
        if (entity is not null)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();    
        }
    }
    


  
}