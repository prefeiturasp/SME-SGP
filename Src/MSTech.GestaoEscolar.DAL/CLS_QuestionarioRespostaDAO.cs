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

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_QuestionarioRespostaDAO : Abstract_CLS_QuestionarioRespostaDAO
    {
        public DataTable SelectByConteudo
            (
                bool paginado
                , int currentPage
                , int pageSize
                , int qtc_id
                , out int totalRecords
            )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioResposta_SelectBy_qtc_id", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtc_id";
                if (qtc_id > 0)
                    Param.Value = qtc_id;
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

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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

        public DataTable GetEntityQuestionarioConteudoResposta
            (
                int qtr_id
            )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioResposta_SelectQuestionarioConteudoResposta_By_qtr_id", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtr_id";                
                Param.Value = qtr_id;
                qs.Parameters.Add(Param);

                #endregion
                
                qs.Execute();
                    
                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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

        public DataTable SelectByConteudo
            (
               int qtc_id
            )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioResposta_SelectBy_qtc_id", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qtc_id";
                if (qtc_id > 0)
                    Param.Value = qtc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion
                
                qs.Execute();
                
                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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

        public DataTable SelectQuestionarioConteudoRespostaMultiplaSelecao_By_rea_id
           (
              int rea_id
           )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioResposta_SelectQuestionarioConteudoRespostaMultiplaSelecao_By_rea_id", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@rea_id";
                if (rea_id > 0)
                    Param.Value = rea_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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


        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_QuestionarioResposta entity)
        {
            entity.qtr_dataCriacao = entity.qtr_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_QuestionarioResposta entity)
        {
            entity.qtr_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@qtr_dataCriacao");
        }

        protected override bool Alterar(CLS_QuestionarioResposta entity)
        {
            __STP_UPDATE = "NEW_CLS_QuestionarioResposta_UPDATE";
            return base.Alterar(entity);
        }

        public override bool Delete(CLS_QuestionarioResposta entity)
        {
            __STP_DELETE = "NEW_CLS_QuestionarioResposta_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_QuestionarioResposta entity)
        {
            if (entity != null & qs != null)
            {
                return true;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}