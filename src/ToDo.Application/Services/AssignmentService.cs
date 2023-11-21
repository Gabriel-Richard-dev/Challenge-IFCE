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
    
    public async Task<Assignment> CreateTask(AssignmentDTO assignment)
    {
        
        Assignment assignmentMapped = _mapper.Map<Assignment>(assignment); 
        Assignment assignmentCreated = await _assignmentRepository.Create(assignmentMapped);
        
        return assignmentCreated;
     
    }
}