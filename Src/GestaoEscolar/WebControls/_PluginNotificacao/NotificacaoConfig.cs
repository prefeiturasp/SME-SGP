using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreSSO.WebControls.PluginNotificacao
{
    public class NotificacaoConfig
    {
        public string UrlNotificationAPI { get; set; }

        public string UrlNotificationSignalR { get; set; }

        public string IDSToken { get; set; }

        public string UserId { get; set; }

        public NotificacaoConfig()
        { }

        public NotificacaoConfig(NotificacaoConfig entity)
        {
            this.UrlNotificationAPI = entity.UrlNotificationAPI;
            this.UrlNotificationSignalR = entity.UrlNotificationSignalR;
        }
    }
}