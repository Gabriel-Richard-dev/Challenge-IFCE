using ToDo.Domain.Entities;

namespace ToDo.Domain.Contracts.Repository;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    Task<List<Assignment>> GetAllTasks(long userId);
    Task RemoveTask(long Id, long UserId);
}