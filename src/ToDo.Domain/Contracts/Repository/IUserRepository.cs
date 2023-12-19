using System;
using ToDo.Domain.Entities;
using ToDo.Domain.Contracts.Repository;


namespace ToDo.Domain.Contracts.Repository;

public interface IUserRepository: IBaseRepository<User>
{
    Task<bool> LoginValid(string email, string password);
    Task<User?> GetByEmail(string email);
}