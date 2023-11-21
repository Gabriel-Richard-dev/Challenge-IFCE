using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services;

public class UserService : IUserService
{
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private readonly IUserRepository _userRepository;
    

    public Task<AssignmentList> CreateList(long id, AssignmentListDTO assignmentList)
    {
        throw new NotImplementedException();
    }
    
    public async Task<List<User>?> GetAllUsers()
    {
        var userList = await _userRepository.GetAll();
        return userList;
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }
}
