using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.Recomendacoes
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        public int Edit_rar_id
        {
            get
            {
                return Convert.ToInt32(grvRecomendacoes.DataKeys[grvRecomendacoes.EditIndex].Values[0] ?? 0);
            }
        }

        /// <summary>
        /// Propriedade em ViewState que armazena valor de rar_id
        /// no caso de atualização de um registro ja existente.
        /// </summary>
        private int VS_rar_id
        {
            get
            {
                if (ViewState["VS_rar_id"] != null)
                {
                    return Convert.ToInt32(ViewState["VS_rar_id"]);
                }

                return -1;
            }

            set
            {
                ViewState["VS_rar_id"] = value;
            }
        }

        #endregion Propriedades

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvRecomendacoes.PageSize = UCComboQtdePaginacao1.Valor;
            grvRecomendacoes.PageIndex = 0;
            // atualiza o grid
            grvRecomendacoes.DataBind();
        }

        #endregion Delegates

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

                grvRecomendacoes.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsResultados.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsResultados.ClientID)), true);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnNovo.UniqueID;
                Page.Form.DefaultFocus = btnNovo.UniqueID;

                // Permissões da pagina
                divResultado.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);

                Pesquisar();
            }
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _grvRecomendacoes_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = ACA_RecomendacaoAlunoResponsavelBO.GetTotalRecords();

            // seta propriedades necessárias para ordenação nas colunas.
            ConfiguraColunasOrdenacao(grvRecomendacoes);
        }

        protected void _grvRecomendacoes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }

                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        protected void _grvRecomendacoes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int rar_id = Convert.ToInt32(grvRecomendacoes.DataKeys[index].Values[0]);

                    ACA_RecomendacaoAlunoResponsavel entity = new ACA_RecomendacaoAlunoResponsavel { rar_id = rar_id };
                    ACA_RecomendacaoAlunoResponsavelBO.GetEntity(entity);

                    if (ACA_RecomendacaoAlunoResponsavelBO.Delete(entity))
                    {
                        grvRecomendacoes.PageIndex = 0;
                        grvRecomendacoes.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "rar_id: " + rar_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Recomendação excluída com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir recomendação a alunos e responsáveis.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Realiza a consulta pelos filtros informados.
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                fdsResultados.Visible = true;

                Dictionary<string, string> filtros = new Dictionary<string, string>();

                odsRecomendacoes.SelectParameters.Clear();

                grvRecomendacoes.PageIndex = 0;
                odsRecomendacoes.SelectParameters.Clear();
                odsRecomendacoes.DataBind();

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvRecomendacoes.PageSize = itensPagina;
                // atualiza o grid
                grvRecomendacoes.DataBind();

                updResultado.Update();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar recomendação.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

    }
}