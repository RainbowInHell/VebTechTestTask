namespace VebTechTestTask.DAL.Repositories
{
    using System.Linq.Expressions;
    
    using Entities.Interfaces;
    
    using Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class BaseReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public BaseReadOnlyRepository(VebTechTestTaskDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        protected VebTechTestTaskDbContext Context { get; }

        protected DbSet<TEntity> DbSet { get; }

        public virtual TEntity Get(TKey id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<TEntity> GetAsQueryable(bool noTracking = true)
        {
            return noTracking ? DbSet.AsNoTracking().AsQueryable() : DbSet.AsQueryable();
        }
    }
}