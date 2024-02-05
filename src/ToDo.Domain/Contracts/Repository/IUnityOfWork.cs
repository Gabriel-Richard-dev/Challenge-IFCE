namespace ToDo.Domain.Contracts.Repository;

public interface IUnityOfWork
{
    Task<bool> Commit();
}