/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using MSTech.Data.Common;
    using System.Web;
    using MSTech.Validation.Exceptions;

    #region Estruturas

    [Serializable]
    public struct sOrientacaoNivelAprendizado
    {
        public long ocr_id { get; set; }
        public int nap_id { get; set; }
        public string nap_sigla { get; set; }
        public string nap_descricao { get; set; }
    }

    #endregion

	/// <summary>
	/// Description: ORC_OrientacaoCurricularNivelAprendizado Business Object. 
	/// </summary>
	public class ORC_OrientacaoCurricularNivelAprendizadoBO : BusinessBase<ORC_OrientacaoCurricularNivelAprendizadoDAO, ORC_OrientacaoCurricularNivelAprendizado>
	{
        public const string Cache_SelecionaPorOrientacaoNivelAprendizado = "Cache_SelecionaPorOrientacaoNivelAprendizado";

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar a lista de nivel de aprendizado
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_SelecionaPorOrientacaoNivelAprendizado(int nap_id, string ocr_ids)
        {
            return string.Format(Cache_SelecionaPorOrientacaoNivelAprendizado + "_{0}_{1}", nap_id, ocr_ids);
        }

        /// <summary>
        /// Busca os níveis de aprendizado da orientação curricular.
        /// </summary>
        /// <param name="ocr_idSuperior">Id da orientação curricular superior</param>
        /// <returns></returns>
        public static List<ORC_OrientacaoCurricularNivelAprendizado> SelectNivelAprendizadoByOcrIdSuperior(long ocr_idSuperior)
        {
            ORC_OrientacaoCurricularNivelAprendizadoDAO dao = new ORC_OrientacaoCurricularNivelAprendizadoDAO();
            return dao.SelectNivelAprendizadoByOcrIdSuperior(ocr_idSuperior);
        }

        /// <summary>
        /// Busca os níveis de aprendizado da orientação curricular.
        /// </summary>
        /// <param name="ocr_id">Id da orientação curricular</param>
        /// <param name="nap_id">Id do nível de aprendizado</param>
        /// <returns></returns>
        public static DataTable SelectNivelAprendizadoByOcrId(long ocr_id, int nap_id)
        {
            return SelectNivelAprendizadoByOcrId(ocr_id, nap_id, (new ORC_OrientacaoCurricularNivelAprendizadoDAO())._Banco);
        }

        /// <summary>
        /// Busca os níveis de aprendizado da orientação curricular.
        /// </summary>
        /// <param name="ocr_id">Id da orientação curricular</param>
        /// <param name="nap_id">Id do nível de aprendizado</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        public static DataTable SelectNivelAprendizadoByOcrId(long ocr_id, int nap_id, TalkDBTransaction banco)
        {
            ORC_OrientacaoCurricularNivelAprendizadoDAO dao = new ORC_OrientacaoCurricularNivelAprendizadoDAO();
            return dao.SelectNivelAprendizadoByOcrId(ocr_id, nap_id, banco);
        }

        /// <summary>
        /// Busca os níveis de aprendizado da orientações curriculares.
        /// </summary>
        /// <param name="ocr_id">Ids da orientação curricular</param>
        /// <param name="nap_id">Id do nível de aprendizado</param>
        /// <returns></returns>
        public static List<sOrientacaoNivelAprendizado> SelecionaPorOrientacaoNivelAprendizado(string ocr_ids, int nap_id, TalkDBTransaction banco = null, int appMinutosCacheLongo = 0)
        {
            List<sOrientacaoNivelAprendizado> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaPorOrientacaoNivelAprendizado(nap_id, ocr_ids);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ORC_OrientacaoCurricularNivelAprendizadoDAO dao = banco == null ?
                                                                          new ORC_OrientacaoCurricularNivelAprendizadoDAO() :
                                                                          new ORC_OrientacaoCurricularNivelAprendizadoDAO { _Banco = banco };
                        DataTable dtDados = dao.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, nap_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sOrientacaoNivelAprendizado
                                 {
                                     ocr_id = Convert.ToInt64(dr["ocr_id"]),
                                     nap_id = Convert.ToInt32(dr["nap_id"]),
                                     nap_descricao = dr["nap_descricao"].ToString(),
                                     nap_sigla = dr["nap_sigla"].ToString(),
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sOrientacaoNivelAprendizado>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ORC_OrientacaoCurricularNivelAprendizadoDAO dao = banco == null ?
                                                                  new ORC_OrientacaoCurricularNivelAprendizadoDAO() :
                                                                  new ORC_OrientacaoCurricularNivelAprendizadoDAO { _Banco = banco };
                DataTable dtDados = dao.SelecionaPorOrientacaoNivelAprendizado(ocr_ids, nap_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sOrientacaoNivelAprendizado
                         {
                             ocr_id = Convert.ToInt64(dr["ocr_id"]),
                             nap_id = Convert.ToInt32(dr["nap_id"]),
                             nap_descricao = dr["nap_descricao"].ToString(),
                             nap_sigla = dr["nap_sigla"].ToString(),
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Busca todos os níveis de aprendizado da orientação curricular, com status 1 e 3(excluído).
        /// </summary>
        /// <param name="ocr_id">Id da orientação curricular</param>
        /// <returns></returns>
        public static List<ORC_OrientacaoCurricularNivelAprendizado> SelectTodosNivelAprendizadoByOcrId(long ocr_id)
        {
            ORC_OrientacaoCurricularNivelAprendizadoDAO dao = new ORC_OrientacaoCurricularNivelAprendizadoDAO();
            return dao.SelectTodosNivelAprendizadoByOcrId(ocr_id);
        }

        /// <summary>
        /// Retorna os registros ativos ou com data de alteração posterios
        /// a ultima sincronizacao.
        /// </summary>
        /// <param name="syncDate">Data da última sincronização</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <returns></returns>
        public static DataTable BuscarPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            ORC_OrientacaoCurricularNivelAprendizadoDAO dao = new ORC_OrientacaoCurricularNivelAprendizadoDAO();
            return dao.SelectPorDataSincronizacao(syncDate, tur_id, cur_id, crr_id, crp_id, cal_id, tds_id);
        }

        /// <summary>
        /// O método realiza as validações necessárias e salva uma orientação curricular.
        /// </summary>
        /// <param name="entity">Entidade que representa uma orientação curricular.</param>
        /// <returns></returns>
        public static new bool Save(ORC_OrientacaoCurricularNivelAprendizado entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                GestaoEscolarUtilBO.LimpaCache(string.Format(ORC_OrientacaoCurricularNivelAprendizadoBO.Cache_SelecionaPorOrientacaoNivelAprendizado));

                ORC_OrientacaoCurricularNivelAprendizadoDAO dao = new ORC_OrientacaoCurricularNivelAprendizadoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }
	}
}