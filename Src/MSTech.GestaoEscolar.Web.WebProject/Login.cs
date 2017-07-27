using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.SAML20;
using MSTech.SAML20.Bindings;
using MSTech.SAML20.Configuration;
using MSTech.SAML20.Schemas.Core;
using MSTech.SAML20.Schemas.Protocol;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.Web.WebProject
{
    public class Login : MotherPage, IHttpHandler, IRequiresSessionState
    {
        public new void ProcessRequest(HttpContext context)
        {
            try
            {
                if (!UserIsAuthenticated())
                {
                    string provider = IdentitySettingsConfig.IDSSettings.AuthenticationType;
                    Context.GetOwinContext().Authentication.Challenge(provider);
                }
                else
                    context.Response.Redirect("~/SAML/Login.aspx", false);
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


        private void ErrorMessage(string message)
        {
            if (__SessionWEB != null && __SessionWEB.UrlCoreSSO != null)
            {
                UtilBO.CreateHtmlFormMessage
                    (
                        this.Context.Response.Output
                        , "SAML SSO"
                        , UtilBO.GetErroMessage(message + "<br />Clique no botão voltar e tente novamente.", UtilBO.TipoMensagem.Erro)
                        , string.Concat(__SessionWEB.UrlCoreSSO, "/Sistema.aspx")
                     );
            }
        }


        #region Propriedades

        //private ResponseType SAMLResponse { get; set; }

        //private SAMLAuthnRequest SAMLRequest { get; set; }

        #endregion Propriedades

        //public new bool IsReusable
        //{
        //    // Return false in case your Managed Handler cannot be reused for another request.
        //    // Usually this would be false in case you have some state information preserved per request.
        //    get { throw new NotImplementedException(); }
        //}


        //public new void ProcessRequest(HttpContext context)
        //{
        //    try
        //    {
        //        // ***** RESPONSE *****
        //        if (!String.IsNullOrEmpty(context.Request[HttpBindingConstants.SAMLResponse]))
        //        {
        //            // Recupera Response
        //            string samlresponse = context.Request[HttpBindingConstants.SAMLResponse];
        //            XmlDocument doc = new XmlDocument();
        //            doc.PreserveWhitespace = true;
        //            doc.LoadXml(samlresponse);

        //            // Verifica Signature do Response
        //            if (XmlSignatureUtils.VerifySignature(doc))
        //            {
        //                SAMLResponse = SAMLUtility.DeserializeFromXmlString<ResponseType>(doc.InnerXml);
        //                if (SAMLResponse.Items.Length > 0)
        //                {
        //                    for (int i = 0; i < SAMLResponse.Items.Length; i++)
        //                    {
        //                        if (SAMLResponse.Items[i] is AssertionType)
        //                        {
        //                            NameIDType nameID = null;
        //                            AssertionType assertion = (AssertionType)SAMLResponse.Items[i];
        //                            for (int j = 0; j < assertion.Subject.Items.Length; j++)
        //                            {
        //                                if (assertion.Subject.Items[j] is NameIDType)
        //                                    nameID = (NameIDType)assertion.Subject.Items[j];
        //                            }
        //                            if (nameID != null)
        //                            {
        //                                FormsAuthentication.Initialize();
        //                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
        //                                    1
        //                                    , nameID.Value
        //                                    , Convert.ToDateTime(assertion.Conditions.NotBefore).ToUniversalTime()
        //                                    , Convert.ToDateTime(assertion.Conditions.NotOnOrAfter).ToUniversalTime()
        //                                    , false
        //                                    , String.Empty
        //                                    , FormsAuthentication.FormsCookiePath);
        //                                string hash = FormsAuthentication.Encrypt(ticket);
        //                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
        //                                cookie.Expires = ticket.Expiration;
        //                                HttpContext.Current.Response.Cookies.Add(cookie);
        //                            }
        //                        }
        //                    }
        //                }
        //                context.Response.Redirect(HttpUtility.UrlDecode(context.Request[HttpBindingConstants.RelayState]), false);
        //            }
        //            else
        //                throw new ValidationException("Não foi possível encontrar assinatura.");
        //        }
        //        // ***** REQUEST *****
        //        else if (!String.IsNullOrEmpty(context.Request[HttpBindingConstants.SAMLRequest]))
        //        {
        //            throw new NotImplementedException();
        //        }
        //        else
        //        {
        //            // Carrega as configurações do ServiceProvider
        //            ServiceProvider config = ServiceProvider.GetConfig();
        //            ServiceProviderEndpoint spend = SAMLUtility.GetServiceProviderEndpoint(config.ServiceEndpoint, SAMLTypeSSO.signon);

        //            // Verifica configuração do ServiceProvider para signon
        //            if (spend == null)
        //                throw new ValidationException("Não foi possível encontrar as configurações do ServiceProvider para signon.");

        //            // Verifica se usuário está autenticado, caso não envia um Resquest solicitando autenticação
        //            if (!UserIsAuthenticated())
        //            {
        //                SAMLRequest = new SAMLAuthnRequest();
        //                SAMLRequest.Issuer = config.id;
        //                SAMLRequest.AssertionConsumerServiceURL = context.Request.Url.AbsoluteUri;

        //                HttpRedirectBinding binding = new HttpRedirectBinding(SAMLRequest, spend.localpath);
        //                binding.SendRequest(context, spend.redirectUrl);
        //            }
        //            else
        //            {
        //                HttpCookie cookie = context.Request.Cookies["SistemasLogged"];
        //                if (cookie == null)
        //                {
        //                    cookie = new HttpCookie("SistemasLogged");
        //                    context.Response.Cookies.Add(cookie);
        //                }

        //                // Armazena sistema no Cookie.
        //                cookie.Values[ApplicationWEB.SistemaID.ToString()] = __SessionWEB.TituloSistema;

        //                // Atualiza dados do Cookie.
        //                context.Response.Cookies.Set(cookie);

        //                HttpContext.Current.Response.Redirect(spend.localpath, false);
        //                HttpContext.Current.ApplicationInstance.CompleteRequest();
        //            }
        //        }
        //    }
        //    catch (ValidationException ex)
        //    {
        //        ErrorMessage(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationWEB._GravaErro(ex);

        //        ErrorMessage("Não foi possível atender a solicitação.");
        //    }
        //}

    }
}