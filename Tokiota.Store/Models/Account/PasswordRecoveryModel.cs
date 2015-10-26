namespace Tokiota.Store.Models.Account
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class PasswordRecoveryModel
    {
        [Required]
        [Display(ResourceType = typeof(LanguageResources), Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}