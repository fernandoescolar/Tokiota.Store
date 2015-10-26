namespace Tokiota.Store.Domain.Identity.Services
{
    using Data;
    using Microsoft.AspNet.Identity;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class UserService : UserManager<ApplicationUser>, IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository, IIdentityMessageService mailService) : base(repository)
        {
            this.repository = repository;
            this.UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;
            this.RegisterTwoFactorProvider("Tokiota Store Auth", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            this.EmailService = mailService;
        }

        public IEnumerable<string> GetAllRoleNames()
        {
            return this.repository.GetRoleNames();
        }

        public IEnumerable<ApplicationUser> GetAllUsers(int pageSize, int page, out int total)
        {
            return this.repository.GetAllUsers(pageSize, page, out total);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password, string[] roles)
        {
            var result = await base.CreateAsync(user, password);
            if (result.Succeeded)
            {
                foreach (var role in roles)
                {
                    await base.AddToRoleAsync(user.Id, role);
                }
            }

            return result;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, string[] roles)
        {
            var errors = new List<string>();
            var r = await base.UpdateAsync(user);
            if (r.Succeeded)
            {
                
                var userRoles = await base.GetRolesAsync(user.Id);
                foreach (var role in roles.Where(rol => !userRoles.Contains(rol)))
                {
                    r = await base.AddToRoleAsync(user.Id, role);
                    if (!r.Succeeded)
                    {
                        errors.AddRange(r.Errors);
                    }
                }

                foreach (var role in userRoles.Where(rol => !roles.Contains(rol)))
                {
                    r = await base.RemoveFromRoleAsync(user.Id, role);
                    if (!r.Succeeded)
                    {
                        errors.AddRange(r.Errors);
                    }
                }
            }
            else
            {
                errors.AddRange(r.Errors);
            }

            return new IdentityResult(errors);
        }
    }
}
