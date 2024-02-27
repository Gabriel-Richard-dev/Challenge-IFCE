using System.ComponentModel;
using System.Xml;
using AutoMapper;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Application.Notifications;
using ToDo.Core.Exceptions;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services;

public class AssignmentService : IAssignmentService
{
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper,
        IAssignmentListService listService, INotification notification)
    {
        _assignmentRepository = assignmentRepository;
        _assignmentListService = listService;
        _mapper = mapper;
        _notification = notification;
    }

    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListService _assignmentListService;
    private readonly IMapper _mapper;
    private readonly INotification _notification;


    public async Task<Assignment> CreateTask(AssignmentDTO assignmentDto)
    {
        var assignment = _mapper.Map<Assignment>(assignmentDto);
        assignment.AssignmentListId = assignmentDto.AtListId;
        
        if (assignment.DateConcluded.Equals("0001-01-01 00:00:00.000000"))
        {
            assignment.DateConcluded = null;
        }

        if (assignment.Concluded == true && assignment.DateConcluded == null)
        {
            _notification.AddNotification("Você não pode dizer que uma task foi concluida sem informar a data de conclusão");
        }

        if (assignment.Deadline.Equals("0001-01-01 00:00:00.000000"))
        {
            assignment.Deadline = null;
        }

        _notification.AddNotification(assignment.Validation());

        var listsuser = await _assignmentListService.GetListById(new SearchAssignmentListDTO()
        {
            ListId = assignmentDto.AtListId,
            UserId = assignment.UserId
        });


        if (listsuser is not null)
        {
            await _assignmentRepository.Create(assignment);
            if (await CommitChanges())
                return assignment;
        }
        
        
        _notification.AddNotification("Lista inexistente.");
        return null;
    }


    public async Task<List<Assignment>> GetTasks(long userid, long listId)
    {
        var list = await _assignmentRepository.GetAll();
        
        return list
            .Where(a => a.AssignmentListId == listId 
                        && a.UserId == userid).ToList();
    }

    public async Task<Assignment> GetTaskById(SearchAssignmentDTO dto)
    {
        var listAssignment = GetTasks(dto.UserId, dto.ListId).Result;

        var assignment = listAssignment.Where(a => a.Id == dto.Id).ToList().FirstOrDefault();
        return assignment;
    }

    public async Task<Assignment> RemoveTask(SearchAssignmentDTO dto)
    {

       var removed = await GetTaskById(dto);
       await _assignmentRepository.RemoveTask(dto.Id, dto.UserId);
       if(await CommitChanges())
        return removed;
       
       _notification.AddNotification("Impossível remover a lista");
       return null;
    }

    public async Task<Assignment> UpdateTask(AddAssignmentDTO dto, long id)
    {
        var baseAssignment = await _assignmentRepository.GetById(id);
        
        if(baseAssignment is not null)
        {
            var assignmentMapped = _mapper.Map<Assignment>(dto);
            assignmentMapped.Id = id;
            assignmentMapped.UserId = baseAssignment.UserId;
            _notification.AddNotification(assignmentMapped.Validation());
            
            if (_notification.HasNotification())
            {
                return null;
            }
            
            await _assignmentRepository.Update(assignmentMapped);
            if (await CommitChanges())
                return assignmentMapped;
            _notification.AddNotification("Não foi possível atualizar a tarefa");
            return null;
        }

        _notification.NotFound();
        return null;
    }


    public async Task<Assignment> UpdateUserTask(AddAssignmentDTO dto, long id)
    {
        
        var baseAssignment = await _assignmentRepository.GetById(id);

        if (baseAssignment is not null && baseAssignment.Id == AuthenticatedUser.Id) 
        {
            var assignmentMapped = _mapper.Map<Assignment>(dto);
            assignmentMapped.Id = id;
            assignmentMapped.UserId = baseAssignment.UserId;
            _notification.AddNotification(assignmentMapped.Validation());


            if (_notification.HasNotification())
            {
                return null;
            }
            
            
            var assignmentUpdated = await _assignmentRepository.Update(assignmentMapped);
            
            if (await CommitChanges())
                return assignmentUpdated;
            _notification.AddNotification("Não foi possível atualizar");
        }

        _notification.NotFound();
        return null;

    }

    public async Task<List<AssignmentDTO>> SearchTaskByTitle(string parseTitle)
    {
        var tasks = await _assignmentRepository.GetAll();

        var listTasks = tasks
            .Where(a => a.UserId == AuthenticatedUser.Id 
                        && a.Title.ToLower().Contains(parseTitle.ToLower()))
            .ToList();

        return _mapper.Map<List<AssignmentDTO>>(listTasks);


    }

    public async Task<bool> CommitChanges() => await _assignmentRepository.UnityOfWork.Commit() ? true : false;



}