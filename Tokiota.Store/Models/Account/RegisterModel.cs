namespace Tokiota.Store.Models.Account
{
    using Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel
    {
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


        [Required]
        [Display(ResourceType = typeof(LanguageResources), Name = "AdditionalLastName")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResources), ErrorMessageResourceName = "StringLengthErrorMessage", MinimumLength = 2)]
        public string AdditionalLastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<SelectableItemModel> Roles { get; set; } 
    }
}
