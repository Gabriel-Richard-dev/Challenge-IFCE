using System.ComponentModel;
using System.Xml;
using AutoMapper;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services;

public class AssignmentService : IAssignmentService
{
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper,
        IAssignmentListService listService)
    {
        _assignmentRepository = assignmentRepository;
        _assignmentListService = listService;
        _mapper = mapper;
    }

    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentListService _assignmentListService;
    private readonly IMapper _mapper;


    public async Task<Assignment> CreateTask(AssignmentDTO assignmentDto)
    {
        var assignment = _mapper.Map<Assignment>(assignmentDto);

        if (assignment.DateConcluded.Equals("0001-01-01 00:00:00.000000"))
        {
            assignment.DateConcluded = null;
        }

        if (assignment.Concluded == true && assignment.DateConcluded == null)
        {
            throw new Exception();
        }

        if (assignment.Deadline.Equals("0001-01-01 00:00:00.000000"))
        {
            assignment.Deadline = null;
        }

        assignment.Validation();

        var listsuser = await _assignmentListService.GetAllLists(assignmentDto.UserId);
        foreach (var item in listsuser)
        {
            if (item.ListId == assignmentDto.AtListId)
            {
                var assignmentCreated = await _assignmentRepository.Create(assignment);
                return assignmentCreated;
            }
        }

        throw new Exception();
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
       return removed;


    }

    public async Task<Assignment> UpdateTask(AddAssignmentDTO dto, long id)
    {
        var baseAssignment = await _assignmentRepository.GetById(id);
        
        if(baseAssignment is not null)
        {
            var assignmentMapped = _mapper.Map<Assignment>(dto);
            assignmentMapped.Id = id;
            assignmentMapped.UserId = baseAssignment.UserId;
            assignmentMapped.Validation();
            await _assignmentRepository.Update(assignmentMapped);
            return assignmentMapped;
        }

        throw new Exception();
    }


    public async Task<Assignment> UpdateUserTask(AddAssignmentDTO dto, long id)
    {
        
        var baseAssignment = await _assignmentRepository.GetById(id);

        if (baseAssignment is not null && baseAssignment.Id == AuthenticatedUser.Id) 
        {
            var assignmentMapped = _mapper.Map<Assignment>(dto);
            assignmentMapped.Id = id;
            assignmentMapped.UserId = baseAssignment.UserId;
            assignmentMapped.Validation();
            await _assignmentRepository.Update(assignmentMapped);
            return assignmentMapped;
        }

        throw new Exception();
        
        
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
  
    
    
}