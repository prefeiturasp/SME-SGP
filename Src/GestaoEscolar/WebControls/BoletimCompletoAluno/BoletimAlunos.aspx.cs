using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web.UI;

namespace GestaoEscolar.WebControls.BoletimCompletoAluno
{
    public partial class BoletimAlunos : MotherPageLogado
    {
        protected string alu_ids
        {
            get { return ViewState["alu_ids"] as string ?? string.Empty; }
            set { ViewState["alu_ids"] = value; }
        }
        protected string mtu_ids
        {
            get { return ViewState["mtu_ids"] as string ?? string.Empty; }
            set { ViewState["mtu_ids"] = value; }
        }
        protected int tpc_id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/angular.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/module.js"));
                sm.Scripts.Add(new ScriptReference("~/Includes/Angular/Boletim.controller.js"));
            }

            if (!IsPostBack)
            {
                int temp;
                if (Session["tpc_id"] != null && int.TryParse(Session["tpc_id"].ToString(), out temp))
                    tpc_id = temp;

                alu_ids = Session["alu_ids"] as string;
                mtu_ids = Session["mtu_ids"] as string;

                bool visible = true;
                if (Session["mostrarPeriodos"] != null)
                {
                    bool.TryParse(Session["mostrarPeriodos"].ToString(), out visible);
                }
                if (visible && !string.IsNullOrEmpty(alu_ids) && !alu_ids.Contains(","))
                {
                    long alu_id = 0;
                    int mtu_id = 0;

                    if (long.TryParse(alu_ids, out alu_id)
                        && (string.IsNullOrEmpty(mtu_ids) || int.TryParse(mtu_ids, out mtu_id)))
                    {
                        var matriculas = ACA_AlunoBO.BuscarMatriculasPeriodos(new long[] { alu_id }, new int[] { mtu_id });

                        rptPeriodos.DataSource = matriculas;
                        rptPeriodos.DataBind();
                    }
                }

                var textoRodape = GetGlobalResourceObject("Mensagens", "MSG_RODAPEBOLETIMCOMPLETO").ToString();
                divRodape.Visible = !string.IsNullOrWhiteSpace(textoRodape);
                lblRodape.Text = textoRodape;

                var textoRodapeInfantil = GetGlobalResourceObject("Mensagens", "MSG_RODAPEBOLETIMCOMPLETOInfantil").ToString();
                divRodapeInfantil.Visible = !string.IsNullOrWhiteSpace(textoRodapeInfantil);
                lblRodapeInfantil.Text = textoRodapeInfantil;

                var textoRodapeFreqExterna = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MENSAGEM_FREQUENCIA_EXTERNA, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                lblFreqExterna.Text = !string.IsNullOrEmpty(textoRodapeFreqExterna) ? "* " + textoRodapeFreqExterna : textoRodapeFreqExterna;
                lblFreqExternaInfantil.Text = !string.IsNullOrEmpty(textoRodapeFreqExterna) ? "* " + textoRodapeFreqExterna : textoRodapeFreqExterna;
            }
        }
    }
}