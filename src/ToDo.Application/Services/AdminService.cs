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
		AssignmentList assignmentmapper = _mapper.Map<AssignmentList>(assignmentlist);
		
		long listid = await _atListRepository.GetListNewID(assignmentlist.UserId);
		assignmentmapper.ListId = listid;
		
		AssignmentList assignmentListcreated = await _atListRepository.Create(assignmentmapper);
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
		var usermapped = _mapper.Map<SearchUserDTO>(user);
		return usermapped;
	}

	public async Task RemoveUser(long id)
	{ 
		await _userRepository.Delete(id);
	}

	public async Task RemoveTask(SearchAssignmentDTO assignmentDto)
	{
		await _assignmentRepository.Delete(assignmentDto.Id);
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
}