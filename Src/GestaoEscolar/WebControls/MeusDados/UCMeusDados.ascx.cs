using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.Security.Cryptography;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using System.Data;
using System.Collections.Generic;
using System.Linq;

public partial class WebControls_MeusDados_UCMeusDados : MotherUserControl
{
    #region Propriedades

    public string PageRedirectCancel
    {
        get
        {
            if (ViewState["PageRedirectCancel"] != null)
                return Convert.ToString(ViewState["PageRedirectCancel"]);
            return string.Empty;
        }
        set
        {
            ViewState["PageRedirectCancel"] = value;
        }
    }

    public bool PermissaoAlterar
    {
        set
        {
            divSenha.Visible = value;
            btnSalvar.Visible = value;
        }
    }

    /// <summary>
    /// Parâmetro que indica se o usuário pode incluir/alterar o email.
    /// </summary>
    private bool PermiteAlterarEmail
    {
        get
        {
            return SYS_ParametroBO.ParametroValorBooleano(SYS_ParametroBO.eChave.PERMITIR_ALTERAR_EMAIL_MEUSDADOS);
        }
    }

    /// <summary>
    /// Parâmetro que indica se o usuário pode incluir/alterar o email.
    /// </summary>
    private bool EmailObrigatorio
    {
        get
        {
            return SYS_ParametroBO.Parametro_ValidarObrigatoriedadeEmailUsuario();
        }
    }


    #endregion

