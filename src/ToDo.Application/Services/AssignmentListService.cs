using AutoMapper;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Domain.Contracts.Repository;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services;

public class AssignmentListService : IAssignmentListService
{

    public AssignmentListService(IAssignmentListRepository assignmentListRepository, IMapper mapperr)
    {
        _assignmentListRepository = assignmentListRepository;
        _mapper = mapperr;
    }

    private readonly IAssignmentListRepository _assignmentListRepository;
    private readonly IMapper _mapper;
    public async Task<List<AssignmentList>> GetAllLists(long userid)
    {
        var list = await _assignmentListRepository.GetAll();
        var tasklist = list.Where(l => l.UserId == userid).ToList();

        return list;
    }

    public async Task<AssignmentList> CreateList(AssignmentListDTO assignmentDto)
    {
        var assignment = _mapper.Map<AssignmentList>(assignmentDto);
        
        var assignmentcreated = await _assignmentListRepository.Create(assignment);
        return assignmentcreated;
    }

    public async Task<AssignmentList> GetListById(SearchAssignmentListDTO search)
    {
        var list = await _assignmentListRepository.GetAll();
        var assignment = list.Where(l => l.Id == search.Id).ToList();
        return assignment.FirstOrDefault();
    }
}