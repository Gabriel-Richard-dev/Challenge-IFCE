using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ToDoContext context) : base(context)
    { }

    public async Task<List<Assignment>> GetAllTasks(long userId)
    {
        var list = await _context.Assignments.AsNoTrackingWithIdentityResolution().ToListAsync();
        var listUser =  list.Where(a => a.UserId == userId).ToList();
        return listUser;
    }

    public async Task RemoveTask(long Id, long UserId)
    {
        var list = await GetAllTasks(UserId);

        foreach (var task in list)
        {
            if(task.Id == Id)
            {
                
                _context.Assignments.Remove(task);
                return;
            }
        }

        return;
    }
    
    public async Task Delete(User user)
    {
        var assignments = await GetAllTasks(user.Id);
 
        foreach (var a in assignments)
        {
            _context.Assignments.Remove(a);
        }
        
    }

    


    public async Task DeleteByList(User user, long listid)
    {
        var assignments = (await GetAllTasks(user.Id))
            .Where(a => a.AssignmentListId == listid)
            .ToList();
 
        foreach (var a in assignments)
        {
            _context.Assignments.Remove(a);
        }
    }
    
    
}