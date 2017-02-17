<%@ Application Inherits="MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB" Language="C#" %>

<script runat="server">

    protected override void Application_Start(object sender, EventArgs e)
    {
        base.Application_Start(sender, e);
        MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB.Config = MSTech.GestaoEscolar.BLL.eConfig.Academico;

               
    }
    
    //void Application_End(object sender, EventArgs e)
    //{
    //}

    void Application_Error(object sender, EventArgs e)
    {
        _GravaErro(Server.GetLastError());
    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
        if (HttpContext.Current.User != null)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    // Get the stored user-data, in this case, our roles
                    string userData = ticket.UserData;
                    string[] roles = userData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
                }
            }
        }
    }

    /// <summary>
    /// Método que retorna a variação do cache de UserControl.
    /// Utilizado o parâmetro "UsuarioLogado" como variação do cache do UCSistemas.ascx.
    /// </summary>
    /// <param name="context">Contexto do request</param>
    /// <param name="custom">Nome da variação de user control</param>
    /// <returns></returns>
    public override string GetVaryByCustomString(HttpContext context, string custom)
    {
        if (custom.Equals("UsuarioLogado"))
        {
            return (__SessionWEB.__UsuarioWEB.Usuario == null
                ? Guid.Empty.ToString()
                : __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        }
        else
        {
            return base.GetVaryByCustomString(context, custom);
        }
    }

</script>