namespace VebTechTestTask.DAL.UnitOfWork
{
    using DAL.UnitOfWork.Interfaces;

    using Entities;

    using Repositories;
    using Repositories.Interfaces;
    using System.Data;

    public class UnitOfWork : IUnitOfWork
    {
        private VebTechTestTaskDbContext context;

        private bool isDisposed;

        private IReadWriteRepository<Role, int> roleRepository;

        private IReadWriteRepository<User, int> userRepository;

        private IReadWriteRepository<UserLink, int> userLinkRepository;

        public UnitOfWork(VebTechTestTaskDbContext context)
        {
            this.context = context;

            isDisposed = false;
        }

        public IReadWriteRepository<Role, int> RoleRepository => roleRepository ?? (roleRepository = new BaseReadWriteRepository<Role, int>(context));

        public IReadWriteRepository<User, int> UserRepository => userRepository ?? (userRepository = new BaseReadWriteRepository<User, int>(context));

        public IReadWriteRepository<UserLink, int> UserLinkRepository => userLinkRepository ?? (userLinkRepository = new BaseReadWriteRepository<UserLink, int>(context));

        public async Task SaveAsync()
        {
            do
            {
                try
                {
                    await context.SaveChangesAsync();
                    break;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            while (true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    context = null;
                }
            }

            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            return new CustomDbContextTransaction(context, isolationLevel);
        }
    }
}