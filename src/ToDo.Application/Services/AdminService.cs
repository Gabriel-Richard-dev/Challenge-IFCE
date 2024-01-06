using System;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Application.DTO;
using ToDo.Domain.Contracts.Repository;
using ToDo.Application.Interfaces;
using AutoMapper;

namespace ToDo.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListRepository _atListRepository;
	private readonly IMapper _mapper;
	
    public AdminService(IUserRepository userRepository, IAssignmentRepository assignmentRepository, IAssignmentListRepository assignmentListRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_assignmentRepository = assignmentRepository;
		_atListRepository = assignmentListRepository;
		_mapper = mapper;
	}
    
	public async Task<AssignmentList> DelegateList(AssignmentListDTO assignmentlist)
	{
		AssignmentList assignmentMapped = _mapper.Map<AssignmentList>(assignmentlist);
		assignmentMapped.Validation();
		long listid = await _atListRepository.GetListNewID(assignmentlist.UserId);
		assignmentMapped.ListId = listid;
		
		AssignmentList assignmentListcreated = await _atListRepository.Create(assignmentMapped);
		return assignmentListcreated;
	}

	// public async Task<Assignment> DelegateTask(AssignmentDTO assignment)

	// {

	// 	Assignment assignmentmapper = _mapper.Map<Assignment>(assignment); 

	// 	var assignmentcreated = await _assignmentRepository.Create(assignmentmapper);

	// 	return assignmentcreated;

	// }

	public async Task<List<User>?> GetAllUsers()
	{
		return await _userRepository.GetAll();
	}

	public async Task<SearchUserDTO?> GetUserById(long id)
	{
		var user = await _userRepository.GetById(id);
		var userMapped = _mapper.Map<SearchUserDTO>(user);
		return userMapped;
	}

	public async Task RemoveUser(long id)
	{ 
		await _userRepository.Delete(id);
	}


	public async Task RemoveTaskList(SearchAssignmentListDTO assignmentDto)
	{
		var list = await _atListRepository
			.GetListByListId(assignmentDto.UserId, assignmentDto.ListId);

		await _atListRepository.Delete(list.Id);
	}

	public async Task<User> GetCredentials(string email)
	{
		return await _userRepository.GetByEmail(email);
	}


    public async Task<User> UserLogged(string email)
	{
		return await _userRepository.GetByEmail(email);
	}
}