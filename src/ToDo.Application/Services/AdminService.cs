using System;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Application.DTO;
using ToDo.Infra.Data.Repository;
using AutoMapper;

namespace ToDo.Application.Services;

public class AdminService : IAdminService
{
	public AdminService(UserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	private readonly IMapper _mapper;
	private readonly UserRepository _userRepository;
		
	public async Task<User> CreateUser(UserDTO user)
	{
		User usermapped = _mapper.Map<User>(user); 
		await _userRepository.Create(usermapped);
		return usermapped;
	}
}