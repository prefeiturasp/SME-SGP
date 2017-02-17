/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class CFG_ConfiguracaoMigracaoRioDAO : Abstract_CFG_ConfiguracaoMigracaoRioDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna um List de entidades CFG_Configuracao contendo
        /// toas as configurações ativas.
        /// </summary>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <param name="totalRecords">Total de registros retornado</param>
        /// <returns>List de entidades CFG_Configuracao</returns>
        public List<CFG_ConfiguracaoMigracaoRio> Select
            (
                bool paginado
                , int currentPage
                , int pageSize
                , out int totalRecords
            )
        {
            List<CFG_ConfiguracaoMigracaoRio> lt = new List<CFG_ConfiguracaoMigracaoRio>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_Configuracao_SELECT", _Banco);

            if (paginado)
            {
                if (pageSize == 0) pageSize = 1;
                totalRecords = qs.Execute(currentPage / pageSize, pageSize);
            }
            else
            {
                qs.Execute();
                totalRecords = qs.Return.Rows.Count;
            }

            foreach (DataRow dr in qs.Return.Rows)
            {
                CFG_ConfiguracaoMigracaoRio entity = new CFG_ConfiguracaoMigracaoRio();
                lt.Add(DataRowToEntity(dr, entity));
            }
            return lt;
        }

        /// <summary>
        /// Verifica se existe uma configuração que possua a mesma chave.
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <returns></returns>
        public bool SelectBy_cfg_chave(CFG_ConfiguracaoMigracaoRio entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CFG_Configuracao_SelectBy_cfg_chave", _Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@cfg_id";
                Param.Size = 16;
                if (entity.cfg_id != Guid.Empty)
                    Param.Value = entity.cfg_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cfg_chave";
                Param.Size = 100;
                Param.Value = entity.cfg_chave;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return true;

                return false;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Configura os parametros do metodo de Inserir
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ConfiguracaoMigracaoRio entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters.RemoveAt("@cfg_id");
            qs.Parameters["@cfg_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@cfg_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ConfiguracaoMigracaoRio entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@cfg_dataCriacao");
            qs.Parameters["@cfg_dataAlteracao"].Value = DateTime.Now;
        }

        protected override bool Inserir(CFG_ConfiguracaoMigracaoRio entity)
        {
            __STP_INSERT = "STP_CFG_Configuracao_INSERT";
            return base.Inserir(entity);
        }

        public override bool Carregar(CFG_ConfiguracaoMigracaoRio entity)
        {
            __STP_LOAD = "STP_CFG_Configuracao_LOAD";
            return base.Carregar(entity);
        }

        protected override bool Alterar(CFG_ConfiguracaoMigracaoRio entity)
        {
            __STP_UPDATE = "NEW_CFG_Configuracao_UPDATE";
            return base.Alterar(entity);
        }

        public override bool Delete(CFG_ConfiguracaoMigracaoRio entity)
        {
            __STP_DELETE = "NEW_CFG_Configuracao_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Métodos comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<CFG_ConfiguracaoMigracaoRio> Select()
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
        //public override IList<CFG_ConfiguracaoMigracaoRio> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CFG_ConfiguracaoMigracaoRio entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override CFG_ConfiguracaoMigracaoRio DataRowToEntity(DataRow dr, CFG_ConfiguracaoMigracaoRio entity)
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
        //public override CFG_ConfiguracaoMigracaoRio DataRowToEntity(DataRow dr, CFG_ConfiguracaoMigracaoRio entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}