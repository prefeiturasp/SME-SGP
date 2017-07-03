using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.Alertas
{
    public partial class Busca : MotherPageLogado
    {
        #region Propriedades

        public short EditItem
        {
            get
            {
                return Convert.ToInt16(grvAlertas.DataKeys[grvAlertas.EditIndex].Value);
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Realiza a consulta dos alertas.
        /// </summary>
        public void Pesquisar()
        {
            grvAlertas.PageIndex = 0;
            odsAlertas.DataBind();
            grvAlertas.DataBind();
        }

        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string message = __SessionWEB.PostMessages;
                    if (!string.IsNullOrEmpty(message))
                    {
                        lblMessage.Text = message;
                    }

                    Pesquisar();
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar os alertas.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void grvAlertas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool permiteConsultar = __SessionWEB.__UsuarioWEB.GrupoPermissao.grp_consultar;
                Label lblAlterar = (Label)e.Row.FindControl("lblAlterar");
                if (lblAlterar != null)
                {
                    lblAlterar.Visible = !permiteConsultar;
                }

                LinkButton btnAlterar = (LinkButton)e.Row.FindControl("btnAlterar");
                if (btnAlterar != null)
                {
                    btnAlterar.Visible = permiteConsultar;
                }
            }
        }

        protected void grvAlertas_DataBound(object sender, EventArgs e)
        {
            UCTotalRegistros1.Total = CFG_AlertaBO.GetTotalRecords();
        }

        #endregion Eventos
    }
}