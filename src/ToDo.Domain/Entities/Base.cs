using ToDo.Domain.Contracts;
using System.Collections.Generic;

namespace ToDo.Domain.Entities;

public abstract class Base : IBaseEntity
{
    public long Id { get; set; }
    public abstract bool Validation();

    internal List<string>? _erros;
    public IReadOnlyCollection<string>? Erros => _erros;
}