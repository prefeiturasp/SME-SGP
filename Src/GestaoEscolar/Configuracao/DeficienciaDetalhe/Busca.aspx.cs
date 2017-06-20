using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;

namespace GestaoEscolar.Configuracao.DeficienciaDetalhe
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        /// <summary>
        /// Recebe o Id para enviar os dados para edição.
        /// </summary>
        public Guid EditItem
        {
            get
            {
                return new Guid(_dgvDeficienciaDetalhe.DataKeys[_dgvDeficienciaDetalhe.EditIndex].Value.ToString());
            }
        }

        #endregion

        #region Eventos Page Life Cycle

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
                    _lblMessage.Text = message;

                // quantidade de itens por página
                string qtItensPagina = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.QT_ITENS_PAGINACAO);
                int itensPagina = string.IsNullOrEmpty(qtItensPagina) ? ApplicationWEB._Paginacao : Convert.ToInt32(qtItensPagina);

                _dgvDeficienciaDetalhe.PageSize = itensPagina;
                _dgvDeficienciaDetalhe.DataBind();

                _dgvDeficienciaDetalhe.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                UCTotalRegistros1.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                _btnNovoDetalhamento.Visible = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir;
            }
        }

        #endregion

        #region Eventos

        protected void _dgvDeficienciaDetalhe_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CFG_DeficienciaDetalheBO.GetTotalRecords();

        }

        protected void odsDeficienciaDetalhe_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
                e.InputParameters.Clear();
        }


        protected void _dgvDeficienciaDetalhe_RowDataBound(object sender, GridViewRowEventArgs e)
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
            }
        }

        protected void _btnNovoDetalhamento_Click(object sender, EventArgs e)
        {
            Response.Redirect(__SessionWEB._AreaAtual._Diretorio + "Configuracao/DeficienciaDetalhe/Cadastro.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion
    }
}