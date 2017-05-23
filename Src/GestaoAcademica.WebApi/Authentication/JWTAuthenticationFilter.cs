namespace GestaoAcademica.WebApi.Authentication
{
    using Controllers.Base;
    using MSTech.CoreSSO.BLL;
    using MSTech.CoreSSO.Entities;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Security.Cryptography;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class JWTAuthenticationFilter : AuthorizationFilterAttribute
    {
        bool Active = true;

        public JWTAuthenticationFilter()
        { }

        /// <summary>
        /// Overriden constructor to allow explicit disabling of this
        /// filter's behavior. Pass false to disable (same as no filter
        /// but declarative)
        /// </summary>
        /// <param name="active"></param>
        public JWTAuthenticationFilter(bool active)
        {
            Active = active;
        }

        /// <summary>
        /// Override to Web API filter method to handle Basic Auth check
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Active)
            {
                var user = AuthHeader(actionContext);
                if (user == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return;
                }
                LoadUser(user, actionContext);
                base.OnAuthorization(actionContext);
            }
        }


        protected virtual void LoadUser(AuthenticationIdentity user, HttpActionContext actionContext)
        {
            if (HttpContext.Current != null)
            {
                BaseApiController baseApiController = actionContext.ControllerContext.Controller as BaseApiController;

                if (baseApiController != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_LoadUser(user);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        #region Load user values

                        UsuarioWEB userLogged = new UsuarioWEB();

                        // Carrega usuário na session através do ticket de authenticação
                        userLogged.Usuario = new SYS_Usuario
                        {
                            ent_id = user.Entity
                               ,
                            usu_login = user.Login
                        };
                        SYS_UsuarioBO.GetSelectBy_ent_id_usu_login(userLogged.Usuario);

                        userLogged.Grupo = SYS_GrupoBO.GetEntity(new SYS_Grupo { gru_id = user.Group });

                        baseApiController.__userLogged = userLogged;

                        #endregion

                        HttpContext.Current.Cache.Insert(chave, userLogged, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                            , System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        baseApiController.__userLogged = cache as UsuarioWEB;
                    }
                }
            }
        }

        private string RetornaChaveCache_LoadUser(AuthenticationIdentity entity)
        {
            return string.Format("LoadUserJWK_{0}_{1}_{2}", entity.Entity, entity.Login, entity.Group);
        }

        /// <summary>
        /// Parses the Authorization header and creates user credentials
        /// </summary>
        /// <param name="actionContext"></param>
        protected virtual AuthenticationIdentity AuthHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Bearer")
                authHeader = auth.Parameter;
            if (string.IsNullOrEmpty(authHeader))
                return null;

            var jwtKey = System.Configuration.ConfigurationManager.AppSettings["jwtKey"];
            SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
            authHeader = JWT.JsonWebToken.Decode(authHeader, sa.Decrypt(jwtKey));
            var user = JsonConvert.DeserializeObject<AuthenticationIdentity>(authHeader);

            return user;
        }

        public static T CopyObject<T>(object from, T to)
        {
            Type tp = to.GetType();
            PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] propertiesCarregada = from.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                PropertyInfo propCarregada = propertiesCarregada.ToList().Find(p => p.Name == prop.Name);
                if (propCarregada != null)
                {
                    prop.SetValue(to, propCarregada.GetValue(from), null);
                }
            }
            return to;
        }
    }

    public class AuthenticationIdentity
    {
        public string Login { get; set; }
        public Guid Entity { get; set; }
        public Guid Group { get; set; }
    }
}
