namespace Tokiota.Store.Models
{
    using Domain.Catalog.Model;
    using Product;
    using System;
    using ProductDomain = Domain.Catalog.Model.Product;

    public class Mapper : IMapper<ProductDomain, ProductModel>, IMapper<ProductDomain, EditProductModel>
    {
        ProductModel IMapper<ProductDomain, ProductModel>.Map(ProductDomain source)
        {
            return new ProductModel
            {
                Id = source.Id,
                Category = source.Category.ToString(),
                Description = source.Description,
                Name = source.Name,
                Image = source.Image,
                Price = source.Price
            };
        }

        ProductDomain IMapper<ProductDomain, ProductModel>.Map(ProductModel source)
        {
            return new ProductDomain
            {
                Id = source.Id,
                Category = (Category)Enum.Parse(typeof(Category), source.Category),
                Description = source.Description,
                Image = source.Image,
                Name = source.Name,
                Price = source.Price
            };
        }

        ProductDomain IMapper<ProductDomain, EditProductModel>.Map(EditProductModel source)
        {
            return new ProductDomain
            {
                Id = source.Id,
                Category = (Category)Enum.Parse(typeof(Category), source.Category),
                Description = source.Description,
                Image = source.ImageUrl,
                Name = source.Name,
                Price = source.Price
            };
        }

        EditProductModel IMapper<ProductDomain, EditProductModel>.Map(ProductDomain source)
        {
            return new EditProductModel
            {
                Id = source.Id,
                Category = source.Category.ToString(),
                Description = source.Description,
                Name = source.Name,
                ImageUrl = source.Image,
                Price = source.Price
            };
        }
    }
}