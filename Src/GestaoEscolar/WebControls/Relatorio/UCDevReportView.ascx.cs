using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.GestaoEscolar.BLL;

public partial class WebControls_Relatorio_UCDevReportView : MotherUserControl
{
    
    #region PROPRIEDADES

    /// <summary>
    /// Propriedade que indica se está Habilitada a impressão sem activeX
    /// </summary>
    protected bool HabilitarImpressaoRel
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.HABILITA_IMPRESSAO_RELATORIO
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    /// <summary>
    /// View State guardar página que está chamando o relatório(Documento ou Relatório)
    /// </summary>
    public tipoRelatorio _VS_TipoRelatorio
    {
        get
        {
            if (ViewState["_VS_TipoRelatorio"] != null)
                return (tipoRelatorio)ViewState["_VS_TipoRelatorio"];
            return tipoRelatorio.Relatorio;
        }
        set
        {
            ViewState["_VS_TipoRelatorio"] = value;
        }
    }

    /// <summary>
    /// Informa o nome da legenda do fieldset
    /// </summary>
    public string SetTitlePage
    {
        set
        {
            this._lblTitulo.Text = value;
        }
    }

    /// <summary>
    /// Guarda a página que deve ser retornado o site em caso de erros
    /// ou quando clicar no botão voltar.
    /// </summary>
    private string _VS_CaminhoPagina
    {
        get
        {
            if (ViewState["_VS_CaminhoPagina"] != null)
                return (string)ViewState["_VS_CaminhoPagina"];
            return string.Empty;
        }
        set
        {
            ViewState["_VS_CaminhoPagina"] = value;
        }
    }

    /// <summary>
    /// Propriedade que indica se está Habilitada a Exportação e Impressão de Documentos
    /// </summary>
    protected bool HabilitarExportacaoDocumentos
    {
        get
        {
            return ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.HABILITA_EXPORTACAO_IMPRESSAO_DOCUMENTOS
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }
    }

    #endregion PROPRIEDADES

    #region PAGELIFE

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <author>juliano.real</author>
    /// <datetime>23/10/2013-18:37</datetime>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["report"] != null)
            DevReportView.Report = Session["report"] as XtraReport;
    }

    #endregion PAGELIFE

    #region METODOS

    /// <summary>
    /// Reports the load.
    /// </summary>
    /// <param name="dxReport">The DevExpress report.</param>
    /// <author>juliano.real</author>
    /// <datetime>24/10/2013-15:41</datetime>
    public void ReportLoad(XtraReport dxReport)
    {
        try
        {
            //Limpa da session os dados do ultimo relatorio carregado
            GestaoEscolarUtilBO.ClearSessionReportParameters();

            if (_VS_TipoRelatorio == tipoRelatorio.Documento)
            {
                //Habilita ou desabilita, conforme configuração do parâmetro HABILITA_EXPORTACAO_IMPRESSAO_DOCUMENTOS, se vai ter botão de exportar documento no ReportView.
                if (!HabilitarExportacaoDocumentos)
                {
                    DevReportTools.Items.RemoveAt(14);
                    DevReportTools.Items.RemoveAt(14);
                    DevReportTools.Items.RemoveAt(14);

                    _lblMessageLayout.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("WebControls", "Relatorios.UCRelatorios.lblMessageLayout.MsgAviso").ToString(), UtilBO.TipoMensagem.Informacao);
                }
                else
                    _lblMessageLayout.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("WebControls", "Relatorios.UCRelatorios.lblMessageLayout.MsgAvisoCompleto").ToString(), UtilBO.TipoMensagem.Informacao);
            }
            else
            {
                _lblMessageLayout.Text = UtilBO.GetErroMessage(GetGlobalResourceObject("WebControls", "Relatorios.UCRelatorios.lblMessageLayout.MsgAvisoCompleto").ToString(), UtilBO.TipoMensagem.Informacao);
            }

            this._VS_CaminhoPagina = Convert.ToString(Request.UrlReferrer);

            if (dxReport != null)
            {
                DevReportView.Report = dxReport;
                DevReportView.IsLoading();
                DevReportView.DataBind();
                Session["report"] = dxReport;
            }
            else
            {
                string nome = _VS_TipoRelatorio == tipoRelatorio.Relatorio ? "relatório" : "documento";
                this.__SessionWEB.PostMessages = UtilBO.GetErroMessage("Não foi possível carregar o " + nome + " " + this._lblTitulo.Text.ToLower() + ".", UtilBO.TipoMensagem.Erro);
                Response.Redirect(_VS_CaminhoPagina, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

        }
        catch (Exception ex)
        {
            string nome = _VS_TipoRelatorio == tipoRelatorio.Relatorio ? "relatório" : "documento";
            this.TrataErro(ex, "Erro ao tentar exibir o " + nome + " " + this._lblTitulo.Text.ToLower() + ".");
        }
    }

    /// <summary>
    /// Handles the Unload event of the DevReportView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <author>juliano.real</author>
    /// <datetime>23/10/2013-18:37</datetime>
    protected void DevReportView_Unload(object sender, EventArgs e)
    {
        ((ReportViewer)sender).Report = null;
    }

    /// <summary>
    /// Grava a exception no banco de dados ou em arquivo texto e retorna uma 
    /// mensagem amigável ao usuário do site.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="msg">Mensagem amigável ao usuário.</param>
    private void TrataErro(Exception ex, string msg)
    {
        ApplicationWEB._GravaErro(ex);
        this.__SessionWEB.PostMessages = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Erro);
        Response.Redirect(_VS_CaminhoPagina, false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    /// <summary>
    /// Handles the Click event of the btnVoltar control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <author>juliano.real</author>
    /// <datetime>23/10/2013-18:37</datetime>
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect(_VS_CaminhoPagina, false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion METODOS
}
