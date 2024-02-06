using ToDo.Domain.Contracts;
using System.Collections.Generic;
using FluentValidation.Results;

namespace ToDo.Domain.Entities;

public abstract class Base : IBaseEntity
{
    public long Id { get; set; }
    public abstract List<ValidationFailure> Validation();

    internal List<string>? _erros;
    
}