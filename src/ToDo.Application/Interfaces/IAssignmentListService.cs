using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IAssignmentListService
{
    Task<List<AssignmentList>> GetAllLists(long userid);
    Task<AssignmentList> CreateList(AssignmentListDTO assignmentDto);
}