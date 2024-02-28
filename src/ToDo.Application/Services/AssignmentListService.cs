using AutoMapper;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Application.Notifications;
using ToDo.Core.Exceptions;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services;

public class AssignmentListService : IAssignmentListService
{

    public AssignmentListService(IAssignmentListRepository assignmentListRepository,
        IMapper mapperr,
        IUserRepository userRepository, INotification notification,
        IAssignmentRepository assignmentRepository )
    {
        _assignmentListRepository = assignmentListRepository;
        _mapper = mapperr;
        _userRepository = userRepository;
        _notification = notification;
        _assignmentRepository = assignmentRepository;
    }

    private readonly IAssignmentListRepository _assignmentListRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly INotification _notification;
    public async Task<List<AssignmentList>> GetAllLists(long userid)
    {
        var list = await _assignmentListRepository.GetAll();
        var taskList = list.Where(l => l.UserId == userid).ToList();

        return taskList;
    }

    public async Task<AssignmentList> CreateList(AssignmentListDTO assignmentDto)
    {
        var assignment = _mapper.Map<AssignmentList>(assignmentDto);
        _notification.AddNotification(assignment.Validation());
        if (_notification.HasNotification())
        {
            return null;
        }
        
        var assignmentCreated = await _assignmentListRepository.Create(assignment);
        if (await CommitChanges())
            return assignmentCreated;
        _notification.AddNotification("Impossível criar a lista");
        return null;
    }
    public async Task<bool> CreateList(string name)
    {
        var list = CreateList(new AssignmentListDTO()
        {
            Name = name
        });


        return list is not null;

    }

    public async Task<AssignmentList> GetListById(SearchAssignmentListDTO search)
    {
        var lAssignment = await _assignmentListRepository.GetListByListId(search.UserId, search.ListId);
        var list = await _assignmentListRepository.GetAll();
        var assignment = list.Where(l => l.Id == lAssignment.Id).ToList();
        return assignment.FirstOrDefault();
    }


    public async Task<AssignmentList> RemoveTaskList(SearchAssignmentListDTO dto)
    {
        var atListExists = await GetListById(dto);

        if(atListExists is not null)
        {
            await _assignmentListRepository.Delete(atListExists);
            await _assignmentRepository.DeleteByList(await _userRepository.GetById(dto.UserId), dto.ListId);
            if (await CommitChanges())
                return atListExists;
            _notification.AddNotification("Impossível remover a lista");
            return null;
        }

        _notification.NotFound();
        return null;

    }
    
    
    async Task<bool> CommitChanges() => await _assignmentListRepository.UnityOfWork.Commit() ? true : false;

}