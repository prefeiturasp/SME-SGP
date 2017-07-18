<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPluginNotificacao.ascx.cs" Inherits="GestaoEscolar.WebControls.PluginNotificacao.UCPluginNotificacao" %>

<script type="text/javascript">
    config = JSON.parse('<%= GetConfig %>');

    new plgnotify(
     {
         url: config.UrlNotificationAPI,
         userId: config.UserId,
         tokenType: 'Bearer ',
         token: config.IDSToken,
         ws: {
             url: config.UrlNotificationSignalR
         }
     }
     );

</script>
