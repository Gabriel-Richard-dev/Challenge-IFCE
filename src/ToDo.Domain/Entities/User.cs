using System.Collections.ObjectModel;
using FluentValidation;
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
        var validator = new UserValidator();
        var validation = validator.Validate(this);

        if (!validation.IsValid)
        {
            foreach (var erros in validation.Errors)
                _erros.Add(erros.ErrorMessage);

            throw new ToDoException("Campo inválido, corrija-os.");
        }

        return true;
    }
    
    
    public string IsAdministrator()
    {
        if(AdminPrivileges == false)
        {
            return "false";
        }
        return "true";
    }




}