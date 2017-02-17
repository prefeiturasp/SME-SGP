using System;
using System.Web;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.SqlServer.ReportingServices;
using MSTech.Validation.Exceptions;
using ReportNameGestaoAcademica = MSTech.GestaoEscolar.BLL.ReportNameGestaoAcademica;
using CFG_Relatorio = MSTech.GestaoEscolar.Entities.CFG_Relatorio;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using CFG_ServidorRelatorioBO = MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO;
using UtilBO = MSTech.CoreSSO.BLL.UtilBO;

public partial class WebControls_Relatorio_UCReportView : MotherUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //recebe valores da sessão e grava em variáveis
                string tipoRel = CFG_RelatorioBO.CurrentReportID;
                string parametrosRel = CFG_RelatorioBO.CurrentReportParameters;

                if (!String.IsNullOrEmpty(tipoRel))
                {
                    this.QueryStringUrlReports = "tipRel=" + HttpUtility.UrlEncode(tipoRel) + "&params=" + HttpUtility.UrlEncode(parametrosRel);

                    //remove os valores da sessão
                    CFG_RelatorioBO.ClearSessionReportParameters();
                    //Recebe os dados do relatório
                    CFG_Relatorio rpt = new CFG_Relatorio() { rlt_id = int.Parse(tipoRel) };
                    CFG_RelatorioBO.GetEntity(rpt);
                    if (rpt.IsNew)
                        throw new ValidationException("Relatório não encontrado.");
                    //Configurações do Relatório
                    CFG_ServidorRelatorio rptServer = CFG_ServidorRelatorioBO.CarregarServidorRelatorioPorEntidade(
                        this.__SessionWEB.__UsuarioWEB.Usuario.ent_id
                        , ApplicationWEB.AppMinutosCacheLongo
                    );

                    if (rptServer.IsNew)
                        throw new ValidationException("O servidor de relatório não está configurado.");

                    VerificaAtributosReport(rpt.rlt_id);

                    //Carrega os parâmetros do relatório
                    MSReportServerParameters param = new MSReportServerParameters(parametrosRel);

                    //Verifica se está Habilitada a impressão sem activeX
                    bool bHabilitarImpressaoRel = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.HABILITA_IMPRESSAO_RELATORIO
                        , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    
                    this.HabilitarImpressaoRel = bHabilitarImpressaoRel;
                    divPdf.Visible = bHabilitarImpressaoRel;
                    ReportViewerRel.ShowPrintButton = !bHabilitarImpressaoRel;

                    //Verifica se usuário vizualizará algum relatório da aba 'DOCUMENTOS'
                    if (_VS_TipoRelatorio == tipoRelatorio.Documento)
                    {
                        // Habilita ou desabilita, conforme configuração do parâmetro HABILITA_EXPORTACAO_IMPRESSAO_DOCUMENTOS, se vai ter botão de exportar documento no ReportView.
                        ReportViewerRel.ShowExportControls = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.HABILITA_EXPORTACAO_IMPRESSAO_DOCUMENTOS
                            , __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                    }

                    //Checa o modo de processamento do servidor
                    if (rptServer.srr_remoteServer)
                    {
                        //Configura o reportviewer
                        ReportViewerRel.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                        Uri urlReport = new Uri(rptServer.srr_diretorioRelatorios);
                        ReportViewerRel.ServerReport.ReportServerUrl = urlReport;
                        ReportViewerRel.ServerReport.ReportServerCredentials = new MSReportServerCredentials(rptServer.srr_usuario, rptServer.srr_senha, rptServer.srr_dominio);
                        ReportViewerRel.ServerReport.ReportPath = String.Concat(rptServer.srr_pastaRelatorios, rpt.rlt_nome);
                        ReportViewerRel.ServerReport.SetParameters(param.getReportParameters());
                        //Recebe as configurações do delegate
                        ConfigRemoteRerpotViewerEvent configRemoteRerpotViewer = Events[ConfigRemoteRerpotViewerKey] as ConfigRemoteRerpotViewerEvent;
                        if (configRemoteRerpotViewer != null)
                            configRemoteRerpotViewer(ReportViewerRel);
                        //Carrega o relatório                      
                        ReportViewerRel.ServerReport.Refresh();
                    }
                    else
                    {
                        //Configura o reportviewer
                        ReportViewerRel.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                        ReportViewerRel.LocalReport.ReportPath = String.Concat(rptServer.srr_pastaRelatorios, rpt.rlt_nome);
                        ReportViewerRel.LocalReport.SetParameters(param.getReportParameters());
                        //Recebe as configurações do delegate
                        ConfigLocalRerpotViewerEvent configLocalRerpotViewer = Events[ConfigLocalRerpotViewerKey] as ConfigLocalRerpotViewerEvent;
                        if (configLocalRerpotViewer != null)
                            configLocalRerpotViewer(ReportViewerRel);
                        //Carrega o relatório
                        ReportViewerRel.LocalReport.Refresh();
                    }
                }
                else
                {
                    string nome = _VS_TipoRelatorio == tipoRelatorio.Relatorio ? "relatório" : "documento";
                    _lblMensagem.Text = UtilBO.GetErroMessage("Não foi possível carregar o " + nome + ". Tipo de relatório indisponível.", UtilBO.TipoMensagem.Informacao);
                }
            }

            this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "reports", " var query= '" + this.QueryStringUrlReports + "';", true);
        }
        catch (ValidationException ex)
        {
            this._TrataErro(ex, ex.Message);
        }
        catch (Exception ex)
        {
            string nome = _VS_TipoRelatorio == tipoRelatorio.Relatorio ? "relatório" : "documento";
            this._TrataErro(ex, "Recurso indisponível ao exibir o " + nome + ".");
        }
    }

    /// <summary>
    /// Verifica de acordo com o relatório, atributos que devem ser configurados.
    /// </summary>
    /// <param name="id_relatorio">ID do relatório que será exibido</param>
    private void VerificaAtributosReport(int id_relatorio)
    {
    }

    #region DELEGATE

    public delegate void ConfigLocalRerpotViewerEvent(Microsoft.Reporting.WebForms.ReportViewer reportViewer);
    private static readonly object ConfigLocalRerpotViewerKey = new object();
    public delegate void ConfigRemoteRerpotViewerEvent(Microsoft.Reporting.WebForms.ReportViewer reportViewer);
    private static readonly object ConfigRemoteRerpotViewerKey = new object();

    #endregion

    #region PROPRIEDADES

    public event ConfigLocalRerpotViewerEvent ConfigLocalRerpotViewer
    {
        add
        {
            Events.AddHandler(ConfigLocalRerpotViewerKey, value);
        }
        remove
        {
            Events.RemoveHandler(ConfigLocalRerpotViewerKey, value);
        }
    }

    public event ConfigRemoteRerpotViewerEvent ConfigRemoteRerpotViewer
    {
        add
        {
            Events.AddHandler(ConfigRemoteRerpotViewerKey, value);
        }
        remove
        {
            Events.RemoveHandler(ConfigRemoteRerpotViewerKey, value);
        }
    }

    protected bool HabilitarImpressaoRel
    {
        get;
        set;
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
    /// Armazena a url com os parametros para chamar o handler que renderiza o pdf
    /// </summary>
    private string QueryStringUrlReports
    {
        get
        {
            if (ViewState["QueryStringUrlReports"] != null)
                return (string)ViewState["QueryStringUrlReports"];
            return string.Empty;
        }
        set
        {
            ViewState["QueryStringUrlReports"] = value;
        }
    }


    #endregion

    #region METODOS

    /// <summary>
    /// Grava a exception no banco de dados ou em arquivo texto e retorna uma 
    /// mensagem amigável ao usuário do site.
    /// </summary>
    /// <param name="ex">Exception.</param>
    /// <param name="msg">Mensagem amigável ao usuário.</param>
    private void _TrataErro(Exception ex, string msg)
    {
        ApplicationWEB._GravaErro(ex);
        _lblMensagem.Text = UtilBO.GetErroMessage(msg, UtilBO.TipoMensagem.Erro);
    }

    #endregion

}
