using System.ComponentModel;
using AutoMapper;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services;

public class AssignmentService : IAssignmentService
{
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
    }

    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    
    public async Task<Assignment> DelegateTask(AssignmentDTO assignment)
    {
        
        Assignment assignmentMapped = _mapper.Map<Assignment>(assignment); 
        Assignment assignmentCreated = await _assignmentRepository.Create(assignmentMapped);
        
        return assignmentCreated;
    }

    public async Task<Assignment> CreateTask(AssignmentDTO assignmentDto)
    {
        var assignment = _mapper.Map<Assignment>(assignmentDto);

        var assignmentcreated = await _assignmentRepository.Create(assignment);
        return assignmentcreated;
    }
    
    
    public async Task<List<Assignment>> GetTasks(long userid, long listid)
    {
        return await _assignmentRepository.GetTasks(userid, listid);
    }

    public async Task<Assignment?> GetTaskById(SearchAssignmentDTO dto)
    {
        return await _assignmentRepository.GetTaskById(dto.UserId,dto.ListId,dto.Id);
    }
}