using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.Questionario
{
    public partial class BuscaResposta : MotherPageLogado
    {
        #region Constantes

        private const int indiceColunaPeso = 1;

        #endregion

        #region Propriedades
        public int _VS_qtc_id
        {
            get
            {
                if (ViewState["_VS_qtc_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_qtc_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_qtc_id"] = value;
            }
        }

        public int _VS_qst_id
        {
            get
            {
                if (ViewState["_VS_qst_id"] != null)
                    return Convert.ToInt32(ViewState["_VS_qst_id"]);
                return -1;
            }
            set
            {
                ViewState["_VS_qst_id"] = value;
            }
        }

        public int PaginaResposta_qtr_id
        {
            get
            {
                if (grvResultado.EditIndex >= 0)
                    return Convert.ToInt32(grvResultado.DataKeys[grvResultado.EditIndex].Values["qtr_id"] ?? 0);
                else return -1;
            }
            set { }
        }

        public bool IsMultiplaSelecao
        {
            get
            {
                if (ViewState["IsMultiplaSelecao"] != null)
                    return Convert.ToBoolean(ViewState["IsMultiplaSelecao"]);
                return false;
            }
            set
            {
                ViewState["IsMultiplaSelecao"] = value;
            }
        }

        #endregion

        #region Delegates

        protected void UCComboQtdePaginacao1_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvResultado.PageSize = UCComboQtdePaginacao1.Valor;
            grvResultado.PageIndex = 0;
            // atualiza o grid
            grvResultado.DataBind();
            if (grvResultado.Rows.Count > 0)
            {
                ((ImageButton)grvResultado.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                ((ImageButton)grvResultado.Rows[grvResultado.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
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
            }

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvResultado.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsResultado.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsResultado.ClientID)), true);
                    }

                    if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                    {
                        _VS_qtc_id = PreviousPage.PaginaConteudo_qtc_id;
                        _VS_qst_id = PreviousPage._VS_qst_id;
                    }

                    else
                    {
                        int qtc_id = !String.IsNullOrEmpty(Session["qtc_id"].ToString()) ? Convert.ToInt32(Session["qtc_id"]) : -1;
                        if (qtc_id > 0)
                            _VS_qtc_id = qtc_id;

                        int qst_id = !String.IsNullOrEmpty(Session["qst_id"].ToString()) ? Convert.ToInt32(Session["qst_id"]) : -1;
                        if (qst_id > 0)
                            _VS_qst_id = qst_id;
                    }

                    Pesquisar();

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                // Permissões da pagina
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session["qtc_id"] = _VS_qtc_id;
            Session["qst_id"] = _VS_qst_id;
            Response.Redirect("~/Configuracao/Questionario/BuscaConteudo.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void grvResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int qtr_id = Convert.ToInt32(grvResultado.DataKeys[index].Values["qtr_id"]);

                    CLS_QuestionarioResposta entity = new CLS_QuestionarioResposta { qtr_id = qtr_id };

                    if (CLS_QuestionarioRespostaBO.Delete(entity))
                    {
                        grvResultado.PageIndex = 0;
                        grvResultado.DataBind();
                        if (grvResultado.Rows.Count > 0)
                        {
                            ((ImageButton)grvResultado.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)grvResultado.Rows[grvResultado.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "qtr_id: " + entity.qtr_id + ", qtc_id: " + entity.qtc_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Resposta excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir resposta.", UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int qtr_idDescer = Convert.ToInt32(grvResultado.DataKeys[index - 1]["qtr_id"]);
                    int qtr_ordemDescer = Convert.ToInt32(grvResultado.DataKeys[index]["qtr_ordem"]);
                    CLS_QuestionarioResposta entityDescer = new CLS_QuestionarioResposta { qtr_id = qtr_idDescer, qtc_id = _VS_qtc_id };
                    CLS_QuestionarioRespostaBO.GetEntity(entityDescer);
                    entityDescer.qtr_ordem = qtr_ordemDescer;

                    int qtr_idSubir = Convert.ToInt32(grvResultado.DataKeys[index]["qtr_id"]);
                    int qtr_ordemSubir = Convert.ToInt32(grvResultado.DataKeys[index - 1]["qtr_ordem"]);
                    CLS_QuestionarioResposta entitySubir = new CLS_QuestionarioResposta { qtr_id = qtr_idSubir, qtc_id = _VS_qtc_id };
                    CLS_QuestionarioRespostaBO.GetEntity(entitySubir);
                    entitySubir.qtr_ordem = qtr_ordemSubir;

                    if (CLS_QuestionarioRespostaBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        grvResultado.DataBind();
                        grvResultado.PageIndex = 0;
                        grvResultado.DataBind();

                        if (grvResultado.Rows.Count > 0)
                        {
                            ((ImageButton)grvResultado.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)grvResultado.Rows[grvResultado.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtr_id: " + qtr_idSubir + ", qtc_id: " + _VS_qtc_id);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtr_id: " + qtr_idDescer + ", qtc_id: " + _VS_qtc_id);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Descer")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int qtr_idDescer = Convert.ToInt32(grvResultado.DataKeys[index]["qtr_id"]);
                    int qtr_ordemDescer = Convert.ToInt32(grvResultado.DataKeys[index + 1]["qtr_ordem"]);
                    CLS_QuestionarioResposta entityDescer = new CLS_QuestionarioResposta { qtr_id = qtr_idDescer, qtc_id = _VS_qtc_id };
                    CLS_QuestionarioRespostaBO.GetEntity(entityDescer);
                    entityDescer.qtr_ordem = qtr_ordemDescer;

                    int qtr_idSubir = Convert.ToInt32(grvResultado.DataKeys[index + 1]["qtr_id"]);
                    int qtr_ordemSubir = Convert.ToInt32(grvResultado.DataKeys[index]["qtr_ordem"]);
                    CLS_QuestionarioResposta entitySubir = new CLS_QuestionarioResposta { qtr_id = qtr_idSubir, qtc_id = _VS_qtc_id };
                    CLS_QuestionarioRespostaBO.GetEntity(entitySubir);
                    entitySubir.qtr_ordem = qtr_ordemSubir;

                    if (CLS_QuestionarioRespostaBO.SaveOrdem(entityDescer, entitySubir))
                    {
                        grvResultado.DataBind();
                        grvResultado.PageIndex = 0;
                        grvResultado.DataBind();

                        if (grvResultado.Rows.Count > 0)
                        {
                            ((ImageButton)grvResultado.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)grvResultado.Rows[grvResultado.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }
                    }

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtr_id: " + qtr_idSubir + ", qtc_id: " + _VS_qtc_id);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtr_id: " + qtr_idDescer + ", qtc_id: " + _VS_qtc_id);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
                if (lblAlterar != null)
                {
                    lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                Label lblPermiteAdicionarTexto = (Label)e.Row.FindControl("lblPermiteAdicionarTexto");
                if (lblPermiteAdicionarTexto != null)
                {
                    lblPermiteAdicionarTexto.Text = Convert.ToBoolean(grvResultado.DataKeys[e.Row.RowIndex].Values["qtr_permiteAdicionarTexto"].ToString()) ? "Sim" : "Não";
                }

                grvResultado.Columns[indiceColunaPeso].Visible = IsMultiplaSelecao;
                Label lblPeso = (Label)e.Row.FindControl("lblPeso");
                if (lblPeso != null)
                {
                    lblPeso.Text = grvResultado.DataKeys[e.Row.RowIndex].Values["qtr_peso"].ToString();
                }

                ImageButton _btnSubir = (ImageButton)e.Row.FindControl("_btnSubir");
                if (_btnSubir != null)
                {
                    _btnSubir.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "cima.png";
                    _btnSubir.CommandArgument = e.Row.RowIndex.ToString();
                    _btnSubir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }

                ImageButton _btnDescer = (ImageButton)e.Row.FindControl("_btnDescer");
                if (_btnDescer != null)
                {
                    _btnDescer.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "baixo.png";
                    _btnDescer.CommandArgument = e.Row.RowIndex.ToString();
                    _btnDescer.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                }
            }
        }

        protected void grvResultado_DataBound(object sender, EventArgs e)
        {
            // Mostra o total de registros
            UCTotalRegistros1.Total = CLS_QuestionarioRespostaBO.GetTotalRecords();
            ConfiguraColunasOrdenacao(grvResultado);
        }

        protected void odsResultado_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        #endregion

        #region Métodos
        private void Pesquisar()
        {
            try
            {
                fdsResultado.Visible = true;

                CLS_QuestionarioConteudo Conteudo = CLS_QuestionarioConteudoBO.GetEntity(new CLS_QuestionarioConteudo { qtc_id = _VS_qtc_id });
                IsMultiplaSelecao = Conteudo.qtc_tipoResposta == (byte)QuestionarioTipoResposta.MultiplaSelecao;

                lblInfo.Text = "<b>Questionário: </b>" + CLS_QuestionarioBO.GetEntity(new CLS_Questionario { qst_id = _VS_qst_id }).qst_titulo +
                                "<br><b>Conteúdo: </b>" + Conteudo.qtc_texto + "<br>";

                odsResultado.SelectParameters.Clear();

                grvResultado.PageIndex = 0;
                odsResultado.SelectParameters.Clear();
                odsResultado.SelectParameters.Add("qtc_id", _VS_qtc_id.ToString());

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvResultado.PageSize = itensPagina;
                // atualiza o grid
                grvResultado.DataBind();

                if (grvResultado.Rows.Count > 0)
                {
                    ((ImageButton)grvResultado.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                    ((ImageButton)grvResultado.Rows[grvResultado.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                }
                
                updResultado.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar respostas.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion
    }
}