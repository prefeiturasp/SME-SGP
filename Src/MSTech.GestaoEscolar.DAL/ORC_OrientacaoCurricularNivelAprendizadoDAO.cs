/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ORC_OrientacaoCurricularNivelAprendizadoDAO : AbstractORC_OrientacaoCurricularNivelAprendizadoDAO
    {
        /// <summary>
        /// Busca os níveis de aprendizado da orientação curricular.
        /// </summary>
        /// <param name="ocr_idSuperior">Id da orientação curricular superior</param>
        /// <returns></returns>
        public List<ORC_OrientacaoCurricularNivelAprendizado> SelectNivelAprendizadoByOcrIdSuperior(long ocr_idSuperior)
        {
            List<ORC_OrientacaoCurricularNivelAprendizado> lt = new List<ORC_OrientacaoCurricularNivelAprendizado>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricularNivelAprendizado_SelectBy_OcrIdSuperior", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_idSuperior";
                Param.Size = 8;
                Param.Value = ocr_idSuperior;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    ORC_OrientacaoCurricularNivelAprendizado entity = new ORC_OrientacaoCurricularNivelAprendizado();
                    lt.Add(DataRowToEntity(dr, entity));
                }

                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca os níveis de aprendizado da orientação curricular.
        /// </summary>
        /// <param name="ocr_id">Id da orientação curricular</param>
        /// <param name="nap_id">Id do nível de aprendizado</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        public DataTable SelectNivelAprendizadoByOcrId(long ocr_id, int nap_id, TalkDBTransaction banco)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricularNivelAprendizado_Select_NapSiglaBy_OcrId", banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                if (ocr_id > 0)
                    Param.Value = ocr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nap_id";
                Param.Size = 4;
                if (nap_id > 0)
                    Param.Value = nap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca os níveis de aprendizado da orientações curriculares.
        /// </summary>
        /// <param name="ocr_id">Ids da orientação curricular</param>
        /// <param name="nap_id">Id do nível de aprendizado</param>
        /// <returns></returns>
        public DataTable SelecionaPorOrientacaoNivelAprendizado(string ocr_ids, int nap_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricularNivelAprendizado_SelecionaPorOrientacaoNivelAprendizado", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@ocr_ids";
                Param.Value = ocr_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@nap_id";
                Param.Size = 4;
                if (nap_id > 0)
                    Param.Value = nap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna os registros ativos ou com data de alteração posterios
        /// a ultima sincronizacao.
        /// </summary>
        /// <param name="syncDate">Data da última sincronização</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <returns></returns>
        public DataTable SelectPorDataSincronizacao(DateTime syncDate, Int64 tur_id, int cur_id, int crr_id, int crp_id, int cal_id, int tds_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ORC_OrientacaoCurricularNivelAprendizadoPorDataSincronizacao", _Banco);
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

                #endregion parametros

                qs.Execute();
                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca todos os níveis de aprendizado da orientação curricular, com status 1 e 3(excluído).
        /// </summary>
        /// <param name="ocr_id">Id da orientação curricular</param>
        /// <returns></returns>
        public List<ORC_OrientacaoCurricularNivelAprendizado> SelectTodosNivelAprendizadoByOcrId(long ocr_id)
        {
            List<ORC_OrientacaoCurricularNivelAprendizado> lt = new List<ORC_OrientacaoCurricularNivelAprendizado>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("STP_ORC_OrientacaoCurricularNivelAprendizado_SELECTBY_ocr_id", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                if (ocr_id > 0)
                    Param.Value = ocr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    ORC_OrientacaoCurricularNivelAprendizado entity = new ORC_OrientacaoCurricularNivelAprendizado();
                    lt.Add(DataRowToEntity(dr, entity));
                }

                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ORC_OrientacaoCurricularNivelAprendizado entity)
        {
            if (entity != null & qs != null)
            {
                if (qs.Return.Rows.Count > 0)
                {
                    entity.ocn_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                    return (entity.ocn_id > 0);
                }
            }

            return false;
        }
    }
}