using System;
using System.Linq;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.Validation.Exceptions;
using System.Collections.Generic;
using MSTech.CoreSSO.Entities;

namespace GestaoEscolar.Academico.Sondagem
{
    public partial class Agendamento : MotherPageLogado
    {
        #region Propriedades
        
        /// <summary>
        /// Propriedade em ViewState que armazena a lista de agendamentos
        /// </summary>
        private List<ACA_SondagemAgendamento> VS_ListaAgendamento
        {
            get
            {
                if (ViewState["VS_ListaAgendamento"] == null)
                    ViewState["VS_ListaAgendamento"] = new List<ACA_SondagemAgendamento>();
                return (List<ACA_SondagemAgendamento>)ViewState["VS_ListaAgendamento"];
            }
            set
            {
                ViewState["VS_ListaAgendamento"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena a lista de periodos dos agendamentos
        /// </summary>
        private List<ACA_SondagemAgendamentoPeriodo> VS_ListaAgendamentoPeriodo
        {
            get
            {
                if (ViewState["VS_ListaAgendamentoPeriodo"] == null)
                    ViewState["VS_ListaAgendamentoPeriodo"] = new List<ACA_SondagemAgendamentoPeriodo>();
                return (List<ACA_SondagemAgendamentoPeriodo>)ViewState["VS_ListaAgendamentoPeriodo"];
            }
            set
            {
                ViewState["VS_ListaAgendamentoPeriodo"] = value;
            }
        }

        /// <summary>
        /// Retorna o valor do parâmetro "Permanecer na tela após gravações"
        /// </summary>
        private bool ParametroPermanecerTela
        {
            get
            {
                return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.BOTAO_SALVAR_PERMANECE_TELA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de snd_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_snd_id
        {
            get
            {
                if (ViewState["VS_snd_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_snd_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_snd_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de sda_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_sda_id
        {
            get
            {
                if (ViewState["VS_sda_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_sda_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_sda_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de esc_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_esc_id
        {
            get
            {
                if (ViewState["VS_esc_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_esc_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_esc_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de uni_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_uni_id
        {
            get
            {
                if (ViewState["VS_uni_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_uni_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_uni_id"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de sda_idRetificada
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_sda_idRetificando
        {
            get
            {
                if (ViewState["VS_sda_idRetificando"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_sda_idRetificando"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_sda_idRetificando"] = value;
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de sda_id
        /// para retificar um registro ja existente.
        /// </summary>
        private int VS_sda_idRetificar
        {
            get
            {
                if (ViewState["VS_sda_idRetificar"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_sda_idRetificar"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_sda_idRetificar"] = value;
            }
        }

        private DataTable dtDadosRepeater;

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados da sondagem
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        private void _LoadFromEntity(int snd_id)
        {
            try
            {
                ACA_Sondagem snd = new ACA_Sondagem { snd_id = snd_id };
                ACA_SondagemBO.GetEntity(snd);

                VS_snd_id = snd.snd_id;

                txtSondagem.Text = snd.snd_titulo;
                txtDescricao.Text = snd.snd_descricao;

                VS_ListaAgendamento = ACA_SondagemAgendamentoBO.SelectAgendamentosBy_Sondagem(snd_id);
                VS_ListaAgendamentoPeriodo = ACA_SondagemAgendamentoPeriodoBO.SelectPeriodosBy_Agendamento(VS_snd_id, 0);

                VS_ListaAgendamento = VS_ListaAgendamento.OrderByDescending(a => a.sda_dataInicio).ThenByDescending(a => a.sda_dataFim).ToList();

                grvAgendamentos.DataSource = VS_ListaAgendamento;
                grvAgendamentos.DataBind();
                
                UCComboUAEscola.Inicializar();

                if (UCComboUAEscola.VisibleUA)
                    UCComboUAEscola_IndexChangedUA();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.ErroCarregarSondagem").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Insere ou altera a sondagem
        /// </summary>
        private void Salvar()
        {
            try
            {
                if (ACA_SondagemAgendamentoBO.Salvar(VS_snd_id, VS_ListaAgendamento, VS_ListaAgendamentoPeriodo))
                {
                    string message = "";
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "snd_id: " + VS_snd_id);
                    message = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.AgendamentoIncluidoSucesso").ToString(), UtilBO.TipoMensagem.Sucesso);
                    
                    if (ParametroPermanecerTela)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                        lblMessage.Text = message;
                        _LoadFromEntity(VS_snd_id);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = message;
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Sondagem/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.ErroSalvarAgendamento").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (DuplicateNameException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (ArgumentException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.ErroSalvarAgendamento").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Abre o pop-up para cadastro de período de agendamento
        /// </summary>
        /// <param name="dataInicio">Data de início de agendamento</param>
        /// <param name="dataFim">Data fim de agendamento</param>
        private void AbrirPopUp(string dataInicio, string dataFim)
        {
            try
            {
                txtDataInicio.Text = dataInicio;
                txtDataFim.Text = dataFim;

                dtDadosRepeater = ACA_TipoCurriculoPeriodoBO.SelectByPesquisa(0, 0).AsEnumerable().Where(p => Convert.ToByte(p["tcp_situacao"]) != 3).CopyToDataTable();
                var dtNivelEnsino = dtDadosRepeater.AsEnumerable().GroupBy(t => new
                {
                    tne_id = Convert.ToInt32(t["tne_id"]),
                    tme_id = Convert.ToInt32(t["tme_id"]),
                    tne_nome = t["tne_nome"].ToString() + " - " + t["tme_nome"].ToString(),
                    tne_nomeSimples = t["tne_nome"].ToString(),
                    tne_ordem = Convert.ToInt32(t["tne_ordem"])
                }).Select(t => t.Key).Where(t => dtDadosRepeater.AsEnumerable().Any(p => Convert.ToInt32(p["tne_id"]) == t.tne_id && Convert.ToInt32(p["tme_id"]) == t.tme_id))
                                     .OrderBy(t => t.tne_ordem).ThenBy(t => t.tne_nome);
                rptNivelEnsino.DataSource = dtNivelEnsino;
                rptNivelEnsino.DataBind();

                //Marca os períodos já selecionados no agendamento
                if (VS_sda_id > 0)
                {
                    List<ACA_SondagemAgendamentoPeriodo> lstPeriodos = VS_ListaAgendamentoPeriodo.Where(p => p.sda_id == VS_sda_id).ToList();
                    foreach(RepeaterItem itemN in rptNivelEnsino.Items)
                    {
                        Repeater rptCampos = (Repeater)itemN.FindControl("rptCampos");
                        if (rptCampos != null)
                            foreach (RepeaterItem item in rptCampos.Items)
                            {
                                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                                if (hdnId != null && !string.IsNullOrEmpty(hdnId.Value) && lstPeriodos.Any(p => p.tcp_id == Convert.ToInt32(hdnId.Value)))
                                {
                                    CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                                    if (ckbCampo != null)
                                        ckbCampo.Checked = true;
                                }
                            }
                    }
                }

                txtDataInicio.Focus();
                updPopUp.Update();
                divEscola.Visible = false;
                lblTituloPopUp.Text = GetGlobalResourceObject("Academico", "Sondagem.Agendamento.lblTituloPopUp.Text").ToString();

                btnAdicionar.Text = VS_sda_id > 0 && VS_sda_idRetificar <= 0 ? 
                                    GetGlobalResourceObject("Academico", "Sondagem.Agendamento.bntAlterar.Text").ToString() :
                                    GetGlobalResourceObject("Academico", "Sondagem.Agendamento.bntAdicionar.Text").ToString();

                if (VS_sda_idRetificar > 0)
                {
                    divEscola.Visible = true;
                    VS_sda_idRetificando = VS_sda_id;
                    VS_sda_id = 0;

                    lblTituloPopUp.Text = string.Format(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.lblPeriodoRetificar.Text").ToString(), dataInicio, dataFim);

                    UCComboUAEscola.Inicializar();

                    if (UCComboUAEscola.VisibleUA)
                        UCComboUAEscola_IndexChangedUA();
                }
                else if (VS_sda_idRetificando > 0)
                {
                    divEscola.Visible = true;

                    string dataInicioR = VS_ListaAgendamento.Where(a => a.sda_id == VS_sda_idRetificando).First().sda_inicio;
                    string dataFimR = VS_ListaAgendamento.Where(a => a.sda_id == VS_sda_idRetificando).First().sda_fim;

                    lblTituloPopUp.Text = string.Format(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.lblPeriodoRetificar.Text").ToString(), dataInicioR, dataFimR);

                    if (UCComboUAEscola.FiltroEscola)
                    {
                        ESC_Escola entEscola = new ESC_Escola
                        {
                            esc_id = VS_esc_id
                        };
                        ESC_EscolaBO.GetEntity(entEscola);
                        SYS_UnidadeAdministrativa entUA = new SYS_UnidadeAdministrativa
                        {
                            ent_id = entEscola.ent_id,
                            uad_id = entEscola.uad_id
                        };
                        SYS_UnidadeAdministrativaBO.GetEntity(entUA);

                        Guid uad_idSuperior = entEscola.uad_idSuperiorGestao.Equals(Guid.Empty) ? entUA.uad_idSuperior : entEscola.uad_idSuperiorGestao;

                        UCComboUAEscola.Uad_ID = uad_idSuperior;

                        UCComboUAEscola.EnableEscolas = (UCComboUAEscola.Uad_ID != Guid.Empty);
                    }

                    if (UCComboUAEscola.Uad_ID != Guid.Empty)
                    {
                        UCComboUAEscola.CarregaEscolaPorUASuperiorSelecionada();
                        UCComboUAEscola.SelectedValueEscolas = new[] { Convert.ToInt32(VS_esc_id), Convert.ToInt32(VS_uni_id) };
                    }
                }

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EditarAulas", "$('#divInserir').dialog('open');", true);
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.ErroAbrirPopUp").ToString(), UtilBO.TipoMensagem.Erro);
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
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsCadastroAgendamento.js"));
            }

            if (!IsPostBack)
            {
                try
                {

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                           GetGlobalResourceObject("Academico", "Sondagem.Agendamento.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Academico", "Sondagem.Agendamento.btnVoltar.Text").ToString();

                        _LoadFromEntity(PreviousPage.SelectedItem);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.SelecioneSondagem").ToString(), UtilBO.TipoMensagem.Alerta);
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Sondagem/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    Page.Form.DefaultFocus = btnAdicionarAgendamento.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.ErroCarregarSistema").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void UCComboUAEscola_IndexChangedUA()
        {
            if (UCComboUAEscola.Uad_ID == Guid.Empty)
                UCComboUAEscola.SelectedValueEscolas = new[] { -1, -1 };
        }

        protected void btnAdicionarAgendamento_Click(object sender, EventArgs e)
        {
            VS_sda_id = 0;
            VS_sda_idRetificar = 0;
            VS_sda_idRetificando = 0;
            AbrirPopUp("", "");
        }

        protected void grvAgendamentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                byte sda_situacao = Convert.ToByte(grvAgendamentos.DataKeys[e.Row.RowIndex]["sda_situacao"]);

                ImageButton btnRetificar = (ImageButton)e.Row.FindControl("btnRetificar");
                if (btnRetificar != null)
                {
                    int sda_idRetificada = string.IsNullOrEmpty(grvAgendamentos.DataKeys[e.Row.RowIndex]["sda_idRetificada"].ToString()) ? 0 : 
                                           Convert.ToInt32(grvAgendamentos.DataKeys[e.Row.RowIndex]["sda_idRetificada"]);
                    btnRetificar.CommandArgument = e.Row.RowIndex.ToString();
                    btnRetificar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && sda_idRetificada <= 0 &&
                                           sda_situacao != (byte)ACA_SondagemAgendamentoSituacao.Cancelado;
                }

                Label lblRetificado = (Label)e.Row.FindControl("lblRetificado");
                if (lblRetificado != null)
                {
                    int sda_idRetificada = string.IsNullOrEmpty(grvAgendamentos.DataKeys[e.Row.RowIndex]["sda_idRetificada"].ToString()) ? 0 :
                                           Convert.ToInt32(grvAgendamentos.DataKeys[e.Row.RowIndex]["sda_idRetificada"]);
                    lblRetificado.Visible = sda_idRetificada > 0;
                }

                ImageButton btnAlterar = (ImageButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                    btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar &&
                                         sda_situacao != (byte)ACA_SondagemAgendamentoSituacao.Cancelado;
                }

                Label lblCancelado = (Label)e.Row.FindControl("lblCancelado");
                if (lblCancelado != null)
                {
                    lblCancelado.Visible = sda_situacao == (byte)ACA_SondagemAgendamentoSituacao.Cancelado;
                }

                ImageButton btnCancelarAgendamento = (ImageButton)e.Row.FindControl("btnCancelarAgendamento");
                if (btnCancelarAgendamento != null)
                {
                    btnCancelarAgendamento.CommandArgument = e.Row.RowIndex.ToString();
                    btnCancelarAgendamento.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar &&
                                           sda_situacao != (byte)ACA_SondagemAgendamentoSituacao.Cancelado;
                }
                
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void grvAgendamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Retificar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idRetificar = Convert.ToInt32(grvAgendamentos.DataKeys[index]["sda_id"]);

                    string dataInicio = grvAgendamentos.DataKeys[index]["sda_inicio"].ToString();
                    string dataFim = grvAgendamentos.DataKeys[index]["sda_fim"].ToString();

                    VS_sda_idRetificar = idRetificar;
                    VS_sda_id = idRetificar;
                    VS_sda_idRetificando = 0;
                    AbrirPopUp(dataInicio, dataFim);
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Alterar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idAlterar = Convert.ToInt32(grvAgendamentos.DataKeys[index]["sda_id"]);

                    string dataInicio = grvAgendamentos.DataKeys[index]["sda_inicio"].ToString();
                    string dataFim = grvAgendamentos.DataKeys[index]["sda_fim"].ToString();
                    
                    VS_sda_id = idAlterar;
                    VS_esc_id = Convert.ToInt32(grvAgendamentos.DataKeys[index]["esc_id"]);
                    VS_uni_id = Convert.ToInt32(grvAgendamentos.DataKeys[index]["uni_id"]);
                    VS_sda_idRetificando = Convert.ToInt32(grvAgendamentos.DataKeys[index]["sda_idRetificada"]);
                    VS_sda_idRetificar = 0;
                    AbrirPopUp(dataInicio, dataFim);
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Cancelar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idCancelar = Convert.ToInt32(grvAgendamentos.DataKeys[index]["sda_id"]);

                    //Cancela as retificações do agendamento
                    foreach (ACA_SondagemAgendamento sda in VS_ListaAgendamento.Where(a => a.sda_idRetificada == idCancelar))
                        VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(sda)].sda_situacao = (byte)ACA_SondagemAgendamentoSituacao.Cancelado;

                    int ind = VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(a => a.sda_id == idCancelar).First());

                    VS_ListaAgendamento[ind].sda_situacao = (byte)ACA_SondagemAgendamentoSituacao.Cancelado;

                    VS_ListaAgendamento = VS_ListaAgendamento.OrderByDescending(a => a.sda_dataInicio).ThenByDescending(a => a.sda_dataFim).ToList();

                    grvAgendamentos.DataSource = VS_ListaAgendamento;
                    grvAgendamentos.DataBind();
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
            else if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idExcluir = Convert.ToInt32(grvAgendamentos.DataKeys[index]["sda_id"]);

                    int ind = VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(a => a.sda_id == idExcluir).First());
                    VS_ListaAgendamento.RemoveAt(ind);
                    VS_ListaAgendamentoPeriodo.RemoveAll(p => p.sda_id == idExcluir);

                    //Exclui as retificações do agendamento
                    VS_ListaAgendamentoPeriodo.RemoveAll(p => VS_ListaAgendamento.Any(a => a.sda_idRetificada == idExcluir && a.sda_id == p.sda_id));
                    VS_ListaAgendamento.RemoveAll(a => a.sda_idRetificada == idExcluir);

                    VS_ListaAgendamento = VS_ListaAgendamento.OrderByDescending(a => a.sda_dataInicio).ThenByDescending(a => a.sda_dataFim).ToList();

                    grvAgendamentos.DataSource = VS_ListaAgendamento;
                    grvAgendamentos.DataBind();
                }
                catch (ValidationException ex)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Academico/Sondagem/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }
        
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dataInicio = new DateTime();
                DateTime dataFim = new DateTime();
                int esc_id = VS_sda_idRetificando > 0 ? UCComboUAEscola.Esc_ID : 0;
                int uni_id = VS_sda_idRetificando > 0 ? UCComboUAEscola.Uni_ID : 0;

                if (string.IsNullOrEmpty(txtDataInicio.Text) || !DateTime.TryParse(txtDataInicio.Text, out dataInicio))
                    throw new ValidationException(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.DataInicioInvalida").ToString());

                if (dataInicio < DateTime.Today)
                    throw new ValidationException(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.DataInicioMenorHoje").ToString());

                if (string.IsNullOrEmpty(txtDataFim.Text) || !DateTime.TryParse(txtDataFim.Text, out dataFim))
                    throw new ValidationException(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.DataFimInvalida").ToString());

                if (dataInicio > dataFim)
                    throw new ValidationException(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.DataFimMenorInicio").ToString());

                if (VS_ListaAgendamento.Any(a => a.sda_id != VS_sda_id && dataInicio <= a.sda_dataFim && dataFim >= a.sda_dataInicio && 
                                                 ((a.esc_id == esc_id && a.uni_id == uni_id) || a.sda_id == VS_sda_idRetificando || a.sda_idRetificada == VS_sda_id)))
                    throw new ValidationException(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.PeriodoJaAdicionado").ToString());

                if (VS_sda_idRetificando > 0 && UCComboUAEscola.Esc_ID <= 0)
                    throw new ValidationException(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.EscolaObrigatorio").ToString());

                bool selecionado = false;

                foreach (RepeaterItem itemN in rptNivelEnsino.Items)
                {
                    Repeater rptCampos = (Repeater)itemN.FindControl("rptCampos");
                    if (rptCampos != null)
                        foreach (RepeaterItem item in rptCampos.Items)
                        {
                            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                            if (ckbCampo != null && ckbCampo.Checked)
                            {
                                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                                if (hdnId != null)
                                {
                                    selecionado = true;
                                    break;
                                }
                            }
                        }
                    if (selecionado)
                        break;
                }

                if (!selecionado)
                    throw new ValidationException(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.SemPeriodosSelecionados").ToString());

                int sda_idAux = VS_sda_id;

                if (VS_sda_id > 0 && VS_ListaAgendamento.Any(a => a.sda_id == VS_sda_id))
                {
                    VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == VS_sda_id).First())].sda_dataInicio = dataInicio;
                    VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == VS_sda_id).First())].sda_dataFim = dataFim;
                    VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == VS_sda_id).First())].sda_inicio = txtDataInicio.Text;
                    VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == VS_sda_id).First())].sda_fim = txtDataFim.Text;
                    VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == VS_sda_id).First())].sda_idRetificada = VS_sda_idRetificando;
                    VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == VS_sda_id).First())].esc_id = esc_id;
                    VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == VS_sda_id).First())].uni_id = uni_id;

                    VS_ListaAgendamentoPeriodo.RemoveAll(p => p.sda_id == VS_sda_id);
                }
                else
                {
                    int sda_id = VS_ListaAgendamento.Any() ? VS_ListaAgendamento.Max(l => l.sda_id) + 1 : 1;
                    VS_ListaAgendamento.Add(new ACA_SondagemAgendamento
                    {
                        snd_id = VS_snd_id,
                        sda_id = sda_id,
                        sda_dataInicio = dataInicio,
                        sda_dataFim = dataFim,
                        sda_idRetificada = VS_sda_idRetificar,
                        esc_id = esc_id,
                        uni_id = uni_id,
                        sda_inicio = txtDataInicio.Text,
                        sda_fim = txtDataFim.Text,
                        sda_situacao = (byte)ACA_SondagemAgendamentoSituacao.Ativo,
                        IsNew = true
                    });

                    sda_idAux = sda_id;
                }
                
                foreach (RepeaterItem itemN in rptNivelEnsino.Items)
                {
                    string tne_nome = "";
                    HiddenField hdnTneNome = (HiddenField)itemN.FindControl("hdnTneNome");
                    if (hdnTneNome != null)
                        tne_nome = hdnTneNome.Value;

                    Repeater rptCampos = (Repeater)itemN.FindControl("rptCampos");
                    if (rptCampos != null)
                        foreach (RepeaterItem item in rptCampos.Items)
                        {
                            CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                            if (ckbCampo != null && ckbCampo.Checked)
                            {
                                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                                if (hdnId != null)
                                {
                                    int tcp_ordem = 0;
                                    HiddenField hdnOrdem = (HiddenField)item.FindControl("hdnOrdem");
                                    if (hdnOrdem != null)
                                        tcp_ordem = Convert.ToInt32(hdnOrdem.Value);

                                    int tne_ordem = 0;
                                    HiddenField hdnTneOrdem = (HiddenField)item.FindControl("hdnTneOrdem");
                                    if (hdnTneOrdem != null)
                                        tne_ordem = Convert.ToInt32(hdnTneOrdem.Value);

                                    VS_ListaAgendamentoPeriodo.Add(new ACA_SondagemAgendamentoPeriodo
                                    {
                                        snd_id = VS_snd_id,
                                        sda_id = sda_idAux,
                                        tcp_id = Convert.ToInt32(hdnId.Value),
                                        tcp_descricao = tne_nome + " - " + ckbCampo.Text,
                                        tcp_ordem = tcp_ordem,
                                        tne_ordem = tne_ordem
                                    });
                                }
                            }
                        }
                }

                string periodos = VS_ListaAgendamentoPeriodo.Where(p => p.sda_id == sda_idAux).OrderBy(p => p.tne_ordem)
                                                            .ThenBy(p => p.tcp_ordem).ThenBy(p => p.tcp_descricao)
                                                            .Select(p => p.tcp_descricao).Aggregate((a, b) => a + ", " + b);
                VS_ListaAgendamento[VS_ListaAgendamento.IndexOf(VS_ListaAgendamento.Where(l => l.sda_id == sda_idAux).First())].periodos = periodos;
                VS_ListaAgendamento = VS_ListaAgendamento.OrderByDescending(a => a.sda_dataInicio).ThenByDescending(a => a.sda_dataFim).ToList();

                grvAgendamentos.DataSource = VS_ListaAgendamento;
                grvAgendamentos.DataBind();

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "FecharPopUp", "$('#divInserir').dialog('close');", true);
            }
            catch (ValidationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('$(\\'#divInserir\\').scrollTo(0,0);', 0);", true);
                lblMessagePopUp.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('$(\\'#divInserir\\').scrollTo(0,0);', 0);", true);
                lblMessagePopUp.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Academico", "Sondagem.Agendamento.ErroAdicionar").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }
        
        protected void rptNivelEnsino_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblNivelEnsino = (Label)e.Item.FindControl("lblNivelEnsino");
                if (lblNivelEnsino != null)
                    lblNivelEnsino.Text = DataBinder.Eval(e.Item.DataItem, "tne_nome").ToString();

                int tne_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tne_id"));
                int tme_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "tme_id"));

                Repeater rptCampos = (Repeater)e.Item.FindControl("rptCampos");
                if (rptCampos != null)
                {
                    DataTable dtCampos = dtDadosRepeater.AsEnumerable()
                                                        .Where(t => Convert.ToInt32(t["tne_id"]) == tne_id && Convert.ToInt32(t["tme_id"]) == tme_id)
                                                        .OrderBy(t => Convert.ToInt32(t["tcp_ordem"]))
                                                        .ThenBy(t => t["tcp_descricao"].ToString()).CopyToDataTable();
                    rptCampos.DataSource = dtCampos;
                    rptCampos.DataBind();
                }
            }
        }

        #endregion
    }
}