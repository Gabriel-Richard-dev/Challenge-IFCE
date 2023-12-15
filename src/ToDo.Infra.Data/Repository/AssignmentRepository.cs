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
        var list = await _context.Assignments.AsNoTracking().ToListAsync();
        var listUser =  list.Where(a => a.UserId == userId).ToList();
        return listUser;
    }
}