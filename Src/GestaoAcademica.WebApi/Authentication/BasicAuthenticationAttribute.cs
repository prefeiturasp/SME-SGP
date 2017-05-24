namespace GestaoAcademica.WebApi.Authentication
{
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Atributo para indicar se será realizada a autenticação básica da API.
    /// </summary>
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        private const string SCHEME = "Basic";

        bool Active;

        public BasicAuthenticationAttribute()
        {
            Active = true;
        }

        public BasicAuthenticationAttribute(bool active)
        {
            Active = active;
        }

        /// <summary>
        /// Realiza a autenticação do usuário.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private bool Autenticar(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers != null && actionContext.Request.Headers.Authorization != null && actionContext.Request.Headers.Authorization.Scheme.Equals(SCHEME))
            {
                try
                {
                    string credenciais = actionContext.Request.Headers.Authorization.Parameter;

                    if (!string.IsNullOrWhiteSpace(credenciais))
                    {
                        string[] parametros = Encoding.UTF8.GetString(Convert.FromBase64String(credenciais)).Split(':');
                        SYS_UsuarioAPI entityUsuarioAPI = new SYS_UsuarioAPI
                        {
                            uap_usuario = parametros[0].Trim()
                            ,
                            uap_senha = parametros[1].Trim()
                        };
                        return SYS_UsuarioAPIBO.AutenticarUsuario(entityUsuarioAPI);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Chamada feita antes da ação do controller.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            bool autenticar = actionContext.ActionDescriptor.GetCustomAttributes<BasicAuthenticationAttribute>().Any() ?
                actionContext.ActionDescriptor.GetCustomAttributes<BasicAuthenticationAttribute>()[0].Active : Active;
            if (autenticar)
            {
                if (Autenticar(actionContext))
                {
                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { message = "Credenciais inválidas" });
                }
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }
    }
}