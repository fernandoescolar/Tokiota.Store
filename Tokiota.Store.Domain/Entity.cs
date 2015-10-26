namespace Tokiota.Store.Domain
{
    public class Entity<T> : IEntity<T>
    {
        public T Id { get; protected set; }
    }
}
