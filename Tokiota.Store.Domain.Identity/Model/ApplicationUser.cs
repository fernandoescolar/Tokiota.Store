namespace Tokiota.Store.Domain.Identity.Model
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string AdditionalLastName { get; set; }
    }
}