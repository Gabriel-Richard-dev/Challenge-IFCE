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
    
}