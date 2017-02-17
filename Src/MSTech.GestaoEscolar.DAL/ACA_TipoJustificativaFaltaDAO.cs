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
	public class ACA_TipoJustificativaFaltaDAO : Abstract_ACA_TipoJustificativaFaltaDAO
    {
        #region Métodos

        /// <summary>
	    /// Retorna todos os Tipo de Justificativa de Falta não excluídos logicamente
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
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoJustificativaFalta_SelectBy_Pesquisa", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tjf_situacao";
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
        /// Verifica se já existe um tipo de justificativa de falta cadastrado com o mesmo nome
        /// </summary>
        /// <param name="tjf_id">ID do tipo de justificativa falta</param>
        /// <param name="tjf_nome">Nome do tipo de justificativa falta</param>        
        public bool SelectBy_Nome
        (
            int tjf_id
            , string tjf_nome
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_TipoJustificativaFalta_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tjf_id";
                Param.Size = 4;
                if (tjf_id > 0)
                    Param.Value = tjf_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tjf_nome";
                Param.Size = 100;                
                Param.Value = tjf_nome;
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

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Override do método ParamInserir
        /// </summary>        
        protected override void ParamInserir(QuerySelectStoredProcedure qs,  ACA_TipoJustificativaFalta entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tjf_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tjf_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método ParamAlterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoJustificativaFalta entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tjf_dataCriacao");
            qs.Parameters["@tjf_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método Alterar
        /// </summary>     
        protected override bool Alterar(ACA_TipoJustificativaFalta entity)
        {
            __STP_UPDATE = "NEW_ACA_TipoJustificativaFalta_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Override do método Delete
        /// </summary>
        public override bool Delete(ACA_TipoJustificativaFalta entity)
        {
            __STP_DELETE = "NEW_ACA_TipoJustificativaFalta_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_TipoJustificativaFalta entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_TipoJustificativaFalta entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_TipoJustificativaFalta entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_TipoJustificativaFalta entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_TipoJustificativaFalta entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_TipoJustificativaFalta entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_TipoJustificativaFalta entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_TipoJustificativaFalta entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_TipoJustificativaFalta entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_TipoJustificativaFalta> Select()
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
        //public override IList<ACA_TipoJustificativaFalta> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_TipoJustificativaFalta entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_TipoJustificativaFalta DataRowToEntity(DataRow dr, ACA_TipoJustificativaFalta entity)
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
        //public override ACA_TipoJustificativaFalta DataRowToEntity(DataRow dr, ACA_TipoJustificativaFalta entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}