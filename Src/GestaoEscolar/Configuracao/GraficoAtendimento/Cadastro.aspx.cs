using System;
using System.Linq;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using MSTech.Validation.Exceptions;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;

namespace GestaoEscolar.Configuracao.GraficoAtendimento
{
    public partial class Cadastro : MotherPageLogado
    {
        #region Propriedades

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
        /// Propriedade em ViewState que armazena valor de rea_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_gra_id
        {
            get
            {
                if (ViewState["VS_gra_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_gra_id"]);
                }
                return -1;
            }
            set
            {
                ViewState["VS_gra_id"] = value;
            }
        }
        
        /// <summary>
        /// Propriedade em ViewState que armazena valor de rea_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private List<CLS_RelatorioAtendimentoQuestionario> VS_lstQuestionarios
        {
            get
            {
                if (ViewState["VS_lstQuestionarios"] == null)
                    ViewState["VS_lstQuestionarios"] = new List<CLS_RelatorioAtendimentoQuestionario>();

                return (List<CLS_RelatorioAtendimentoQuestionario>)ViewState["VS_lstQuestionarios"];
            }
            set
            {
                ViewState["VS_lstQuestionarios"] = value;
            }
        }

        private List<REL_GraficoAtendimento_FiltrosFixos> VS_lstFiltrosFixos
        {
            get
            {
                if (ViewState["VS_lstFiltrosFixos"] == null)
                    ViewState["VS_lstFiltrosFixos"] = new List<REL_GraficoAtendimento_FiltrosFixos>();

                return (List<REL_GraficoAtendimento_FiltrosFixos>)ViewState["VS_lstFiltrosFixos"];
            }
            set
            {
                ViewState["VS_lstFiltrosFixos"] = value;
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Carrega dados do relatório
        /// </summary>
        /// <param name="rea_id">ID do relatório</param>
        private void _LoadFromEntity(int gra_id)
        {
            try
            {
                VS_gra_id = gra_id;

                REL_GraficoAtendimento gra = new REL_GraficoAtendimento { gra_id = VS_gra_id };
                REL_GraficoAtendimentoBO.GetEntity(gra);

                txtTitulo.Text = gra.gra_titulo;
                ddlTipo.Enabled = false;
               
                CLS_RelatorioAtendimento rea = new CLS_RelatorioAtendimento { rea_id = gra.rea_id };
                CLS_RelatorioAtendimentoBO.GetEntity(rea);
                ddlTipo.SelectedValue = rea.rea_tipo.ToString();
                UCComboRelatorioAtendimento._Combo.SelectedValue = gra.rea_id.ToString();
                UCComboRelatorioAtendimento._Combo.Enabled = false;

                ddlEixoAgrupamento.Enabled = false;
                ddlEixoAgrupamento.SelectedValue = gra.gra_eixo.ToString();
  
                CarregaFiltrosFixos();
                CarregaQuestionarios();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Insere ou altera o relatório
        /// </summary>
        private void Salvar()
        {
            try
            {
                REL_GraficoAtendimento gra = new REL_GraficoAtendimento
                {
                    gra_id = VS_gra_id,
                    rea_id = UCComboRelatorioAtendimento.Valor,
                    gra_titulo = txtTitulo.Text,
                    gra_eixo = Convert.ToByte(ddlEixoAgrupamento.SelectedValue),
                    gra_tipo = Convert.ToByte(REL_GraficoAtendimentoTipo.Barra),
                    gra_dataAlteracao = DateTime.Now,
                    IsNew = VS_gra_id <= 0
                };

                if(!gra.IsNew)
                    gra.gra_dataCriacao = DateTime.Now;

                if (!VS_lstQuestionarios.Any(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido))
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.NenhumQuestionarioAdicionado").ToString());

                if (VS_lstFiltrosFixos.Count == 0 && VS_lstQuestionarios.Count ==0)
                    throw new ValidationException("Selecione pelo menos um filtro.");

                if (REL_GraficoAtendimentoBO.Save(gra)) //, VS_lstFiltrosFixos, VS_lstQuestionarios))
                {
                    string message = "";
                    if (VS_gra_id <= 0)
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Insert, "gra_id: " + gra.gra_id);
                        message = UtilBO.GetErroMessage("Gráfico cadastrado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    else
                    {
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "gra_id: " + gra.gra_id);
                        message = UtilBO.GetErroMessage("Gráfico alterado com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                    if (ParametroPermanecerTela)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                        lblMessage.Text = message;
                        VS_gra_id = gra.gra_id;
                        _LoadFromEntity(VS_gra_id);
                    }
                    else
                    {
                        __SessionWEB.PostMessages = message;
                        Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/RelatorioAtendimento/Busca.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroSalvarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
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
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroSalvarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }


        /// <summary>
        /// Carrega os cargos
        /// </summary>
        private void CarregaFiltrosFixos()
        {

        }

        /// <summary>
        /// Carrega os questionarios
        /// </summary>
        private void CarregaQuestionarios()
        {
            //VS_lstQuestionarios = CLS_RelatorioAtendimentoQuestionarioBO.SelectBy_rea_id(VS_rea_id);
            VS_lstQuestionarios = VS_lstQuestionarios.OrderBy(q => q.raq_ordem).ThenBy(q => q.qst_titulo).ToList();

            gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
            gvQuestionario.DataBind();
        }

        /// <summary>
        /// Carrega os cargos
        /// </summary>
        private List<CFG_DeficienciaDetalhe> CarregaDetalhePreenchidos()
        {
            List<CFG_DeficienciaDetalhe> lstDetalhes = new List<CFG_DeficienciaDetalhe>();

            foreach (GridViewRow item in gvDetalhe.Rows)
            {
                CheckBox chkSeleciona = (CheckBox)item.FindControl("chkSelecionar");

                if (chkSeleciona != null)
                {
                    if (!chkSeleciona.Checked)
                        continue;

                    lstDetalhes.Add(new CFG_DeficienciaDetalhe
                    {
                        tde_id = ComboTipoDeficiencia.Valor,
                        dfd_id = Convert.ToInt32(gvDetalhe.DataKeys[item.RowIndex]["dfd_id"].ToString()),
                    });
                }
            }

            return lstDetalhes;
        }

        
        /// <summary>
        /// Inicializa os campos da tela
        /// </summary>
        private void Inicializar()
        {
            VS_gra_id = -1;
            txtTitulo.Text = "";
            ddlTipo.SelectedValue = "0";
            UCComboRelatorioAtendimento._Combo.Enabled = false;
            ddlEixoAgrupamento.SelectedValue = "0";
            gvQuestionario.DataSource = VS_lstQuestionarios;
            gvQuestionario.DataBind();
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
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
            }

            if (!IsPostBack)
            {
                try
                {
                    Inicializar();

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar ?
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnVoltar.Text").ToString();

                        _LoadFromEntity(PreviousPage.EditItem);
                    }
                    else
                    {
                        bntSalvar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                        btnCancelar.Text = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir ?
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnCancelar.Text").ToString() :
                                           GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.btnVoltar.Text").ToString();
                        ckbBloqueado.Visible = false;

                        CarregaFiltrosFixos();
                        CarregaQuestionarios();
                    }


                    Page.Form.DefaultFocus = txtTitulo.ClientID;
                    Page.Form.DefaultButton = bntSalvar.UniqueID;
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar dados do gráfico.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/GraficoAtendimento/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void bntSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Salvar();
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //carrega os relatorios
                if (ddlTipo.SelectedIndex > 0)
                {
                    UCComboRelatorioAtendimento.CarregarPorPermissaoUuarioTipo((CLS_RelatorioAtendimentoTipo)Convert.ToByte(ddlTipo.SelectedValue));
                    UCComboRelatorioAtendimento._Combo.Enabled = true;
                    if (Convert.ToByte(ddlTipo.SelectedValue) == (byte)CLS_RelatorioAtendimentoTipo.AEE)
                        ddlFiltroFixo.Items.Add("Detalhamento das deficiências");
                    else
                        ddlFiltroFixo.Items.Remove("Detalhamento das deficiências");

                     updFiltro.Update();
                }
                else
                {
                    UCComboRelatorioAtendimento.SelectedIndex = 0;
                    UCComboRelatorioAtendimento._Combo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ScrollToTop", "setTimeout('window.scrollTo(0,0);', 0);", true);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao carregar relatórios.", UtilBO.TipoMensagem.Erro);
            }
        }


        protected void ddlFiltroFixo_SelectedIndexChanged(object sender, EventArgs e)
        {
            divBotoesFiltro.Visible = divRacaCor.Visible = divSexo.Visible = divIdade.Visible = divDataPreenchimento.Visible = divDetalhamentoDeficiencia.Visible = false;

            //carrega os relatorios
            if (ddlFiltroFixo.SelectedIndex > 0)
            {
                switch (ddlFiltroFixo.SelectedIndex)
                {
                    case 1:
                        divDataPreenchimento.Visible = true;
                        break;
                    case 2:
                        divRacaCor.Visible = true;
                        break;
                    case 3:
                        divIdade.Visible = true;                        
                        break;
                    case 4:
                        divSexo.Visible = true;
                        break;
                    default:
                        ComboTipoDeficiencia._Combo.DataBind();
                        divDetalhe.Visible = false;
                        divDetalhamentoDeficiencia.Visible = true;
                        break;
                }
                divBotoesFiltro.Visible = true;
                updFiltro.Update();
            }          
            
        }

        protected void btnAdicionarQuestionario_Click(object sender, EventArgs e)
        {
            try
            {
                if (UCComboQuestionario.Valor <= 0)
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.QuestionarioObrigatorio").ToString());

                if (VS_lstQuestionarios.Any(q => q.qst_id == UCComboQuestionario.Valor && q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido))
                    throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.QuestionarioJaAdicionado").ToString());

                int raq_id = (VS_lstQuestionarios.Any() ? VS_lstQuestionarios.Max(q => q.raq_id) + 1 : 1);
                while (VS_lstQuestionarios.Any(q => q.raq_id == raq_id))
                    raq_id++;

                VS_lstQuestionarios.Add(new CLS_RelatorioAtendimentoQuestionario
                {
                    rea_id = UCComboRelatorioAtendimento.Valor,
                    raq_id = raq_id,
                    qst_id = UCComboQuestionario.Valor,
                    qst_titulo = UCComboQuestionario.Texto,
                    raq_ordem = (VS_lstQuestionarios.Any() ? VS_lstQuestionarios.Max(q => q.raq_ordem) + 1 : 1),
                    raq_situacao = 1,
                    IsNew = true
                });

                gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
                gvQuestionario.DataBind();
                
                UCComboQuestionario.Valor = -1;
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
                lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroAdicionarQuestionario").ToString(), UtilBO.TipoMensagem.Erro);
            }
        }
        


        protected void gvQuestionario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    bool isNewExcluir = Convert.ToBoolean(gvQuestionario.DataKeys[e.Row.RowIndex]["IsNew"]);
                    bool emUsoExcluir = Convert.ToBoolean(gvQuestionario.DataKeys[e.Row.RowIndex]["emUso"]);

                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluir.Visible = (isNewExcluir || !emUsoExcluir) && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void gvQuestionario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
             if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idExcluir = Convert.ToInt32(gvQuestionario.DataKeys[index]["raq_id"]);

                    int qst_idExcluir = Convert.ToInt32(gvQuestionario.DataKeys[index]["qst_id"]);
                    bool isNewExcluir = Convert.ToBoolean(gvQuestionario.DataKeys[index]["IsNew"]);

                  //  if (VS_rea_id > 0 && !isNewExcluir && CLS_QuestionarioBO.VerificaQuestionarioEmUso(qst_idExcluir, VS_rea_id))
                  //      throw new ValidationException(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.QuestionarioEmUso").ToString());

                    if (idExcluir > 0 && VS_lstQuestionarios.Any(l => l.raq_id == idExcluir))
                    {
                        int ind = VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idExcluir).First());
                        int ordem = VS_lstQuestionarios.Where(l => l.raq_id == idExcluir).First().raq_ordem;

                        //Ajusta as ordens
                        for (int i = ind + 1; i < VS_lstQuestionarios.Count; i++)
                        {
                            VS_lstQuestionarios[i].raq_ordem = ordem;
                            ordem += 1;
                        }

                        VS_lstQuestionarios.RemoveAt(ind);
                    }
                    VS_lstQuestionarios = VS_lstQuestionarios.OrderBy(q => q.raq_ordem).ThenBy(q => q.qst_titulo).ToList();

                    gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
                    gvQuestionario.DataBind();

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
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void gvFiltroFixo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    bool isNewExcluir = Convert.ToBoolean(gvQuestionario.DataKeys[e.Row.RowIndex]["IsNew"]);
                    bool emUsoExcluir = Convert.ToBoolean(gvQuestionario.DataKeys[e.Row.RowIndex]["emUso"]);

                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    btnExcluir.Visible = (isNewExcluir || !emUsoExcluir) && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void gvFiltroFixo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int idExcluir = Convert.ToInt32(gvQuestionario.DataKeys[index]["raq_id"]);

                    int qst_idExcluir = Convert.ToInt32(gvQuestionario.DataKeys[index]["qst_id"]);
                    bool isNewExcluir = Convert.ToBoolean(gvQuestionario.DataKeys[index]["IsNew"]);

                    if (idExcluir > 0 && VS_lstQuestionarios.Any(l => l.raq_id == idExcluir))
                    {
                        int ind = VS_lstQuestionarios.IndexOf(VS_lstQuestionarios.Where(l => l.raq_id == idExcluir).First());
                        int ordem = VS_lstQuestionarios.Where(l => l.raq_id == idExcluir).First().raq_ordem;

                        //Ajusta as ordens
                        for (int i = ind + 1; i < VS_lstQuestionarios.Count; i++)
                        {
                            VS_lstQuestionarios[i].raq_ordem = ordem;
                            ordem += 1;
                        }

                        VS_lstQuestionarios.RemoveAt(ind);
                    }
                    VS_lstQuestionarios = VS_lstQuestionarios.OrderBy(q => q.raq_ordem).ThenBy(q => q.qst_titulo).ToList();

                    gvQuestionario.DataSource = VS_lstQuestionarios.Where(q => q.raq_situacao != (byte)CLS_RelatorioAtendimentoQuestionarioSituacao.Excluido);
                    gvQuestionario.DataBind();

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
                    lblMessage.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("Configuracao", "RelatorioAtendimento.Cadastro.ErroCarregarRelatorio").ToString(), UtilBO.TipoMensagem.Erro);
                }
            }
        }

