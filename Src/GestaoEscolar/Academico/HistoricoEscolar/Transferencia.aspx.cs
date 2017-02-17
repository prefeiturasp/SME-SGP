using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.HistoricoEscolar
{
    public partial class Transferencia : MotherPageLogado
    {
        #region Propriedades

        private long VS_alu_id
        {
            get
            {
                if (ViewState["VS_alu_id"] != null)
                    return Convert.ToInt64(ViewState["VS_alu_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        private int VS_mtu_id
        {
            get
            {
                if (ViewState["VS_mtu_id"] != null)
                    return Convert.ToInt32(ViewState["VS_mtu_id"]);
                return -1;
            }
            set
            {
                ViewState["VS_mtu_id"] = value;
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "TrocaClassesTab2", "$(document).ready(function() { $('.btnTabs').parent().removeClass('ui-tabs-selected ui-state-active'); $('.btnTab2').parent().addClass('ui-tabs-selected ui-state-active'); $('.btnTab2').removeAttr('href'); });", true);

            if (!IsPostBack)
            {
                if (!__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_inserir && !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_alterar && !__SessionWEB.__UsuarioWEB.GrupoPermissao.grp_excluir)
                    HabilitaControles(divUC.Controls, false);
                
                VS_alu_id = Convert.ToInt64(Session["alu_id"]);
                VS_mtu_id = Convert.ToInt32(Session["mtu_id"]);

                UCTransferencia.VS_alu_id = VS_alu_id;
                UCTransferencia.VS_mtu_id = VS_mtu_id;
                UCTransferencia.CarregarAluno();
            }

            UCTransferencia.clickVisualizar += VisualizarHistorico_Click;
            UCTransferencia.clickVoltar += Voltar_Click;
        }

        #endregion

        #region Métodos

        private void VisualizarHistorico_Click()
        {
            try
            {
                string report = ((int)MSTech.GestaoEscolar.BLL.ReportNameDocumentos.HistoricoEscolarPedagogico).ToString();

                XtraReport DevReport = new MSTech.GestaoEscolar.RelatoriosDevExpress.Doctos.Spo.HistoricoEscolar(VS_alu_id.ToString(), 
                                        VS_mtu_id.ToString(), __SessionWEB.__UsuarioWEB.Usuario.ent_id,
                                        GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Municipio").ToString(),
                                        GetGlobalResourceObject("Reporting", "Reporting.DocHistoricoEscolarPedagogico.Secretaria").ToString());

                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);

                GestaoEscolarUtilBO.SendParametersToReport(DevReport);
                Response.Redirect("~/Documentos/RelatorioDev.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                UCTransferencia.message = UtilBO.GetErroMessage((string)GetGlobalResourceObject("HistoricoEscolar", "HistoricoEscolar.ErroCarregarHistoricoAluno"),
                                                                UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Voltar para a tela de busca
        /// </summary>
        private void Voltar_Click()
        {
            RedirecionarPagina("Busca.aspx");
        }

        #endregion
    }
}