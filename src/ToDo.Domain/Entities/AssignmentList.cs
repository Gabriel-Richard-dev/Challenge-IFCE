using System.Collections.ObjectModel;
using ToDo.Domain.Validators;

namespace ToDo.Domain.Entities;

public class AssignmentList : Base
{
    protected AssignmentList() { }
    
    public AssignmentList(long id, string name, long userId, long listId)
    {
        Id = id;
        Name = name;
        UserId = userId;
        ListId = listId;
        Validation();
    }

    #region props
        public string Name { get; set; }
        public long UserId { get; set; }
        public long ListId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Collection<Assignment> Assignments { get; set; } = null!;
     
    #endregion

    public override bool Validation()
    {
        var validator = new  AssignmentListValidator().Validate(this);
        return validator.IsValid;
    }
}