        //adicionar detalhes
        protected void btnAdicionarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagem = "";

                //carrega o valor

                if (VS_lstFiltrosFixos.Any(p => p.gff_tipoFiltro.Equals(ddlFiltroFixo.SelectedIndex)))
                    throw new ValidationException(string.Format("Filtro já existe."));

                VS_lstFiltrosFixos.Add(new REL_GraficoAtendimento_FiltrosFixos
                {     gra_id = VS_gra_id,
                    gff_tipoFiltro = Convert.ToByte(ddlFiltroFixo.SelectedIndex),
                    IsNew = true
                });


                VS_lstFiltrosFixos = VS_lstFiltrosFixos.OrderBy(q => q.gff_tipoFiltro).ToList();

                gvFiltroFixo.DataSource = VS_lstFiltrosFixos;
                gvFiltroFixo.DataBind();

                divBotoesFiltro.Visible = divRacaCor.Visible = divSexo.Visible = divIdade.Visible = divDataPreenchimento.Visible = divDetalhamentoDeficiencia.Visible = false;
                updFiltro.Update();
            }
            catch (ValidationException ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao adicionar filtro fixo.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnCancelarFiltro_Click(object sender, EventArgs e)
        {
            divBotoesFiltro.Visible = divRacaCor.Visible = divSexo.Visible = divIdade.Visible = divDataPreenchimento.Visible = divDetalhamentoDeficiencia.Visible = false;
            updFiltro.Update();
        }

        #endregion
    }
}