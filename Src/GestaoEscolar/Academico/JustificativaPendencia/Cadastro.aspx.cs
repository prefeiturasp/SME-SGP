using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using System.Data;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.Entities;

public partial class Academico_JustificativaPendencia_Cadastro : MotherPageLogado
{
    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.ExitPageConfirm));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsSetExitPageConfirmer.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroJustificativaPendencia.js"));

                if (!Convert.ToString(btnCancelar.CssClass).Contains("btnMensagemUnload"))
                {
                    btnCancelar.CssClass += " btnMensagemUnload";
                }
            }

            if (!IsPostBack)
            {
                comboCalendario.PermiteEditar = comboTurmaDisciplina.PermiteEditar = false;
                comboUAEscola.FocusUA();
                comboUAEscola.Inicializar();

                if (comboUAEscola.VisibleUA)
                    comboUAEscola_IndexChangedUA();
                else
                    comboUAEscola_IndexChangedUnidadeEscola();

                Page.Form.DefaultFocus = comboUAEscola.VisibleUA ? comboUAEscola.ComboUA_ClientID : comboUAEscola.ComboEscola_ClientID;
                btnSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                    LoadFromEntity(PreviousPage.EditItem);
                    Page.Form.DefaultFocus = txtJustificativa.ClientID;
                }
            }

            Page.Form.DefaultButton = btnSalvar.UniqueID;
            comboTurmaDisciplina.Obrigatorio = true;
            comboUAEscola.IndexChangedUA += comboUAEscola_IndexChangedUA;
            comboUAEscola.IndexChangedUnidadeEscola += comboUAEscola_IndexChangedUnidadeEscola;
            comboCalendario.IndexChanged += comboCalendario_IndexChanged;
            comboTurmaDisciplina.IndexChanged += comboTurmaDisciplina_IndexChanged;

            RegistrarParametrosMensagemSair(btnSalvar.Visible, __SessionWEB.__UsuarioWEB.Docente.doc_id > 0);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/JustificativaPendencia/Busca.aspx", false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        if (Page.IsValid)
            Salvar();
    }

    #endregion Eventos

    #region Delegates

    protected void comboUAEscola_IndexChangedUA()
    {
        try
        {
            comboUAEscola.CarregaEscolaPorUASuperiorSelecionada();

            if (comboUAEscola.Uad_ID != Guid.Empty)
            {
                comboUAEscola.FocoEscolas = true;
                comboUAEscola.PermiteAlterarCombos = true;
            }

            comboUAEscola.SelectedValueEscolas = new[] { -1, -1 };

            comboUAEscola_IndexChangedUnidadeEscola();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void comboUAEscola_IndexChangedUnidadeEscola()
    {
        try
        {
            comboCalendario.CarregarPorEscola(comboUAEscola.Esc_ID);
            comboCalendario.PermiteEditar = comboUAEscola.Esc_ID > 0;
            if (comboCalendario.QuantidadeItensCombo == 2)
            {
                comboCalendario.SelectedIndex = 1;
            }
            comboCalendario_IndexChanged();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void comboCalendario_IndexChanged()
    {
        try
        {
            comboTurmaDisciplina.CarregarTurmaDisciplinaEletivaAluno(comboUAEscola.Esc_ID, comboCalendario.Valor);
            comboTurmaDisciplina.PermiteEditar = comboCalendario.Valor > 0;
            if (comboTurmaDisciplina.QuantidadeItensCombo == 2)
            {
                comboTurmaDisciplina.SelectedIndex = 1;
            }
            comboTurmaDisciplina_IndexChanged();
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void comboTurmaDisciplina_IndexChanged()
    {
        try
        {
            DataTable dtPeriodoCalendario = ACA_TipoPeriodoCalendarioBO.SelecionarPeriodosComMatricula(comboCalendario.Valor, comboTurmaDisciplina.Valor);
            int tpcId = Convert.ToInt32(hdnTpcId.Value);
            if (tpcId > 0)
            {
                rptCampos.DataSource = dtPeriodoCalendario.Select("tpc_id = " + tpcId).CopyToDataTable();
            }
            else
            {
                rptCampos.DataSource = dtPeriodoCalendario;
            }
            rptCampos.DataBind();
            fdsPeriodoCalendario.Visible = comboCalendario.Valor > 0 && comboTurmaDisciplina.Valor > 0;
            lblNenhumPeriodo.Visible = fdsPeriodoCalendario.Visible && rptCampos.Items.Count == 0;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "CarregarPeriodos", "$(document).ready(function() { AjustarCssPeriodos(); });", true);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
        }
    }

    protected void rptCampos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) ||
            (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            int tpcId = Convert.ToInt32(hdnTpcId.Value);
            if (tpcId > 0)
            {
                CheckBox ckbCampo = (CheckBox)e.Item.FindControl("ckbCampo");
                ckbCampo.Checked = true;
                ckbCampo.Enabled = false;
            }
        }
    }

    #endregion Delegates

    #region Métodos

    /// <summary>
    /// Método para salvar uma justificativa de pendência.
    /// </summary>
    private void Salvar()
    {
        try
        {
            List<int> periodosCalendario;
            if (!VerificarPeriodoCalendarioSelecionado(out periodosCalendario))
            {
                throw new MSTech.Validation.Exceptions.ValidationException(GetGlobalResourceObject("Academico", "JustificativaPendencia.Cadastro.ValidaPeriodoCalendarioObrigatorio").ToString());
            }

            List<CLS_FechamentoJustificativaPendencia> lstFechamentoJustificativaPendenciaBanco = CLS_FechamentoJustificativaPendenciaBO.GetSelectBy_TurmaDisciplina(comboTurmaDisciplina.Valor);

            int fjpId = Convert.ToInt32(hdnFjpId.Value);
            bool sucesso = true;
            string tpcId = string.Empty;
            List<CLS_FechamentoJustificativaPendencia> lstFechamentoJustificativaPendencia = new List<CLS_FechamentoJustificativaPendencia>();
            foreach (int periodoCalendario in periodosCalendario)
            {
                // Verifica se ja existe uma justificativa cadastrada para a turma disciplina no mesmo período do calendário.
                if (lstFechamentoJustificativaPendenciaBanco.Any(p => p.tpc_id == periodoCalendario && p.fjp_id != fjpId))
                {
                    throw new MSTech.Validation.Exceptions.ValidationException(GetGlobalResourceObject("Academico", "JustificativaPendencia.Cadastro.ValidaExisteJustificativa").ToString());
                }

                CLS_FechamentoJustificativaPendencia justificativaPendencia = new CLS_FechamentoJustificativaPendencia();
                justificativaPendencia.tud_id = comboTurmaDisciplina.Valor;
                justificativaPendencia.cal_id = comboCalendario.Valor;
                justificativaPendencia.tpc_id = periodoCalendario;
                justificativaPendencia.fjp_id = fjpId;
                justificativaPendencia.fjp_justificativa = txtJustificativa.Text;
                justificativaPendencia.usu_id = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                justificativaPendencia.usu_idAlteracao = __SessionWEB.__UsuarioWEB.Usuario.usu_id;
                justificativaPendencia.fjp_situacao = (byte)CLS_FechamentoJustificativaPendenciaSituacao.Ativo;
                lstFechamentoJustificativaPendencia.Add(justificativaPendencia);
                tpcId += string.IsNullOrEmpty(tpcId) ? periodoCalendario.ToString() : "," + periodoCalendario.ToString();
            }
            if (lstFechamentoJustificativaPendencia.Count > 0)
            {
                sucesso = CLS_FechamentoJustificativaPendenciaBO.SalvarEmLote(lstFechamentoJustificativaPendencia);
            }

            if (sucesso)
            {
                if (fjpId > 0)
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "fjp_id: " + fjpId + ", tud_id: " + comboTurmaDisciplina.Valor + ", cal_id: " + comboCalendario.Valor + ", tpc_id: " + tpcId);               
                }
                else
                {
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "tud_id: " + comboTurmaDisciplina.Valor + ", cal_id: " + comboCalendario.Valor + ", tpc_id: " + tpcId);
                }
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "JustificativaPendencia.Cadastro.SucessoSalvar").ToString(), UtilBO.TipoMensagem.Sucesso);
                Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/JustificativaPendencia/Busca.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "JustificativaPendencia.Cadastro.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }
        catch (ValidationException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (ArgumentException e)
        {
            lblMessage.Text = UtilBO.GetErroMessage(e.Message, UtilBO.TipoMensagem.Alerta);
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "JustificativaPendencia.Cadastro.ErroSalvar").ToString(), UtilBO.TipoMensagem.Erro);
        }
    }

    /// <summary>
    /// Verifica se tem um período do calendário selecionado na tela.
    /// </summary>
    /// <returns></returns>
    private bool VerificarPeriodoCalendarioSelecionado(out List<int> periodosCalendarioSelecionados)
    {
        periodosCalendarioSelecionados = new List<int>();
        foreach (RepeaterItem item in rptCampos.Items)
        {
            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
            if (ckbCampo != null && ckbCampo.Checked)
            {
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                periodosCalendarioSelecionados.Add(Convert.ToInt32(hdnId.Value));
            }
        }
        return periodosCalendarioSelecionados.Count > 0;
    }

    /// <summary>
    /// Carrega os dados da justificativa de pendência na tela.
    /// </summary>
    /// <param name="editItem">Dados da justificativa</param>
    private void LoadFromEntity(string[] strKeys)
    {
        try
        {
            string escUniId = strKeys[5];
            if (comboUAEscola.FiltroEscola)
            {
                string uaSuperior = strKeys[4];
                if (!string.IsNullOrEmpty(uaSuperior))
                    comboUAEscola.DdlUA.SelectedValue = uaSuperior;

                if (uaSuperior != Guid.Empty.ToString())
                    SelecionarEscola(comboUAEscola.FiltroEscola, escUniId);
            }
            else
                SelecionarEscola(comboUAEscola.FiltroEscola, escUniId);
            comboUAEscola_IndexChangedUnidadeEscola();
            comboUAEscola.PermiteAlterarCombos = false;

            //Calendario
            int calId = Convert.ToInt32(strKeys[1]);
            comboCalendario.Valor = calId;
            comboCalendario_IndexChanged();
            comboCalendario.PermiteEditar = false;

            //Periodo Calendario
            hdnTpcId.Value = strKeys[2];

            //Disciplina
            long tudId = Convert.ToInt64(strKeys[0]);
            comboTurmaDisciplina.Valor = tudId;
            comboTurmaDisciplina_IndexChanged();
            comboTurmaDisciplina.PermiteEditar = false;

            //Justificativa
            txtJustificativa.Text = strKeys[6];

            hdnFjpId.Value = strKeys[3];           
        }
        catch (Exception ex)
        {
            ApplicationWEB._GravaErro(ex);
            lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "JustificativaPendencia.Cadastro.ErroCarregar").ToString(), UtilBO.TipoMensagem.Erro);
        }

    }

    /// <summary>
    /// Seleciona a escola no combo de acordo com o item editado.
    /// </summary>
    /// <param name="filtroEscolas"></param>
    private void SelecionarEscola(bool filtroEscolas, string escUniId)
    {
        if (filtroEscolas)
            comboUAEscola_IndexChangedUA();

        comboUAEscola.DdlEscola.SelectedValue = escUniId;
    }

    #endregion Métodos
}