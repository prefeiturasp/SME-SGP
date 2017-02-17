/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System.Data;
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class ORC_OrientacaoCurricularRelacionamentoDAO : AbstractORC_OrientacaoCurricularRelacionamentoDAO
	{
        /// <summary>
        /// Seleciona os relacionamentos pelo id da orientação curricular.
        /// </summary>
        /// <param name="ocr_id">ID da orientação curricular</param>
        /// <returns></returns>
        public List<ORC_OrientacaoCurricularRelacionamento> SelecionaPorOrientacaoCurricular(Int64 ocr_id, Int64 ocr_idSuperiorRelacionada)
        {
            List<ORC_OrientacaoCurricularRelacionamento> retorno = new List<ORC_OrientacaoCurricularRelacionamento>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("STP_ORC_OrientacaoCurricularRelacionamento_SELECTBY_ocr_id", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = ocr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_idSuperiorRelacionada";
                Param.Size = 8;
                if (ocr_idSuperiorRelacionada > 0)
                    Param.Value = ocr_idSuperiorRelacionada;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    foreach (DataRow row in qs.Return.Rows)
                    {
                        retorno.Add(DataRowToEntity(row, new ORC_OrientacaoCurricularRelacionamento(), false));
                    }
                }
                return retorno;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, Entities.ORC_OrientacaoCurricularRelacionamento entity)
        {
            base.ParamInserir(qs, entity);
            qs.Parameters["@ocrr_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@ocrr_dataAlteracao"].Value = DateTime.Now;
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, Entities.ORC_OrientacaoCurricularRelacionamento entity)
        {
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@ocrr_dataCriacao");
            qs.Parameters["@ocrr_dataAlteracao"].Value = DateTime.Now;
        }

        protected override bool Alterar(Entities.ORC_OrientacaoCurricularRelacionamento entity)
        {
            __STP_UPDATE = "NEW_ORC_OrientacaoCurricularRelacionamento_UPDATE";
            return base.Alterar(entity);
        }

        #endregion
	}
}