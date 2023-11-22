using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IAssignmentService
{
    Task<Assignment> CreateTask(AssignmentDTO assignment);
    Task<List<Assignment>> GetTasks(long userid, long listid);
    Task<Assignment> GetTaskById(long userid, long listid, long taskid);
}