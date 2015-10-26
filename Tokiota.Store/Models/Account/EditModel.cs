namespace Tokiota.Store.Models.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Resources;

    public class EditModel
    {
        public string Id { get; set; }

        [Required]
        [Display(ResourceType = typeof (LanguageResources), Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(ResourceType = typeof (LanguageResources), Name = "Name")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResources), ErrorMessageResourceName = "StringLengthErrorMessage", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [Display(ResourceType = typeof (LanguageResources), Name = "LastName")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResources), ErrorMessageResourceName = "StringLengthErrorMessage", MinimumLength = 2)]
        public string LastName { get; set; }

        public List<SelectableItemModel> Roles { get; set; } 
    }
}
