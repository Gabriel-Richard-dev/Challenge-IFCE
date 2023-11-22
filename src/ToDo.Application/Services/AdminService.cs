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

	public Task<User?> GetUserById(long id)
	{
		return _userRepository.GetById(id);
	}

	public async Task RemoveUser(long id)
	{ 
		await _userRepository.Delete(id);
	}

	public async Task RemoveTask(searchAssignmentDTO assignmentDto)
	{
		await _assignmentRepository.DeleteTask(assignmentDto.UserId, assignmentDto.ListId, assignmentDto.Id);
	}

	public async Task RemoveTaskList(SearchAssignmentListDTO assignmentDto)
	{
		await _atListRepository.DeleteList(assignmentDto.UserId, assignmentDto.Id);
	}
}