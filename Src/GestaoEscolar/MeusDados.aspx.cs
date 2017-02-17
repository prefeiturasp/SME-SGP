using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.Services;

public partial class MeusDados : MotherPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ApplicationWEB.UtilizarIntegracaoADUsuario)
        {
            // Não pode utilizar "Meus dados" caso o sistema esteja utilizando a integração com AD (a alteração dos dados
            // será feita pelo Core).
            Response.Redirect("~/Index.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else
        {
            if (!IsPostBack)
                UCMeusDados1.PageRedirectCancel = Request.UrlReferrer.AbsoluteUri;
        }
    }

    /// <summary>
    /// Valida se a senha atual está correta.
    /// </summary>
    /// <param name="senhaAtual">Senha atual.</param>
    /// <param name="usu_id">ID do usuário.</param>
    /// <returns></returns>
    [WebMethod]
    public static bool ValidarSenhaAtual(string senhaAtual, Guid usu_id)
    {
        return WebControls_MeusDados_UCMeusDados.ValidarSenhaAtual(senhaAtual, usu_id);
    }


    /// <summary>
    /// Valida se já existe usuário com o email.
    /// </summary>
    /// <param name="email">Email.</param>
    /// <param name="usu_id">ID do usuário.</param>
    /// <returns></returns>
    [WebMethod]
    public static bool ValidarEmailExistente(string email, Guid usu_id)
    {
        return WebControls_MeusDados_UCMeusDados.ValidarEmailExistente(email, usu_id);
    }

    /// <summary>
    /// Validação de senha de acordo com suas senhas anteriores.
    /// </summary>
    /// <param name="novaSenha">Nova senha.</param>
    /// <param name="usu_id">ID do usuário.</param>
    /// <returns></returns>
    [WebMethod]
    public static bool ValidarHistoricoSenha(string novaSenha, Guid usu_id)
    {
        return WebControls_MeusDados_UCMeusDados.ValidarHistoricoSenha(novaSenha, usu_id);
    }
}
