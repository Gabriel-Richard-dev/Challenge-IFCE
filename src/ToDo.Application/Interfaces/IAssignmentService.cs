using ToDo.Application.DTO;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IAssignmentService
{
    public Task<Assignment> CreateTask(AssignmentDTO assignment);
}