using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;
using ToDo.Domain.Contracts;
using ToDo.Domain.Validators;
using ToDo.Core.Exceptions;

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
        _erros = new List<string>();

        Validation();

    }

    #region props

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool AdminPrivileges { get; set; }

    public Collection<AssignmentList> AssignmentLists { get; set; } = new();
    
    
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

    public override List<string?> Validation()
    {
        var validator = new UserValidator();
        var validation = validator.Validate(this);

        var listErros = new List<string>();
        
        
        foreach (var erro in validation.Errors)
        {
            listErros.Add(erro.ErrorMessage);
        }
        
        if(listErros is not null)
            return listErros;
        
        
        return null;
    }
    
    
    public string IsAdministrator()
    {
        if(AdminPrivileges == false)
        {
            return "false";
        }
        return "true";
    }

    public void AddFirstList()
    {
        AssignmentLists.Add(new AssignmentList()
        {
            Name = "Your List",
            ListId = 0,
            UserId = this.Id
        });
    }

}