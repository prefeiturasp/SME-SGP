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
    public class TUR_TurmaDisciplinaTerritorioDAO : Abstract_TUR_TurmaDisciplinaTerritorioDAO
	{
        #region Métodos de consulta

        /// <summary>
        /// Seleciona relação de experiências e territórios do saber por turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public List<TUR_TurmaDisciplinaTerritorio> SelecionaPorTurma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplinaTerritorio_SelecionaPorTurma", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@tur_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                            qs.Return.Select().Select(p => DataRowToEntity(p, new TUR_TurmaDisciplinaTerritorio())).ToList() :
                            new List<TUR_TurmaDisciplinaTerritorio>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona territórios vigentes por experiência
        /// </summary>
        /// <param name="tud_idExperiencia">Id da turma disciplina da experiência.</param>
        /// <returns></returns>
        public List<TUR_TurmaDisciplinaTerritorio> SelecionaVigentesPorExperienciaPeriodo(long tud_idExperiencia, DateTime cap_dataInicio, DateTime cap_dataFim)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplinaTerritorio_SelecionaVigentesPorExperienciaPeriodo", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_idExperiencia";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tud_idExperiencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@cap_dataInicio";
                Param.DbType = DbType.DateTime;
                Param.Value = cap_dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@cap_dataFim";
                Param.DbType = DbType.DateTime;
                Param.Value = cap_dataFim;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Select().Select(p => DataRowToEntity(p, new TUR_TurmaDisciplinaTerritorio())).ToList() :
                    new List<TUR_TurmaDisciplinaTerritorio>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Verifica se a "Experiência" é oferecida no período do calendário informado
        /// </summary>
        /// <param name="tud_idExperiencia">Id da turma disciplina da experiência</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="tpc_id">Id do período do calendário</param>
        /// <returns>True: Experiência oferecida | False: Experiência não oferecida</returns>
        public bool VerificaOferecimentoExperienciaBimestre(long tud_idExperiencia, int cal_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplinaTerritorio_VerificaOferecimentoExperienciaBimestre", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_idExperiencia";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tud_idExperiencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@cal_id";
                Param.DbType = DbType.Int32;
                Param.Size = 8;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tpc_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);
                #endregion Parâmetro

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return Convert.ToInt32(qs.Return.Rows[0][0].ToString()) > 0 ? true : false;
                }
                else
                {
                    return false;
                }                
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de consulta

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaDisciplinaTerritorio entity)
        {
            entity.tte_dataCriacao = entity.tte_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaDisciplinaTerritorio entity)
        {
            entity.tte_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@tte_dataCriacao");
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaDisciplinaTerritorio entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tte_situacao";
            Param.Size = 1;
            Param.Value = entity.tte_situacao;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tte_dataAlteracao";

            Param.Value = entity.tte_dataAlteracao;
            qs.Parameters.Add(Param);
        }

        protected override bool Alterar(TUR_TurmaDisciplinaTerritorio entity)
        {
            __STP_UPDATE = "NEW_TUR_TurmaDisciplinaTerritorio_UPDATE";
            return base.Alterar(entity); 
        }

        public override bool Delete(TUR_TurmaDisciplinaTerritorio entity)
        {
            __STP_DELETE = "NEW_TUR_TurmaDisciplinaTerritorio_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion Métodos sobrescritos
    }
}