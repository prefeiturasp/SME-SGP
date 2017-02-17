using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.Xml;
using System.Web.Security;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using MSTech.Security.Cryptography;
using MSTech.CoreSSO.BLL;
using MSTech.Web.Mail;
using MSTech.SAML20.Configuration;
using MSTech.SAML20;

namespace MSTech.GestaoEscolar.Web.WebProject.HttpHandlers
{
    /// <summary>
    ///  Classe que implementa IHttpHandler para:
    ///  - Apresentar informações do Banco de dados e testar conexões.
    ///  - Apresentar informações do Identity e validar dados.
    ///  - Apresentar informações do SAML e validar dados.
    ///  - Apresentar informações da pasta do Site.
    ///  - Testar envio de email utilizando o email informado por parâmetro e usando o smtp da 
    ///    configuração do site.
    ///    
    ///  Parâmetros que devem ser passados:
    ///  - Parâmetro level: valor de "1" a "3". Onde quando maior o level, maior o detalhamento
    ///  das informações.
    ///     Ex: level=1 | level=2 | level=3
    ///  - Parâmetro email: passar a conta para a qual será enviado o email de teste. Caso não 
    ///  seja passado, o teste do email não será realizado.
    ///     Ex: email=email@dominio.com
    ///     
    ///  Exemplos de url:
    ///  Status.ashx?level=1
    ///  Status.ashx?level=1&email=email@dominio.com
    /// </summary>
    public class Status : MotherPage, IHttpHandler, IRequiresSessionState
    {
        #region Propriedades

        private XmlDocument xml { get; set; }

        #endregion

        #region IHttpHandler Members

