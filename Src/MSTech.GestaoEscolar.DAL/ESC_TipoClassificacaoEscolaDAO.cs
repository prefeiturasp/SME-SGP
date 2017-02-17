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
    /// Classe de acesso ao banco de dados da tabela ESC_TipoClassificacaoEscola
	/// </summary>
	public class ESC_TipoClassificacaoEscolaDAO : Abstract_ESC_TipoClassificacaoEscolaDAO
	{
        #region Métodos

        /// <summary>
        /// Tipos de classificação de escola ativos.
        /// </summary>                
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        /// <returns>Retorna DataTable com os tipos de classificação de escola</returns>
        public DataTable SelectBy_Pesquisa
            (
                 bool paginado
                , int currentPage
                , int pageSize
                , out int totalRecords
            )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_TipoClassificacaoEscola_SelectBy_Pesquisa", _Banco);
            try
            {
                if (paginado)
                {
                    if (pageSize == 0)
                    {
                        pageSize = 1;
                    }

                    totalRecords = qs.Execute(currentPage / pageSize, pageSize);
                }
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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