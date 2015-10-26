namespace Tokiota.Store.Helpers
{
    using Domain.Identity.Model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class RolesHelper
    {
        private static readonly string[] AllRoles = new[]{ TokiotaStoreRoles.Admin, TokiotaStoreRoles.User }; 
        public static bool UserIsInRole(string role)
        {
            return HttpContext.Current.User.IsInRole(role);
        }
        public static bool UserIsOnlyInRole(string role)
        {
            var roles = new List<string>(AllRoles);
            roles.Remove(role);

            return HttpContext.Current.User.IsInRole(role) && !roles.Any(UserIsInRole);
        }
    }
}