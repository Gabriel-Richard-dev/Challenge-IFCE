using ToDo.Domain.Entities;

namespace ToDo.Domain.Contracts.Repository;

public interface IAssignmentListRepository : IBaseRepository<AssignmentList>
{
    Task<List<AssignmentList>> GetAllLists(long userid);
}