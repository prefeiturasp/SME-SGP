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
	public class MTR_ParametroTipoMovimentacaoCurriculoPeriodoDAO : Abstract_MTR_ParametroTipoMovimentacaoCurriculoPeriodoDAO
    {
        /// <summary>
        /// Seleciona o valor de um parametro
        /// filtrados por pmp_chave
        /// </summary>
        /// <param name="tmo_id">ID do parâmetro de movimentação</param>
        /// <param name="tmp_id">ID do currículo período</param>
        /// <param name="pmp_chave">Campo pmp_chave da tabela MTR_ParametroTipoMovimentacaoCurriculoPeriodo do bd</param>                
        /// <returns>pmp_valor</returns>
        public string SelectBy_pmp_chave
        (
            int tmo_id
            , int tmp_id
            , string pmp_chave
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ParametroTipoMovimentacaoCurriculoPeriodo_SelectBy_pmp_chave", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tmo_id";
                Param.Size = 4;
                Param.Value = tmo_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tmp_id";
                Param.Size = 4;
                Param.Value = tmp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@pmp_chave";
                Param.Size = 50;                
                Param.Value = pmp_chave;                
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return.Rows[0]["pmp_valor"].ToString() : string.Empty;
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
        /// Retorna os parâmetros cadastrados para o curso período do parâmetro de movimentação
        /// </summary>
        /// <param name="tmo_id">ID do parâmetro de movimentação</param>
        /// <param name="tmp_id">ID do currículo período</param>        
        /// <returns>true/false</returns>
        public DataTable SelectBy_tmp_id
        (
            int tmo_id
            , int tmp_id            
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_ParametroTipoMovimentacaoCurriculoPeriodo_SelectBy_tmp_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tmo_id";
                Param.Size = 4;
                Param.Value = tmo_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tmp_id";
                Param.Size = 4;
                Param.Value = tmp_id;
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
        /// Override do método inserir
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@pmp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@pmp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@pmp_dataCriacao");
            qs.Parameters["@pmp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade MTR_ParametroTipoMovimentacaoCurriculoPeriodo</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        {
            __STP_UPDATE = "NEW_MTR_ParametroTipoMovimentacaoCurriculoPeriodo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tmo_id";
            Param.Size = 4;
            Param.Value = entity.tmo_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tmp_id";
            Param.Size = 4;
            Param.Value = entity.tmp_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@pmp_id";
            Param.Size = 4;
            Param.Value = entity.pmp_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@pmp_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@pmp_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade MTR_ParametroTipoMovimentacaoCurriculoPeriodo</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        {
            __STP_DELETE = "NEW_MTR_ParametroTipoMovimentacaoCurriculoPeriodo_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<MTR_ParametroTipoMovimentacaoCurriculoPeriodo> Select()
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
        //public override IList<MTR_ParametroTipoMovimentacaoCurriculoPeriodo> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override MTR_ParametroTipoMovimentacaoCurriculoPeriodo DataRowToEntity(DataRow dr, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity)
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
        //public override MTR_ParametroTipoMovimentacaoCurriculoPeriodo DataRowToEntity(DataRow dr, MTR_ParametroTipoMovimentacaoCurriculoPeriodo entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}