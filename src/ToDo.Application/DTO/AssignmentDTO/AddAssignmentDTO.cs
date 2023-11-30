namespace ToDo.Application.DTO;

public class AddAssignmentDTO
{
    
    public string Title { get; set; }
    public string Description { get; set; }
    public long AtListId { get; set; }
    public bool Concluded { get; set; }
    public DateTime? DateConcluded { get; set; }
    public DateTime? DeadLine { get; set; }
}