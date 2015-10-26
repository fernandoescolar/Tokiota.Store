namespace Tokiota.Store.Domain
{
    public interface IEntity<T>
    {
        T Id { get; }
    }
}