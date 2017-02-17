/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Entities;
    using GestaoEscolar.DAL.Abstracts;
    using MSTech.Data.Common;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class TUR_TurmaAberturaAnosAnterioresDAO : Abstract_TUR_TurmaAberturaAnosAnterioresDAO
    {
        public DataTable SelecionaAberturasAnosLetivosBy_AnoDreEscStatus
        (
            int tab_ano,
            Guid uad_idSuperior,
            int uni_id,
            int esc_id,
            int tab_status,
            bool paginado,
            int currentPage,
            int pageSize,
            out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaAberturaAnosAnteriores_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tab_ano";
                Param.Size = 8;
                if (tab_ano > 0)
                    Param.Value = tab_ano;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == new Guid())
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 8;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@tab_status";
                Param.Size = 8;
                if (tab_status > 0)
                    Param.Value = tab_status;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public bool VerificaRegistroExistente
        (
            long tab_id,
            int tab_ano,
            Guid uad_idSuperior,
            int uni_id,
            int esc_id,
            DateTime tab_dataInicio,
            DateTime tab_dataFim
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaAberturaAnosAnteriores_VerificaRegistroExistente", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tab_id";
                Param.Size = 8;
                Param.Value = tab_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tab_ano";
                Param.Size = 8;
                Param.Value = tab_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior == new Guid())
                    Param.Value = DBNull.Value;
                else
                    Param.Value = uad_idSuperior;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 8;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@tab_dataInicio";
                Param.Size = 8;
                Param.Value = tab_dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@tab_dataFim";
                Param.Size = 8;
                if (tab_dataFim != DateTime.MinValue)
                    Param.Value = tab_dataFim;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Métodos Sobrescritos

        /// <summary>
        /// Override do método ParamInserir
        /// </summary>        
        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaAberturaAnosAnteriores entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tab_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tab_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método ParamAlterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaAberturaAnosAnteriores entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tab_dataCriacao");
            qs.Parameters["@tab_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método Alterar
        /// </summary>     
        protected override bool Alterar(TUR_TurmaAberturaAnosAnteriores entity)
        {
            __STP_UPDATE = "NEW_TUR_TurmaAberturaAnosAnteriores_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Override do método Delete
        /// </summary>
        public override bool Delete(TUR_TurmaAberturaAnosAnteriores entity)
        {
            __STP_DELETE = "NEW_TUR_TurmaAberturaAnosAnteriores_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}