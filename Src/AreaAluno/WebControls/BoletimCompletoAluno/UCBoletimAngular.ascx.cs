using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Web.UI;
using System.Linq;

namespace AreaAluno.WebControls.BoletimCompletoAluno
{
    public partial class UCBoletimAngular : MotherUserControl
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
                sm.Scripts.Add(new ScriptReference("~/includes/Angular/angular.js"));
                sm.Scripts.Add(new ScriptReference("~/includes/Angular/module.js"));
                sm.Scripts.Add(new ScriptReference("~/includes/Angular/Boletim.controller.js"));
            }

            if (!IsPostBack)
            {
                var textoRodape = GetGlobalResourceObject("Mensagens", "MSG_RODAPEBOLETIMCOMPLETO").ToString();
                divRodape.Visible = !string.IsNullOrWhiteSpace(textoRodape);
                lblRodape.Text = textoRodape;

                var textoRodapeInfantil = GetGlobalResourceObject("Mensagens", "MSG_RODAPEBOLETIMCOMPLETOInfantil").ToString();
                divRodapeInfantil.Visible = !string.IsNullOrWhiteSpace(textoRodapeInfantil);
                lblRodapeInfantil.Text = textoRodapeInfantil;
            }
        }

        public void CarregaBoletim(long aluId, int mtuId, int tpcId, bool mostrarPeriodos)
        {

            tpc_id = tpcId;
            alu_ids = aluId.ToString();
            mtu_ids = mtuId.ToString();

            if (aluId > 0)
            {
                var matriculas = ACA_AlunoBO.BuscarMatriculasPeriodos(new long[] { aluId }, new int[] { mtuId });

                int temp;
                if (tpc_id == 0 && int.TryParse(matriculas.Rows[0]["tpc_id"].ToString(), out temp))
                {
                    tpc_id = temp;
                }

                rptPeriodos.DataSource = matriculas;
                rptPeriodos.DataBind();

            }
        }
    }
}