using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using GestaoEscolar.Api.Models;

namespace GestaoEscolar.Api.Areas.HelpPage
{
    /// <summary>
    /// Use this class to customize the Help Page.
    /// For example you can set a custom <see cref="System.Web.Http.Description.IDocumentationProvider"/> to supply the documentation
    /// or you can provide the samples for the requests/responses.
    /// </summary>
    public static class HelpPageConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //// Uncomment the following to use the documentation from XML documentation file.
             config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/XmlDocument.xml")));

            //// Uncomment the following to use "sample string" as the sample for all actions that have string as the body parameter or return type.
            //// Also, the string arrays will be used for IEnumerable<string>. The sample objects will be serialized into different media type 
            //// formats by the available formatters.
             config.SetSampleObjects(new Dictionary<Type, object>
            {
                {typeof(string), "sample string"},
                {typeof(IEnumerable<string>), new string[]{"sample 1", "sample 2"}}
            });

            //// Uncomment the following to use "[0]=foo&[1]=bar" directly as the sample for all actions that support form URL encoded format
            //// and have IEnumerable<string> as the body parameter or return type.
            //config.SetSampleForType("[0]=item1&[1]=item2", new MediaTypeHeaderValue("application/x-www-form-urlencoded"), typeof(string));

             Type[] types = { typeof(Exemplo)/*add more here*/ };

             foreach (Type t in types)
             {
                 List<string> propExample = new List<string>();
                 foreach (var p in t.GetProperties())
                 {
                     propExample.Add(p.Name + "=value");
                 }

                 config.SetSampleForType(string.Join("&", propExample), new MediaTypeHeaderValue("application/x-www-form-urlencoded"), t);
             }

            //// Uncomment the following to use "1234" directly as the request sample for media type "text/plain" on the controller named "Values"
            //// and action named "Put".
            //config.SetSampleRequest("1234", new MediaTypeHeaderValue("text/plain"), "Exemplo", "Put");

            //// Uncomment the following to use the image on "../images/aspNetHome.png" directly as the response sample for media type "image/png"
            //// on the controller named "Values" and action named "Get" with parameter "id".
            //config.SetSampleResponse(new ImageSample("../images/aspNetHome.png"), new MediaTypeHeaderValue("image/png"), "Exemplo", "Get", "id");

            //// Uncomment the following to correct the sample request when the action expects an HttpRequestMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Get" were having string as the body parameter.
            //config.SetActualRequestType(typeof(string), "Exemplo", "Get");

            //// Uncomment the following to correct the sample response when the action returns an HttpResponseMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Post" were returning a string.
            //config.SetActualResponseType(typeof(string), "Exemplo", "Post");
        }
    }
}