using AutoMapper;
using System.Net.WebSockets;
using ToDo.Application.Criptografy;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Core.Exceptions;

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
        User userMapped = _mapper.Map<User>(user);
        
        var userExists = await GetByEmail(user.Email);

        if (userExists != null)
        {

            throw new ToDoException("Email já cadastrado.");

        }

        userMapped.Validation();
        
        userMapped.Password = userMapped.Password.GenerateHash();
        var userCreated = await _userRepository.Create(userMapped);
        return userCreated;
        
    }

    private readonly IUserRepository _userRepository;
    
    
    
    public async Task<List<SearchUserDTO>?> GetAllUsers()
    {
        var userList = await _userRepository.GetAll();
        var userListMapped = _mapper.Map<List<SearchUserDTO>>(userList);
        return userListMapped;
    }

    public async Task<SearchUserDTO?> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user != null) { 
        var userMapped = _mapper.Map<SearchUserDTO>(user);
        return userMapped;
        }

        return null;
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
        userMapped.Validation();
        userMapped.Password = userMapped.Password.GenerateHash();
        return await _userRepository.Update(userMapped);
    }
    public async Task<User> UpdatePassword(LoginUserDTO user, string confirmpass, string newpass)
    {
        User userExist = await _userRepository.GetByEmail(user.Email);
        confirmpass  = userExist.Password;
        userExist.AtualizaPassword(confirmpass, confirmpass, newpass);
        userExist.Validation();
        userExist.Password = userExist.Password.GenerateHash();
        return await _userRepository.Update(userExist);
    }
    
        
    public async Task<List<SearchUserDTO>> SearchByName(string parsename)
    {
        var users = await _userRepository.GetAll();

        var listUser = users.Where(u => u.Name.ToLower().Contains(parsename.ToLower())).ToList();

        return _mapper.Map<List<SearchUserDTO>>(listUser);

    }
    public async Task<List<SearchUserDTO>> SearchByEmail(string parseEmail)
    {
        var users = await _userRepository.GetAll();

        var listUsers = users.Where(u => u.Email.ToLower().Contains(parseEmail.ToLower())).ToList();
        var listUsersDTO = _mapper.Map<List<SearchUserDTO>>(listUsers);

        return listUsersDTO;


    }

   
}
