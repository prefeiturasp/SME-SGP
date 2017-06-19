using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    public static class UserIdentityExtension
    {
        public static string GetEntityId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.PrimarySid);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetUsuLogin(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetGrupoId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.GroupSid);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static void AddGrupoId(this IIdentity identity, HttpRequest request, string grupoId)
        {
            var identityUser = (ClaimsIdentity)identity;
            var claimGrupo = new Claim(ClaimTypes.GroupSid, grupoId);
            identityUser.AddClaim(claimGrupo);
            request.GetOwinContext().Authentication.SignIn(identityUser);
        }
    }

}
