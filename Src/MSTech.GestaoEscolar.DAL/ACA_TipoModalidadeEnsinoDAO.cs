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
            DataTable dt = new DataTable();

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

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        ///// protected override bool Alterar(ACA_TipoModalidadeEnsino entity)
        ///// {
        /////    this.__STP_UPDATE = "NEW_ACA_TipoModalidadeEnsino_UPDATE";
        /////    return base.Alterar(entity);
        ///// }
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        ///// protected override bool Inserir(ACA_TipoModalidadeEnsino entity)
        ///// {
        /////    this.__STP_INSERT = "NEW_ACA_TipoModalidadeEnsino_INSERT";
        /////    return base.Inserir(entity);
        ///// }
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        ///// public override bool Carregar(ACA_TipoModalidadeEnsino entity)
        ///// {
        /////    return base.Carregar(entity);
        ///// }
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_TipoModalidadeEnsino entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>=
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_TipoModalidadeEnsino entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_TipoModalidadeEnsino> Select()
        //{
        //    return base.Select();
        //}
        ///// <summary>
        ///// Realiza o select da tabela com paginacao
        ///// </summary>
        ///// <param name="currentPage">Pagina atual</param>
        ///// <param name="pageSize">Tamanho da pagina</param>
        ///// <param name="totalRecord">Total de registros na tabela original</param>
        ///// <returns>Lista com todos os registros da p�gina</returns>
        //public override IList<ACA_TipoModalidadeEnsino> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_TipoModalidadeEnsino entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_TipoModalidadeEnsino DataRowToEntity(DataRow dr, ACA_TipoModalidadeEnsino entity)
        //{
        //    return base.DataRowToEntity(dr, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <param name="limparEntity">Indica se a entidade deve ser limpada antes da transferencia</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_TipoModalidadeEnsino DataRowToEntity(DataRow dr, ACA_TipoModalidadeEnsino entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}


    }
}