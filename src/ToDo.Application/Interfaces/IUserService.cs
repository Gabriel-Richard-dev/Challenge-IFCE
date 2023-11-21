using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IUserService
{
    Task<AssignmentList> CreateList(long id, AssignmentListDTO assignmentList);
    Task<List<User>?> GetAllUsers();
    Task<User> GetByEmail(string email);
}