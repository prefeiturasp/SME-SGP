using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Data;
using MSTech.Data.Common;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DCL_RequisicaoDAO : Abstract_DCL_RequisicaoDAO
    {
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Busca requisições que podem ter agendamentos
        /// </summary>
        /// <param name="currentPage">Pagina atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="totalRecord">Total de registros</param>
        /// <returns>DataTable contendo requisições</returns>
        public DataTable SelectBy_permiteAgenda(int currentPage, int pageSize, out int totalRecord)
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_Requisicao_SelectBy_permiteAgenda", this._Banco);
            try
            {
                totalRecord = qs.Execute(currentPage / pageSize, pageSize);

                if (qs.Return.Rows.Count > 0)
                {
                    dt = qs.Return;
                }

                qs.Parameters.Clear();

                return dt;
            }
            catch
            {
                throw;
            }
        }
    }
}
