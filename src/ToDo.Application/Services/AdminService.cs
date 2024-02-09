using System;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Application.DTO;
using ToDo.Domain.Contracts.Repository;
using ToDo.Application.Interfaces;
using AutoMapper;
using ToDo.Application.Notifications;
using ToDo.Core.Exceptions;

namespace ToDo.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListRepository _atListRepository;
	private readonly IMapper _mapper;
	private readonly INotification _notificator;
	private readonly IBaseRepository<Base> _baseRepository;
    public AdminService(IUserRepository userRepository, IAssignmentRepository assignmentRepository, 
	    IAssignmentListRepository assignmentListRepository, IMapper mapper,
	    INotification notificator)
	{
		_userRepository = userRepository;
		_assignmentRepository = assignmentRepository;
		_atListRepository = assignmentListRepository;
		_mapper = mapper;
		_notificator = notificator;
	}
    
	public async Task<AssignmentList> DelegateList(AssignmentListDTO assignmentlist)
	{
		AssignmentList assignmentMapped = _mapper.Map<AssignmentList>(assignmentlist);
		assignmentMapped.Validation();
		long listid = await _atListRepository.GetListNewID(assignmentlist.UserId);
		assignmentMapped.ListId = listid;
		
		AssignmentList assignmentListcreated = await _atListRepository.Create(assignmentMapped);
		if(await CommitChanges())
			return assignmentListcreated;

		_notificator.AddNotification("Não foi possível criar tasklist");
		return null;
	}
	

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

	public async Task<bool> RemoveUser(long id)
	{
		var user = await _userRepository.GetById(id);
		if(user is not null)
		{
			await _atListRepository.Delete(user);
			await _userRepository.Delete(user);
		}
		if (await CommitChanges()) 
			return true;
		_notificator.AddNotification("Impossível remover o usuário.");
		return false;
	}


	public async Task RemoveTaskList(SearchAssignmentListDTO assignmentDto)
	{
		var list = await _atListRepository
			.GetListByListId(assignmentDto.UserId, assignmentDto.ListId);

		await _atListRepository.Delete(list);
		if (await CommitChanges())
			return;
		_notificator.AddNotification("Impossível remover a tasklist.");
	}

	public async Task<User> GetCredentials(string email)
	{
		return await _userRepository.GetByEmail(email);
	}


    public async Task<User> UserLogged(string email)
	{
		return await _userRepository.GetByEmail(email);
	}


	async Task<bool> CommitChanges()
	{
		if (await _userRepository.UnityOfWork.Commit() 
		    || await _atListRepository.UnityOfWork.Commit() 
		    || await _assignmentRepository.UnityOfWork.Commit())
		{
			return true;
		}

		return false;
	}

}