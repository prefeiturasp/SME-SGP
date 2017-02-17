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
    public class MTR_TipoMovimentacaoDAO : Abstract_MTR_TipoMovimentacaoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna os tipos de movimentação não excluídos logicamente.
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Todos()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_Select", _Banco);
            
            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna todos os parâmetros de movimentação não excluídos logicamente
        /// </summary>                
        /// <param name="tmo_nome">Nome do parâmetro de movimentação</param>        
        /// <param name="tmv_idSaida">ID do tipo de movimentação de saída</param>
        /// <param name="tmv_idEntrada">ID do tipo de movimentação de entrada</param>
        /// <param name="tmo_tipoMovimento">Tipo do parâmetro de movimentação</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param> 
        /// <param name="currentPage">Página atual do grid</param>
        /// <param name="pageSize">Total de registros por página do grid</param>
        /// <param name="totalRecords">Total de registros retornado na busca</param>   
        public DataTable SelectBy_Pesquisa
        (
            string tmo_nome
            , int tmv_idSaida
            , int tmv_idEntrada
            , byte tmo_tipoMovimento
            , Guid ent_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_SelectBy_Pesquisa", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tmo_nome";
                Param.Size = 100;
                if (!string.IsNullOrEmpty(tmo_nome))
                    Param.Value = tmo_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tmo_tipoMovimento";
                Param.Size = 1;
                if (tmo_tipoMovimento > 0)
                    Param.Value = tmo_tipoMovimento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tmv_idSaida";
                Param.Size = 4;
                if (tmv_idSaida > 0)
                    Param.Value = tmv_idSaida;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tmv_idEntrada";
                Param.Size = 4;
                if (tmv_idEntrada > 0)
                    Param.Value = tmv_idEntrada;
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
        /// Retorna os parâmetros de movimentação não excluídos logicamente por categoria
        /// </summary>                
        /// <param name="inclusao">True para exibir tipo de movimentação de inclusão</param>
        /// <param name="realocacao">True para exibir tipo de movimentação de reolocação</param>
        /// <param name="exclusao">True para exibir tipo de movimentação de exclusão</param>
        /// <param name="outros">True para exibir outros tipos de movimentação</param> 
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_Categoria
        (
            bool inclusao
            , bool realocacao
            , bool exclusao
            , bool outros
            , bool renovacao
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_SelectBy_Categoria", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@inclusao";
                Param.Size = 1;
                Param.Value = inclusao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@realocacao";
                Param.Size = 1;
                Param.Value = realocacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@exclusao";
                Param.Size = 1;
                Param.Value = exclusao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@outros";
                Param.Size = 1;
                Param.Value = outros;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@renovacao";
                Param.Size = 1;
                Param.Value = renovacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

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

        /// <summary>
        /// Seleciona o tipo de movimentação, filtrado pelo tipo de movimento e nome do tipo
        /// de movimentação, e preenche a entidade.
        /// </summary>                
        /// <param name="inclusao">True para exibir tipo de movimentação de inclusão</param>
        /// <param name="realocacao">True para exibir tipo de movimentação de reolocação</param>
        /// <param name="exclusao">True para exibir tipo de movimentação de exclusão</param>
        /// <param name="outros">True para exibir outros tipos de movimentação</param> 
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="entity">Entidade MTR_TipoMovimentacao com o nome do tipo de movimentação.</param>
        public bool SelectBy_Categoria_Nome
        (
            bool inclusao
            , bool realocacao
            , bool exclusao
            , bool outros
            , Guid ent_id
            , MTR_TipoMovimentacao entity
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_SelectBy_Categoria_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@inclusao";
                Param.Size = 1;
                Param.Value = inclusao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@realocacao";
                Param.Size = 1;
                Param.Value = realocacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@exclusao";
                Param.Size = 1;
                Param.Value = exclusao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@outros";
                Param.Size = 1;
                Param.Value = outros;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tmo_nome";
                Param.Size = 100;
                Param.Value = entity.tmo_nome;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count == 1)
                {
                    entity = DataRowToEntity(qs.Return.Rows[0], entity, false);
                    return true;
                }
                return false;
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
        /// Retorna a chave de identificação do tipo de movimentação.
        /// </summary>
        /// <param name="tmo_tipoMovimento">Tipo de movimentação</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <returns>tmo_id</returns>               
        public int Retorna_TipoMovimentacaoId
        (
            byte tmo_tipoMovimento
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_Retorna_TipoMovimentacaoId", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tmo_tipoMovimento";
                Param.Size = 1;
                Param.Value = tmo_tipoMovimento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return Convert.ToInt32(qs.Return.Rows[0]["tmo_id"].ToString());
                else
                    return 0;
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
        /// Retorna um dataTable informando se está no período de início e fim da movimentação informada.
        /// Considera os parâmetros cadastrados no ano letivo informado, para o curso.
        /// </summary>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="tmo_id">ID do tipo de movimentação</param>
        /// <param name="mom_ano">Ano letivo</param>
        /// <param name="dataComparar">Data para comparação do momento</param>
        /// <returns></returns>
        public DataTable SelectBy_PeriodoValido_Curso
        (
            int cur_id
            , int crr_id
            , int tmo_id
            , int mom_ano
            , DateTime dataComparar
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_SelectBy_PeriodoValido_Curso", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cur_id";
            Param.Size = 4;
            Param.Value = cur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crr_id";
            Param.Size = 4;
            Param.Value = crr_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tmo_id";
            Param.Size = 4;
            Param.Value = tmo_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_ano";
            Param.Size = 4;
            Param.Value = mom_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@dataComparar";
            Param.Size = 8;
            Param.Value = dataComparar;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna o TipoMovimento da última movimentação do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns>tmo_tipoMovimento</returns>               
        public DataTable RetornaTipoMomentoUltimaMovimentacaoAluno(Int64 alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_SelecionaUltimaMovimentacaoAluno", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;

                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna todos os parâmetros de movimentação não excluídos logicamente por entidade
        /// </summary>                
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelecionaTipoMovimentacaoPorEntidade
        (
            Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacao_SelectBy_Entidade", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

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

        #endregion
        
    }
}