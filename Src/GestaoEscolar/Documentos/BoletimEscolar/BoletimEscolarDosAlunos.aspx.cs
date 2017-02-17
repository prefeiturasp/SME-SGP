using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using GestaoEscolar.WebControls.BoletimCompletoAluno;
using CFG_Relatorio = MSTech.GestaoEscolar.Entities.CFG_Relatorio;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;

namespace GestaoEscolar.Documentos.BoletimEscolar
{
    public partial class BoletimEscolarDosAlunos : MotherPageLogado
    {
        #region Eventos

        /// <summary>
        /// Load da pagina
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (string.IsNullOrEmpty(CFG_RelatorioBO.CurrentReportID))
                        RedirecionarPagina("~/Documentos/DocumentoAluno/Busca.aspx");
                    else
                    {
                        try
                        {
                            ucBoletim.Visible = false;
                            //recebe valores da sessão e grava em variáveis
                            var rlt_id = int.Parse(CFG_RelatorioBO.CurrentReportID);
                            string parametrosRel = CFG_RelatorioBO.CurrentReportParameters;

                            //Recebe os dados do relatório
                            CFG_Relatorio rpt = new CFG_Relatorio { rlt_id = rlt_id };
                            CFG_RelatorioBO.GetEntity(rpt);
                            if (rpt.IsNew)
                            {
                                lblMessage.Text = UtilBO.GetErroMessage("Relatório não encontrado.", UtilBO.TipoMensagem.Alerta);
                            }
                            else
                            {
                                string[] param = parametrosRel.ToLower().Split('&');

                                var tpc_id = Convert.ToInt32(param.First(p => p.StartsWith("periodoavaliacao=")).Split('=')[1]);

                                var alu_ids = param.First(p => p.StartsWith("alu_id=")).Split('=')[1].Split(',').Where(a => !string.IsNullOrEmpty(a)).Select(a => Convert.ToInt64(a)).ToArray();
                                var mtu_ids = param.First(p => p.StartsWith("mtu_id=")).Split('=')[1].Split(',').Where(a => !string.IsNullOrEmpty(a)).Select(a => Convert.ToInt32(a)).ToArray();

                                ucBoletim.Carregar(tpc_id, alu_ids, mtu_ids, false);
                                ucBoletim.Visible = true;
                            }
                        }
                        catch (ValidationException ex)
                        {
                            lblMessage.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                        }
                        catch (Exception err)
                        {
                            ApplicationWEB._GravaErro(err);
                            lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o boletim do aluno.", UtilBO.TipoMensagem.Erro);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMessage.Text = UtilBO.GetErroMessage("Erro ao tentar carregar o relatório.", UtilBO.TipoMensagem.Erro);
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Documentos/DocumentoAluno/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion
    }
}