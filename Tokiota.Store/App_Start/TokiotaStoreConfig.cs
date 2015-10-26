namespace Tokiota.Store.App_Start
{
    using Domain.Catalog.Model;
    using Infrastructure;
    using Models;
    using Models.Product;
    using System.Web.Mvc;

    public class TokiotaStoreConfig : IModule
    {
        public void Register(IBuilder builder)
        {
            builder.RegisterAssemblyTypesAsSelf<ControllerBase>(this.GetType().Assembly);
            builder.RegisterAsSingleInstance<IMapper<Product, ProductModel>, Mapper>();
            builder.RegisterAsSingleInstance<IMapper<Product, EditProductModel>, Mapper>();
        }
    }
}
