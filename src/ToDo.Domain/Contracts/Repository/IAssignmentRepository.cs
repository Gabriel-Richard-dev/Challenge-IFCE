using ToDo.Domain.Entities;

namespace ToDo.Domain.Contracts.Repository;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    Task<List<Assignment>> GetTasks(long userid, long listid);
    Task<Assignment?> GetTaskById(long userid, long listid, long taskid);
    Task DeleteTask(long userid, long listid, long taskid);
    
}