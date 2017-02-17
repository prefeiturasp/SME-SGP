/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MSTech.Business.Common;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using System.Web;
using System.Data;
using MSTech.CoreSSO.Entities;
using MSTech.Validation.Exceptions;
using CFG_Relatorio = MSTech.GestaoEscolar.Entities.CFG_Relatorio;

namespace MSTech.GestaoEscolar.BLL
{
    
	/// <summary>
	/// Description: CFG_RelatorioDocumentoDocente Business Object. 
	/// </summary>
	public class CFG_RelatorioDocumentoDocenteBO : BusinessBase<CFG_RelatorioDocumentoDocenteDAO, CFG_RelatorioDocumentoDocente>
	{
        #region Propriedades

        private static IDictionary<ReportNameGestaoAcademicaDocumentosDocente, string[]> parametros;

        /// <summary>
        /// Retorna os parâmetros de mensagens do sistema.
        /// </summary>
        public static IDictionary<ReportNameGestaoAcademicaDocumentosDocente, string[]> Parametros
        {
            get
            {
                if ((parametros == null) || (parametros.Count == 0))
                    RecarregaDocumentosAtivos();

                return parametros;
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Recarrega os documentos ativos do aluno.
        /// </summary>
        public static void RecarregaDocumentosAtivos()
        {
            parametros = new Dictionary<ReportNameGestaoAcademicaDocumentosDocente, string[]>();
            lock (parametros)
            {
                SelecionaParametrosAtivos(out parametros);
            }
        }

        /// <summary>
        /// Retorna os parâmetros de integração ativos.
        /// </summary>
        private static void SelecionaParametrosAtivos(out IDictionary<ReportNameGestaoAcademicaDocumentosDocente, string[]> dictionary)
        {
            IList<CFG_RelatorioDocumentoDocente> lt = GetSelect();
           
            dictionary = (from CFG_RelatorioDocumentoDocente ent in lt
                          where Enum.IsDefined(typeof(ReportNameGestaoAcademicaDocumentosDocente), ent.rlt_id) && ent.rdd_situacao != 3
                          group ent by ent.rlt_id.ToString() into t
                          select new
                          {
                              chave = t.Key
                              ,
                              valor = t.Select(p => p.rdd_nomeDocumento).ToArray()
                          }).ToDictionary(
                                p => (ReportNameGestaoAcademicaDocumentosDocente)Enum.Parse(typeof(ReportNameGestaoAcademicaDocumentosDocente), p.chave)
                                , p => p.valor);
        }

        /// <summary>
        /// Seleciona os relatórios dos documentos de escola.
        /// </summary>
        /// <returns></returns>
        public static IDictionary<ReportNameGestaoAcademicaDocumentosDocente, string[]> SelecionaRelatorios()
        {
            IList<CFG_Relatorio> lt = CFG_RelatorioBO.GetSelect().Where(p => p.rlt_situacao == 1).ToList();

            return (from CFG_Relatorio ent in lt
                    where Enum.IsDefined(typeof(ReportNameGestaoAcademicaDocumentosDocente), ent.rlt_id)
                    group ent by ent.rlt_id.ToString() into t
                    select new
                    {
                        chave = t.Key
                        ,
                        valor = t.Select(p => p.rlt_nome).ToArray()
                    }).ToDictionary(
                                p => (ReportNameGestaoAcademicaDocumentosDocente)Enum.Parse(typeof(ReportNameGestaoAcademicaDocumentosDocente), p.chave)
                                , p => p.valor);
        }

        /// <summary>
        /// Seleciona os documentos da escola ativos, filtrados por entidade.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public static DataTable SelecionaPorEntidade(Guid ent_id)
        {
            return new CFG_RelatorioDocumentoDocenteDAO().SelecionaPorEntidade(ent_id);
        }
        
        /// <summary>
        /// Retorna todos os relatórios do documento do docente para a visão informada
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CFG_RelatorioDocumentoDocente> SelecionaVisaoCache(int vis_id, int appMinutosCacheLongo = 0)
        {
            List<CFG_RelatorioDocumentoDocente> dadosAux = null;
            List<CFG_RelatorioDocumentoDocente> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = string.Format(RetornaChaveCache_DocumentosDocenteVisao(), vis_id); ;
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        dados = new List<CFG_RelatorioDocumentoDocente>();
                        dadosAux = SelecionaVisao(vis_id);

                        foreach (KeyValuePair<ReportNameGestaoAcademicaDocumentosDocente, string[]> item in CFG_RelatorioDocumentoDocenteBO.Parametros)
                        {
                            CFG_RelatorioDocumentoDocente rel = dadosAux.FirstOrDefault(vis => vis.rlt_id == (int)item.Key);
                            if (rel != null && !rel.IsNew)
                                dados.Add(rel);
                        }

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<CFG_RelatorioDocumentoDocente>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                dados = new List<CFG_RelatorioDocumentoDocente>();
                dadosAux = SelecionaVisao(vis_id);

                foreach (KeyValuePair<ReportNameGestaoAcademicaDocumentosDocente, string[]> item in CFG_RelatorioDocumentoDocenteBO.Parametros)
                {
                    CFG_RelatorioDocumentoDocente rel = dadosAux.FirstOrDefault(vis => vis.rlt_id == (int)item.Key);
                    if (rel != null)
                    {
                        if (!rel.IsNew)
                            dados.Add(rel);
                    }
                }
            }

            return dados;
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_DocumentosDocenteVisao()
        {
            return "RetornaChaveCache_DocumentosDocenteVisao{0}";
        }

        /// <summary>
        /// Retorna todos os relatórios do documento do docente para a visão informada
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CFG_RelatorioDocumentoDocente> SelecionaVisao(int vis_id)
        {
            CFG_RelatorioDocumentoDocenteDAO dao = new CFG_RelatorioDocumentoDocenteDAO();
            return dao.SelecionaVisao(vis_id);
        }
        
        #endregion Métodos
	}
}