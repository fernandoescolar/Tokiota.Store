namespace Tokiota.Store.Domain.Identity.Services
{
    using Microsoft.AspNet.Identity;
    using Model;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType);
        Task<ApplicationUser> FindAsync(string userName, string password);
        Task<IdentityResult> AddPasswordAsync(string userId, string password);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<IList<string>> GetRolesAsync(string userId);
        IEnumerable<string> GetAllRoleNames();
        IEnumerable<ApplicationUser> GetAllUsers(int pageSize, int page, out int total);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password, string[] roles);
        Task<IdentityResult> UpdateAsync(ApplicationUser user, string[] roles);
    }
}