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
    
    
    public Task<List<Assignment>> GetTasks(long userid, long listid)
    {
        return _assignmentRepository.GetTasks(userid, listid);
    }

    public Task<Assignment> GetTaskById(long userid, long listid, long taskid)
    {
        throw new NotImplementedException();
    }
}