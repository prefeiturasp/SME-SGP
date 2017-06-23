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

namespace GestaoEscolar.Configuracao.RelatorioAtendimento
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        private bool cancelaSelect = true;

        public int EditItem
        {
            get
            {
                return Convert.ToInt32(grvDados.DataKeys[grvDados.EditIndex].Value);
            }
        }

        #endregion

        #region Métodos

        private void Pesquisar()
        {
            try
            {
                cancelaSelect = false;
                odsDados.SelectParameters.Clear();
                odsDados.SelectParameters.Add("rea_tipo", ddlTipoRelatorio.SelectedValue);

                grvDados.DataBind();

                fdsResultados.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os relatórios.",
                                                         UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
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
            }

            UCComboQtdePaginacao1.GridViewRelacionado = grvDados;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    lblMessage.Text = message;

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;

                Page.Form.DefaultButton = btnNovo.UniqueID;

                btnPesquisar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
                fdsResultados.Visible = false;
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            grvDados.PageIndex = 0;
            Pesquisar();
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/RelatorioAtendimento/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void UCComboQtdePaginacao1_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            grvDados.PageSize = UCComboQtdePaginacao1.Valor;
            grvDados.PageIndex = 0;
            // atualiza o grid
            Pesquisar();
        }

        protected void odsDados_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();

            e.Cancel = cancelaSelect;
        }

        protected void grvDados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int rea_id = Convert.ToInt32(grvDados.DataKeys[index].Value.ToString());

                    CLS_RelatorioAtendimento entity = new CLS_RelatorioAtendimento { rea_id = rea_id };
                    CLS_RelatorioAtendimentoBO.GetEntity(entity);

                    if (CLS_RelatorioAtendimentoBO.Delete(entity))
                    {
                        grvDados.PageIndex = 0;
                        Pesquisar();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "rea_id: " + rea_id);
                        lblMessage.Text = UtilBO.GetErroMessage("Relatório excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o relatório.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void grvDados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label _lblAlterar = (Label)e.Row.FindControl("_lblAlterar");
                    if (_lblAlterar != null)
                    {
                        _lblAlterar.Visible = !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    }

                    LinkButton _btnAlterar = (LinkButton)e.Row.FindControl("_btnAlterar");
                    if (_btnAlterar != null)
                    {
                        _btnAlterar.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar;
                    }
                    ImageButton _btnExcluir = (ImageButton)e.Row.FindControl("_btnExcluir");
                    if (_btnExcluir != null)
                    {
                        _btnExcluir.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir;
                        _btnExcluir.CommandArgument = e.Row.RowIndex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os relatórios.",
                                                         UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void grvDados_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CLS_RelatorioAtendimentoBO.GetTotalRecords();
        }

        protected void grvDados_PageIndexChanged(object sender, EventArgs e)
        {
            cancelaSelect = false;
        }

        #endregion

    }
}