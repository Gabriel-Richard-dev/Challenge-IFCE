using AutoMapper;
using ToDo.Application.DTO;
using ToDo.Application.Interfaces;
using ToDo.Core.Exceptions;
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
        var taskList = list.Where(l => l.UserId == userid).ToList();

        return taskList;
    }

    public async Task<AssignmentList> CreateList(AssignmentListDTO assignmentDto)
    {
        var assignment = _mapper.Map<AssignmentList>(assignmentDto);
        assignment.Validation();
        var assignmentCreated = await _assignmentListRepository.Create(assignment);
        if (await CommitChanges())
            return assignmentCreated;
        throw new ToDoException();
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
            _assignmentListRepository.Delete(atListExists.Id);
            if (await CommitChanges())
                return atListExists;
        }

        throw new ToDoException();


    }
    
    
    async Task<bool> CommitChanges() => await _assignmentListRepository.UnityOfWork.Commit() ? true : false;

}