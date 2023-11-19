using System;
using ToDo.Domain.Entities;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class UserRepository : BaseRepository<User>
{
    private readonly ToDoContext _context;
    public UserRepository(ToDoContext context) : base(context)
    {
        _context= context;
    } 
}