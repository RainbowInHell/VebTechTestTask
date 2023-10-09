namespace VebTechTestTask.DAL.Repositories.Interfaces
{
    using Entities.Interfaces;
    using System.Linq.Expressions;

    public interface IReadOnlyRepository<TEntity, in TKey> : IRepository
        where TEntity : IEntity<TKey>
    {
        TEntity Get(TKey id);

        IQueryable<TEntity> GetAsQueryable(bool noTracking = true);
    }
}