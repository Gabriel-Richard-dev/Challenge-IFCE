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
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper, IAssignmentListService listService)
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
        var list = await _assignmentRepository.GetAllTasks(userid);
        var listHandler = list.Where(a => a.AssignmentListId == listId).ToList();
        
        return listHandler;
    }

    public async Task<Assignment?> GetTaskById(SearchAssignmentDTO dto)
    {
        var listAssignment =  await GetTasks(dto.UserId, dto.Id);
        
        // var assignment = listAssignment.Where(a => a.Id == dto.Id).ToList().FirstOrDefault();
        // if (assignment is not null)
        // {
        //     return assignment;
        // }

        return listAssignment.FirstOrDefault();

    }

    public async Task RemoveTask(long id)
    {
      
            await _assignmentRepository.Delete(id);
     
        
    }

   
    
    
}