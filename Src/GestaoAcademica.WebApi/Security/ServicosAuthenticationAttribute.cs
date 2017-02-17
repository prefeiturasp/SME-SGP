using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GestaoAcademica.WebApi.Security
{
    public class ServicosAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        protected string Username { get; set; }
        protected string Password { get; set; }

        public ServicosAuthenticationAttribute(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            var req = filterContext.Request;
            IEnumerable<string> pass;            
            if (req.Headers.TryGetValues(Username, out pass))
            {
                string password = pass.FirstOrDefault();
                if (password == Password)
                    return;
            }

            filterContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}