namespace Tokiota.Store.Models.Product
{
    using System.ComponentModel.DataAnnotations;
    using Resources;
    using System.Web;

    public class EditProductModel 
    {
        public string Id { get; set; }

        [Display(ResourceType = typeof(LanguageResources), Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Html)]
        public string Description { get; set; }

        [DataType("Category")]
        public string Category { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public string ImageUrl { get; set; }
    }
}
