using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ToDoContext context) : base(context)
    { }

    public override async Task<Assignment> Create(Assignment assignment)
    {
        assignment.Validation();
        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();
        return assignment;
    }

    public async Task DeleteTask(long userid, long listid, long taskid)
    {
        var task = await GetTaskById(userid, listid, taskid);
        _context.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Assignment>> GetTasks(long userid, long listid)
    {
        var list = await _context.Assignments.AsNoTrackingWithIdentityResolution()
            .Where(a => a.UserId == userid 
                        && a.AtListId == listid).ToListAsync();
        return list.ToList();
    }
    
    public async Task<Assignment?> GetTaskById(long userid, long listid, long taskid)
    {
        List<Assignment>? list = _context.Assignments
            .Where(a => a.UserId == userid 
                        && a.AtListId == listid 
                        && a.Id == taskid)
            .ToList();

              
        return list.FirstOrDefault();
        
    }
}