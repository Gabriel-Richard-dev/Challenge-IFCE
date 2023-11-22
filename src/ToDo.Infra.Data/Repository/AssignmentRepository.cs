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

    public async Task<List<Assignment>> GetTasks(long userid, long listid)
    {
        var list = await _context.Assignments.AsNoTrackingWithIdentityResolution()
            .Where(a => a.UserId == userid 
                        && a.AtListId == listid).ToListAsync();
        return list;
    }
}