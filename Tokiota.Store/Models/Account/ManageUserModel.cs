namespace Tokiota.Store.Models.Account
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class ManageUserModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (LanguageResources), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (LanguageResources), ErrorMessageResourceName = "StringLengthErrorMessage", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (LanguageResources), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (LanguageResources), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (LanguageResources), ErrorMessageResourceName = "ConfirmPasswordErrorMessage")]
        public string ConfirmPassword { get; set; }
    }
}