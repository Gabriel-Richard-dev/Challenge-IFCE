using System.Collections.ObjectModel;
using FluentValidation;
using ToDo.Domain.Contracts;
using ToDo.Domain.Validators;

namespace ToDo.Domain.Entities;

public class User : Base
{
    protected User(){ }
    public User(long id, string name, string email, string password, bool adminPrivileges)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        AdminPrivileges = adminPrivileges;
        Validation();
    }

    #region props

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool AdminPrivileges { get; set; }
  
    #endregion

    public void AtualizaPassword(string pass, string confirmpass, string newpassword)
    {
        if (pass.Equals(confirmpass))
        {
            Password = newpassword;
            Validation();
            return;
        }

        throw new Exception();
    }

    public override bool Validation()
    {
        var validator = new  UserValidator().Validate(this);
        if (!validator.IsValid)
        {
            throw new Exception();
        }

        return validator.IsValid;
    }
    
    

}