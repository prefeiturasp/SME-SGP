using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.Questionario
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        public int PaginaQuestionario_qst_id
        {
            get
            {
                if (grvResultado.EditIndex >= 0)
                    return Convert.ToInt32(grvResultado.DataKeys[grvResultado.EditIndex].Values["qst_id"] ?? 0);
                else return -1;
            }
            set { }
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

        #region Delegates

        protected void UCComboQtdePaginacao1_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvResultado.PageSize = UCComboQtdePaginacao1.Valor;
            grvResultado.PageIndex = 0;
            // atualiza o grid
            grvResultado.DataBind();
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

            UCComboQtdePaginacao1.GridViewRelacionado = grvResultado;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvResultado.PageIndex = 0;
                grvResultado.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsResultado.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsResultado.ClientID)), true);
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

        protected void grvResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int qst_id = Convert.ToInt32(grvResultado.DataKeys[index].Values["qst_id"]);

                    CLS_Questionario entity = new CLS_Questionario { qst_id = qst_id };

                    //Verifica os conteúdos do questionário
                    DataTable ConteudosQuestionario = CLS_QuestionarioConteudoBO.SelectByQuestionario(qst_id);
                    var listQtc_ids = ConteudosQuestionario.AsEnumerable().Select(q => q.Field<int>("qtc_id")).ToArray();
                    string qtc_ids = string.Join(",", listQtc_ids.Select(item => item.ToString()).ToArray());
                    
                    //Verifica se o questionário não foi respondido
                    if (!CLS_QuestionarioConteudoPreenchimentoBO.ConteudoPreenchido(qtc_ids))
                    {
                        if (CLS_QuestionarioBO.Delete(entity))
                        {
                            grvResultado.PageIndex = 0;
                            grvResultado.DataBind();
                            ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "qst_id: " + qst_id);
                            lblMessage.Text = UtilBO.GetErroMessage("Questionário excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                        }
                    }
                    else
                    {
                        grvResultado.PageIndex = 0;
                        grvResultado.DataBind();
                        throw new ValidationException("O questionário não pode ser excluído, pois já foi respondido.");
                    }
                        
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir questionário.", UtilBO.TipoMensagem.Erro);
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
                    bool emUso = Convert.ToBoolean(grvResultado.DataKeys[e.Row.RowIndex].Values["emUso"].ToString());

                    btnExcluir.Visible = !emUso && __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void grvResultado_DataBound(object sender, EventArgs e)
        {
            // Mostra o total de registros
            UCTotalRegistros1.Total = CLS_QuestionarioBO.GetTotalRecords();
            ConfiguraColunasOrdenacao(grvResultado);
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/Questionario/CadastroQuestionario.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region Métodos

        private void Pesquisar()
        {
            try
            {
                fdsResultado.Visible = true;
                
                grvResultado.PageIndex = 0;
                odsResultado.SelectParameters.Clear();

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                grvResultado.Sort("", SortDirection.Ascending);
                
                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvResultado.PageSize = itensPagina;
                // atualiza o grid
                grvResultado.DataBind();
                //grvResultado.Sort("qst_titulo", SortDirection.Ascending);
                updResultado.Update();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar questionários.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        protected void odsResultado_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }
    }
}