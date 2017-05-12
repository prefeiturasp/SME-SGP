using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MSTech.Rest.Core.Formatter;
using MSTech.Rest.Core.Security;
using MSTech.Rest.Core.Compress;
using GestaoAcademica.WebApi.Security;
using System.Web.Http.Description;
using GestaoAcademica.WebApi.Documentations;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace GestaoAcademica.WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Chave de Configuração.
            MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.Config = MSTech.GestaoEscolar.BLL.eConfig.Academico;

            // seta a autenticação
            //GlobalConfiguration.Configuration.MessageHandlers.
            //    Add(new BasicAuthMessageHandler()
            //    {
            //        PrincipalProvider = new PrincipalProvider()
            //    });

            // removendo o retorno padrao em XML, força retorno em Json
            var appXmlType = GlobalConfiguration.Configuration.Formatters.XmlFormatter.
                SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.
                SupportedMediaTypes.Remove(appXmlType);

            // identa o retorno json
            GlobalConfiguration.Configuration.Formatters.
                JsonFormatter.Indent = true;

            // Ignora a marcação de Serializable das entidades, evitando sujeira ao serializar para json.
            ((DefaultContractResolver)(GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver)).IgnoreSerializableAttribute = true;

            // Ignora valores nulos na serialização.
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            /// rota default
            //GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            //    "defaultHttpRoute", routeTemplate: "v1/{controller}/{id}"
            //    );

            //Add
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "WithActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultV1",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            // força retorno em formato json
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.
                MediaTypeMappings.Add(
                new QueryStringMapping("format", "json", "application/json")
                );

            // força retorno em formato xml
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.
                MediaTypeMappings.Add(
                new QueryStringMapping("format", "xml", "application/xml")
                );

            GlobalConfiguration.Configuration.Formatters.Add(
                new CSVMediaTypeFormatter(
                    new QueryStringMapping("format", "csv", "text/csv")
                    ));

            // documentacao utilizando os comentarios
            GlobalConfiguration.Configuration.Services.Replace(typeof(IDocumentationProvider),
                new XmlCommentDocumentationProvider(HttpContext.Current.Server.MapPath("~/Areas/HelpPage/Data/GestaoAcademica.WebApi.XML")));

            //compressão
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CompressHandler());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


    }
}