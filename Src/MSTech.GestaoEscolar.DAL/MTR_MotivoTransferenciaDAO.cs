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
    /// Classe de acesso ao banco de dados da tabela MTR_MotivoTransferencia
	/// </summary>
    public class MTR_MotivoTransferenciaDAO : Abstract_MTR_MotivoTransferenciaDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna todos os motivos de transferência não excluídos logicamente
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
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MotivoTransferencia_SelectBy_Pesquisa", _Banco);
            try
            {

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

                return qs.Return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion
    }
}