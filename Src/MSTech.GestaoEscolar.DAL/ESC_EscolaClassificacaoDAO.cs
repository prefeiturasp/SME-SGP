/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
	
	/// <summary>
	/// 
	/// </summary>
	public class ESC_EscolaClassificacaoDAO : Abstract_ESC_EscolaClassificacaoDAO
    {
        #region Métodos de consulta

        /// <summary>
        /// Retorna os tipos de classificação associados a escola por vigência.
        /// </summary>        
        /// <param name="esc_id">ID da escola</param>
        /// <param name="ecv_id">ID da vigência</param>
        /// <returns>DataTable de tipos de classificação</returns>
        public DataTable SelectTipoClassificacaoAssociado(int esc_id, long ecv_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_EscolaClassificacao_SelectTipoClassificacaoAssociado", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ecv_id";
                Param.Size = 8;
                if (ecv_id > 0)
                    Param.Value = ecv_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna os tipos de classificação não associados a escola
        /// </summary>        
        /// <param name="esc_id">ID da escola</param>
        /// <returns>DataTable de tipos de classificação</returns>
        public DataTable SelectTipoClassificacaoNaoAssociado(int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_EscolaClassificacao_SelectTipoClassificacaoNaoAssociado", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
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
        /// Seleciona histórico de classfificações da escola.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <returns>Lista com classificações</returns>
        public List<ESC_EscolaClassificacao> SelecionaHistoricoEscola(int esc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ESC_EscolaClassificacaoVigencia_HistoricoEscola", _Banco);
            List<ESC_EscolaClassificacao> lst = new List<ESC_EscolaClassificacao>();

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                foreach (DataRow dr in qs.Return.Rows)
                {
                    ESC_EscolaClassificacao entity = new ESC_EscolaClassificacao();
                    entity = DataRowToEntity(dr, entity);
                    lst.Add(entity);
                }

                return lst;
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
        /// 	Verifica se existe o cargo na classificação da escola.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="crg_id">Id do cargo</param>
        /// <returns></returns>
        public bool VerificaExisteCargoClassificacao(int esc_id, int crg_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_ESC_EscolaClassificacao_VerificaExisteCargoClassificacao", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@esc_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@crg_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = crg_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Sobrecarga do método carregar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Carregar(ESC_EscolaClassificacao entity)
        {
            __STP_LOAD = "NEW_ESC_EscolaClassificacao_LOAD";
            return base.Carregar(entity);
        }

        #endregion

        #region Métodos Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ESC_EscolaClassificacao entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ESC_EscolaClassificacao entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ESC_EscolaClassificacao entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ESC_EscolaClassificacao entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ESC_EscolaClassificacao entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ESC_EscolaClassificacao entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ESC_EscolaClassificacao entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ESC_EscolaClassificacao entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ESC_EscolaClassificacao entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ESC_EscolaClassificacao> Select()
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
        //public override IList<ESC_EscolaClassificacao> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ESC_EscolaClassificacao entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ESC_EscolaClassificacao DataRowToEntity(DataRow dr, ESC_EscolaClassificacao entity)
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
        //public override ESC_EscolaClassificacao DataRowToEntity(DataRow dr, ESC_EscolaClassificacao entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
                
    }
}