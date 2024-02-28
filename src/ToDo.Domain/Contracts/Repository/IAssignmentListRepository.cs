using ToDo.Domain.Entities;

namespace ToDo.Domain.Contracts.Repository;

public interface IAssignmentListRepository : IBaseRepository<AssignmentList>
{
    Task<long> GetListNewID(long userid);

    Task<AssignmentList> GetListByListId(long userId, long listId);

    Task Delete(User user);
    
}