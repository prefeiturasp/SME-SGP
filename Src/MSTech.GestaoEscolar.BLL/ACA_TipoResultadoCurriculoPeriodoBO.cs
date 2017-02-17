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
    using MSTech.Data.Common;
    using System.Web;
    using System;
    using System.Linq;

    #region Estruturas

    /// <summary>
    /// Estrutura para a consulta dos tipos de resultado
    /// </summary>
    [Serializable]
    public struct Struct_TipoResultado
    {
        public int tpr_id;
        public int cur_id;
        public int crr_id;
        public int crp_id;
        public byte tpr_resultado;
        public string tpr_nomenclatura;
        public byte tpr_situacao;
        public DateTime tpr_dataCriacao;
        public DateTime tpr_dataAlteracao;
        public string crp_descricao;
    }

    #endregion Estruturas
	
	/// <summary>
	/// Description: ACA_TipoResultadoCurriculoPeriodo Business Object. 
	/// </summary>
	public class ACA_TipoResultadoCurriculoPeriodoBO : BusinessBase<ACA_TipoResultadoCurriculoPeriodoDAO, ACA_TipoResultadoCurriculoPeriodo>
    {
        public const string Cache_TipoResultado = "Cache_SelecionaTipoResultado";

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta de tipos de resultados.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="tipoLancamento">Tipo de lançamento</param>
        /// <param name="tds_id">Tipo de disciplina</param>
        /// <returns></returns>
        public static string RetornaChaveCache_TipoResultado(int cur_id, int crr_id, int crp_id, EnumTipoLancamento tipoLancamento, int tds_id)
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}", Cache_TipoResultado, cur_id, crr_id, crp_id, (byte)tipoLancamento, tds_id);
        }

        /// <summary>
        /// Busca os tipos de resultados, de acordo com o curso, o currículo e o período.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <returns></returns>
        public static List<Struct_TipoResultado> SelecionaTipoResultado(int cur_id, int crr_id, int crp_id, EnumTipoLancamento tipoLancamento, int tds_id = -1)
        {
            List<Struct_TipoResultado> dados = null;
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_TipoResultado(cur_id, crr_id, crp_id, tipoLancamento, tds_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    dados = SelecionaTipoResultado(cur_id, crr_id, crp_id, (byte)tipoLancamento, tds_id);

                    // Adiciona cache com validade de 6h.
                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(GestaoEscolarUtilBO.MinutosCacheMedio)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<Struct_TipoResultado>)cache;
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                dados = SelecionaTipoResultado(cur_id, crr_id, crp_id, (byte)tipoLancamento, tds_id);
            }

            return dados;
        }

        /// <summary>
        /// Busca os tipos de resultados, de acordo com o curso, o currículo e o período.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="tipoLancamento">Tipo de lançamento</param>
        /// <param name="tds_id">Tipo de disciplina</param>
        /// <returns></returns>
        private static List<Struct_TipoResultado> SelecionaTipoResultado(int cur_id, int crr_id, int crp_id, byte tipoLancamento, int tds_id)
        {
            // Se não retornou os dados do cache, carrega do banco.
            DataTable dt = new ACA_TipoResultadoCurriculoPeriodoDAO().SelectTipoResultadoBy_Filtros(cur_id, crr_id, crp_id, tipoLancamento, tds_id);
            List<Struct_TipoResultado> dados = (from DataRow row in dt.Rows
                                                select new Struct_TipoResultado
                                                    {
                                                        tpr_id = Convert.ToInt32(row["tpr_id"])
                                                        ,
                                                        cur_id = Convert.ToInt32(row["cur_id"])
                                                        ,
                                                        crr_id = Convert.ToInt32(row["crr_id"])
                                                        ,
                                                        crp_id = Convert.ToInt32(row["crp_id"])
                                                        ,
                                                        tpr_resultado = Convert.ToByte(row["tpr_resultado"])
                                                        ,
                                                        tpr_nomenclatura = row["tpr_nomenclatura"].ToString()
                                                        ,
                                                        tpr_situacao = Convert.ToByte(row["tpr_situacao"])
                                                       ,
                                                        tpr_dataCriacao = Convert.ToDateTime(row["tpr_dataCriacao"])
                                                       ,
                                                        tpr_dataAlteracao = Convert.ToDateTime(row["tpr_dataAlteracao"])
                                                       ,
                                                        crp_descricao = row["crp_descricao"].ToString()
                                                    }).ToList();
            return dados;
        }

        /// <summary>
        /// Busca os tipos de resultados com base no tpr_id.
        /// </summary>
        /// <param name="tpr_id">Id do tipo de resultado</param>
        /// <returns></returns>
        public static DataTable SelectBy_tpr_id(int tpr_id, TalkDBTransaction banco = null)
        {
            ACA_TipoResultadoCurriculoPeriodoDAO dao;
            if (banco != null)
                dao = new ACA_TipoResultadoCurriculoPeriodoDAO { _Banco = banco };
            else
                dao = new ACA_TipoResultadoCurriculoPeriodoDAO();
            return dao.SelectBy_tpr_id(tpr_id);
        }	
	}
}