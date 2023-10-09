namespace VebTechTestTask.DAL.UnitOfWork.Interfaces
{
    using System.Data;

    public interface ITransaction : IDisposable
    {
        IDbTransaction DbTransaction { get; }

        void Commit();

        void Rollback();
    }
}