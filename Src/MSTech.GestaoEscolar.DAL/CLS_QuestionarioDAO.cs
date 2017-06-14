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
    public class CLS_QuestionarioDAO : Abstract_CLS_QuestionarioDAO
	{
        /// <summary>
        /// Busca os questionários filtrado por título
        /// </summary>
        /// <param name="qst_titulo"></param>
        /// <returns></returns>
        public DataTable SelectBy_qst_titulo(string qst_titulo)
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_Questionario_SelectBy_qst_titulo", _Banco);
            try
            {
                #region PARAMETROS


                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@qst_titulo";
                if (!String.IsNullOrEmpty(qst_titulo))
                    Param.Value = qst_titulo;
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

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_Questionario entity)
        {
            entity.qst_dataCriacao = entity.qst_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_Questionario entity)
        {
            entity.qst_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@qst_dataCriacao");
        }

        protected override bool Alterar(CLS_Questionario entity)
        {
            __STP_UPDATE = "NEW_CLS_Questionario_UPDATE";
            return base.Alterar(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_Questionario entity)
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