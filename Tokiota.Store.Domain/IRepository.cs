namespace Tokiota.Store.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TKey, TEntity> where TEntity : class, IEntity<TKey>
    {
        IEnumerable<TEntity> GetList();

        Task<List<TEntity>> GetListAsync();

        TEntity FindById(TKey id);

        Task<TEntity> FindByIdAsync(TKey id);

        TEntity Find(Expression<Func<TEntity, bool>> condition);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> condition);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}