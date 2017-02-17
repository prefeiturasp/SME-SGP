using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.Validation.Exceptions;
using System.Data;

namespace GestaoEscolar.Configuracao.ObservacaoBoletim
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Salva o valor de obb_id quando edicao.
        /// </summary>
        /// <value>
        /// edit_obb_id.
        /// </value>
        public int Edit_obb_id
        {
            get
            {
                return Convert.ToInt32(grvObservacoes.DataKeys[grvObservacoes.EditIndex].Values[0] ?? 0);
            }
        }

        #endregion Propriedades

        #region Delegates

        /// <summary>
        /// Trata o numero de linhas por pagina da grid.
        /// </summary>
        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvObservacoes.PageSize = UCComboQtdePaginacao1.Valor;
            grvObservacoes.PageIndex = 0;
            // atualiza o grid
            grvObservacoes.DataBind();
        }

        #endregion Delegates

        #region Eventos

        /// <summary>
        /// Load da pagina
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                grvObservacoes.PageSize = ApplicationWEB._Paginacao;

                try
                {
                    if (__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), fdsEscola.ClientID, String.Format("MsgInformacao('{0}');", String.Concat("#", fdsEscola.ClientID)), true);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o sistema.", UtilBO.TipoMensagem.Erro);
                }

                Page.Form.DefaultButton = btnNovo.UniqueID;
                Page.Form.DefaultFocus = grvObservacoes.ClientID;

                // Permissões da pagina
                grvObservacoes.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        /// <summary>
        /// Controle do databound da grid e tratamentos necessarios.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void grvObservacoes_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CFG_ObservacaoBoletimBO.GetTotalRecords();
        }

        /// <summary>
        /// Controle do row databound da grid e tratamentos necessarios.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grvObservacoes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = (ImageButton)e.Row.FindControl("btnExcluir");
                if (btnExcluir != null)
                {
                    btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                    btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                }
            }
        }

        /// <summary>
        /// Controle do rowcommand da grid e tratamentos necessarios.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void grvObservacoes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int obb_id = Convert.ToInt32(grvObservacoes.DataKeys[index].Values[0]);

                    CFG_ObservacaoBoletim entity = new CFG_ObservacaoBoletim { obb_id = obb_id, 
                                                                               ent_id = __SessionWEB.__UsuarioWEB.Usuario.ent_id };

                    if (CFG_ObservacaoBoletimBO.Delete(entity))
                    {
                        grvObservacoes.PageIndex = 0;
                        grvObservacoes.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "obb_id: " + obb_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Observação do boletim excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao excluir observação do boletim.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        /// <summary>
        /// Chama tela cadastro para insercao de nova observacao.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/ObservacaoBoletim/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion Eventos

    }
}