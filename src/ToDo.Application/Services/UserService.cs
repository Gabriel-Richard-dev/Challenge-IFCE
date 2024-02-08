using AutoMapper;
using System.Net.WebSockets;
using ToDo.Application.Criptografy;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Application.Notifications;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;
using ToDo.Core.Exceptions;

namespace ToDo.Application.Services;

public class UserService : IUserService
{
    public UserService(IUserRepository userRepository, IAssignmentListService listService, IMapper mapper, INotification notificator)
    {
        _userRepository = userRepository;
        _listService = listService;
        _mapper = mapper;
        _notificator = notificator;
    }
    private readonly IUserRepository _userRepository;
    private readonly IAssignmentListService _listService;
    private readonly INotification _notificator;
    private readonly IMapper _mapper;

    public async Task<User> CreateUser(UserDTO user)
    {
        

        var userMapped = _mapper.Map<User>(user);

        var userExists = await GetByEmail(user.Email);

        if (userExists != null)
        {
            _notificator.AddNotification("Já existe um usuário cadastrado com esse email!");
            return null;
        }
        
        _notificator.AddNotification(userMapped.Validation());


        if (_notificator.HasNotification())
            return null;
        
        
        var quantUser = (await GetAllUsers()).Count();
        
        if(quantUser == 0)
        {
            userMapped.AdminPrivileges = true;
        }
        
        userMapped.AddFirstList();
        userMapped.Password = userMapped.Password.GenerateHash();
        User userCreated = await _userRepository.Create(userMapped);
        
        if(await CommitChanges())
            return userCreated;
        _notificator.AddNotification("Erro ao criar usuário");
        return null;
    }

    
    
    
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
        
        _notificator.AddNotification(userMapped.Validation());
        if (_notificator.HasNotification())
            return null;

        var userExist = await GetByEmail(user.Email);
        
        if(userExist is not null)
        {
            _notificator.AddNotification("User already exists");
            return null;
        }
        
        userMapped.Password = userMapped.Password.GenerateHash();
        var userCreated = await _userRepository.Update(userMapped);
        
        
        
        if (await CommitChanges())
            return userCreated;
        _notificator.AddNotification("Erro ao criar o usuário");
        return null;

    }
    public async Task<User> UpdatePassword(LoginUserDTO user, string confirmpass, string newpass)
    {
        User userExist = await _userRepository.GetByEmail(user.Email);

        if(userExist is null)
        {
            throw new ToDoException("User inexistent.");
        }


        confirmpass  = userExist.Password;
        userExist.AtualizaPassword(confirmpass, confirmpass, newpass);
        userExist.Validation();
        userExist.Password = userExist.Password.GenerateHash();
        var userUpdated = await _userRepository.Update(userExist);

        if (await CommitChanges())
            return userUpdated;

        _notificator.AddNotification("Não foi possível retornar o usuário");
        return null;


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

    public async Task<bool> CommitChanges()
    {
        if (await _userRepository.UnityOfWork.Commit())
        {
            return true;
        }

        return false;
    }
}
