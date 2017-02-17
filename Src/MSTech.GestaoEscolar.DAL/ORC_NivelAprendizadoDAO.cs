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
    public class ORC_NivelAprendizadoDAO : AbstractORC_NivelAprendizadoDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna um datatable com os niveis ativos (quando syncDate == null)
        /// ou todos os niveis com a data de alteração >= syncDate.
        /// </summary>
        /// <param name="syncDate">Data da última sincronização</param>
        /// <param name="cur_id">Id do curso da turma</param>
        /// <param name="crr_id">Id do curriculo da turma</param>
        /// <param name="crp_id">Id do curriculoPeriodo da turma</param>
        /// <returns></returns>
        public DataTable SelectNiveisPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_NivelAprendizado_PorDataSincronizacao", _Banco);
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
        /// Retorna um datatable contendo todos os níveis de aprendizado cadastrados no BD
        /// não excluidos logicamente.
        /// </summary>
        /// <returns>Datatable com os níveis de aprendizado</returns>
        public DataTable SelectNiveisAprendizadoAtivos(int cur_id, int crr_id, int crp_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_NivelAprendizado_Select", _Banco);
            try
            {
                #region Parâmetros

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
        /// Busca o nível de aprendizado com a mesma sigla e ativo
        /// </summary>
        /// <param name="nap_sigla">Sigla do nível de aprendizado da orientação curricular</param>
        /// <returns>Datatable com os níveis de aprendizado</returns>
        public bool SelectNivelAprendizadoBySigla(ORC_NivelAprendizado nivelAprendizado)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_NivelAprendizado_SelectBy_Sigla", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@nap_sigla";
                Param.Size = 10;
                Param.Value = nivelAprendizado.nap_sigla;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nap_id";
                Param.Size = 4;
                if (nivelAprendizado.nap_id > 0)
                    Param.Value = nivelAprendizado.nap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = nivelAprendizado.cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = nivelAprendizado.crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = nivelAprendizado.crp_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return qs.Return.Rows.Count > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public DataTable SelectNivelAprendizadoByCursoPeriodo(ORC_NivelAprendizado nivelAprendizado)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_NivelAprendizado_SelectBy_CursoPeriodo", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = nivelAprendizado.cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = nivelAprendizado.crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = nivelAprendizado.crp_id;
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

        public int SelectCursoPeriodoBy_nap_id(int nap_id, int nvl_id, TalkDBTransaction banco)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_NivelAprendizado_SelectCursoPeriodoBy_nap_id", banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nap_id";
                Param.Size = 4;
                Param.Value = nap_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nvl_id";
                Param.Size = 4;
                Param.Value = nvl_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0]["nap_id"]) : 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ORC_NivelAprendizado entity)
        {
            entity.nap_dataCriacao = entity.nap_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ORC_NivelAprendizado entity)
        {
            entity.nap_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@nap_dataCriacao");
        }

        protected override bool Alterar(ORC_NivelAprendizado entity)
        {
            __STP_UPDATE = "NEW_ORC_NivelAprendizado_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, ORC_NivelAprendizado entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@nap_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@nap_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(ORC_NivelAprendizado entity)
        {
            __STP_DELETE = "NEW_ORC_NivelAprendizado_UpdateSituacao";
            return base.Delete(entity);
        }

        #endregion
    }
}