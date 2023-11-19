using System;
using ToDo.Domain.Entities;
using ToDo.Domain.Contracts.Repository;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly ToDoContext _context;
    public UserRepository(ToDoContext context) : base(context)
    {
        _context= context;
    } 
}