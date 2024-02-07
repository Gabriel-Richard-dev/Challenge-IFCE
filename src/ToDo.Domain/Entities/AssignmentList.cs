using System.Collections.ObjectModel;
using FluentValidation.Results;
using ToDo.Domain.Validators;

namespace ToDo.Domain.Entities;

public class AssignmentList : Base
{
    
    public AssignmentList() { }
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

     
    #endregion

    public override List<string> Validation()
    {
        var validator = new  AssignmentListValidator().Validate(this);
        return new List<string>();
    }
}