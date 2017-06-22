/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.Collections.Generic;
    using System.Data;
    using System;
    using Data.Common;
    using Validation.Exceptions;
    using System.Linq;

    /// <summary>
    /// Description: ACA_CargaHorariaExtraclasse Business Object. 
    /// </summary>
    public class ACA_CargaHorariaExtraclasseBO : BusinessBase<ACA_CargaHorariaExtraclasseDAO, ACA_CargaHorariaExtraclasse>
	{
        #region Métodos de consulta

        /// <summary>
        /// Seleciona as cargas horárias das disciplinas por curso, calendário e período do curso
        /// </summary>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <param name="cal_id"></param>
        /// <returns></returns>
        public static List<ACA_CargaHorariaExtraclasse> SelecionaPorCurriculoPeriodoCalendario(int cur_id, int crr_id, int crp_id, int cal_id)
        {
            return new ACA_CargaHorariaExtraclasseDAO().SelecionaPorCurriculoPeriodoCalendario(cur_id, crr_id, crp_id, cal_id);
        }

        /// <summary>
        /// Verifica se existe atividade cadastrada ou lançamento cadastrado para a carga horária.
        /// </summary>
        /// <param name="dis_ids">Id da disciplina</param>
        /// <returns></returns>
        public static DataTable VerificaAtividadeLancamento(string dis_ids)
        {
            return new ACA_CargaHorariaExtraclasseDAO().SelectAtividadeLancamento(dis_ids);
        }

        #endregion

        #region Métodos de inclusão/Alteração

        /// <summary>
        /// Salva uma carga horária de atividade extraclasse.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static new bool Save(ACA_CargaHorariaExtraclasse entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                return new ACA_CargaHorariaExtraclasseDAO { _Banco = banco }.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Salva uma lista de cargas horárias de atividades extraclasse.
        /// </summary>
        /// <param name="lstCargaHoraria"></param>
        /// <returns></returns>
        public static bool Salvar(List<ACA_CargaHorariaExtraclasse> lstCargaHoraria)
        {
            ACA_CargaHorariaExtraclasseDAO dao = new ACA_CargaHorariaExtraclasseDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                return lstCargaHoraria.Aggregate(true, (salvou, entity) => salvou & Save(entity, dao._Banco));
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (dao._Banco.ConnectionIsOpen)
                {
                    dao._Banco.Close();
                }
            }
        }

        #endregion
    }
}