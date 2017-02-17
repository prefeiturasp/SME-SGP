using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.SAML20;
using MSTech.SAML20.Bindings;
using MSTech.SAML20.Configuration;
using MSTech.SAML20.Schemas.Core;
using MSTech.SAML20.Schemas.Protocol;
using MSTech.Validation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace AreaAluno.SAML
{
    public partial class Signon : MotherPage
    {
        #region Propriedades

        private ResponseType SAMLResponse { get; set; }
        private SAMLAuthnRequest SAMLRequest { get; set; }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Trace

            // Write a trace message
            if (Trace.IsEnabled)
            {
                if (HttpContext.Current.User != null)
                {
                    Trace.Write("HttpContext.Current.User", HttpContext.Current.User.ToString());
                    Trace.Write("HttpContext.Current.User.Identity", HttpContext.Current.User.Identity.ToString());
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        Trace.Write("HttpContext.Current.User.Identity.IsAuthenticated", HttpContext.Current.User.Identity.IsAuthenticated.ToString());
                        if (HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                            Trace.Write("FormsIdentity.Ticket.Name", id.Ticket.Name);
                            Trace.Write("FormsIdentity.Ticket.IssueDate", id.Ticket.IssueDate.ToString());
                        }
                    }
                }
                else
                {
                    Trace.Write("HttpContext.Current.User", "NULL");
                }
            }

            #endregion

            try
            {
                // Verifica autenticação
                if (UserIsAuthenticated())
                {
                    if ((!String.IsNullOrEmpty(Request[HttpBindingConstants.SAMLRequest])) &&
                        (!String.IsNullOrEmpty(Request[HttpBindingConstants.RelayState])))
                    {
                        // Recupera Request
                        SAMLRequest = new SAMLAuthnRequest();
                        string request = HttpUtility.UrlDecode(Request[HttpBindingConstants.SAMLRequest]);
                        SAMLRequest.UnPackRequest(request.Replace(" ", "+"));

                        // Criação e configuração do Response
                        SAMLResponse = new ResponseType();
                        CreateSAMLResponse();

                        // Armazena dados do sistema emissor do Request
                        // em Cookie de sistemas autenticados
                        AddSAMLCookie();
                    }
                    else
                        throw new ValidationException("Não foi possível atender a solicitação, requisição inválida.");
                }
                else
                    throw new ValidationException("Não foi possível atender a solicitação, o usuário não tem permissão de acesso ao sistema.");
            }
            catch (ValidationException ex)
            {
                __SessionWEB.PostMessages = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);

                __SessionWEB.PostMessages = UtilBO.GetErroMessage("Não foi possível atender a solicitação.", UtilBO.TipoMensagem.Erro);
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        #endregion

        #region Métodos

        private void CreateSAMLResponse()
        {
            FormsIdentity id = null;

            if (HttpContext.Current.User != null)
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                        id = (FormsIdentity)HttpContext.Current.User.Identity;

            DateTime notBefore = (id != null ? id.Ticket.IssueDate.ToUniversalTime() : DateTime.UtcNow);
            DateTime notOnOrAfter = (id != null ? id.Ticket.Expiration.ToUniversalTime() : DateTime.UtcNow.AddMinutes(20));

            IDProvider config = IDProvider.GetConfig();

            SAMLResponse.Status = new StatusType();
            SAMLResponse.Status.StatusCode = new StatusCodeType();
            SAMLResponse.Status.StatusCode.Value = SAMLUtility.StatusCodes.Success;

            AssertionType assert = new AssertionType();
            assert.ID = SAMLUtility.GenerateID();
            assert.IssueInstant = DateTime.UtcNow.AddMinutes(10);

            assert.Issuer = new NameIDType();
            assert.Issuer.Value = config.id;

            SubjectConfirmationType subjectConfirmation = new SubjectConfirmationType();
            subjectConfirmation.Method = "urn:oasis:names:tc:SAML:2.0:cm:bearer";
            subjectConfirmation.SubjectConfirmationData = new SubjectConfirmationDataType();
            subjectConfirmation.SubjectConfirmationData.Recipient = SAMLRequest.Issuer;
            subjectConfirmation.SubjectConfirmationData.InResponseTo = SAMLRequest.Request.ID;
            subjectConfirmation.SubjectConfirmationData.NotOnOrAfter = notOnOrAfter;

            NameIDType nameID = new NameIDType();
            nameID.Format = SAMLUtility.NameIdentifierFormats.Transient;
            nameID.Value = (id != null ? id.Name : UtilBO.FormatNameFormsAuthentication(this.__SessionWEB.__UsuarioWEB.Usuario));

            assert.Subject = new SubjectType();
            assert.Subject.Items = new object[] { subjectConfirmation, nameID };

            assert.Conditions = new ConditionsType();
            assert.Conditions.NotBefore = notBefore;
            assert.Conditions.NotOnOrAfter = notOnOrAfter;
            assert.Conditions.NotBeforeSpecified = true;
            assert.Conditions.NotOnOrAfterSpecified = true;

            AudienceRestrictionType audienceRestriction = new AudienceRestrictionType();
            audienceRestriction.Audience = new string[] { SAMLRequest.Issuer };
            assert.Conditions.Items = new ConditionAbstractType[] { audienceRestriction };

            AuthnStatementType authnStatement = new AuthnStatementType();
            authnStatement.AuthnInstant = DateTime.UtcNow;
            authnStatement.SessionIndex = SAMLUtility.GenerateID();

            authnStatement.AuthnContext = new AuthnContextType();
            authnStatement.AuthnContext.Items =
                new object[] { "urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport" };

            authnStatement.AuthnContext.ItemsElementName =
                new ItemsChoiceType5[] { ItemsChoiceType5.AuthnContextClassRef };

            StatementAbstractType[] statementAbstract = new StatementAbstractType[] { authnStatement };
            assert.Items = statementAbstract;
            SAMLResponse.Items = new object[] { assert };

            string xmlResponse = SAMLUtility.SerializeToXmlString(SAMLResponse);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlResponse);
            XmlSignatureUtils.SignDocument(doc, assert.ID);
            SAMLResponse = SAMLUtility.DeserializeFromXmlString<ResponseType>(doc.InnerXml);

            HttpPostBinding binding = new HttpPostBinding(SAMLResponse, HttpUtility.UrlDecode(Request[HttpBindingConstants.RelayState]));
            binding.SendResponse(this.Context, HttpUtility.UrlDecode(SAMLRequest.AssertionConsumerServiceURL), SAMLTypeSSO.signon);
        }

        private void AddSAMLCookie()
        {
            if (!string.IsNullOrEmpty(SAMLRequest.AssertionConsumerServiceURL))
            {
                HttpCookie cookie = Request.Cookies["SistemasLogged"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("SistemasLogged");
                    Response.Cookies.Add(cookie);
                }

                // Carrega a Entidade SYS_Sistema apartir do caminho de login
                SYS_Sistema entitySistema = new SYS_Sistema { sis_caminho = SAMLRequest.AssertionConsumerServiceURL };
                if (SYS_SistemaBO.GetSelectBy_sis_caminho(entitySistema, SYS_SistemaBO.TypePath.login))
                {
                    // Armazena sistema no Cookie
                    cookie.Values[entitySistema.sis_id.ToString()] = entitySistema.sis_nome;
                    // Atualiza dados do Cookie
                    Response.Cookies.Set(cookie);
                }
                else
                    throw new ValidationException("Não foi possível atender a solicitação, sistema emissor da requisição não está cadastrado corretamente.");
            }
            else
                throw new ValidationException("Não foi possível atender a solicitação, requisição inválida.");
        }

        #endregion
    }
}