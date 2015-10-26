namespace Tokiota.Store.Models
{
    public interface IMapper<TSource, TTarget>
    {
        TTarget Map(TSource source);

        TSource Map(TTarget source);
    }
}