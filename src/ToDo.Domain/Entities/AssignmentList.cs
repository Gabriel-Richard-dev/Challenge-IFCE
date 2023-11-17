using ToDo.Domain.Validators;

namespace ToDo.Domain.Entities;

public class AssignmentList : Base
{
    protected AssignmentList() { }
    
    public AssignmentList(long id, string name, long userId)
    {
        Id = id;
        Name = name;
        UserId = userId;
        Validation();
    }

    #region props
        public string Name { get; set; }
        public long UserId { get; set; }
     
    #endregion

    public override bool Validation()
    {
        var validator = new  AssignmentListValidator().Validate(this);
        return validator.IsValid;
    }
}