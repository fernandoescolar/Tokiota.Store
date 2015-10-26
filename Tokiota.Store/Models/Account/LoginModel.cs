namespace Tokiota.Store.Models.Account
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [Required]
        [Display(ResourceType = typeof (LanguageResources), Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (LanguageResources), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof (LanguageResources), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}