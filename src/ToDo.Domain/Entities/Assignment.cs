using ToDo.Domain.Contracts;
using ToDo.Domain.Validators;

namespace ToDo.Domain.Entities;

public class Assignment : Base
{
    protected Assignment() { }
    
    public Assignment(long id ,string title, string description, long userId,
        long atListid, bool concluded,
        DateTime dateConcluded, DateTime deadline)
    {
        Id = id;
        Title = title;
        Description = description;
        UserId = userId;
        AssignmentListId = atListid;
        Concluded = concluded;
        DateConcluded = dateConcluded;
        Deadline = deadline;
        Validation();
    }

    #region props

        public string Title { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public long AssignmentListId { get; set; }
        public bool Concluded { get; set; }
        public DateTime? DateConcluded { get; set; }
        public DateTime? Deadline { get; set; }


        public User User { get; set; } = null!;
        public AssignmentList AssignmentList { get; set; } = null!;


    #endregion
    
    public override bool Validation()
    {
        var validator = new  AssignmentValidator().Validate(this);
        return validator.IsValid;
    }
}