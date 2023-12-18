using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IAssignmentService
{
    Task<Assignment> CreateTask(AssignmentDTO assignment);
    Task<List<Assignment>> GetTasks(long userid, long listid);
    Task<Assignment> GetTaskById(SearchAssignmentDTO dto);
    Task<Assignment> RemoveTask(SearchAssignmentDTO dto);
    Task<Assignment> UpdateTask(AddAssignmentDTO dto, long id);

    Task<Assignment> UpdateUserTask(AddAssignmentDTO dto, long id);

    Task<List<AssignmentDTO>> SearchTaskByTitle(string parseTitle);
}