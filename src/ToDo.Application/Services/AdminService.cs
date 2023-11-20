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

		
	public async Task<User> CreateUser(UserDTO user)
	{
		User usermapped = _mapper.Map<User>(user); 
		var usercreated = await _userRepository.Create(usermapped);
		return usercreated;
	}
	public async Task<Assignment> DelegateTask(AssignmentDTO assignment)
	{
		Assignment assignmentmapper = _mapper.Map<Assignment>(assignment); 
		var assignmentcreated = await _assignmentRepository.Create(assignmentmapper);
		return assignmentcreated;
	}
	public async Task<AssignmentList> DelegateList(AssignmentListDTO assignmentlist)
	{
		AssignmentList assignmentmapper = _mapper.Map<AssignmentList>(assignmentlist); 
		AssignmentList assignmentListcreated = await _atListRepository.Create(assignmentmapper);
		return assignmentListcreated;
	}
	
	
	
}