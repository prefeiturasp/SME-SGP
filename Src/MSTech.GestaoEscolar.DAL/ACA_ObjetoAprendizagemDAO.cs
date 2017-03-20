/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using Entities;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using System;
    using System.Data;

    /// <summary>
    /// Description: .
    /// </summary>
    public class ACA_ObjetoAprendizagemDAO : Abstract_ACA_ObjetoAprendizagemDAO
	{
        /// <summary>
        /// Retorna os parâmetros ativos e vigentes.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectBy_TipoDisciplina(int tds_id, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ObjetoAprendizagem_SELECT_ByTipoDisciplina", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tds_id";
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

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
        /// Retorna os parâmetros ativos e vigentes.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectListaBy_TurmaDisciplina(long tud_id, int cal_id, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ObjetoAprendizagemCollection_SELECT_By_tud_id_cal_id", _Banco);
            try
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 4;
                Param.Value = tud_id;                
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                qs.Execute();

                totalRecords = qs.Return.Rows.Count;

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Métodos Sobrescritos

        /// <summary>
        /// Override do nome da ConnectionString.
        /// </summary>
        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        {
            entity.oap_dataCriacao = DateTime.Now;
            entity.oap_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        {
            entity.oap_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@oap_dataCriacao");

        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade ACA_AreaConhecimento</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(ACA_ObjetoAprendizagem entity)
        {
            __STP_UPDATE = "NEW_ACA_ObjetoAprendizagem_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@oap_id";
            Param.Size = 4;
            Param.Value = entity.oap_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@oap_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@oap_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade ACA_AreaConhecimento</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        public override bool Delete(ACA_ObjetoAprendizagem entity)
        {
            __STP_DELETE = "NEW_ACA_ObjetoAprendizagem_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion
        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool Alterar(ACA_ObjetoAprendizagem entity)
        // {
        //    return base.Alterar(entity);
        // }
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool Inserir(ACA_ObjetoAprendizagem entity)
        // {
        //    return base.Inserir(entity);
        // }
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Carregar(ACA_ObjetoAprendizagem entity)
        // {
        //    return base.Carregar(entity);
        // }
        ///// <summary>
        ///// Exclui um registro do banco.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Delete(ACA_ObjetoAprendizagem entity)
        // {
        //    return base.Delete(entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        // {
        //    base.ParamAlterar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        // {
        //    base.ParamCarregar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        // {
        //    base.ParamDeletar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        // {
        //    base.ParamInserir(qs, entity);
        // }
        ///// <summary>
        ///// Salva o registro no banco de dados.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Salvar(ACA_ObjetoAprendizagem entity)
        // {
        //    return base.Salvar(entity);
        // }
        ///// <summary>
        ///// Realiza o select da tabela.
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela.</returns>
        // public override IList<ACA_ObjetoAprendizagem> Select()
        // {
        //    return base.Select();
        // }
        ///// <summary>
        ///// Realiza o select da tabela com paginacao.
        ///// </summary>
        ///// <param name="currentPage">Pagina atual.</param>
        ///// <param name="pageSize">Tamanho da pagina.</param>
        ///// <param name="totalRecord">Total de registros na tabela original.</param>
        ///// <returns>Lista com todos os registros da p�gina.</returns>
        // public override IList<ACA_ObjetoAprendizagem> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        // {
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        // }
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade. 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_ObjetoAprendizagem entity)
        // {
        //    return base.ReceberAutoIncremento(qs, entity);
        // }
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade.
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido.</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados.</param>
        ///// <returns>Entidade preenchida.</returns>
        // public override ACA_ObjetoAprendizagem DataRowToEntity(DataRow dr, ACA_ObjetoAprendizagem entity)
        // {
        //    return base.DataRowToEntity(dr, entity);
        // }
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade.
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido.</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados.</param>
        ///// <param name="limparEntity">Indica se a entidade deve ser limpada antes da transferencia.</param>
        ///// <returns>Entidade preenchida.</returns>
        // public override ACA_ObjetoAprendizagem DataRowToEntity(DataRow dr, ACA_ObjetoAprendizagem entity, bool limparEntity)
        // {
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        // }
    }
}