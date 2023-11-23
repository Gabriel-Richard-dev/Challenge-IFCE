using System;
using ToDo.Application.DTO;
using ToDo.Domain.Entities;
namespace ToDo.Application.Interfaces;

public interface IAdminService
{
    
    Task<AssignmentList> DelegateList(AssignmentListDTO list);
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserById(long id);

    Task RemoveUser(long id);
    Task RemoveTask(SearchAssignmentDTO assignmentDto);
    Task RemoveTaskList(SearchAssignmentListDTO assignmentDto);
}