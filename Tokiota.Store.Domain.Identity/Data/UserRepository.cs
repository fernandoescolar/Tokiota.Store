namespace Tokiota.Store.Domain.Identity.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    internal class UserRepository : UserStore<ApplicationUser>, IUserRepository
    {
        private readonly IApplicationDbContext context;

        public UserRepository(IApplicationDbContext context) : base(context as DbContext)
        {
            this.context = context;
        }

        public IEnumerable<ApplicationUser> GetAllUsers(int pageSize, int page, out int total)
        {
            total = this.context.Users.Count();
            return this.context.Users.OrderBy(u => u.LastName).Skip(page * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<string> GetRoleNames()
        {
            return this.context.Roles.Select(r => r.Name).OrderBy(r => r).ToList();
        }
    }
}
