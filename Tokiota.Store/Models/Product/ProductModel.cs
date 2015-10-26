namespace Tokiota.Store.Models.Product
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class ProductModel
    {
        public string Id { get; set; }

        [Display(ResourceType = typeof(LanguageResources), Name = "Name")]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [DataType("Category")]
        public string Category { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }
    }
}