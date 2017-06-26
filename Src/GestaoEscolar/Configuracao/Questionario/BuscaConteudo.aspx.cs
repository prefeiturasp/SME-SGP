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
    public partial class BuscaConteudo : MotherPageLogado
    {
        #region Propriedades
        public int PaginaConteudo_qtc_id
        {
            get
            {
                if (grvResultado.EditIndex >= 0)
                    return Convert.ToInt32(grvResultado.DataKeys[grvResultado.EditIndex].Values["qtc_id"] ?? 0);
                else return -1;
            }
            set { }
        }

        private int _VS_qtc_id
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
                        _VS_qst_id = PreviousPage.PaginaQuestionario_qst_id;
                    else
                    {
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

                byte tipoConteudo = Convert.ToByte(grvResultado.DataKeys[e.Row.RowIndex].Values["qtc_tipo"].ToString());
                byte tipoResposta = Convert.ToByte(grvResultado.DataKeys[e.Row.RowIndex].Values["qtc_tipoResposta"].ToString());

                Label lblTipoConteudo = (Label)e.Row.FindControl("lblTipoConteudo");
                if (lblTipoConteudo != null)
                {
                    lblTipoConteudo.Text = GestaoEscolarUtilBO.GetEnumDescription((QuestionarioTipoConteudo)tipoConteudo);
                }
                Label lblTipoResposta = (Label)e.Row.FindControl("lblTipoResposta");
                if (lblTipoResposta != null)
                {
                    lblTipoResposta.Text = tipoResposta > 0 ? GestaoEscolarUtilBO.GetEnumDescription((QuestionarioTipoResposta)tipoResposta) : "-";
                }

                Button btnIncluirRespostas = (Button)e.Row.FindControl("btnIncluirRespostas");
                if (btnIncluirRespostas != null)
                {
                    btnIncluirRespostas.Visible = (tipoConteudo == (byte)QuestionarioTipoConteudo.Pergunta) && (tipoResposta != (byte)QuestionarioTipoResposta.TextoAberto);
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

        protected void grvResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int qtc_id = Convert.ToInt32(grvResultado.DataKeys[index].Values["qtc_id"]);

                    CLS_QuestionarioConteudo entity = new CLS_QuestionarioConteudo { qtc_id = qtc_id };

                    if (CLS_QuestionarioConteudoBO.Delete(entity))
                    {
                        grvResultado.PageIndex = 0;
                        grvResultado.DataBind();

                        if (grvResultado.Rows.Count > 0)
                        {
                            ((ImageButton)grvResultado.Rows[0].FindControl("_btnSubir")).Style.Add("visibility", "hidden");
                            ((ImageButton)grvResultado.Rows[grvResultado.Rows.Count - 1].FindControl("_btnDescer")).Style.Add("visibility", "hidden");
                        }

                        

                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "qst_id: " + entity.qst_id + ", qtc_id: " + entity.qtc_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Conteúdo excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir conteúdo.", UtilBO.TipoMensagem.Erro);
                }
            }

            if (e.CommandName == "Subir")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());

                    int qtc_idDescer = Convert.ToInt32(grvResultado.DataKeys[index - 1]["qtc_id"]);
                    int qtc_ordemDescer = Convert.ToInt32(grvResultado.DataKeys[index]["qtc_ordem"]);
                    CLS_QuestionarioConteudo entityDescer = new CLS_QuestionarioConteudo { qtc_id = qtc_idDescer, qst_id = _VS_qst_id };
                    CLS_QuestionarioConteudoBO.GetEntity(entityDescer);
                    entityDescer.qtc_ordem = qtc_ordemDescer;

                    int qtc_idSubir = Convert.ToInt32(grvResultado.DataKeys[index]["qtc_id"]);
                    int qtc_ordemSubir = Convert.ToInt32(grvResultado.DataKeys[index - 1]["qtc_ordem"]);
                    CLS_QuestionarioConteudo entitySubir = new CLS_QuestionarioConteudo { qtc_id = qtc_idSubir, qst_id = _VS_qst_id };
                    CLS_QuestionarioConteudoBO.GetEntity(entitySubir);
                    entitySubir.qtc_ordem = qtc_ordemSubir;

                    if (CLS_QuestionarioConteudoBO.SaveOrdem(entityDescer, entitySubir))
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

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtc_id: " + qtc_idSubir + ", qts_id: " + _VS_qst_id);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtc_id: " + qtc_idDescer + ", qts_id: " + _VS_qst_id);
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

                    int qtc_idDescer = Convert.ToInt32(grvResultado.DataKeys[index]["qtc_id"]);
                    int qtc_ordemDescer = Convert.ToInt32(grvResultado.DataKeys[index + 1]["qtc_ordem"]);
                    CLS_QuestionarioConteudo entityDescer = new CLS_QuestionarioConteudo { qtc_id = qtc_idDescer, qst_id = _VS_qst_id };
                    CLS_QuestionarioConteudoBO.GetEntity(entityDescer);
                    entityDescer.qtc_ordem = qtc_ordemDescer;

                    int qtc_idSubir = Convert.ToInt32(grvResultado.DataKeys[index + 1]["qtc_id"]);
                    int qtc_ordemSubir = Convert.ToInt32(grvResultado.DataKeys[index]["qtc_ordem"]);
                    CLS_QuestionarioConteudo entitySubir = new CLS_QuestionarioConteudo { qtc_id = qtc_idSubir, qst_id = _VS_qst_id };
                    CLS_QuestionarioConteudoBO.GetEntity(entitySubir);
                    entitySubir.qtc_ordem = qtc_ordemSubir;

                    if (CLS_QuestionarioConteudoBO.SaveOrdem(entityDescer, entitySubir))
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

                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtc_id: " + qtc_idSubir + ", qts_id: " + _VS_qst_id);
                    ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Update, "qtc_id: " + qtc_idDescer + ", qts_id: " + _VS_qst_id);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvResultado_DataBound(object sender, EventArgs e)
        {
            // Mostra o total de registros
            UCTotalRegistros1.Total = CLS_QuestionarioConteudoBO.GetTotalRecords();
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

                odsResultado.SelectParameters.Clear();

                grvResultado.PageIndex = 0;
                odsResultado.SelectParameters.Clear();
                odsResultado.SelectParameters.Add("qst_id", _VS_qst_id.ToString());

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

                lblInfo.Text = "<b>Questionário: </b>"+ CLS_QuestionarioBO.GetEntity(new CLS_Questionario { qst_id = _VS_qst_id }).qst_titulo +"<br>";

                updResultado.Update();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar conteúdos.", UtilBO.TipoMensagem.Erro);
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

    }
}