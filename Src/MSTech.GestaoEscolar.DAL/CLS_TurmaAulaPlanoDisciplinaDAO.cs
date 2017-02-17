/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_TurmaAulaPlanoDisciplinaDAO : Abstract_CLS_TurmaAulaPlanoDisciplinaDAO
    {
        /// <summary>
        /// Deleta os relacionamentos do tud_id e tau_id informado
        /// </summary>
        /// <param name="tud_id"></param>
        public bool DeleteBy_aulaDisciplina(long tud_id, int tau_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaPlanoDisciplina_DeleteBy_aulaDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tau_id";
            Param.Size = 4;
            Param.Value = tau_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return (qs.Return.Rows.Count > 0);
        }

        /// <summary>
        /// Deleta os relacionamentos na tabela
        /// </summary>
        /// <param name="tbTurmaAulaPlano"></param>
        /// <returns></returns>
        public bool DeletarAulas(DataTable tbTurmaAulaPlano)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaPlanoDisciplina_DeletarAulas", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaPlano";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaPlanoDisciplina";
                sqlParam.Value = tbTurmaAulaPlano;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona todos os relacionamentos pelo tud_id da regência
        /// </summary>
        /// <param name="tud_id"></param>
        public List<CLS_TurmaAulaPlanoDisciplina> SelectBy_tud_id(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaPlanoDisciplina_SelectBy_tud_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new CLS_TurmaAulaPlanoDisciplina())).ToList() :
                    new List<CLS_TurmaAulaPlanoDisciplina>();
        }

        /// <summary>
        /// Seleciona todos os relacionamentos pelo tud_id e tau_id da regência
        /// </summary>
        /// <param name="tud_id"></param>
        public List<CLS_TurmaAulaPlanoDisciplina> SelectBy_aulaDisciplina(long tud_id, int tau_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaPlanoDisciplina_SelectBy_aulaDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tau_id";
            Param.Size = 4;
            Param.Value = tau_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new CLS_TurmaAulaPlanoDisciplina())).ToList() :
                    new List<CLS_TurmaAulaPlanoDisciplina>();
        }

        /// <summary>
        /// Salva os relacionamentos na tabela
        /// </summary>
        /// <param name="tbTurmaAulaPlano"></param>
        /// <returns></returns>
        public bool SalvarAulas(DataTable tbTurmaAulaPlano)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaPlanoDisciplina_SalvaAulas", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaPlano";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaPlanoDisciplina";
                sqlParam.Value = tbTurmaAulaPlano;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Salva o listão de planos de aula.
        /// </summary>
        /// <param name="dtTurmaAula">Tabela de aulas</param>
        /// <param name="dtTurmaAulaPlanoDeletar">Tabela de planos de aula para deletar</param>
        /// <param name="dtTurmaAulaPlanoSalvar">Tabela de planos de aula para salvar</param>
        /// <returns></returns>
        public bool SalvarEmLote(DataTable dtTurmaAula, DataTable dtTurmaAulaPlanoDeletar, DataTable dtTurmaAulaPlanoSalvar)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaPlanoDisciplina_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetros

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAula";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAula";
                sqlParam.Value = dtTurmaAula;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaPlanoDeletar";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaPlanoDisciplina";
                sqlParam.Value = dtTurmaAulaPlanoDeletar;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaPlanoSalvar";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaPlanoDisciplina";
                sqlParam.Value = dtTurmaAulaPlanoSalvar;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna se o registro existe no banco de dados.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public bool VerificaExisteRegistro(CLS_TurmaAulaPlanoDisciplina entity)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAulaPlanoDisciplina_VerificaExisteRegistro", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                Param.Value = entity.tau_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_idPlano";
                Param.Size = 8;
                Param.Value = entity.tud_idPlano;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

		///// <summary>
        ///// Inseri os valores da classe em um registro ja existente.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool Alterar(CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    return base.Alterar(entity);
        // }
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool Inserir(CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    return base.Inserir(entity);
        // }
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Carregar(CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    return base.Carregar(entity);
        // }
        ///// <summary>
        ///// Exclui um registro do banco.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Delete(CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    return base.Delete(entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    base.ParamAlterar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    base.ParamCarregar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    base.ParamDeletar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    base.ParamInserir(qs, entity);
        // }
        ///// <summary>
        ///// Salva o registro no banco de dados.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Salvar(CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    return base.Salvar(entity);
        // }
        ///// <summary>
        ///// Realiza o select da tabela.
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela.</returns>
        // public override IList<CLS_TurmaAulaPlanoDisciplina> Select()
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
        // public override IList<CLS_TurmaAulaPlanoDisciplina> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        // {
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        // }
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade. 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAulaPlanoDisciplina entity)
        // {
        //    return base.ReceberAutoIncremento(qs, entity);
        // }
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade.
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido.</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados.</param>
        ///// <returns>Entidade preenchida.</returns>
        // public override CLS_TurmaAulaPlanoDisciplina DataRowToEntity(DataRow dr, CLS_TurmaAulaPlanoDisciplina entity)
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
        // public override CLS_TurmaAulaPlanoDisciplina DataRowToEntity(DataRow dr, CLS_TurmaAulaPlanoDisciplina entity, bool limparEntity)
        // {
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        // }
    }
}