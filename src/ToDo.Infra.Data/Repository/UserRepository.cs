using System;
using Microsoft.EntityFrameworkCore;
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

    public override async Task<List<User>?> GetAll()
    {
        return await _context.Users.ToListAsync();
    }
    public override async Task<User?> GetById(long id)
    {
        var userexists = await _context.Users.FindAsync(id);
        if (userexists is not null)
        {
            var user = _context.Users.Where(u => u.Id == id).ToListAsync().Result;
            return user.FirstOrDefault();
        }

        throw new Exception();
    }
    
    public override async Task<User> GetByEmail(string email)
    {
        var userexists = await _context.Users.FindAsync(email);
        if (userexists is not null)
        {
            var user = _context.Users.Where(u => u.Email == email).ToListAsync().Result;
            return user.FirstOrDefault();
        }

        throw new Exception();
    }


    public override async Task Delete(long id)
    {
        var userExists = await GetById(id);
        if (userExists is not null)
        {
            _context.Users.Remove(userExists);
            await _context.SaveChangesAsync();
            return;
        }

        throw new Exception();
    }
    
    
}