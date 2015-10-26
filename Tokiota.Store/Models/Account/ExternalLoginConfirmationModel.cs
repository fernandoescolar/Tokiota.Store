namespace Tokiota.Store.Models.Account
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginConfirmationModel
    {
        [Required]
        [Display(ResourceType = typeof (LanguageResources), Name = "Email")]
        public string Email { get; set; }
    }
}