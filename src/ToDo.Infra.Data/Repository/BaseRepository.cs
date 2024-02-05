using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public abstract class BaseRepository<T> : IUnityOfWork, IBaseRepository<T> where T : Base
{
    public BaseRepository(ToDoContext context)
    {
        _context = context;
        _dbset = context.Set<T>();
    }
    
    protected readonly ToDoContext _context;
    private readonly DbSet<T> _dbset;


    public IUnityOfWork UnityOfWork => _context;
    
    public virtual async Task<T> Create(T entity)
    {
        _dbset.Add(entity);
        return entity;
    }
    
    public virtual async Task<List<T>?> GetAll()
    {
        return await _dbset.AsNoTrackingWithIdentityResolution().ToListAsync();
    }
    public virtual async Task<T?> GetById(long id)
    {
        var list = await _dbset.Where(o => o.Id == id)
            .AsNoTrackingWithIdentityResolution().ToListAsync();

        return list.FirstOrDefault();
    }    
    
    public async virtual Task<T> Update(T entity)
    {
        var PreviousEntity = await GetById(entity.Id);
        var entityEntry = _dbset.Entry(PreviousEntity).State;
        entityEntry = EntityState.Detached;
        _dbset.Update(entity);
        return entity;
    }

    public virtual async Task Delete(long id)
    {
        var entity = await GetById(id);
        if (entity is not null)
        {
            _dbset.Entry(entity).State = EntityState.Deleted;
            _dbset.Remove(entity);
            return;
        }

        throw new Exception();
    }


    public async Task<bool> Commit()
    {
        return await _context.SaveChangesAsync() > 0;
    }

  
}