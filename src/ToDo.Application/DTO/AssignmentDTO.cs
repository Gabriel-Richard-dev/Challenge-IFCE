namespace ToDo.Application.DTO;

public class AssignmentDTO
{
    
    public long Id { get; set; }
    public string Title { get; set; }
    public long UserId { get; set; }
    public long AtListId { get; set; }
    public bool Concluded { get; set; }
    public DateTime DateConcluded { get; set; }
    public DateTime DeadLine { get; set; }
    
}