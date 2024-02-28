using System;
using ToDo.Application.DTO;
using ToDo.Domain.Entities;
namespace ToDo.Application.Interfaces;

public interface IAdminService
{
    
    Task<AssignmentList> DelegateList(AssignmentListDTO list);
    Task<List<User>> GetAllUsers();
    Task<SearchUserDTO?> GetUserById(long id);
    Task<User> UserLogged(string email);
    Task<bool> RemoveUser(long id);

    Task<User> GetCredentials(string email);
}