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
    using System.Linq;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_TipoJustificativaExclusaoAulasDAO : Abstract_ACA_TipoJustificativaExclusaoAulasDAO
	{
        /// <summary>
        /// Retorna todos os tipos de justificativas para exclusão de aulas não excluídos logicamente
        /// </summary>                
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="situacao"></param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        public DataTable SelectBy_Pesquisa
        (
             bool paginado
            , int currentPage
            , int pageSize
            , int situacao
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoJustificativaExclusaoAulas_SelectBy_Pesquisa", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tje_situacao";
                Param.Size = 1;
                if (situacao > 0)
                    Param.Value = situacao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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

        /// <summary>
        /// Verifica se já existe um tipo de justificativa para exclusão de aulas cadastrado com o mesmo nome
        /// </summary>
        /// <param name="tje_id">ID do tipo de justificativa para exclusão de aulas</param>
        /// <param name="tje_nome">Nome do tipo de justificativa para exclusão de aulas</param>        
        public bool SelectBy_Nome
        (
            int tje_id
            , string tje_nome
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoJustificativaExclusaoAulas_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tje_id";
                Param.Size = 4;
                if (tje_id > 0)
                    Param.Value = tje_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tje_nome";
                Param.Size = 100;
                Param.Value = tje_nome;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);

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
        /// Retorna os tipos de justificativas para exclusão de aulas ativos.
        /// </summary>
        /// <returns></returns>
        public List<ACA_TipoJustificativaExclusaoAulas> SelectAtivos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoJustificativaExclusaoAulas_SelectBy_PesquisaAtivos", _Banco);

            try
            {
                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new ACA_TipoJustificativaExclusaoAulas())).ToList() :
                    new List<ACA_TipoJustificativaExclusaoAulas>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Métodos Sobrescritos

        /// <summary>
        /// Override do método ParamInserir
        /// </summary>        
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoJustificativaExclusaoAulas entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tje_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tje_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método ParamAlterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoJustificativaExclusaoAulas entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tje_dataCriacao");
            qs.Parameters["@tje_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método Alterar
        /// </summary>     
        protected override bool Alterar(ACA_TipoJustificativaExclusaoAulas entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoJustificativaExclusaoAulas_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Override do método Delete
        /// </summary>
        public override bool Delete(ACA_TipoJustificativaExclusaoAulas entity)
        {
            __STP_DELETE = "NEW_ACA_TipoJustificativaExclusaoAulas_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}