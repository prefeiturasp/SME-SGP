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

namespace GestaoEscolar.Configuracao.ParametroResultado
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Propriedade que guarda o tpr_id do registro para alteração
        /// </summary>
        public int Edit_tpr_id
        {
            get
            {
                return Convert.ToInt32(grvResultado.DataKeys[grvResultado.EditIndex].Values[0] ?? 0);
            }
        }

        #endregion Propriedades

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvResultado.PageSize = UCComboQtdePaginacao1.Valor;
            grvResultado.PageIndex = 0;
            // atualiza o grid
            grvResultado.DataBind();
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

                grvResultado.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsResultado.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsResultado.ClientID)), true);
                    }

                    //Inicializa os campos de busca
                    InicializaCamposBusca();

                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnPesquisar.UniqueID;
                Page.Form.DefaultFocus = UCCCursoCurriculo.ClientID;

                // Permissões da pagina
                divPesquisa.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
            }
        }



        protected void _btnLimparPesquisa_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/ParametroResultado/Busca.aspx", false);
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/ParametroResultado/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void _btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Pesquisar();
            }
        }

        protected void grvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir && (__SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.UnidadeAdministrativa);
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
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
                    int tpr_id = Convert.ToInt32(grvResultado.DataKeys[index].Values[0]);

                    ACA_TipoResultado entity = new ACA_TipoResultado { tpr_id = tpr_id };

                    if (ACA_TipoResultadoBO.Delete(entity))
                    {
                        grvResultado.PageIndex = 0;
                        grvResultado.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "tpr_id: " + tpr_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Parâmetro de resultado excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir parâmetro de resultado.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvResultado_DataBound(object sender, EventArgs e)
        {
            // Mostra o total de registros
            UCTotalRegistros1.Total = ACA_TipoResultadoBO.GetTotalRecords();
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

                odsResultado.SelectParameters.Clear();

                grvResultado.PageIndex = 0;
                odsResultado.SelectParameters.Clear();
                odsResultado.SelectParameters.Add("cur_id", UCCCursoCurriculo.Valor[0].ToString());
                odsResultado.SelectParameters.Add("crr_id", UCCCursoCurriculo.Valor[1].ToString());

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                grvResultado.PageSize = itensPagina;
                // atualiza o grid
                grvResultado.DataBind();

                updResultado.Update();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar parâmetro de resultado.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega os campos de busca com a configuração padrão para uma nova busca
        /// </summary>
        private void InicializaCamposBusca()
        {
            UCCCursoCurriculo.CarregarPorSituacaoCurso(1);
        }

        #endregion
    }
}