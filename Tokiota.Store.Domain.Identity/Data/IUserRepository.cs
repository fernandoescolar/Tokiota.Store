namespace Tokiota.Store.Domain.Identity.Data
{
    using Microsoft.AspNet.Identity;
    using Model;
    using System.Collections.Generic;

    public interface IUserRepository : IUserStore<ApplicationUser>
    {
        IEnumerable<string> GetRoleNames();
        IEnumerable<ApplicationUser> GetAllUsers(int pageSize, int page, out int total);
    }
}
