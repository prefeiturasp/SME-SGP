/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
	/// <summary>
	/// Description: .
	/// </summary>
	public class CFG_RelatorioDocumentoDocenteDAO : AbstractCFG_RelatorioDocumentoDocenteDAO
    {
        #region Verificações

        /// <summary>
        /// Retorna todos os tipos de eventos ativos e relacionados com tipo periodo de calendario.
        /// </summary>
        public List<CFG_RelatorioDocumentoDocente> SelecionaVisao(int vis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoDocente_SelectPorRelatorio_SelectBy_All", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.ParameterName = "@vis_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = vis_id;
                qs.Parameters.Add(Param);


                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new CFG_RelatorioDocumentoDocente())).ToList();
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

        /// <summary>
        ///  Verifica se já existe um documento salvo com determinado relatório.
        /// </summary>
        /// <param name="ent_id">ID da endidade</param>
        /// <param name="rdd_id">ID do documento</param>
        /// <param name="rlt_id">ID do relatório</param>
        /// <returns></returns>
        public bool VerificaRelatorioExistente(Guid ent_id, int rdd_id, int rlt_id, int vis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoDocente_SelectPorRelatorio", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rdd_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (rdd_id > 0)
                    Param.Value = rdd_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rlt_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rlt_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@vis_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = vis_id;
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

        /// <summary>
        ///  Verifica se já existe um documento salvo com determinada ordem.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rdd_id">ID do documento</param>
        /// <param name="rdd_ordem">Ordem do documento</param>
        /// <returns></returns>
        public bool VerificaOrdemExistente(Guid ent_id, int rdd_id, int rdd_ordem, int vis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoDocente_SelectPorOrdem", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rdd_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (rdd_id > 0)
                    Param.Value = rdd_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rdd_ordem";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = rdd_ordem;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@vis_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = vis_id;
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

        /// <summary>
        ///  Verifica se já existe um documento salvo com determinado nome.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rdd_id">ID do documento</param>
        /// <param name="rdd_nomeDocumento">Nome do documento</param>
        /// <returns></returns>
        public bool VerificaNomeExistente(Guid ent_id, int rdd_id, string rdd_nomeDocumento, int vis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoDocente_SelectPorNome", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rdd_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (rdd_id > 0)
                    Param.Value = rdd_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rdd_nomeDocumento";
                Param.DbType = DbType.AnsiString;
                Param.Size = 200;
                Param.Value = rdd_nomeDocumento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@vis_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = vis_id;
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

        /// <summary>
        ///  Verifica se já existe um documento salvo com determinado nome.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="rdd_id">ID do documento</param>
        /// <param name="rdd_nomeDocumento">Nome do documento</param>
        /// <returns></returns>
        public bool VerificaVisaoExistente(Guid ent_id, int rdd_id, Int64 vis_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoDocente_SelectPorVisao", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@rdd_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (rdd_id > 0)
                    Param.Value = rdd_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@vis_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = vis_id;
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

        #endregion

        #region Consultas

        /// <summary>
        /// Consulta os documentos e seus relatórios por entidade
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <returns></returns>
        public DataTable SelecionaPorEntidade(Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_RelatorioDocumentoDocente_SelectBy_Entidade", _Banco);

            try
            {
                #region Paramentros

                Param = qs.NewParameter();
                Param.ParameterName = "@ent_id";
                Param.DbType = DbType.Guid;
                Param.Size = 16;
                Param.Value = ent_id;
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
        
    }
}