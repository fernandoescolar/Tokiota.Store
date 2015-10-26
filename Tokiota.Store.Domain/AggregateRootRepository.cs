namespace Tokiota.Store.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public abstract class AggregateRootRepository : Repository<Guid, IAggregateRoot>
    {
        private readonly IDbContext context;

        protected AggregateRootRepository(IDbContext context) : base(context)
        {
            this.context = context;
        }

        protected void UpdateCollection<TEntity, TKey>(ICollection<TEntity> source, ICollection<TEntity> target, Action<TEntity, TEntity> updateAction, Func<TEntity, bool> validation = null)
           where TEntity : class, IEntity<TKey>
        {
            this.AddOrModifyItems<TEntity, TKey>(source, target, updateAction);
            this.DeleteItems<TEntity, TKey>(source, target, validation);
        }

        private void AddOrModifyItems<TEntity, TKey>(ICollection<TEntity> source, ICollection<TEntity> target, Action<TEntity, TEntity> updateAction)
            where TEntity : class, IEntity<TKey>
        {
            foreach (var item in source)
            {
                var other = item.Id.Equals(default(TKey)) ? null : target.FirstOrDefault(e => e.Id.Equals(item.Id));
                if (other == null)
                {
                    target.Add(item);
                    this.context.Entry(item).State = EntityState.Added;
                }
                else
                {
                    this.context.Entry(other).State = EntityState.Modified;
                    updateAction(item, other);
                }
            }
        }

        private void DeleteItems<TEntity, TKey>(ICollection<TEntity> source, ICollection<TEntity> target, Func<TEntity, bool> validation = null)
            where TEntity : class, IEntity<TKey>
        {
            var toDelete = new List<TEntity>();
            foreach (var item in target)
            {
                if (item.Id.Equals(default(TKey)))
                {
                    continue;
                }

                var other = source.FirstOrDefault(e => e.Id.Equals(item.Id));
                if (other == null)
                {
                    if (validation != null && !validation(item))
                    {
                        throw new InvalidOperationException("You could not delete this entity or its dependencies");
                    }

                    toDelete.Add(item);
                }
            }

            foreach (var item in toDelete)
            {
                target.Remove(item);
                this.context.Entry(item).State = EntityState.Deleted;
            }
        }
    }
}
