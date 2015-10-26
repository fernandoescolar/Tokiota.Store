namespace Tokiota.Store
{
    using Domain.Identity.Model;
    using Domain.Identity.Services;
    using Infrastructure.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<UserManager<ApplicationUser>>(CreateUserService);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager<ApplicationUser>, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => {
                            return Task.FromResult(default(ClaimsIdentity));
                        }
                    )
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "63d1bb01-a0da-4729-8bd3-5e70c5311606",
            //    clientSecret: "NUFF6wsLphFV66CYj1x1PZzf7h/w8I74sl9x4rzaPVE=");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        private static UserManager<ApplicationUser> CreateUserService(IdentityFactoryOptions<UserManager<ApplicationUser>> options, IOwinContext context)
        {
            var service = TokiotaConfig.Resolve<IUserService>();
            var manager = service as UserManager<ApplicationUser>;
            if (manager != null)
            {
                var dataProtectionProvider = options.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
                }

                return manager;
            }

            return null;
        }

        public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
        {
            public ApplicationSignInManager(UserManager<ApplicationUser> userManager, IAuthenticationManager authenticationManager)
                : base(userManager, authenticationManager)
            {
            }

            public override async Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
            {
                var service = TokiotaConfig.Resolve<IUserService>();
                var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                var roles = await service.GetRolesAsync(identity.GetUserId());
                if (roles == null || roles.Count == 0) roles = new List<string> { TokiotaStoreRoles.User };
                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, "Tokiota.Store"));
                }

                return identity;
            }

            public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
            {
                return new ApplicationSignInManager(context.GetUserManager<UserManager<ApplicationUser>>(), context.Authentication);
            }
        }
    }
}