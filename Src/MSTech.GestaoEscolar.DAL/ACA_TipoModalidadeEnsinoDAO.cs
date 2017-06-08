/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class ACA_TipoModalidadeEnsinoDAO : Abstract_ACA_TipoModalidadeEnsinoDAO
	{
        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// </summary>                
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>  
        public DataTable SelectBy_Pesquisa
        (
          bool paginado
          , int currentPage
          , int pageSize
          , int tme_idSuperior
          , out int totalRecords
        )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoModalidadeEnsino_SelectBy_Pesquisa", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tme_idSuperior";
                Param.Size = 4;
                if (tme_idSuperior > 0)
                {
                    Param.Value = tme_idSuperior;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion Parâmetros

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
        
        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// Vinculados a escola informada.
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade escolar</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="uad_idSuperior">Id da entidade superior</param>
        /// </summary>        
        public DataTable SelecionaTipoModalidadeEnsino_Por_Escola
        (   int esc_id, 
            int uni_id,
            Guid ent_id,
            Guid uad_idSuperior
        )
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoModalidadeEnsino_SelectBy_Escola", _Banco);
            try
            {
                #region PARAMETROS
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
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

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// </summary>                
        public DataTable SelectAtivos(out int totalRecords)
        {
            DataTable dt = new DataTable();

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoModalidadeEnsino_SelectAtivos", _Banco);
            try
            {
                qs.Execute();
                totalRecords = qs.Return.Rows.Count;
                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// </summary>                
        public DataTable SelectFilhosAtivos(out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoModalidadeEnsino_SelectFilhosAtivos", _Banco);
            try
            {
                qs.Execute();
                totalRecords = qs.Return.Rows.Count;
                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna todos os tipos de modalidade de ensino não excluídos logicamente
        /// de acordo com as atribuições do docente.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>               
        public DataTable SelecionaTipoModalidadeEnsinoDocenteEvento(long doc_id, string eventosAbertos)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoModalidadeEnsino_SelecionaTipoModalidadeEnsinoDocenteEvento", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@eventosAbertos";
                Param.Value = eventosAbertos;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

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
    }
}