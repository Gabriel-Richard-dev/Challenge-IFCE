using AutoMapper;
using ToDo.Application.Criptografy;
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
        usermapped.Password = usermapped.Password.GenerateHash();
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

    public async Task<SearchUserDTO> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        var usermapped = _mapper.Map<SearchUserDTO>(user);
        return usermapped;
    }

    public async Task<bool> LoginValid(LoginUserDTO dto)
    {
        dto.Password = dto.Password.GenerateHash();
        return await _userRepository.LoginValid(dto.Email, dto.Password);
    }

    public async Task<User> Update(UserDTO user, long id)
    {
        var userMapped = _mapper.Map<User>(user);
        userMapped.Id = id;
        return await _userRepository.Update(userMapped);
    }
    public async Task<User> UpdatePassword(LoginUserDTO user, string confirmpass, string newpass)
    {
        User userExist = await _userRepository.GetByEmail(user.Email);
        confirmpass  = userExist.Password;
        userExist.AtualizaPassword(confirmpass, confirmpass, newpass);
        userExist.Password = userExist.Password.GenerateHash();
        return await _userRepository.Update(userExist);
    }
    
   
}
