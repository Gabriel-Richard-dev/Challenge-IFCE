using FluentValidation.Results;
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



    #endregion
    
    public override List<ValidationFailure> Validation()
    {
        var validator = new  AssignmentValidator().Validate(this);
        return validator.Errors;
    }
}