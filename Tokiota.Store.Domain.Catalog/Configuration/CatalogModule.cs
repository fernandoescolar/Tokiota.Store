namespace Tokiota.Store.Domain.Catalog.Configuration
{
    using Data;
    using Infrastructure;
    using Services;
    using System.Data.Entity;

    public class CatalogModule : IModule
    {
        public void Register(IBuilder builder)
        {
            builder.Register<ICatalogDbContext, CatalogDbContext>();
            builder.Register<ICatalogService, CatalogService>();
            builder.Register<IImageStorageService, ImageStorageService>();
            builder.Register<ITopListService, TopListService>();

            Database.SetInitializer(new CatalogDbInitializer());
        }
    }
}
