using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class AssignmentListRepository : BaseRepository<AssignmentList>, IAssignmentListRepository
{
    public AssignmentListRepository(ToDoContext context) : base(context)
    { }
    
    public override async Task<AssignmentList> Create(AssignmentList assignmentlist)
    {
        assignmentlist.Validation();
        _context.AssignmentLists.Add(assignmentlist);
        await _context.SaveChangesAsync();
        return assignmentlist;
    }
    
}