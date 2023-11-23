using AutoMapper;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services;

public class UserService : IUserService
{
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    private readonly IMapper _mapper;

    public async Task<User> CreateUser(UserDTO user)
    {
        User usermapped = _mapper.Map<User>(user); 
        var usercreated = await _userRepository.Create(usermapped);
        return usercreated;
    }

    private readonly IUserRepository _userRepository;
    

    public Task<AssignmentList> CreateList(long id, AssignmentListDTO assignmentList)
    {
        throw new NotImplementedException();
    }
    
    public async Task<List<SearchUserDTO>?> GetAllUsers()
    {
        var userList = await _userRepository.GetAll();
        var userListMapped = _mapper.Map<List<SearchUserDTO>>(userList);
        return userListMapped;
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }

    public async Task<bool> LoginValid(LoginUserDTO dto)
    {
        return await _userRepository.LoginValid(dto.Email, dto.Password);
    }
   
}
