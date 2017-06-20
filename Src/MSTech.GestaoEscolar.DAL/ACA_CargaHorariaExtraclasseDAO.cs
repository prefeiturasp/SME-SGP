/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_CargaHorariaExtraclasseDAO : Abstract_ACA_CargaHorariaExtraclasseDAO
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
        public List<ACA_CargaHorariaExtraclasse> SelecionaPorCurriculoPeriodoCalendario(int cur_id, int crr_id, int crp_id, int cal_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CargaHorariaExtraclasse_SelecionaPorCurriculoPeriodoCalendario", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@cur_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@crr_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@crp_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@cal_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Select().Select(p => DataRowToEntity(p, new ACA_CargaHorariaExtraclasse())).ToList() : new List<ACA_CargaHorariaExtraclasse>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            entity.che_dataCriacao = entity.che_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            entity.che_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@che_dataCriacao");
        }

        protected override bool Alterar(ACA_CargaHorariaExtraclasse entity)
        {
            __STP_UPDATE = "NEW_ACA_CargaHorariaExtraclasse_UPDATE";
            return base.Alterar(entity); 
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@che_situacao";
            Param.Size = 1;
            Param.Value = entity.che_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@che_dataAlteracao";
            Param.Value = entity.che_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(ACA_CargaHorariaExtraclasse entity)
        {
            __STP_DELETE = "NEW_ACA_CargaHorariaExtraclasse_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_CargaHorariaExtraclasse entity)
        {
            if (entity != null & qs != null && qs.Return.Rows.Count > 0)
            {
                entity.che_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.che_id > 0;
            }

            return false;
        }

        #endregion
    }
}