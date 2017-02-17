/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using MSTech.Data.Common;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web;

    #region Estruturas

    [Serializable]
    public struct sPlanejamentoProjetoRelacionadas
    {
        public int tds_id { get; set; }
        public string tds_nome { get; set; }
        public int tds_ordem { get; set; }
        public int plp_id { get; set; }
        public byte crd_tipo { get; set; }
    }

    #endregion
    /// <summary>
	/// Description: CLS_PlanejamentoProjetoRelacionada Business Object. 
	/// </summary>
	public class CLS_PlanejamentoProjetoRelacionadaBO : BusinessBase<CLS_PlanejamentoProjetoRelacionadaDAO, CLS_PlanejamentoProjetoRelacionada>
	{
        public const string Cache_Seleciona_CursosRelacionados_Por_Escola = "Cache_Seleciona_CursosRelacionados_Por_Escola";
        public static string RetornaChaveCache_SelecionaPlanejamentoProjetoRelacionada(int esc_id, int uni_id, int cal_id, int cur_id, int plp_id)
        {
            return string.Format(Cache_Seleciona_CursosRelacionados_Por_Escola + "_{0}_{1}_{2}_{3}_{4}", esc_id, uni_id, cal_id, cur_id, plp_id);
        }

        /// <summary>
        /// Seleciona as disciplinas relacionadas do projeto e as disciplinas do curso
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade escolar</param>
        /// <param name="cal_id">ID do calendario</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="plp_id">ID do planejamento do projeto</param>
        /// <param name="appMinutosCacheLongo">Tempo de cache longo</param>
        /// <returns></returns>
        public static List<sPlanejamentoProjetoRelacionadas> SelecionaPlanejamentoProjetoRelacionada(int esc_id, int uni_id, int cal_id, int cur_id, int plp_id, int appMinutosCacheLongo)
        {
            List<sPlanejamentoProjetoRelacionadas> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaPlanejamentoProjetoRelacionada(esc_id, uni_id, cal_id, cur_id, plp_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        CLS_PlanejamentoProjetoRelacionadaDAO dao = new CLS_PlanejamentoProjetoRelacionadaDAO();
                        DataTable dtDados = dao.SelecionaPlanejamentoProjetoRelacionada(esc_id, uni_id, cal_id, cur_id, plp_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sPlanejamentoProjetoRelacionadas
                                 {
                                     tds_id = Convert.ToInt32(dr["tds_id"]),
                                     tds_nome = dr["tds_nome"].ToString(),
                                     tds_ordem = Convert.ToInt32(dr["tds_ordem"]),
                                     plp_id = Convert.ToInt32(dr["plp_id"]),
                                     crd_tipo = Convert.ToByte(dr["crd_tipo"])
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sPlanejamentoProjetoRelacionadas>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                CLS_PlanejamentoProjetoRelacionadaDAO dao = new CLS_PlanejamentoProjetoRelacionadaDAO();
                DataTable dtDados = dao.SelecionaPlanejamentoProjetoRelacionada(esc_id, uni_id, cal_id, cur_id, plp_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sPlanejamentoProjetoRelacionadas
                         {
                                     tds_id = Convert.ToInt32(dr["tds_id"]),
                                     tds_nome = dr["tds_nome"].ToString(),
                                     tds_ordem = Convert.ToInt32(dr["tds_ordem"]),
                                     plp_id = Convert.ToInt32(dr["plp_id"]),
                                     crd_tipo = Convert.ToByte(dr["crd_tipo"])
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Remove todas tipo disciplinas relacionadas ao planejamento do projeto
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade escolar</param>
        /// <param name="cal_id">ID do calendario</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="plp_id">ID do planejamento do projeto</param>
        /// <param name="banco">Transação do banco</param>
        public static void LimparRelacionadas(int esc_id, int uni_id, int cal_id, int cur_id, int plp_id, TalkDBTransaction banco)
        {
            GestaoEscolarUtilBO.LimpaCache(string.Format(Cache_Seleciona_CursosRelacionados_Por_Escola + "_{0}_{1}_{2}_{3}", esc_id, uni_id, cal_id, cur_id));
            CLS_PlanejamentoProjetoRelacionadaDAO dao = new CLS_PlanejamentoProjetoRelacionadaDAO();
            dao._Banco = banco;
            dao.LimparRelacionadas(esc_id, uni_id, cal_id, cur_id, plp_id);
        }
    }
}