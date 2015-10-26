namespace Tokiota.Store.Domain.Identity.Configuration
{
    using Data;
    using Infrastructure;
    using Microsoft.AspNet.Identity;
    using Services;
    using System.Data.Entity;
    using Model;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class IdentityModule : IModule
    {
        public void Register(IBuilder builder)
        {
            builder.Register<IApplicationDbContext, ApplicationDbContext>();
            builder.Register<DbContext, ApplicationDbContext>();
            builder.Register<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            builder.Register<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            builder.Register<IUserRepository, UserRepository>();
            builder.Register<IUserService, UserService>();
            builder.Register<IEmailService, EmailService>();
            builder.Register<IIdentityMessageService, EmailService>();

            Database.SetInitializer(new ApplicationDbInitializer());
        }
    }
}
