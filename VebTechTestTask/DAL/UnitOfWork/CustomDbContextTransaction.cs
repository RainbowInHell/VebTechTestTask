using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VebTechTestTask.DAL.UnitOfWork.Interfaces;

namespace VebTechTestTask.DAL.UnitOfWork
{
    public class CustomDbContextTransaction : ITransaction
    {
        private bool isDisposed;
        private readonly IDbContextTransaction dbContextTransaction;

        public CustomDbContextTransaction(DbContext context, IsolationLevel isolatedLevel)
        {
            dbContextTransaction = context.Database.BeginTransaction(isolatedLevel);
            DbTransaction = dbContextTransaction.GetDbTransaction();
            isDisposed = false;
        }

        public IDbTransaction DbTransaction { get; }

        public void Commit()
        {
            dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            dbContextTransaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    dbContextTransaction.Dispose();
                }
            }

            isDisposed = true;
        }
    }
}