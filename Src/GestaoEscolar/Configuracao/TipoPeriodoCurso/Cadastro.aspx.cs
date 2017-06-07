using System;
using System.Data;
using System.Web;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.CoreSSO.BLL;
using System.Web.UI;

public partial class Configuracao_TipoPeriodoCurso_Cadastro : MotherPageLogado
{
    #region Propriedades

    /// <summary>
    /// Propriedade em ViewState que armazena se a pagina é postBack
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private bool VS_previous
    {
        get
        {
            if (ViewState["VS_previous"] != null)
            {
                return Convert.ToBoolean(ViewState["VS_previous"]);
            }
            return false;
        }

        set
        {
            ViewState["VS_previous"] = value;
        }
    }

    /// <summary>
    /// Propriedade em ViewState que armazena valor de tcp_id (ID do tipo de curriculo periodo)
    /// no caso de atualização de um registro ja existente.
    /// </summary>
    private int VS_tcp_id
    {
        get
        {
            if (ViewState["VS_tcp_id"] != null)
            {
                return Convert.ToInt32(ViewState["VS_tcp_id"]);
            }

            return -1;
        }
        set
        {
            ViewState["VS_tcp_id"] = value;
        }
    }

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this);
        if (sm != null)
        {
            sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
        }
        if (!IsPostBack)
        {
            try
            {
                if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                {
                    VS_tcp_id = PreviousPage.EditItem;

                    //seta true se a página é postBack
                    VS_previous = true;
                    Carregar(VS_tcp_id);
                }
                else
                {
                    Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoPeriodoCurso/Busca.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                UCComboTipoNivelEnsino.CarregarTipoNivelEnsino();
                Page.Form.DefaultFocus = UCComboTipoNivelEnsino.Combo_ClientID;
                UCComboTipoModalidadeEnsino.CarregarTipoModalidadeEnsino();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os dados.", UtilBO.TipoMensagem.Erro);
            }

            lblLegendaCadastroPeriodoCurso.Text = "Cadastro de" + " " + "tipo de" + " " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower();
            lblDescricao.Text = "Tipo de" + " " + GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower() + "*";
        }
    }

    protected void btnCancelarClick(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/TipoPeriodoCurso/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Salvar();
        }
        else
            UtilBO.GetErroMessage(string.Format("Erro ao tentar salvar o(a) {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Erro);
    }

    #endregion Eventos

    #region Métodos

    /// <summary>
    /// Insere e altera um tipo curriculo periodo
    /// </summary>
    private void Salvar()
    {
        try
        {
            ACA_TipoCurriculoPeriodo TipoCurriculoPeriodo;

            TipoCurriculoPeriodo = new ACA_TipoCurriculoPeriodo { tcp_id = VS_tcp_id };
            ACA_TipoCurriculoPeriodoBO.GetEntity(TipoCurriculoPeriodo);

            TipoCurriculoPeriodo.tcp_id = VS_tcp_id;
            TipoCurriculoPeriodo.tne_id = UCComboTipoNivelEnsino.Valor;
            TipoCurriculoPeriodo.tme_id = UCComboTipoModalidadeEnsino.Valor;
            TipoCurriculoPeriodo.tcp_descricao = txtDescricao.Text;
            TipoCurriculoPeriodo.tcp_objetoAprendizagem = chkObjetoAprendizagem.Checked;

            if (ACA_TipoCurriculoPeriodoBO.SalvarCurriculoPeriodo(TipoCurriculoPeriodo))
            {
                ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "tcp_id: " + TipoCurriculoPeriodo.tcp_id);
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(string.Format("Tipo de {0} ", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()) + (VS_tcp_id > 0 ? "alterado" : "incluído") + " com sucesso.", UtilBO.TipoMensagem.Sucesso);

                Response.Redirect("Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        catch (MSTech.Validation.Exceptions.ValidationException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(string.Format("Descrição tipo de {0} pode conter até 100 caracteres.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar salvar {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Alerta);
        }
        catch (DuplicateNameException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(string.Format("Já existe um registro cadastrado com o mesmo tipo de {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(string.Format("Erro ao tentar salvar {0}.", GestaoEscolarUtilBO.nomePadraoPeriodo(__SessionWEB.__UsuarioWEB.Usuario.ent_id).ToLower()), UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Método para carregar um registro de tipo de curriculo periodo, a fim de atualizar suas informações.
    /// Recebe dados referente ao tipo de curriculo periodo para realizar a busca.
    /// </summary>
    /// <param name="tci_id">ID do tipo de curriculo periodo</param>
    public void Carregar(int tcp_id)
    {
        try
        {
            VS_tcp_id = tcp_id;

            ACA_TipoCurriculoPeriodo tipoCurPeriodo = new ACA_TipoCurriculoPeriodo { tcp_id = tcp_id };
            ACA_TipoCurriculoPeriodoBO.GetEntity(tipoCurPeriodo);

            txtDescricao.Text = tipoCurPeriodo.tcp_descricao;
            UCComboTipoNivelEnsino.Valor = tipoCurPeriodo.tne_id;
            UCComboTipoModalidadeEnsino.Valor = tipoCurPeriodo.tme_id;
            chkObjetoAprendizagem.Checked = tipoCurPeriodo.tcp_objetoAprendizagem;
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar tipo de ciclo.", UtilBO.TipoMensagem.Erro);
        }
    }

    #endregion Métodos
}