        public new bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public new void ProcessRequest(HttpContext context)
        {
            try
            {
                MSTech.CoreSSO.Entities.Status entityStatus = new MSTech.CoreSSO.Entities.Status()
                {
                    SistemaID = ApplicationWEB.SistemaID
                ,
                    SistemaVersao = GetVersion()
                ,
                    SistemaNome = (string.IsNullOrEmpty(__SessionWEB.TituloSistema) ? __SessionWEB.TituloGeral : __SessionWEB.TituloSistema)
                ,
                    EmailSuporte = ApplicationWEB._EmailSuporte
                ,
                    EmailHost = ApplicationWEB._EmailHost
                ,
                    EmailTo = context.Request.QueryString["email"]
                ,
                    UsuarioID = (UserIsAuthenticated() ? __SessionWEB.__UsuarioWEB.Usuario.usu_id : Guid.Empty)
                ,
                    UsuarioIsAuthorized = (__SessionWEB.__UsuarioWEB.Grupo != null ? __SessionWEB.__UsuarioWEB.Grupo.vis_id == SysVisaoID.Administracao : false)
                };

                StatusBO status = new StatusBO(entityStatus);
                StatusBO.eLevel level = (String.IsNullOrEmpty(context.Request.QueryString["level"]) ? StatusBO.eLevel.level0 : (StatusBO.eLevel)Enum.Parse(typeof(StatusBO.eLevel), context.Request.QueryString["level"], true));
                xml = status.GetXmlDocument(level);

                // Add XmlElement SAML
                if (entityStatus.UsuarioIsAuthorized)
                {
                    XmlElement xmlElementRoot = (XmlElement)xml.GetElementsByTagName("Status")[0];

                    switch (level)
                    {
                        case StatusBO.eLevel.level2:
                            {
                                xmlElementRoot.AppendChild(GetElementSAML(false));
                                break;
                            }
                        case StatusBO.eLevel.level3:
                            {
                                xmlElementRoot.AppendChild(GetElementSAML(true));
                                break;
                            }
                    }
                }

                context.Response.ContentType = "text/xml";
                context.Response.Write(xml.OuterXml);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                context.Response.ContentType = "text/plain";
                context.Response.Write("Não foi possível gerar status.");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        ///  Retorna a versão do sistema
        /// </summary>
        /// <returns></returns>
        private string GetVersion()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                String str = String.Empty;

                xmlDoc.Load(ApplicationWEB._DiretorioFisico + "version.xml");
                XmlNode xmlNd = xmlDoc.SelectSingleNode("//versionNumber");

                if (xmlNd != null)
                    str = String.Format("Versão: {0}.{1}.{2}.{3}", xmlNd.ChildNodes[0].Attributes["value"].Value,
                        xmlNd.ChildNodes[1].Attributes["value"].Value,
                        xmlNd.ChildNodes[2].Attributes["value"].Value,
                        xmlNd.ChildNodes[3].Attributes["value"].Value
                        );

                return str;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Retorna um XmlElement contendo informações do Saml
        /// </summary>
        /// <returns></returns>
        private XmlElement GetElementSAML(bool full)
        {
            // Cria elemento SAML
            XmlElement xmlElementSAML = xml.CreateElement("Saml");
            xmlElementSAML.SetAttribute("type", "Service Provider");
            xmlElementSAML.SetAttribute("version", SAMLUtility.VERSION);

            // Cria elemento ServiceProvider
            string statusServiceProvider = string.Empty;
            XmlElement xmlElementServiceProvider = xml.CreateElement("ServiceProvider");
            xmlElementSAML.AppendChild(xmlElementServiceProvider);
            try
            {
                ServiceProvider config = ServiceProvider.GetConfig();
                xmlElementServiceProvider.SetAttribute("id", config.id);

                if (full)
                {
                    // Cria Elemento ServiceEndpoint signon
                    string statusSignon = string.Empty;
                    XmlElement xmlElementSignon = xml.CreateElement("ServiceEndpoint");
                    xmlElementSAML.AppendChild(xmlElementSignon);
                    xmlElementSignon.SetAttribute("type", Enum.GetName(typeof(SAMLTypeSSO), SAMLTypeSSO.signon));
                    try
                    {
                        ServiceProviderEndpoint spendSignon = SAMLUtility.GetServiceProviderEndpoint(config.ServiceEndpoint, SAMLTypeSSO.signon);
                        if (spendSignon != null)
                        {
                            xmlElementSignon.SetAttribute("localpath", spendSignon.localpath);
                            xmlElementSignon.SetAttribute("redirectUrl", spendSignon.redirectUrl);

                            // Validação dos valores para signon 
                            if (String.IsNullOrEmpty(spendSignon.localpath))
                                statusSignon += "Atributo localpath contém um valor inválido.";
                            if (String.IsNullOrEmpty(spendSignon.redirectUrl))
                                statusSignon += "Atributo redirectUrl contém um valor inválido.";
                            if (String.Equals(spendSignon.localpath, spendSignon.redirectUrl, StringComparison.OrdinalIgnoreCase))
                                statusSignon += "Atributo localpath e redirectUrl não podem conter valores iguais";

                            if (String.IsNullOrEmpty(statusSignon))
                                statusSignon = StatusBO.Success;
                        }
                        else
                        {
                            statusSignon = "Não foi possível encontrar as configurações.";
                        }
                    }
                    catch (Exception ex)
                    {
                        statusSignon = ex.Message;
                    }
                    xmlElementSignon.SetAttribute("status", statusSignon);

                    // Cria Elemento ServiceEndpoint logout
                    string statusLogout = string.Empty;
                    XmlElement xmlElementLogout = xml.CreateElement("ServiceEndpoint");
                    xmlElementSAML.AppendChild(xmlElementLogout);
                    xmlElementLogout.SetAttribute("type", Enum.GetName(typeof(SAMLTypeSSO), SAMLTypeSSO.logout));
                    try
                    {
                        ServiceProviderEndpoint spendLogout = SAMLUtility.GetServiceProviderEndpoint(config.ServiceEndpoint, SAMLTypeSSO.logout);
                        if (spendLogout != null)
                        {
                            xmlElementLogout.SetAttribute("localpath", spendLogout.localpath);
                            xmlElementLogout.SetAttribute("redirectUrl", spendLogout.redirectUrl);

                            // Validação dos valores para logout 
                            if (String.IsNullOrEmpty(spendLogout.localpath))
                                statusLogout += "Atributo localpath contém um valor inválido.";
                            if (String.IsNullOrEmpty(spendLogout.redirectUrl))
                                statusLogout += "Atributo redirectUrl contém um valor inválido.";
                            if (String.Equals(spendLogout.localpath, spendLogout.redirectUrl, StringComparison.OrdinalIgnoreCase))
                                statusLogout += "Atributo localpath e redirectUrl não podem conter valores iguais";

                            if (String.IsNullOrEmpty(statusLogout))
                                statusLogout = StatusBO.Success;
                        }
                        else
                        {
                            statusLogout = "Não foi possível encontrar as configurações.";
                        }
                    }
                    catch (Exception ex)
                    {
                        statusLogout = ex.Message;
                    }
                    xmlElementLogout.SetAttribute("status", statusLogout);
                }
                statusServiceProvider = StatusBO.Success;
            }
            catch (Exception ex)
            {
                statusServiceProvider = ex.Message;
            }
            xmlElementServiceProvider.SetAttribute("status", statusServiceProvider);

            return xmlElementSAML;
        }

        #endregion
    }
}
