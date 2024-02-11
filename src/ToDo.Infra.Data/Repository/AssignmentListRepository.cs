using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class AssignmentListRepository : BaseRepository<AssignmentList>, IAssignmentListRepository
{
    public AssignmentListRepository(ToDoContext context) : base(context)
    {
    }




    public async Task<long> GetListNewID(long userid)
    {

        var lists = await GetAll();
        var listToUser = lists.Where(l => l.UserId == userid).ToList();
        long newid = 0;

        foreach (var teste in listToUser)
        {
            newid = teste.ListId + 1;
        }

        return newid;

    }

    public async Task<AssignmentList> GetListByListId(long userId, long listId)
    {
        var list = await _context.AssignmentLists.Where(l =>
            l.UserId == userId && l.ListId == listId).ToListAsync();
        if (list.FirstOrDefault() is not null)
        {
            return list.FirstOrDefault();
        }

        return null;

    }
    public async Task<List<AssignmentList>> GetListByListId(long userId)
    {
        var list = await _context.AssignmentLists.Where(l =>
            l.UserId == userId).ToListAsync();


        return list;
    }

    public async Task Delete(User user)
    {
        var lists = await GetListByListId(user.Id);


        foreach (var list in lists)
        {
            _context.AssignmentLists.Remove(list);
        }
        
    }
}