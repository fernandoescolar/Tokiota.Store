namespace Tokiota.Store.Domain.Catalog.Data
{
    using Model;
    using System.Data.Entity;

    internal interface ICatalogDbContext : IDbContext
    {
        IDbSet<Product> Products { get; set; }
    }
}