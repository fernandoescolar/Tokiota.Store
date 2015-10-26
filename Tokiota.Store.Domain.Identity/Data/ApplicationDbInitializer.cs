namespace Tokiota.Store.Domain.Identity.Data
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System.Data.Entity;
    using System.Diagnostics;

    internal class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> //DropCreateDatabaseAlways<ApplicationDbContext>
    {
        private static readonly string[] RoleNames = { TokiotaStoreRoles.Admin, TokiotaStoreRoles.User };
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        protected override void Seed(ApplicationDbContext context)
        {
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            foreach (var name in RoleNames)
            {
                this.CreateRoleIfNotExists(name);
            }

            this.CreateUserIfNotExists("admin", "123456", TokiotaStoreRoles.Admin);

            base.Seed(context);
        }

        private bool CreateRoleIfNotExists(string name)
        {
            var result = false;
            if (!this.roleManager.RoleExists(name))
            {
                var roleresult = this.roleManager.Create(new IdentityRole(name));
                if (roleresult.Succeeded)
                {
                    result = true;
                }
                else
                {
                    Trace.WriteLine("I can't create the role: '" + name + "'.");    
                }
            }

            return result;
        }

        private bool CreateUserIfNotExists(string name, string password, params string[] roles)
        {
            var result = false;
            var user = this.userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser{ UserName = name, Name = name };
                var userresult = this.userManager.Create(user, password);
                if (userresult.Succeeded)
                {
                    foreach (var role in roles)
                    {
                        var roleresult = this.userManager.AddToRole(user.Id, role);
                        if (!roleresult.Succeeded)
                        {
                            Trace.WriteLine("I can't add the user: '" + name + "' to the role: '" + role + "'.");
                        }
                    }
                    result = true;
                }
                else
                {
                    Trace.WriteLine("I can't create the user: '" + name + "'.");
                }
            }

            return result;
        }
    }
}
