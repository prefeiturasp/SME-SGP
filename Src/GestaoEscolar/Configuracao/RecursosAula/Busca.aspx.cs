using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.RecursosAula
{
    public partial class Busca : MotherPageLogado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MsgConfirmExclusao));
            }

            UCComboQtdePaginacao1.GridViewRelacionado = dgvRecurso;

            if (!IsPostBack)
            {
                string message = __SessionWEB.PostMessages;
                if (!String.IsNullOrEmpty(message))
                    _lblMessage.Text = message;

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                // mostra essa quantidade no combobox
                UCComboQtdePaginacao1.Valor = itensPagina;
                // atribui essa quantidade para o grid
                dgvRecurso.PageSize = itensPagina;
                // atualiza o grid
                dgvRecurso.DataBind();

                Page.Form.DefaultButton = _btnNovo.UniqueID;

                dgvRecurso.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnNovo.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        #region Propriedades

        public int EditItem
        {
            get
            {
                return Convert.ToInt32(dgvRecurso.DataKeys[dgvRecurso.EditIndex].Value);
            }
        }

        #endregion

        #region Delegates

        protected void UCComboQtdePaginacao_IndexChanged()
        {
            // atribui nova quantidade itens por página para o grid
            dgvRecurso.PageSize = UCComboQtdePaginacao1.Valor;
            dgvRecurso.PageIndex = 0;
            // atualiza o grid
            dgvRecurso.DataBind();
        }

        #endregion

        #region Eventos

        protected void odsDados_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }

        protected void dgvRecurso_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int rsa_id = Convert.ToInt32(dgvRecurso.DataKeys[index].Value.ToString());

                    ACA_RecursosAula entity = new ACA_RecursosAula { rsa_id = rsa_id };
                    ACA_RecursosAulaBO.GetEntity(entity);

                    if (ACA_RecursosAulaBO.Delete(entity))
                    {
                        dgvRecurso.PageIndex = 0;
                        dgvRecurso.DataBind();
                        ApplicationWEB._GravaLogSistema(LOG_SistemaTipo.Delete, "rsa_id: " + rsa_id);
                        _lblMessage.Text = UtilBO.GetErroMessage("Recurso de aula excluído com sucesso.", UtilBO.TipoMensagem.Sucesso);
                    }
                }
                catch (ValidationException ex)
                {
                    _lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                }
                catch (Exception ex)
                {
                    ApplicationWEB._GravaErro(ex);
                    _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar excluir o recurso de aula.", UtilBO.TipoMensagem.Erro);
                }
            }
        }

        protected void dgvRecurso_RowDataBound(object sender, GridViewRowEventArgs e)
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
                _lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar recursos de aula.",
                                                         UtilBO.TipoMensagem.Erro);
                ApplicationWEB._GravaErro(ex);
            }
        }

        protected void _btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configuracao/RecursosAula/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion
    }
}
