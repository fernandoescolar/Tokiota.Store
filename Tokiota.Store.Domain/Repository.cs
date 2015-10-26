namespace Tokiota.Store.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class Repository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class, IEntity<TKey>
    {
        private readonly IDbContext context;
        private bool isDisposed;

        protected Repository(IDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TEntity> GetList()
        {
            return this.GetEntitySetForList().ToList();
        }

        public Task<List<TEntity>> GetListAsync()
        {
            return this.GetEntitySetForList().ToListAsync();
        }

        public TEntity FindById(TKey id)
        {
            return this.GetEntitySetForOne().FirstOrDefault(WhereByIdClausule(id));
        }

        public Task<TEntity> FindByIdAsync(TKey id)
        {
            return this.GetEntitySetForOne().FirstOrDefaultAsync(WhereByIdClausule(id));
        }

        public TEntity Find(Expression<Func<TEntity, bool>> condition)
        {
            return this.GetEntitySetForList().FirstOrDefault(condition);
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> condition)
        {
            return this.GetEntitySetForList().FirstOrDefaultAsync(condition);
        }

        public void Create(TEntity entity)
        {
            this.OnCreating(entity);
            this.context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            this.OnUpdating(entity);
            this.context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            this.OnDeleting(entity);
            this.context.SaveChanges();
        }

        public Task CreateAsync(TEntity entity)
        {
            this.OnCreating(entity);
            return this.context.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            this.OnUpdating(entity);
            return this.context.SaveChangesAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            this.OnDeleting(entity);
            return this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void OnCreating(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
        }

        protected virtual void OnUpdating(TEntity entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }

        protected virtual void OnDeleting(TEntity entity)
        {
            this.context.Set<TEntity>().Remove(entity);
        }

        protected virtual IQueryable<TEntity> GetEntitySetForList()
        {
            return this.context.Set<TEntity>();
        }

        protected virtual IQueryable<TEntity> GetEntitySetForOne()
        {
            return this.context.Set<TEntity>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    //release unamaged resources
                }

                this.context.Dispose();
                this.isDisposed = true;
            }
        }

        protected static Expression<Func<TEntity, bool>> WhereByIdClausule(TKey id)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var property = Expression.Property(parameter, "Id");
            var equalsTo = Expression.Constant(id);
            var equality = Expression.Equal(property, equalsTo);

            return Expression.Lambda<Func<TEntity, bool>>(equality, new[] { parameter });
        }
    }
}
