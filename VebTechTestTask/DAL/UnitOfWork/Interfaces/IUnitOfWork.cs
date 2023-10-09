namespace VebTechTestTask.DAL.UnitOfWork.Interfaces
{
    using System.Data;

    using Entities;
    
    using Repositories.Interfaces;

    public interface IUnitOfWork : IDisposable
    {
        IReadWriteRepository<Role, int> RoleRepository {  get; }

        IReadWriteRepository<User, int> UserRepository { get; }

        IReadWriteRepository<UserLink, int> UserLinkRepository { get; }

        Task SaveAsync();
    
        ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}