    #region Eventos Life Cycle

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this.Page);
        if (sm != null)
        {
            string script = String.Format(@"var idRfvConfNovaSenha = '{0}'; 
                                            var idRevNovaSenhaFormato = '{1}';
                                            var idRevNovaSenhaTamanho = '{2}';
                                            var idCpvNovaSenha = '{3}';
                                            var idCpvConfNovaSenha = '{4}';
                                            var permiteAlterarEmail = {5}; 
                                            var idCvNovaSenhaHistorico = '{6}';
                                            var usu_id = '{7}';",
                                            rfvConfNovaSenha.ClientID.Replace("#", ""),
                                            revNovaSenhaFormato.ClientID.Replace("#", ""),
                                            revNovaSenhaTamanho.ClientID.Replace("#", ""),
                                            cpvNovaSenha.ClientID.Replace("#", ""),
                                            cpvConfNovaSenha.ClientID.Replace("#", ""),
                                            PermiteAlterarEmail.ToString().ToLower(),
                                            cvNovaSenhaHistorico.ClientID.Replace("#", ""),
                                            __SessionWEB.__UsuarioWEB.Usuario.usu_id);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Validators", script, true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Validators", script, true);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        txtLogin.Text = __SessionWEB.__UsuarioWEB.Usuario.usu_login;

        ScriptManager sm = ScriptManager.GetCurrent(this.Page);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmBtn));
            sm.Scripts.Add(new ScriptReference("~/Includes/jsMeusDados.js"));
        }

        string mensagem = PermiteAlterarEmail ?
            SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemConfirmarcaoAlteracaoSenhaEmail) :
            SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemConfirmarcaoAlteracaoSenha);

        if (string.IsNullOrEmpty(mensagem))
        {
            mensagem = "Deseja realmente alterar a senha?<br /><br />Caso confirme você será redirecionado para a página de login.";
        }


        string script = String.Format("SetConfirmDialogButton('{0}','{1}');",
                            String.Concat("#", btnSalvar.ClientID),
                            mensagem);
        Page.ClientScript.RegisterStartupScript(GetType(), btnSalvar.ClientID, script, true);

        // Carrega os grupos do usuário
        if (!IsPostBack)
        {
            try
            {
                InicializarTela();

                LoadGrupos();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar os dados.", UtilBO.TipoMensagem.Erro);
            }
        }
    }

    #endregion

    #region Eventos

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Salvar();
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(PageRedirectCancel, false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void odsGrupos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (e.ExecutingSelectCount)
            e.InputParameters.Clear();
    }

    #endregion

    #region Métodos

    /// <summary>
    /// Inicializa os componentes da tela.
    /// </summary>
    private void InicializarTela()
    {
        // Mensagem de informação sobre o cadastro de email da tela de meus dados.
        string mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemEmail);

        lblInformacao.Text = string.IsNullOrEmpty(mensagem) ? string.Empty :
            UtilBO.GetErroMessage(mensagem, UtilBO.TipoMensagem.Informacao);

        txtEmail.Enabled = PermiteAlterarEmail;

        if (PermiteAlterarEmail)
        {

            rfvConfNovaSenha.Enabled = revNovaSenhaFormato.Enabled = revNovaSenhaTamanho.Enabled =
                cpvNovaSenha.Enabled = rfvNovaSenha.Enabled = cpvConfNovaSenha.Enabled = false;
            RemoveAsteriscoObrigatorio(lblNovaSenha);
            RemoveAsteriscoObrigatorio(lblConfNovaSenha);

            // Mensagem de validação quando o email for inválido.
            mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemEmailInvalido);
            revEmail.ErrorMessage = string.IsNullOrEmpty(mensagem) ? "E-mail está fora do padrão ( seuEmail@seuProvedor )." : mensagem;

            rfvEmail.Visible = revEmail.Visible = cvEmailExistente.Visible = EmailObrigatorio;
            if (EmailObrigatorio)
                AdicionaAsteriscoObrigatorio(lblEmail);
            else
                RemoveAsteriscoObrigatorio(lblEmail);

        }
        else
        {
            rfvConfNovaSenha.Enabled = revNovaSenhaFormato.Enabled = revNovaSenhaTamanho.Enabled =
                cpvNovaSenha.Enabled = rfvNovaSenha.Enabled = cpvConfNovaSenha.Enabled = true;

            rfvEmail.Visible = revEmail.Visible = cvEmailExistente.Visible = false;
            RemoveAsteriscoObrigatorio(lblEmail);
            AdicionaAsteriscoObrigatorio(lblNovaSenha);
            AdicionaAsteriscoObrigatorio(lblConfNovaSenha);

        }

        // Mensagem de validação de confirmação da nova senha.
        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemConfirmarSenhaNaoIdentico);

        if (!string.IsNullOrEmpty(mensagem))
        {
            cpvConfNovaSenha.ErrorMessage = mensagem;
        }

        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemValidacaoComplexidadeSenhaFormato);

        if (!string.IsNullOrEmpty(mensagem))
        {
            revNovaSenhaFormato.ErrorMessage = mensagem;
        }

        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemValidacaoComplexidadeSenhaTamanho);

        if (!string.IsNullOrEmpty(mensagem))
        {
            revNovaSenhaTamanho.ErrorMessage = mensagem;
        }

        // Configura formato senha
        revNovaSenhaFormato.ValidationExpression = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.FORMATO_SENHA_USUARIO);
        // Configura tamanho senha
        revNovaSenhaTamanho.ValidationExpression = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.TAMANHO_SENHA_USUARIO);

        // Mensagem de validação do formato da senha.
        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemValidacaoComplexidadeSenhaFormato);

        if (!string.IsNullOrEmpty(mensagem))
        {
            revNovaSenhaFormato.ErrorMessage = mensagem;
        }

        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemValidacaoComplexidadeSenhaTamanho);

        if (!string.IsNullOrEmpty(mensagem))
        {
            revNovaSenhaTamanho.ErrorMessage = mensagem;
        }
        else
        {
            revNovaSenhaTamanho.ErrorMessage = String.Format(revNovaSenhaTamanho.ErrorMessage, UtilBO.GetMessageTamanhoByRegex(revNovaSenhaTamanho.ValidationExpression));
        }

        // Mensagem de complexidade da nova senha.
        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemComplexidadeNovaSenha);

        if (!string.IsNullOrEmpty(mensagem))
        {
            lblMsnNovaSenha.Text = mensagem;
        }
        else
        {
            lblMsnNovaSenha.Text = String.Format(lblMsnNovaSenha.Text, UtilBO.GetMessageTamanhoByRegex(revNovaSenhaTamanho.ValidationExpression));
        }

        // Mensagem de validação da senha atual.
        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemSenhaAtualIncorreta);
        if (!string.IsNullOrEmpty(mensagem))
        {
            cvSenhaAtual.ErrorMessage = mensagem;
        }

        // Mensagem de validação de email existente.
        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemValidacaoEmailExistente);
        if (!string.IsNullOrEmpty(mensagem))
        {
            cvEmailExistente.ErrorMessage = mensagem;
        }

        if (SYS_ParametroBO.ParametroValorBooleano(SYS_ParametroBO.eChave.SALVAR_HISTORICO_SENHA_USUARIO))
        {
            // Mensagem de validação de validação de histórico de senhas..
            mensagem =
            String.Format(SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemValidacaoHistoricoSenha),
                                                                  SYS_ParametroBO.ParametroValorInt32(SYS_ParametroBO.eChave.QUANTIDADE_ULTIMAS_SENHAS_VALIDACAO).ToString());

            if (!string.IsNullOrEmpty(mensagem))
            {
                cvNovaSenhaHistorico.ErrorMessage = mensagem;
            }
        }

        mensagem = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemSenhaAtualSenhaNovaDiferenca);

        if (!string.IsNullOrEmpty(mensagem))
        {
            cpvNovaSenha.ErrorMessage = mensagem;
        }

        txtEmail.Text = __SessionWEB.__UsuarioWEB.Usuario.usu_email;
        if (PermiteAlterarEmail)
        {
            txtEmail.Focus();
        }
        else
        {
            txtSenhaAtual.Focus();
        }
    }

    private void LoadGrupos()
    {
        dgvGrupos.PageIndex = 0;
        odsGrupos.SelectParameters.Clear();
        odsGrupos.SelectParameters.Add("usu_id", __SessionWEB.__UsuarioWEB.Usuario.usu_id.ToString());
        odsGrupos.SelectParameters.Add("paginado", "true");
        dgvGrupos.DataBind();
    }

    private void Salvar()
    {
        // Configura criptografia da senha
        eCriptografa criptografia = (eCriptografa)Enum.Parse(typeof(eCriptografa), Convert.ToString(__SessionWEB.__UsuarioWEB.Usuario.usu_criptografia), true);
        if (!Enum.IsDefined(typeof(eCriptografa), criptografia))
            criptografia = eCriptografa.TripleDES;

        try
        {
            if (UtilBO.EqualsSenha(__SessionWEB.__UsuarioWEB.Usuario.usu_senha, UtilBO.CriptografarSenha(txtSenhaAtual.Text, criptografia), criptografia))
            {
                // Configura entidade SYS_Usuario
                SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id };
                SYS_UsuarioBO.GetEntity(entityUsuario);
                if (!string.IsNullOrEmpty(txtNovaSenha.Text))
                {
                    entityUsuario.usu_senha = txtNovaSenha.Text;
                }

                entityUsuario.usu_email = txtEmail.Text;
                // Altera os dados do usuário
                SYS_UsuarioBO.AlterarDadosUsuario(entityUsuario, !string.IsNullOrEmpty(txtNovaSenha.Text));
                // Grava Log de sistema
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "usu_id: " + entityUsuario.usu_id);
                Response.Redirect(ApplicationWEB._DiretorioVirtual + ApplicationWEB._PaginaLogoff, false);

            }
            else
            {
                string mensagemSenhaAtualInvalida = SYS_MensagemSistemaBO.RetornaValor(SYS_MensagemSistemaChave.MeusDadosMensagemSenhaAtualIncorreta);
                lblMessage.Text = UtilBO.GetErroMessage(string.IsNullOrEmpty(mensagemSenhaAtualInvalida) ? "Senha atual inválida." : mensagemSenhaAtualInvalida, UtilBO.TipoMensagem.Alerta);
            }
        }
        catch (DuplicateNameException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ValidationException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException ex)
        {
            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar salvar o usuário.", UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Valida se a senha atual está correta.
    /// </summary>
    /// <param name="senhaAtual">Senha atual.</param>
    /// <param name="usu_id">ID do usuário.</param>
    /// <returns></returns>
    public static bool ValidarSenhaAtual(string senhaAtual, Guid usu_id)
    {
        SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = usu_id };
        SYS_UsuarioBO.GetEntity(entityUsuario);

        // Configura criptografia da senha
        eCriptografa criptografia = (eCriptografa)Enum.Parse(typeof(eCriptografa), Convert.ToString(entityUsuario.usu_criptografia), true);
        if (!Enum.IsDefined(typeof(eCriptografa), criptografia))
            criptografia = eCriptografa.TripleDES;

        return UtilBO.EqualsSenha(entityUsuario.usu_senha, UtilBO.CriptografarSenha(senhaAtual, criptografia), criptografia);
    }

    /// <summary>
    /// Valida se já existe usuário com o email.
    /// </summary>
    /// <param name="email">Email.</param>
    /// <param name="usu_id">ID do usuário.</param>
    /// <returns></returns>
    public static bool ValidarEmailExistente(string email, Guid usu_id)
    {
        SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = usu_id };
        SYS_UsuarioBO.GetEntity(entityUsuario);

        entityUsuario.usu_email = email;

        return string.IsNullOrEmpty(entityUsuario.usu_email) || !SYS_UsuarioBO.VerificaEmailExistente(entityUsuario);
    }

    /// <summary>
    /// Validação de senha de acordo com suas senhas anteriores.
    /// </summary>
    /// <param name="novaSenha">Nova senha.</param>
    /// <param name="usu_id">ID do usuário.</param>
    /// <returns></returns>
    public static bool ValidarHistoricoSenha(string novaSenha, Guid usu_id)
    {
        if (SYS_ParametroBO.ParametroValorBooleano(SYS_ParametroBO.eChave.SALVAR_HISTORICO_SENHA_USUARIO))
        {
            SYS_Usuario entityUsuario = new SYS_Usuario { usu_id = usu_id };
            SYS_UsuarioBO.GetEntity(entityUsuario);

            List<SYS_UsuarioSenhaHistorico> listaHistoricoSenhas = SYS_UsuarioSenhaHistoricoBO.SelecionaUltimasSenhas(entityUsuario.usu_id);

            // Configura criptografia da senha
            eCriptografa criptografia = (eCriptografa)Enum.Parse(typeof(eCriptografa), Convert.ToString(entityUsuario.usu_criptografia), true);
            if (!Enum.IsDefined(typeof(eCriptografa), criptografia))
                criptografia = eCriptografa.TripleDES;

            return !listaHistoricoSenhas.Any(p => p.ush_senha == UtilBO.CriptografarSenha(novaSenha, criptografia) && p.ush_criptografia == entityUsuario.usu_criptografia);
        }

        return true;
    }

    #endregion
}
