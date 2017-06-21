/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_RelatorioAtendimentoDAO : Abstract_CLS_RelatorioAtendimentoDAO
	{
        #region Métodos de consulta

        /// <summary>
        /// Carrega os tipos de relatório verificando a permissão do usuário.
        /// </summary>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        public List<byte> SelecionaTiposPorPermissao(Guid usu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioAtendimento_SelecionaTiposPorPermissao", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@usu_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Select().Select(p => Convert.ToByte(p["rea_tipo"])).ToList() : new List<byte>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Carrega a estrutura do relatório
        /// </summary>
        /// <param name="rea_id"></param>
        /// <param name="usu_id"></param>
        /// <returns></returns>
        public DataSet SelecionaRelatorio(int rea_id, Guid usu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_RelatorioAtendimento_SelecionaRelatorio", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@rea_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rea_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@usu_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion 
    }
}