using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateUser(UserDTO user);
    Task<AssignmentList> CreateList(long id, AssignmentListDTO assignmentList);
    Task<List<SearchUserDTO>?> GetAllUsers();
    Task<User> GetByEmail(string email);
    Task<bool> LoginValid(LoginUserDTO dto);
}