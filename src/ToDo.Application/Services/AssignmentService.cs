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
    


    public async Task<Assignment> CreateTask(AssignmentDTO assignmentDto)
    {
        var assignment = _mapper.Map<Assignment>(assignmentDto);

        var assignmentcreated = await _assignmentRepository.Create(assignment);
        return assignmentcreated;
    }
    
    
    public async Task<List<Assignment>> GetTasks(long userid, long listid)
    {
        var list = await _assignmentRepository.GetAll();
        list = list.Where(a => a.UserId == userid && a.AtListId == listid).ToList();
        return list;
    }

    public async Task<Assignment?> GetTaskById(SearchAssignmentDTO dto)
    {
        var listassignment =  await GetTasks(dto.UserId, dto.ListId);
        var assignment = listassignment.Where(a => a.Id == dto.Id).ToList().FirstOrDefault();
        if (assignment is not null)
        {
            return assignment;
        }

        throw new Exception();
    }
}