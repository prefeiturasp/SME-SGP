using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GestaoAcademica.WebApi.Authentication
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        private const string SCHEME = "Basic";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers != null && actionContext.Request.Headers.Authorization != null && actionContext.Request.Headers.Authorization.Scheme.Equals(SCHEME))
            {
                base.OnActionExecuting(actionContext);
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}