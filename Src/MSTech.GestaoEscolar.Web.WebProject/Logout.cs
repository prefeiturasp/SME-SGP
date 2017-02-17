using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using MSTech.CoreSSO.BLL;
using MSTech.SAML20;
using MSTech.SAML20.Bindings;
using MSTech.SAML20.Configuration;
using MSTech.SAML20.Schemas.Core;
using MSTech.SAML20.Schemas.Protocol;
using MSTech.Validation.Exceptions;
using MSTech.CoreSSO.Entities;
using System.Linq;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    public class Logout : MotherPage, IHttpHandler, IRequiresSessionState
    {
        #region Propriedades

        private ResponseType SAMLResponse { get; set; }

        private LogoutRequestType SAMLRequest { get; set; }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// You will need to configure this handler in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>

        #region IHttpHandler Members

        public new bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public new void ProcessRequest(HttpContext context)
        {
            try
            {
                // Carrega as configurações do ServiceProvider
                ServiceProvider config = ServiceProvider.GetConfig();
                ServiceProviderEndpoint spend = SAMLUtility.GetServiceProviderEndpoint(config.ServiceEndpoint, SAMLTypeSSO.logout);

                // Verifica configuração do ServiceProvider para logout
                if (spend == null)
                    throw new ValidationException("Não foi possível encontrar as configurações do ServiceProvider para logout.");

                // ***** RESPONSE *****
                if (!String.IsNullOrEmpty(context.Request[HttpBindingConstants.SAMLResponse]))
                {
                    // Recupera LogoutResponse
                    string samlresponse = context.Request[HttpBindingConstants.SAMLResponse];
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(samlresponse);
                    SAMLResponse = SAMLUtility.DeserializeFromXmlString<ResponseType>(doc.InnerXml);

                    FormsAuthentication.SignOut();
                    if (context.Session != null)
                        context.Session.Abandon();

                    if (ApplicationWEB.LoginProprioDoSistema)
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                    else
                    {
                        string url = spend.redirectUrl;

                        if (ApplicationWEB.RedirecionarAutomaticoSistema)
                        {
                            // Redirecionar passando o sis_id como parâmetro, caso esteja configurado
                            // para redirecionar automaticamente (quando logar no core, já vai cair direto
                            // no gestão).
                            url += "?sis=" + ApplicationWEB.SistemaID;
                        }
                        
                        HttpContext.Current.Response.Redirect(url, false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                // ***** REQUEST *****
                else if (!String.IsNullOrEmpty(context.Request[HttpBindingConstants.SAMLRequest]))
                {
                    // Recupera LogoutRequest
                    StringBuilder result = new StringBuilder();

                    byte[] encoded = Convert.FromBase64String(HttpUtility.UrlDecode(context.Request[HttpBindingConstants.SAMLRequest]).Replace(" ", "+"));
                    MemoryStream memoryStream = new MemoryStream(encoded);
                    using (DeflateStream stream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                    {
                        StreamReader reader = new StreamReader(new BufferedStream(stream), Encoding.GetEncoding("iso-8859-1"));
                        reader.Peek();
                        result.Append(reader.ReadToEnd());
                        stream.Close();
                    }
                    SAMLRequest = SAMLUtility.DeserializeFromXmlString<LogoutRequestType>(result.ToString());
                    
                    // Criação e configuração LogoutResponse
                    SAMLResponse = new ResponseType();
                    CreateSAMLResponse();
                }
                else
                {
                    // Criação e configuração LogoutRequest
                    SAMLRequest = new LogoutRequestType();
                    SAMLRequest.ID = SAMLUtility.GenerateID();
                    SAMLRequest.Version = SAMLUtility.VERSION;
                    SAMLRequest.IssueInstant = DateTime.UtcNow.AddMinutes(10);
                    SAMLRequest.SessionIndex = new string[] { context.Session.SessionID };

                    NameIDType nameID = new NameIDType();
                    nameID.Format = SAMLUtility.NameIdentifierFormats.Transient;
                    nameID.Value = spend.localpath;

                    SAMLRequest.Item = nameID;
                    SAMLRequest.Issuer = new NameIDType();
                    SAMLRequest.Issuer.Value = config.id;

                    MemoryStream ms = new MemoryStream();
                    using (StreamWriter writer = new StreamWriter(new DeflateStream(ms, CompressionMode.Compress, true), Encoding.GetEncoding("iso-8859-1")))
                    {
                        writer.Write(SAMLUtility.SerializeToXmlString(SAMLRequest));
                        writer.Close();
                    }
                    string message = HttpUtility.UrlEncode(Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length, Base64FormattingOptions.None));
                    HttpRedirectBinding binding = new HttpRedirectBinding(message, spend.localpath);
                    binding.SendRequest(context, spend.redirectUrl);
                }
            }
            catch (ValidationException ex)
            {
                ErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                ErrorMessage("Não foi possível atender a solicitação.");
            }
        }

        #endregion IHttpHandler Members

        private void ErrorMessage(string message)
        {
            UtilBO.CreateHtmlFormMessage
                (
                    this.Context.Response.Output
                    , "SAML SSO"
                    , UtilBO.GetErroMessage(message + "<br />Clique no botão voltar e tente novamente.", UtilBO.TipoMensagem.Erro)
                    , string.Concat(__SessionWEB.UrlCoreSSO, "/Sistema.aspx")
                 );
        }

        private void CreateSAMLResponse()
        {
            IDProvider config = IDProvider.GetConfig();

            SAMLResponse.ID = SAMLUtility.GenerateID();
            SAMLResponse.Version = SAMLUtility.VERSION;
            SAMLResponse.IssueInstant = DateTime.UtcNow.AddMinutes(10);
            SAMLResponse.InResponseTo = SAMLRequest.ID;

            SAMLResponse.Issuer = new NameIDType();
            SAMLResponse.Issuer.Value = config.id;

            SAMLResponse.Status = new StatusType();
            SAMLResponse.Status.StatusCode = new StatusCodeType();

            // Atualiza Cookie de sistemas autenticados e configura Status
            HttpCookie cookie = this.Context.Request.Cookies["SistemasLogged"];
            if (cookie != null)
            {
                // Carrega a Entidade SYS_Sistema apartir do caminho de logout
                SYS_Sistema entitySistema = new SYS_Sistema { sis_caminhoLogout = ((NameIDType)SAMLRequest.Item).Value };
                if (SYS_SistemaBO.GetSelectBy_sis_caminho(entitySistema, SYS_SistemaBO.TypePath.logout))
                {
                    // Remove o sistema do Cookie
                    cookie.Values.Remove(entitySistema.sis_id.ToString());
                    // Atualiza dados do Cookie
                    this.Context.Response.Cookies.Set(cookie);

                    if (!cookie.Values.AllKeys.Contains(entitySistema.sis_id.ToString()))
                    {
                        SAMLResponse.Status.StatusCode.Value = SAMLUtility.StatusCodes.Success;
                        SAMLResponse.Status.StatusMessage = "A solicitação foi realizada com sucesso.";
                    }
                    else
                    {
                        SAMLResponse.Status.StatusCode.Value = SAMLUtility.StatusCodes.RequestDenied;
                        SAMLResponse.Status.StatusMessage = "Não foi possível atender a solicitação, o sistema emissor da requisição não está autenticado.";
                    }
                }
                else
                {
                    SAMLResponse.Status.StatusCode.Value = SAMLUtility.StatusCodes.RequestDenied;
                    SAMLResponse.Status.StatusMessage = "Não foi possível atender a solicitação, sistema emissor da requisição não está cadastrado corretamente."; ;
                }
            }
            else
            {
                SAMLResponse.Status.StatusCode.Value = SAMLUtility.StatusCodes.RequestDenied;
                SAMLResponse.Status.StatusMessage = "Não foi possível atender a solicitação.";
            }

            HttpPostBinding binding = new HttpPostBinding(SAMLResponse, HttpUtility.UrlDecode(this.Context.Request[HttpBindingConstants.RelayState]));
            binding.SendResponse(this.Context, HttpUtility.UrlDecode(this.Context.Request[HttpBindingConstants.RelayState]), SAMLTypeSSO.logout);
        }

        #endregion Métodos
    }
}