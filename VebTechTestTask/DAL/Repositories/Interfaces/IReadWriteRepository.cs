namespace VebTechTestTask.DAL.Repositories.Interfaces
{
    using Entities.Interfaces;

    public interface IReadWriteRepository<TEntity, in TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        TEntity AddOrUpdate(TEntity entity);

        void AddOrUpdate(IEnumerable<TEntity> entities);

        void Delete(TKey id);

        void Delete(TEntity entity);
    }
}