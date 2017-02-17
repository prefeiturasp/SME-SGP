using MSTech.GestaoEscolar.BLL;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Configuracao.FilaFechamento
{
    public partial class Busca : MotherPageLogado
    {
        #region Métodos

        /// <summary>
        /// Atualiza os grids.
        /// </summary>
        private void CarregarDadosFila()
        {
            try
            {
                grvQtFila.DataSource = CLS_AlunoFechamentoPendenciaBO.SelecionaFila_PorSituacao();
                grvQtFila.DataBind();

                int qt;
                if (!int.TryParse(txtQtdeRegistros.Text, out qt))
                {
                    qt = 20;
                }

                grvQtLogs.DataSource = CLS_AlunoFechamentoPendenciaBO.SelecionaExecucoesFila(qt, chkSomenteCompleta.Checked);
                grvQtLogs.DataBind();

            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao carregar o sistema.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion

        #region Eventos


        protected void Page_Load(object sender, EventArgs e)
        {
            if (__SessionWEB.__UsuarioWEB == null ||
                __SessionWEB.__UsuarioWEB.Usuario == null ||
                __SessionWEB.__UsuarioWEB.Grupo == null ||
                __SessionWEB.__UsuarioWEB.Grupo.vis_id != SysVisaoID.Administracao)
            {
                Response.Redirect("~/logout.ashx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                //sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }

            if (!IsPostBack)
            {
                CarregarDadosFila();
            }
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarDadosFila();
        }

        #endregion
    }
}