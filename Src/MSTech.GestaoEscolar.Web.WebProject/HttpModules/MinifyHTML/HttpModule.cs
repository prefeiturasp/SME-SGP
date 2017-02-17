using System;
using System.Web;

namespace MSTech.GestaoEscolar.Web.WebProject.HttpModules.MinifyHTML
{
    public class HttpModule : IHttpModule
    {
        void IHttpModule.Init(HttpApplication context)
        {
            //if (!context.Context.IsDebuggingEnabled)
            context.PostRequestHandlerExecute += ProcessResponse;
        }

        private void ProcessResponse(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null)
            {
                HttpContext context = app.Context;
                if (context != null)
                {
                    string contentType = context.Response.ContentType;
                    //if (app.Request.RawUrl.Contains(".aspx"))
                    //if (context.Request.HttpMethod == "GET" && contentType.Equals("text/html") && context.Response.StatusCode == 200 && context.CurrentHandler != null)
                    if (contentType.Equals("text/html"))
                    {
                        context.Response.Filter = new WhitespaceFilter(context.Response.Filter);
                    }
                }
            }
        }

        void IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }
    }
}