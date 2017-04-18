/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Data.Common;
    using System.Web;
    /// <summary>
    /// Description: CFG_PermissaoModuloOperacao Business Object. 
    /// </summary>
    public class CFG_PermissaoModuloOperacaoBO : BusinessBase<CFG_PermissaoModuloOperacaoDAO, CFG_PermissaoModuloOperacao>
	{
	    public enum Operacao
        {
            HistoricoEscolarDadosaluno = 1, 
            HistoricoEscolarEnsinoFundamental = 2, 
            HistoricoEscolarEJA = 3, 
            HistoricoEscolarTransferencia = 4,
            HistoricoEscolarInformacoesComplementares = 5,
            DiarioClasseExclusaoAulas = 6,
            DiarioClasseAnotacoesAluno = 7,
            FechamentoVisualizacaoObservacoes = 8,
            FechamentoExibicaoAbaParecerConclusivo = 9,
            FechamentoExibicaoAbaJustificativaPosConselho = 10,
            FechamentoExibicaoAbaDesempenhoAprendizagem = 11,
            FechamentoExibicaoAbaRecomendacaoAluno = 12,
            FechamentoExibicaoAbaRecomendacaoResponsavel = 13,
            FechamentoExibicaoAbaAnotacoesAluno = 14,
            DiarioClasseLancamentoFrequencia = 15,
            DiarioClasseLancamentoFrequenciaInfantil = 16
        }

        public static List<CFG_PermissaoModuloOperacao> VerificaPermissao(int sis_id, int mod_id, System.Guid gru_id, List<Operacao> list)
        {
            CFG_PermissaoModuloOperacaoDAO dao = new CFG_PermissaoModuloOperacaoDAO();
            string operacoes = string.Join(",", (from Operacao i in list select Convert.ToInt32(i)).ToArray());

            return dao.VerificaPermissao(sis_id, mod_id, gru_id, operacoes);
        }
        
        public static List<CFG_PermissaoModuloOperacao> VerificaPermissaoModuloOperacao(int sis_id, int mod_id, Guid gru_id, List<string> list)
        {
            CFG_PermissaoModuloOperacaoDAO dao = new CFG_PermissaoModuloOperacaoDAO();
            string operacoes = string.Join(",", list.ToArray());

            return dao.VerificaPermissao(sis_id, mod_id, gru_id, operacoes);
        }

        public static bool Salvar(List<CFG_PermissaoModuloOperacao> listaPermissoes)
        {
            TalkDBTransaction bancoGestao = new CFG_PermissaoModuloOperacaoDAO()._Banco.CopyThisInstance();
            try
            {
                bancoGestao.Open();
                foreach (CFG_PermissaoModuloOperacao permissao in listaPermissoes)
                {
                    Save(permissao, bancoGestao);
                    LimpaCache(permissao);
                }
            }
            catch (Exception e)
            {
                bancoGestao.Close(e);
                throw;
            }
            finally
            {
                if (bancoGestao.ConnectionIsOpen)
                {
                    bancoGestao.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static CFG_PermissaoModuloOperacao GetEntity(CFG_PermissaoModuloOperacao entity, TalkDBTransaction banco = null)
        {
            CFG_PermissaoModuloOperacaoDAO dao = banco == null ? new CFG_PermissaoModuloOperacaoDAO() : new CFG_PermissaoModuloOperacaoDAO { _Banco = banco };

            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntity(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dao.Carregar(entity);
                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheLongo)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    GestaoEscolarUtilBO.CopiarEntity(cache, entity);
                }

                return entity;
            }

            dao.Carregar(entity);

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(CFG_PermissaoModuloOperacao entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                HttpContext.Current.Cache.Remove(RetornaChaveCache_GetEntity(entity));
            }
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(CFG_PermissaoModuloOperacao entity)
        {
            return string.Format("CFG_PermissaoModuloOperacao_GetEntity_{0}_{1}_{2}_{3}", entity.gru_id, entity.sis_id, entity.mod_id, entity.pmo_operacao);
        }

    }
}