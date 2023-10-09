namespace VebTechTestTask.DAL.Repositories
{
    using Entities.Interfaces;
    
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class BaseReadWriteRepository<TEntity, TKey> : BaseReadOnlyRepository<TEntity, TKey>, IReadWriteRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public BaseReadWriteRepository(VebTechTestTaskDbContext context)
            : base(context)
        {
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual TEntity AddOrUpdate(TEntity entity)
        {
            if (EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey)))
            {
                Add(entity);
            }
            else
            {
                Update(entity);
            }

            return entity;
        }

        public virtual void AddOrUpdate(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                AddOrUpdate(entity);
            }
        }

        public virtual void Delete(TKey id)
        {
            var entityToDelete = Get(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }
    }
}