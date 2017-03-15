using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ObjetoAprendizagem
{
    public partial class Cadastro : MotherPageLogado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();

                if ((PreviousPage != null) && (PreviousPage.IsCrossPagePostBack))
                {
                }
            }
        }

        protected void _btnSalvar_Click(object sender, EventArgs e)
        {

        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Academico/ObjetoAprendizagem/Busca.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void LoadPage()
        {
            var tds_id = Convert.ToInt32(Session["tds_id_oap"]);

            var tds = new ACA_TipoDisciplina { tds_id = tds_id };
            ACA_TipoDisciplinaBO.GetEntity(tds);

            txtDisciplina.Text = tds.tds_nome;

            rptCampos.DataSource = ACA_TipoCicloBO.GetSelect();
            rptCampos.DataBind();
        }

        protected void cvCiclos_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            foreach (RepeaterItem item in rptCampos.Items)
            {
                CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                if (ckbCampo != null && ckbCampo.Checked)
                {
                    args.IsValid = true;
                    break;
                }
            }
        }
    }
}