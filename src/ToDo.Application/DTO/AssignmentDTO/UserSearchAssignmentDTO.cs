using ToDo.Domain.Entities;

namespace ToDo.Application.DTO;

public class UserSearchAssignmentDTO
{
    public long Id { get; set; }
    public long ListId { get; set; }
    public long UserId = AuthenticatedUser.Id;
}