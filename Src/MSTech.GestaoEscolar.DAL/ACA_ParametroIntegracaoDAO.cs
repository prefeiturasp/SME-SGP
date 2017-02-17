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
    public class ACA_ParametroIntegracaoDAO : Abstract_ACA_ParametroIntegracaoDAO
    {
        #region Propriedades

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        #endregion

        #region Métodos criados

        /// <summary>
        /// Verifica se existe um parâmetro de integração que possua a mesma chave.
        /// </summary>
        /// <param name="entity">Entidade do parâmetro de integração.</param>
        /// <returns>Verdadeiro se já existe um parâmetro de integração com a mesma chave cadastrado.</returns>
        public bool SelecionaPorChave(ACA_ParametroIntegracao entity)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroIntegracao_SelectBy_pri_chave", this._Banco);
            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@pri_id";
                Param.Size = 4;
                if (entity.pri_id != 0)
                    Param.Value = entity.pri_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@pri_chave";
                Param.Size = 100;
                Param.Value = entity.pri_chave;
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
        /// Retorna todos os parâmetros de integração ativos.
        /// </summary>
        /// <returns>Lista de entidades ACA_ParametroIntegracao</returns>
        public List<ACA_ParametroIntegracao> SelecionaTodosAtivos()
        {
            List<ACA_ParametroIntegracao> lt = new List<ACA_ParametroIntegracao>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_ParametroIntegracao_SelectAll", this._Banco);

            qs.Execute();

            foreach (DataRow dr in qs.Return.Rows)
            {
                ACA_ParametroIntegracao entity = new ACA_ParametroIntegracao();
                lt.Add(DataRowToEntity(dr, entity));
            }
            
            return lt;
        }

        #endregion

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ParametroIntegracao entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@pri_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@pri_dataAlteracao"].Value = DateTime.Now;
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ParametroIntegracao entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@pri_dataCriacao");
            qs.Parameters["@pri_dataAlteracao"].Value = DateTime.Now;
        }

        protected override bool Alterar(ACA_ParametroIntegracao entity)
        {
            __STP_UPDATE = "NEW_ACA_ParametroIntegracao_UPDATE";
            return base.Alterar(entity);
        }

        public override bool Delete(ACA_ParametroIntegracao entity)
        {
            __STP_DELETE = "NEW_ACA_ParametroIntegracao_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentado

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(ACA_ParametroIntegracao entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_ParametroIntegracao entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_ParametroIntegracao entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(ACA_ParametroIntegracao entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, ACA_ParametroIntegracao entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_ParametroIntegracao entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, ACA_ParametroIntegracao entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_ParametroIntegracao entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_ParametroIntegracao entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_ParametroIntegracao> Select()
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
        //public override IList<ACA_ParametroIntegracao> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_ParametroIntegracao entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_ParametroIntegracao DataRowToEntity(DataRow dr, ACA_ParametroIntegracao entity)
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
        //public override ACA_ParametroIntegracao DataRowToEntity(DataRow dr, ACA_ParametroIntegracao entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}