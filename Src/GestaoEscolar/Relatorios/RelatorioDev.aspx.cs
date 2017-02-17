using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Relatorios_RelatorioDev : MotherPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.UCDevReportView1._VS_TipoRelatorio = tipoRelatorio.Relatorio;
            this.UCDevReportView1.SetTitlePage = ((MotherMasterPage)Page.Master).GetSiteMapTitle;
            this.UCDevReportView1.ReportLoad(GestaoEscolarUtilBO.CurrentDevReport);
        }
    }
}
