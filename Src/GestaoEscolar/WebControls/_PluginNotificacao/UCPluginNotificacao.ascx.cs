using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreSSO.WebControls.PluginNotificacao
{
    public partial class UCPluginNotificacao : System.Web.UI.UserControl
    {
        private static NotificacaoConfig notifConfig = null;

        public string GetConfig
        {
            get {
                var cfg = new NotificacaoConfig(notifConfig);
                cfg.UserId = GetUserId();
                cfg.IDSToken = GetToken();
                return Newtonsoft.Json.JsonConvert.SerializeObject(cfg, Newtonsoft.Json.Formatting.None); }
        }
        
        private string GetToken()
        {
            var principal = this.Context.User as ClaimsPrincipal;

            var token = from c in principal.Identities.First().Claims
                        where c.Type == "access_token"
                        select c.Value;//.FirstOrDefault();

            return token.FirstOrDefault();
        }

        private string GetUserId()
        {
            var principal = this.Context.User as ClaimsPrincipal;

            var token = from c in principal.Identities.First().Claims
                        where c.Type == "sub"
                        select c.Value;//.FirstOrDefault();

            return token.FirstOrDefault();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (notifConfig == null)
            {
                var api = ConfigurationManager.AppSettings["UrlNotificationApi"];
                var signalR = ConfigurationManager.AppSettings["UrlNotificationSignalR"];

                notifConfig = new NotificacaoConfig()
                {
                    UrlNotificationAPI = api,
                    UrlNotificationSignalR = signalR                    
                };
            }
        }
    }
}