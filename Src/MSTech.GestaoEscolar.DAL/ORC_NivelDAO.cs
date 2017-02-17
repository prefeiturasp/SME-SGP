/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ORC_NivelDAO : Abstract_ORC_NivelDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna os níveis por curso, período e calendário
        /// </summary>                
        /// <param name="cur_id">ID da curso</param>
        /// <param name="crr_id">ID da curriculo do curso</param>
        /// <param name="crp_id">ID da periodo do curriculo</param>
        /// <param name="cal_id">ID do calendário</param>
        public DataTable SelectBy_cur_id_crr_id_crp_id_cal_id
        (
            int cur_id
            , int crr_id
            , int crp_id
            , int cal_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_Nivel_SelectBy_cur_id_crr_id_crp_id_cal_id", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os níveis por curso, período, calendário e tipo de disciplina.
        /// </summary>                
        /// <param name="cur_id">ID da curso</param>
        /// <param name="crr_id">ID da curriculo do curso</param>
        /// <param name="crp_id">ID da periodo do curriculo</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        public DataTable SelecionaPorCursoGrupamentoCalendarioTipoDisciplina
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            int tds_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_Nivel_SelectBy_CursoGrupamentoCalendarioDisciplina", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);
                

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os níveis por curso, período, calendário e tipo de disciplina.
        /// </summary>                
        /// <param name="cur_id">ID da curso</param>
        /// <param name="crr_id">ID da curriculo do curso</param>
        /// <param name="crp_id">ID da periodo do curriculo</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <param name="mat_id">ID da matriz.</param>
        public DataTable SelecionaPorCursoGrupamentoCalendarioTipoDisciplinaMatriz
        (
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            int tds_id,
            long mat_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_Nivel_SelectBy_CursoGrupamentoCalendarioDisciplinaMatriz", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@mat_id";
                Param.Size = 8;
                Param.Value = mat_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
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
        public DataTable SelectNiveisPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaNiveis_PorDataSincronizacao", _Banco);

            try
            {
                #region parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                if (tds_id > 0)
                    Param.Value = tds_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os níveis por  calendário
        /// </summary>                       
        /// <param name="cal_id">ID do calendário</param>
        public DataTable BuscaNiveisCalendario
        (
            int cal_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaNiveisCalendario", _Banco);
            try
            {
                #region Parâmetros
                              

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;                
                Param.Value = cal_id;
                
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
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
        public DataTable SelectNiveis
        (
            int nvl_id,
            int cur_id,
            int crr_id,
            int crp_id,
            int cal_id,
            int nvl_ordem
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_Nivel_SelectBy_Filtros", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nvl_id";
                Param.Size = 4;
                if (nvl_id > 0)
                    Param.Value = nvl_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nvl_ordem";
                Param.Size = 4;
                if (nvl_ordem > 0)
                    Param.Value = nvl_ordem;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos para inserir ou alterar

        /// <summary>
        /// O método realiza a correção das ordens dos níveis de orientação curricular.
        /// </summary>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do período do currículo.</param>
        /// <param name="cal_id">ID do calendário anual.</param>
        /// <param name="tds_id">ID do tipo de disciplina.</param>
        /// <returns></returns>
        public bool CorrigirOrdem(int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ORC_Nivel_CorrigeOrdem", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Size = 4;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ORC_Nivel entity)
        {
            entity.nvl_dataCriacao = entity.nvl_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ORC_Nivel entity)
        {
            entity.nvl_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@nvl_dataCriacao");
        }

        protected override bool Alterar(Entities.ORC_Nivel entity)
        {
            __STP_UPDATE = "NEW_ORC_Nivel_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ORC_Nivel entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@nvl_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@nvl_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(ORC_Nivel entity)
        {
            __STP_DELETE = "NEW_ORC_Nivel_UpdateSituacao";
            return base.Delete(entity);
        }
        #endregion
    }
}