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
	public class MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO : Abstract_MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodoDAO
    {
        #region Métodos

        /// <summary>
        /// Override do método LOAD.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Carregar(MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        {
            __STP_LOAD = "NEW_MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo_LOAD";
            return base.Carregar(entity);
        }

        public DataTable GetSelectByTipoMovimentacao(int tmo_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("STP_MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo_SELECTBY_tmo_id", _Banco);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tmo_id";
            Param.Size = 4;
            Param.Value = tmo_id;
            qs.Parameters.Add(Param);

            qs.Execute();

            return qs.Return;
        }
        
        /// <summary>
        /// Retorna os periodos de um calendario para o tipo de movimentaçao do curso momento informado
        /// utilizando o objeto para conexao com o banco TalkDBTransaction.
        /// </summary>
        /// <param name="tmo_id">campo id do tipo movimentaçao</param>
        /// <param name="cur_id">campo id do curso</param>
        /// <param name="crr_id">campo id do curriculo</param>
        /// <param name="tcm_id">campo id do tipo curso momento</param>
        public DataTable GetSelectByTipoMovimentacaoCursoMomento(int tmo_id, int cur_id, int crr_id, int tcm_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo_SELECTBY_TipoMovimentacaoCursoMomento", _Banco);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tmo_id";
            Param.Size = 4;
            Param.Value = tmo_id;
            qs.Parameters.Add(Param);

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
            Param.ParameterName = "@tcm_id";
            Param.Size = 4;
            Param.Value = tcm_id;
            qs.Parameters.Add(Param);

            qs.Execute();

            return qs.Return;
        }


        #endregion

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

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> Select()
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
        //public override IList<MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo DataRowToEntity(DataRow dr, MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity)
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
        //public override MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo DataRowToEntity(DataRow dr, MTR_TipoMovimentacaoCursoMomentoCalendarioPeriodo entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}