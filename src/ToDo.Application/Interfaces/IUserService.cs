using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateUser(UserDTO user);

    Task<User> Update(UserDTO userDto, long id);
    Task<List<SearchUserDTO>?> GetAllUsers();
    Task<SearchUserDTO?> GetByEmail(string email);
    Task<bool> LoginValid(LoginUserDTO dto);
    Task<User> UpdatePassword(LoginUserDTO user, string newPassword);
    Task<List<SearchUserDTO>> SearchByName(string parsename);
    Task<List<SearchUserDTO>> SearchByEmail(string parseEmail);
}