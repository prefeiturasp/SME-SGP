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
    public class CLS_QuestionarioConteudoDAO : Abstract_CLS_QuestionarioConteudoDAO
    {
        /// <summary>
        ///Busca os conteúdos filtrado por questionário
        /// </summary>
        /// <param name="qst_id"></param>
        /// <returns></returns>
        public DataTable SelectByQuestionarioPaginado
            (
                bool paginado
                , int currentPage
                , int pageSize
                , int qst_id
                , out int totalRecords
            )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioConteudo_SelectBy_qst_id", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qst_id";
                if (qst_id > 0)
                    Param.Value = qst_id;
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

        public DataTable SelectByQuestionario
            (
                int qst_id
            )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_QuestionarioConteudo_SelectBy_qst_id", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qst_id";
                if (qst_id > 0)
                    Param.Value = qst_id;
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

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_QuestionarioConteudo entity)
        {
            entity.qtc_dataCriacao = entity.qtc_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_QuestionarioConteudo entity)
        {
            entity.qtc_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@qtc_dataCriacao");
        }

        protected override bool Alterar(CLS_QuestionarioConteudo entity)
        {
            __STP_UPDATE = "NEW_CLS_QuestionarioConteudo_UPDATE";
            return base.Alterar(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_QuestionarioConteudo entity)
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