/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ESC_TipoClassificacaoEscolaCargosDAO : Abstract_ESC_TipoClassificacaoEscolaCargosDAO
	{
        /// <summary>
        /// Retorna os cargos vinculados ao tipo de classificação da escola.
        /// </summary>
        /// <param name="tce_id">Tipo de classificação da escola</param>
        public List<ESC_TipoClassificacaoEscolaCargos> SelectTipoClassificacaoEscolaCargosByTipoClassificacaoEscola(int tce_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_TipoClassificacaoEscolaCargos_SelectByTipoClassificacaoEscola", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tce_id";
                Param.Size = 4;
                Param.Value = tce_id;
                qs.Parameters.Add(Param);
                #endregion

                qs.Execute();

                DataTable dt = qs.Return;
                List<ESC_TipoClassificacaoEscolaCargos> list = new List<ESC_TipoClassificacaoEscolaCargos>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in qs.Return.Rows)
                    {
                        ESC_TipoClassificacaoEscolaCargos entity = new ESC_TipoClassificacaoEscolaCargos();
                        list.Add(DataRowToEntity(dr, entity));
                    }

                }

                return list;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
    }
}