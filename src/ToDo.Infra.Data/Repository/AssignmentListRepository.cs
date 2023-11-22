using Microsoft.EntityFrameworkCore;
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

    public async Task<List<AssignmentList>> GetAllLists(long userid)
    {
        var list = await _context.AssignmentLists
            .Where(l => l.UserId == userid)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        return list;
    }

    public async Task DeleteList(long userid, long id)
    {
        var list = await _context.AssignmentLists
            .Where(l => l.UserId == userid && l.Id == id)
            .AsNoTrackingWithIdentityResolution().ToListAsync();
        _context.Remove(list);
        await _context.SaveChangesAsync();
    }


}