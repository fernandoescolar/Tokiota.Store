namespace Tokiota.Store.Domain.Catalog.Data
{
    using Model;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    internal class CatalogDbContext : DbContext, ICatalogDbContext
    {
        public CatalogDbContext()
            : base("DefaultConnection")
        {
        }

        public IDbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}

