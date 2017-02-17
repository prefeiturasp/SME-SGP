/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using Data.Common;
    using System;
    using Caching;
    using System.Data;

    /// <summary>
    /// Description: TUR_TurmaDisciplinaTerritorio Business Object. 
    /// </summary>
    public class TUR_TurmaDisciplinaTerritorioBO : BusinessBase<TUR_TurmaDisciplinaTerritorioDAO, TUR_TurmaDisciplinaTerritorio>
	{
        #region Métodos de consulta

        /// <summary>
        /// Seleciona relação de experiências e territórios do saber por turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplinaTerritorio> SelecionaPorTurma(long tur_id, int appMinutosCacheLongo = 0)
        {
            TalkDBTransaction banco = new TUR_TurmaDisciplinaTerritorioDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return SelecionaPorTurma(tur_id, banco, appMinutosCacheLongo);
            }
            catch (Exception ex)
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close(ex);
                }

                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                {
                    banco.Close();
                }
            }
        }

        /// <summary>
        /// Seleciona relação de experiências e territórios do saber por turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplinaTerritorio> SelecionaPorTurma(long tur_id, TalkDBTransaction banco, int appMinutosCacheLongo = 0)
        {
            Func<List<TUR_TurmaDisciplinaTerritorio>> retorno = () => { return new TUR_TurmaDisciplinaTerritorioDAO { _Banco = banco }.SelecionaPorTurma(tur_id); };

            if (appMinutosCacheLongo > 0)
            {

                return CacheManager.Factory.Get
                    (
                        string.Format(ModelCache.TURMA_DISCIPLINA_TERRITORIO_TURMA_MODEL_KEY, tur_id)
                        ,
                        retorno
                        ,
                        appMinutosCacheLongo
                    );
            }

            return retorno();
        }

        /// <summary>
        /// Seleciona territórios vigentes por experiência
        /// </summary>
        /// <param name="tud_idExperiencia"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplinaTerritorio> SelecionaVigentesPorExperienciaPeriodo(long tud_idExperiencia, DateTime cap_dataInicio, DateTime cap_dataFim, TalkDBTransaction banco = null)
        {
            TUR_TurmaDisciplinaTerritorioDAO dao = banco == null ? new TUR_TurmaDisciplinaTerritorioDAO() : new TUR_TurmaDisciplinaTerritorioDAO { _Banco = banco };

            if (banco == null)
            {
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            }

            try
            {
                return dao.SelecionaVigentesPorExperienciaPeriodo(tud_idExperiencia, cap_dataInicio, cap_dataFim);
            }
            catch (Exception ex)
            {
                if (banco == null && dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close(ex);
                }
                throw;
            }
            finally
            {
                if (banco == null && dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }

        /// <summary>
        /// Verifica se a "Experiência" é oferecida no período do calendário informado
        /// </summary>
        /// <param name="tud_idExperiencia">Id da turma disciplina da experiência</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="tpc_id">Id do período do calendário</param>
        /// <returns>True: Experiência oferecida | False: Experiência não oferecida</returns>
        public static bool VerificaOferecimentoExperienciaBimestre(long tud_idExperiencia, int cal_id, int tpc_id)
        {
            TUR_TurmaDisciplinaTerritorioDAO dao = new TUR_TurmaDisciplinaTerritorioDAO();
            return dao.VerificaOferecimentoExperienciaBimestre(tud_idExperiencia, cal_id, tpc_id);
        }

        #endregion Métodos de consulta
    }
}