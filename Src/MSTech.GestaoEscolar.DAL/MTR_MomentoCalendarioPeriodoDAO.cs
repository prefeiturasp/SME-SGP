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
	public class MTR_MomentoCalendarioPeriodoDAO : Abstract_MTR_MomentoCalendarioPeriodoDAO
	{
        /// <summary>
        /// Retorna datatable contendo dados referentes a cada periodo de cada calendário da entidade,
        /// filtrado por ano
        /// </summary>
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="mom_ano">ano base</param>
        /// <returns></returns>
        public DataTable SelectByCalendarioAnoEntidade(int cal_id, int mom_ano, int mom_id, Guid ent_id)
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MomentoCalendarioPeriodo_SelectBy_AnoEntidade", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mom_ano";
                Param.Size = 4;
                Param.Value = mom_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mom_id";
                Param.Size = 4;
                Param.Value = mom_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
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
        /// Retorna os períodos do calendário por curso e ano do momento        
        /// </summary>
        /// </summary>                
        /// <param name="cur_id">ID do curso</param>        
        /// <param name="mom_ano">Ano do tipo de momento</param>
        /// <param name="mom_id">ID do ano do tipo de momento</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        /// <returns>DataTable com os dados</returns>
        public DataTable SelectBy_cur_id_mom_ano_mom_id
        (
            int cur_id            
            , int mom_ano
            , int mom_id
            , Guid ent_id
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_MomentoCalendarioPeriodo_SelectBy_cur_id_mom_ano_mom_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mom_ano";
                Param.Size = 4;
                Param.Value = mom_ano;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mom_id";
                Param.Size = 4;
                Param.Value = mom_id;
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
        protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@mcp_inicio"].DbType = DbType.DateTime;
            qs.Parameters["@mcp_fim"].DbType = DbType.DateTime;
            qs.Parameters["@mcp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@mcp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Override do método alterar
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@mcp_inicio"].DbType = DbType.DateTime;
            qs.Parameters["@mcp_fim"].DbType = DbType.DateTime;
            qs.Parameters.RemoveAt("@mcp_dataCriacao");
            qs.Parameters["@mcp_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity"> Entidade MTR_MomentoCalendarioPeriodo</param>
        /// <returns>true = sucesso | false = fracasso</returns>  
        protected override bool Alterar(MTR_MomentoCalendarioPeriodo entity)
        {
            __STP_UPDATE = "NEW_MTR_MomentoCalendarioPeriodo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_ano";
            Param.Size = 4;
            Param.Value = entity.mom_ano;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@mom_id";
            Param.Size = 4;
            Param.Value = entity.mom_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = entity.cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cap_id";
            Param.Size = 4;
            Param.Value = entity.cap_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@mcp_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@mcp_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity"> Entidade MTR_MomentoCalendarioPeriodo</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(MTR_MomentoCalendarioPeriodo entity)
        {
            __STP_DELETE = "NEW_MTR_MomentoCalendarioPeriodo_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(MTR_MomentoCalendarioPeriodo entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(MTR_MomentoCalendarioPeriodo entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(MTR_MomentoCalendarioPeriodo entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(MTR_MomentoCalendarioPeriodo entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(MTR_MomentoCalendarioPeriodo entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<MTR_MomentoCalendarioPeriodo> Select()
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
        //public override IList<MTR_MomentoCalendarioPeriodo> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_MomentoCalendarioPeriodo entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override MTR_MomentoCalendarioPeriodo DataRowToEntity(DataRow dr, MTR_MomentoCalendarioPeriodo entity)
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
        //public override MTR_MomentoCalendarioPeriodo DataRowToEntity(DataRow dr, MTR_MomentoCalendarioPeriodo entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion 
    }
}