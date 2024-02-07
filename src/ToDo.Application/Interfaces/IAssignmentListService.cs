using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IAssignmentListService
{
    Task<List<AssignmentList>> GetAllLists(long userid);
    Task<AssignmentList> GetListById(SearchAssignmentListDTO search);
    Task<AssignmentList> CreateList(AssignmentListDTO assignmentDto);
    Task<bool> CreateList(string name);
    Task<AssignmentList> RemoveTaskList(SearchAssignmentListDTO dto);
}