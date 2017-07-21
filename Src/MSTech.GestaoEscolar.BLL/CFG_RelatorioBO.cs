/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Web;
    using MSTech.Security.Cryptography;
    using MSTech.Validation.Exceptions;
    using System.Collections.Generic;
using System.Web.Security;
    using System.Security.Principal;
    using System.Security.Claims;

    /// <summary>
    /// Description: CFG_Relatorio Business Object. 
    /// </summary>
    public class CFG_RelatorioBO : BusinessBase<CFG_RelatorioDAO, CFG_Relatorio>
    {
        #region CONSTANTES

        public const string SessReportName = "SessReportName";
        public const string SessParameters = "SessParameters";

        #endregion CONSTANTES

        #region PROPRIEDADES

        public static string ExternalUrlReport { get; set; }
        

        #region Dados do relatório por Session (sem IFrame)

        /// <summary>
        /// Grava o valor do id do relatório na session do usuário corrente.
        /// </summary>
        public static string CurrentReportID
        {
            get
            {
                object rptName = HttpContext.Current.Session[SessReportName];
                if (rptName != null)
                {
                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                    return sa.Decrypt(rptName.ToString(), System.Text.Encoding.GetEncoding("iso-8859-1"));
                }
                return String.Empty;
            }
            set
            {
                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                HttpContext.Current.Session[SessReportName] = sa.Encrypt(value, System.Text.Encoding.GetEncoding("iso-8859-1"));
            }
        }

        /// <summary>
        /// Grava o valor dos parâmetros do relatório na session do usuário corrente.
        /// </summary>
        public static string CurrentReportParameters
        {
            get
            {
                object rptParam = HttpContext.Current.Session[SessParameters];
                if (rptParam != null)
                {
                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                    return sa.Decrypt(rptParam.ToString(), System.Text.Encoding.GetEncoding("iso-8859-1"));
                }
                return String.Empty;
            }
            set
            {
                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                HttpContext.Current.Session[SessParameters] = sa.Encrypt(value, System.Text.Encoding.GetEncoding("iso-8859-1"));
            }
        }

        #endregion


        #region Dados do relatório por QueryString (usado com o IFrame)

        /// <summary>
        /// Busca o valor do id do relatório pela QueryString "tipRel".
        /// </summary>
        public static string qsCurrentReportID
        {
            get
            {
                string rptName = qsCurrentReportID_Encrypted;
                if (!String.IsNullOrEmpty(rptName))
                {
                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                    return sa.Decrypt(rptName.ToString(), System.Text.Encoding.GetEncoding("iso-8859-1"));
                }
                return String.Empty;
            }
        }

        public static string qsCurrentReportID_Encrypted
        {
            get
            {
                object rptName = HttpContext.Current.Request["tipRel"];
                if (rptName != null)
                {
                    return rptName.ToString();
                }
                return String.Empty;
            }
        }


        /// <summary>
        /// Busca o valor dos parâmetros do relatório pela QueryString "params".
        /// </summary>
        public static string qsCurrentReportParameters
        {
            get
            {
                string rptParam = qsCurrentReportParameters_Encrypted;
                if (!String.IsNullOrEmpty(rptParam))
                {
                    SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                    return sa.Decrypt(rptParam.ToString(), System.Text.Encoding.GetEncoding("iso-8859-1"));
                }
                return String.Empty;
            }
        }

        public static string qsCurrentReportParameters_Encrypted
        {
            get
            {
                object rptParam = HttpContext.Current.Request["params"];
                if (rptParam != null)
                {
                    return rptParam.ToString();
                }
                return String.Empty;
            }
        }


        #endregion

        #endregion PROPRIEDADES


        #region MÉTODOS

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(CFG_Relatorio entity)
        {
            return String.Format("CFG_Relatorio_GetEntity_{0}", entity.rlt_id);
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static new CFG_Relatorio GetEntity(CFG_Relatorio entity)
        {
            if (HttpContext.Current != null)
            {
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    if (new CFG_RelatorioDAO().Carregar(entity))
                    {
                        HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            new CFG_RelatorioDAO().Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Carrega a session corrente do usuário com as informações para a geração do relatório.
        /// </summary>
        /// <param name="reportID">id do relatório.</param>
        /// <param name="parameters">Parâmetros do relatório.</param>
        public static void SendParametersToReport(string reportID, string parameters)
        {
            #region VALIDA PARAMETROS DE ENTRADA

            if (String.IsNullOrEmpty(reportID))
                throw new ValidationException("O parâmetro reportID é obrigatório.");

            #endregion VALIDA PARAMETROS DE ENTRADA

            CurrentReportID = reportID;
            CurrentReportParameters = parameters;
        }

        /// <summary>
        /// Utilizado na chamada do relatório.
        /// </summary>
        /// <param name="PastaRelatorio"></param>
        /// <param name="reportID"></param>
        /// <param name="parameters"></param>
        /// <param name="contexto"></param>
        public static void CallReport(string PastaRelatorio, string reportID, string parameters, HttpContext contexto)
        {
            #region VALIDA PARAMETROS DE ENTRADA

            if (String.IsNullOrEmpty(reportID))
                throw new ValidationException("O parâmetro reportID é obrigatório.");

            #endregion VALIDA PARAMETROS DE ENTRADA

            var linkReport = LinkReport(PastaRelatorio, reportID, parameters);
            contexto.Response.Redirect(linkReport, false);
            contexto.ApplicationInstance.CompleteRequest();
        }

        public static string LinkReport(string PastaRelatorio, string reportID, string parameters)
        {
            #region VALIDA PARAMETROS DE ENTRADA

            if (String.IsNullOrEmpty(reportID))
                return "~/" + PastaRelatorio + "/Relatorio.aspx";

            #endregion VALIDA PARAMETROS DE ENTRADA

            if (String.IsNullOrEmpty(ExternalUrlReport))
            {
                CurrentReportID = reportID;
                CurrentReportParameters = parameters;

                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                return String.Format("~/" + PastaRelatorio + "/Relatorio.aspx?dummy='{0}'"
                                                                                    , HttpUtility.UrlEncode(sa.Encrypt(reportID)));
            }
            else
            {
                //Adiciona o UserID nos parâmetros do relatório para uma validação de segurança quando executado pelo 
                //site do GestaoReports através do IFrame.
                parameters = parameters + "&usu_login=" + GetUsu_Login_FromCookie();

                SymmetricAlgorithm sa = new SymmetricAlgorithm(SymmetricAlgorithm.Tipo.TripleDES);
                string report = sa.Encrypt(reportID, System.Text.Encoding.GetEncoding("iso-8859-1"));
                string tipRel = sa.Encrypt(reportID, System.Text.Encoding.GetEncoding("iso-8859-1"));
                string paramsRel = sa.Encrypt(parameters, System.Text.Encoding.GetEncoding("iso-8859-1"));

                return String.Format("~/" + PastaRelatorio + "/Relatorio.aspx?dummy={0}&tipRel={1}&params={2}"
                                                                                    , HttpUtility.UrlEncode(report)
                                                                                    , HttpUtility.UrlEncode(tipRel)
                                                                                    , HttpUtility.UrlEncode(paramsRel));
            }
        }


        public static string GetUsu_Login_FromCookie()
        {
            var usu_login = String.Empty;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var identity = HttpContext.Current.User.Identity as FormsIdentity;
                if (identity != null)
                {
                    // Recupera Ticket de autenticação gravado em Cookie
                    FormsIdentity id = identity;
                    FormsAuthenticationTicket ticket = id.Ticket;

                    usu_login = GetUsuLogin(HttpContext.Current.User.Identity);// MSTech.CoreSSO.BLL.UtilBO.GetNameFormsAuthentication(ticket.Name, MSTech.CoreSSO.BLL.UtilBO.TypeName.Login);
                }
            }
            return usu_login;
        }

        public static string GetUsuLogin(IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name);
            return (claim != null) ? claim.Value : string.Empty;
        }

        /// <summary>
        /// Remover os valores da session com parâmetros para geração dos relatórios.
        /// </summary>
        public static void ClearSessionReportParameters()
        {
            HttpContext.Current.Session.Remove(SessReportName);
            HttpContext.Current.Session.Remove(SessParameters);
        }

        /// <summary>
        /// Retorna uma lista de relatórios que não foram excluídos logicamente, 
        /// filtrados por ent_id e srr_id
        /// </summary>                
        /// <param name="ent_id">Entidade em que o usuário esta logado</param>        
        /// <param name="srr_id">ID do servidor de relatórios</param>        
        /// /// <returns>Lista de relatórios</returns>
        public static IList<CFG_Relatorio> RelatoriosAtivos_SelectBy_EntidadeServidor
        (
            Guid ent_id
            , int srr_id
        )
        {
            CFG_RelatorioDAO dao = new CFG_RelatorioDAO();
            return dao.SelectBy_RelatoriosAtivos_EntidadeServidor(ent_id, srr_id);
        }

        #endregion MÉTODOS
    }
    
}