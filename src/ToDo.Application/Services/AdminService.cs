using System;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Application.DTO;
using ToDo.Infra.Data.Repository;
using ToDo.Domain.Contracts.Repository;
using ToDo.Application.Interfaces;
using AutoMapper;

namespace ToDo.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

    public AdminService(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

		
	public async Task<User> CreateUser(UserDTO user)
	{
		User usermapped = _mapper.Map<User>(user); 
		var usercreated = await _userRepository.Create(usermapped);
		return usercreated;
	}
}