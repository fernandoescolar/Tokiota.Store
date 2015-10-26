namespace Tokiota.Store.Domain.Catalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Data;
    using Model;

    internal class CatalogService : ICatalogService
    {
        private readonly ICatalogDbContext context;

        public CatalogService(ICatalogDbContext context)
        {
            this.context = context;
        }

        public ICallResult CreateProduct(Product product)
        {
            var result = new CallResult { Succeeded = false };
            product.Id = Guid.NewGuid().ToString();

            try
            {
                this.context.Products.Add(product);
                this.context.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.AddErrors(ex);
            }

            return result;
        }

        public ICallResult UpdateProduct(Product product)
        {
            var result = new CallResult { Succeeded = false };
            try
            {
                this.context.Products.Attach(product);
                this.context.Entry(product).State = EntityState.Modified;
                this.context.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.AddErrors(ex);
            }

            return result;
        }

        public IEnumerable<Product> GetProducts(int pageSize, int page, out int total)
        {
            total = this.context.Products.Count();
            return this.context.Products
                                .OrderBy(u => u.Name).Skip(page * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<Product> SearchProducts(string category, string word, int pageSize, int page, out int total)
        {
            IEnumerable<Product> query = this.context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.ToString() == category);
            }
            if (!string.IsNullOrEmpty(word))
            {
                query = query.Where(p => p.Name.ToLower().Contains(word.ToLower()) || p.Description.ToLower().Contains(word.ToLower()));
            }

            total = query.Count();
            return query.OrderBy(u => u.Name).Skip(page * pageSize).Take(pageSize).ToList();
        }

        public Product GetProduct(string id)
        {
            return this.context.Products
                                .FirstOrDefault(p => p.Id == id);
        }
    }
}
