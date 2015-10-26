namespace Tokiota.Store.Domain.Identity.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System.Data.Entity;

    internal interface IApplicationDbContext : IDbContext
    {
        IDbSet<ApplicationUser> Users { get; set; }
        IDbSet<IdentityRole> Roles { get; set; }
    }
}