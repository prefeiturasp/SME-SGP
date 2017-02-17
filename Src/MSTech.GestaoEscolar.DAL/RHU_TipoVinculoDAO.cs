using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    public class RHU_TipoVinculoDAO : Abstract_RHU_TipoVinculoDAO
    {
        /// <summary>
        /// Retorna todos os tipos de vínculo não excluídos logicamente
        /// </summary>
        /// <param name="tvi_nome">Nome do tipo de vínculo</param> 
        /// <param name="tvi_descricao">Descrição do tipo de vínculo</param> 
        /// <param name="ent_id">Entidade do usuário logado</param> 
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        public DataTable SelectBy_Pesquisa
        (
            string tvi_nome
            , string tvi_descricao
            , Guid ent_id
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_RHU_TipoVinculo_SelectBy_Pesquisa",
                                                                           _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tvi_nome";
                Param.Size = 100;
                if (!String.IsNullOrEmpty(tvi_nome))
                    Param.Value = tvi_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tvi_descricao";
                if (!String.IsNullOrEmpty(tvi_descricao))
                    Param.Value = tvi_descricao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                totalRecords = qs.Return.Rows.Count;

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
