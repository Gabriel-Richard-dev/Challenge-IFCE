using System;
using ToDo.Domain.Entities;
using ToDo.Domain.Contracts.Repository;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{

    public UserRepository(ToDoContext context) : base(context)
    { }

    public override async Task<User> Create(User user)
    {
        user.Validation();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}