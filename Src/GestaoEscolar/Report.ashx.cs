using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSTech.Validation.Exceptions;
using System.Web.SessionState;
using MSTech.SqlServer.ReportingServices;
using Microsoft.Reporting.WebForms;
using System.IO;
using MSTech.GestaoEscolar.Web.WebProject;
using System.Threading;
using iTextSharp.text.pdf;
using iTextSharp.text;
using CFG_Relatorio = MSTech.GestaoEscolar.Entities.CFG_Relatorio;
using CFG_RelatorioBO = MSTech.GestaoEscolar.BLL.CFG_RelatorioBO;
using CFG_ServidorRelatorioBO = MSTech.GestaoEscolar.BLL.CFG_ServidorRelatorioBO;
using MSTech.GestaoEscolar.Entities;


namespace GestaoEscolar
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Report : IHttpHandler, IRequiresSessionState
    {

        public bool IsReusable
        {
            get
            {
                return false;
            }    
        }
                 
        public SessionWEB __SessionWEB
        {
            get
            {
                return (SessionWEB)HttpContext.Current.Session[MSTech.Web.WebProject.ApplicationWEB.SessSessionWEB];
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.IsAuthenticated)
            {             
                try
                {
                    var tipRel = HttpUtility.UrlDecode(HttpContext.Current.Request["tipRel"]);
                    var parametrosRel = HttpUtility.UrlDecode(HttpContext.Current.Request["params"]);
                    int rlt_id;

                    if (int.TryParse(tipRel, out rlt_id))
                    {
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        Warning[] warnings;
                        string[] streamids;
                        byte[] exportBytes;

                        //Recebe os dados do relatório
                        CFG_Relatorio rpt = new CFG_Relatorio() { rlt_id = rlt_id };
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

                        //Carrega os parâmetros do relatório
                        MSReportServerParameters param = new MSReportServerParameters(parametrosRel);
                        //Checa o modo de processamento do servidor
                        if (rptServer.srr_remoteServer)
                        {
                            Microsoft.Reporting.WebForms.ReportViewer ReportViewerRel = new Microsoft.Reporting.WebForms.ReportViewer();

                            //Configura o reportviewer
                            ReportViewerRel.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                            Uri urlReport = new Uri(rptServer.srr_diretorioRelatorios);
                            ReportViewerRel.ServerReport.ReportServerUrl = urlReport;
                            ReportViewerRel.ServerReport.ReportServerCredentials = new MSReportServerCredentials(rptServer.srr_usuario, rptServer.srr_senha, rptServer.srr_dominio);
                            ReportViewerRel.ServerReport.ReportPath = String.Concat(rptServer.srr_pastaRelatorios, rpt.rlt_nome);
                            ReportViewerRel.ServerReport.SetParameters(param.getReportParameters());

                            //Carrega o relatório
                            exportBytes = ReportViewerRel.ServerReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streamids, out warnings);


                            #region PDF Sharp
                            //MemoryStream fs = new MemoryStream(exportBytes);
                            //PdfDocument document = PdfReader.Open(fs, PdfDocumentOpenMode.Modify);
                            //PdfDictionary dictJS = new PdfDictionary(document);
                            //dictJS.Elements["/S"] = new PdfName("/JavaScript");
                            //dictJS.Elements["/JS"] = new PdfString("this.print(true);\r");

                            //document.Internals.AddObject(dictJS);
                            //document.Internals.Catalog.Elements["/OpenAction"] = PdfInternals.GetReference(dictJS);


                            //MemoryStream ms = new MemoryStream();
                            //document.Save(ms, false); 

                            #endregion

                            #region ITextSharp

                            MemoryStream ms = new MemoryStream();

                            PdfReader reader = new PdfReader(exportBytes);
                            Document document = null;
                            PdfCopy writer = null;
                            int n = reader.NumberOfPages;

                            document = new Document(reader.GetPageSizeWithRotation(1));
                            writer = new PdfCopy(document, ms);
                            document.Open();
                            PdfImportedPage page;
                            for (int i = 0; i < n; )
                            {
                                ++i;
                                page = writer.GetImportedPage(reader, i);
                                writer.AddPage(page);
                            }
                            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
                            writer.AddJavaScript(jAction);

                            document.Close();
                            writer.Close();

                            #endregion

                            HttpResponse response = HttpContext.Current.Response;
                            response.ClearContent();
                            response.ClearHeaders();
                            response.Buffer = true;
                            response.Cache.SetCacheability(HttpCacheability.Private);
                            response.ContentType = "application/pdf";

                            response.AddHeader("Content-Disposition", "inline;");
                            response.BinaryWrite(ms.ToArray());
                            ms.Close();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();

                            response.End();
                        }
                    }
                }
                catch (ThreadAbortException)
                {

                }
                catch (Exception ex)
                {
                    MSTech.GestaoEscolar.Web.WebProject.ApplicationWEB._GravaErro(ex);

                }
            }
        }
    }
}
