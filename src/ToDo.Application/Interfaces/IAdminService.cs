using System;
using ToDo.Application.DTO;
using ToDo.Domain.Entities;
namespace ToDo.Application.Interfaces;

public interface IAdminService
{
    Task<User> CreateUser(UserDTO user);
    Task<Assignment> DelegateTask(AssignmentDTO assignment);
    Task<AssignmentList> DelegateList(AssignmentListDTO list);
}