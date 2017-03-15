using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GestaoEscolar.Academico.ObjetoAprendizagem
{
    public partial class Cadastro : MotherPageLogado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                LoadPage();
        }

        protected void _btnSalvar_Click(object sender, EventArgs e)
        {

        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
        }

        private void LoadPage()
        {
            rptCampos.DataSource = ACA_TipoCicloBO.GetSelect();
            rptCampos.DataBind();
        }

        protected void cvCiclos_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            var ciclosSelecionados = new List<int>();
            foreach (RepeaterItem item in rptCampos.Items)
            {
                CheckBox ckbCampo = (CheckBox)item.FindControl("ckbCampo");
                if (ckbCampo != null && ckbCampo.Checked)
                {
                    HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                    ciclosSelecionados.Add(Convert.ToInt32(hdnId.Value));
                }
            }

            args.IsValid = ciclosSelecionados.Count > 0;
        }
    }
}