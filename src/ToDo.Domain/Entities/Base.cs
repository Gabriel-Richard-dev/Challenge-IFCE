using ToDo.Domain.Contracts;

namespace ToDo.Domain.Entities;

public abstract class Base : IBaseEntity
{ 
    public long Id { get; set; }
    public abstract bool Validation();
}