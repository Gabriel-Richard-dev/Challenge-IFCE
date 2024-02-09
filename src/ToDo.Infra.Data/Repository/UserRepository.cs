using System;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Contracts.Repository;
using ToDo.Infra.Data.Context;
using ToDo.Core.Exceptions;

namespace ToDo.Infra.Data.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ToDoContext context) : base(context)
    { }
    
    public override async Task Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<User?> GetByEmail(string email)
    {
       
        var user = await _context.Users.Where(u => u.Email == email).AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync();
        if (user is not null)
        {
            return user;
        }

        return null;

    }

    public async Task<bool> LoginValid(string email, string password)
    {
        
        var userExists = await _context.Users
            .Where(u => u.Email == email && u.Password == password).AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        if (userExists.FirstOrDefault() is not null)
        {
            return true;
        }

        return false;
    }


}