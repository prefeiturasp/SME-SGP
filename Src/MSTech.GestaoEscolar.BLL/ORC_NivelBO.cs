/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using System.Data;
    using MSTech.Validation.Exceptions;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web;
    using MSTech.Data.Common;
    using System.ComponentModel;

    #region Estruturas

    [Serializable]
    public struct sNivelOrientacaoCurricular
    {
        public int nvl_id { get; set; }
		public int cur_id { get; set; }
		public int crr_id { get; set; }
		public int crp_id { get; set; }
		public int cal_id { get; set; }
		public int tds_id { get; set; }
		public int nvl_ordem { get; set; }
		public string nvl_nome { get; set; }
		public string nvl_nomePlural { get; set; }
		public byte nvl_situacao { get; set; }
		public DateTime nvl_dataCriacao { get; set; }
		public DateTime nvl_dataAlteracao { get; set; }
        public long ocr_id { get; set; }
    }

    #endregion

    /// <summary>
	/// Description: ORC_Nivel Business Object. 
	/// </summary>
    public class ORC_NivelBO : BusinessBase<ORC_NivelDAO, ORC_Nivel>
    {
        #region Constantes

        public const string Cache_SelecionaPorCursoGrupamentoCalendarioTipoDisciplina = "Cache_SelecionaPorCursoGrupamentoCalendarioTipoDisciplina";

        #endregion
        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de curso
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(int cur_id, int crr_id, int crp_id, int cal_id, int tds_id, long mat_id)
        {
            return string.Format(Cache_SelecionaPorCursoGrupamentoCalendarioTipoDisciplina + "_{0}_{1}_{2}_{3}_{4}_{5}", cur_id, crr_id, crp_id, cal_id, tds_id, mat_id);
        }

        /// <summary>
        /// Retorna os níveis por curso, período e calendário
        /// </summary>                
        /// <param name="cur_id">ID da curso</param>
        /// <param name="crr_id">ID da curriculo do curso</param>
        /// <param name="crp_id">ID da periodo do curriculo</param>
        /// <param name="cal_id">ID do calendário</param>
        public static DataTable SelecionaNivelPorCursoPeriodoCalendario
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int cal_id
        )
        {
            ORC_NivelDAO dao = new ORC_NivelDAO();
            return dao.SelectBy_cur_id_crr_id_crp_id_cal_id(cur_id, crr_id, crp_id, cal_id);
        }

        /// <summary>
        /// Retorna os níveis por curso, período, calendário e tipo de disciplina.
        /// </summary>                
        /// <param name="cur_id">ID da curso</param>
        /// <param name="crr_id">ID da curriculo do curso</param>
        /// <param name="crp_id">ID da periodo do curriculo</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        public static DataTable SelecionaPorCursoGrupamentoCalendarioTipoDisciplina
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            int tds_id

        )
        {
            return new ORC_NivelDAO().SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(cur_id, crr_id, crp_id, cal_id, tds_id);
        }

        /// <summary>
        /// Retorna os níveis por curso, período, calendário e tipo de disciplina e matriz.
        /// </summary>                
        /// <param name="cur_id">ID da curso</param>
        /// <param name="crr_id">ID da curriculo do curso</param>
        /// <param name="crp_id">ID da periodo do curriculo</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="mat_id">ID da matriz.</param>
        public static List<sNivelOrientacaoCurricular> SelecionaPorCursoGrupamentoCalendarioTipoDisciplina
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            int tds_id,
            long mat_id,
            int appMinutosCacheLongo
        )
        {
            List<sNivelOrientacaoCurricular> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(cur_id, crr_id, crp_id, cal_id, tds_id, mat_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ORC_NivelDAO dao = new ORC_NivelDAO();
                        DataTable dtDados = dao.SelecionaPorCursoGrupamentoCalendarioTipoDisciplinaMatriz(cur_id, crr_id, crp_id, cal_id, tds_id, mat_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sNivelOrientacaoCurricular
                                 {
                                     nvl_id = Convert.ToInt32(dr["nvl_id"]),
                                     cur_id = Convert.ToInt32(dr["cur_id"]),
                                     crr_id = Convert.ToInt32(dr["crr_id"]),
                                     crp_id = Convert.ToInt32(dr["crp_id"]),
                                     cal_id = Convert.ToInt32(dr["cal_id"]),
                                     tds_id = Convert.ToInt32(dr["tds_id"]),
                                     nvl_ordem = Convert.ToInt32(dr["nvl_ordem"]),
                                     nvl_nome = dr["nvl_nome"].ToString(),
                                     nvl_nomePlural = dr["nvl_nomePlural"].ToString(),
                                     nvl_dataCriacao = Convert.ToDateTime(dr["nvl_dataCriacao"]),
                                     nvl_dataAlteracao = Convert.ToDateTime(dr["nvl_dataAlteracao"]),
                                     nvl_situacao = Convert.ToByte(dr["nvl_situacao"]),
                                     ocr_id = Convert.ToInt64(dr["ocr_id"])
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sNivelOrientacaoCurricular>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ORC_NivelDAO dao = new ORC_NivelDAO();
                DataTable dtDados = dao.SelecionaPorCursoGrupamentoCalendarioTipoDisciplinaMatriz(cur_id, crr_id, crp_id, cal_id, tds_id, mat_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sNivelOrientacaoCurricular
                         {
                             nvl_id = Convert.ToInt32(dr["nvl_id"]),
                             cur_id = Convert.ToInt32(dr["cur_id"]),
                             crr_id = Convert.ToInt32(dr["crr_id"]),
                             crp_id = Convert.ToInt32(dr["crp_id"]),
                             cal_id = Convert.ToInt32(dr["cal_id"]),
                             tds_id = Convert.ToInt32(dr["tds_id"]),
                             nvl_ordem = Convert.ToInt32(dr["nvl_ordem"]),
                             nvl_nome = dr["nvl_nome"].ToString(),
                             nvl_nomePlural = dr["nvl_nomePlural"].ToString(),
                             nvl_dataCriacao = Convert.ToDateTime(dr["nvl_dataCriacao"]),
                             nvl_dataAlteracao = Convert.ToDateTime(dr["nvl_dataAlteracao"]),
                             nvl_situacao = Convert.ToByte(dr["nvl_situacao"]),
                             ocr_id = Convert.ToInt64(dr["ocr_id"])
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna os níveis por  calendário
        /// </summary>                       
        /// <param name="cal_id">ID do calendário</param>
        public static DataTable BuscaNiveisCalendario
        (
            int cal_id
        )
        {
            ORC_NivelDAO dao = new ORC_NivelDAO();
            return dao.BuscaNiveisCalendario(cal_id);
        }

        /// <summary>
        /// Retorna os niveis ativos caso syncDate nulo ou 
        /// retorna apenas os niveis alterados apos a ultima sincronizacao.
        /// </summary>
        /// <param name="syncDate">Data da última sincronização</param>
        /// <param name="cur_id">Curso</param>
        /// <param name="crr_id">Curriculo</param>
        /// <param name="crp_id">Curriculo periodo</param>
        /// <param name="cal_id">Calendario</param>
        /// <param name="tds_id">Tipo de disciplina</param>
        /// <returns></returns>
        public static DataTable BuscaNiveisPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            ORC_NivelDAO dao = new ORC_NivelDAO();
            return dao.SelectNiveisPorDataSincronizacao(syncDate, tur_id, cur_id, crr_id, crp_id, cal_id, tds_id);
        }

        /// <summary>
        /// Altera a ordem do nível de orientação curricular
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo de periodo do calendário</param>
        /// <param name="entityDescer">Entidade do tipo de periodo do calendário</param>
        public static bool SaveOrdem
        (
            ORC_Nivel entityDescer
            , ORC_Nivel entitySubir
        )
        {
            ORC_NivelDAO dao = new ORC_NivelDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {

                if (entityDescer.Validate())
                    Save(entityDescer, dao._Banco);
                else
                    throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

                if (entitySubir.Validate())
                    Save(entitySubir, dao._Banco);
                else
                    throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

                // limpa o cache
                if (HttpContext.Current != null)
                {
                    string chave = RetornaChaveCache_SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(entityDescer.cur_id, entityDescer.crr_id, entityDescer.crp_id, entityDescer.cal_id, entityDescer.tds_id, entityDescer.mat_id);
                    HttpContext.Current.Cache.Remove(chave);
                }
                return true;
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw ex;
            }
            finally
            {
                dao._Banco.Close();
            }
        }

        /// <summary>
        /// Retorna os níveis da orientação curricular de acordo com os filtros
        /// </summary>
        /// <param name="nvl_id">ID do nível</param>
        /// <param name="cur_id">ID do nível</param>
        /// <param name="crr_id">ID do nível</param>
        /// <param name="crp_id">ID do nível</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="nvl_ordem">ID do nível</param>
        public static DataTable GetSelectNiveis
        (
            int nvl_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            int nvl_ordem
        )
        {
            ORC_NivelDAO dao = new ORC_NivelDAO();
            return dao.SelectNiveis(nvl_id, cur_id, crr_id, crp_id, cal_id, nvl_ordem);
        }

        /// <summary>
        /// O método realiza a correção das ordens dos níveis de orientação curricular.
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do período do currículo.</param>
        /// <param name="cal_id">ID do calendário anual.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <returns></returns>
        public static bool CorrigirOrdem(int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            return new ORC_NivelDAO().CorrigirOrdem(cur_id, crr_id, crp_id, cal_id, tds_id);
        }

        /// <summary>
        /// Método save sobrescrito para limpar o cache.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ORC_Nivel entity
        )
        {
            if (new ORC_NivelDAO().Salvar(entity))
            {
                // limpa o cache
                if (HttpContext.Current != null)
                {
                    string chave = RetornaChaveCache_SelecionaPorCursoGrupamentoCalendarioTipoDisciplina(entity.cur_id, entity.crr_id, entity.crp_id, entity.cal_id, entity.tds_id, entity.mat_id);
                    HttpContext.Current.Cache.Remove(chave);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}