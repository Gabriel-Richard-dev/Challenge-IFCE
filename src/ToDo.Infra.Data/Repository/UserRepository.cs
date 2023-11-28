using System;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Contracts.Repository;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ToDoContext context) : base(context)
    {
    }
    
    public async Task<User> GetByEmail(string email)
    {
        var list = await _context.Users.ToListAsync();
        var userexists = list.Where(u => u.Email == email).ToList();
        if (userexists.FirstOrDefault() is not null)
        {
            var user = _context.Users.Where(u => u.Email == email).ToListAsync().Result;
            return user.FirstOrDefault();
        }

        throw new Exception();
    }

    public async Task<bool> LoginValid(string email, string password)
    {
        
        var userExists = await _context.Users.Where(u => u.Email == email && u.Password == password).ToListAsync();

        if (userExists.FirstOrDefault() is not null)
        {
            return true;
        }

        throw new Exception();
    }


}