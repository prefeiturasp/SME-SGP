/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;

    /// <summary>
    /// Description: .
    /// </summary>
    public class CLS_TurmaNotaOrientacaoCurricularDAO : Abstract_CLS_TurmaNotaOrientacaoCurricularDAO
    {
        #region Metodos de Consulta

        /// <summary>
        /// Seleciona as Orientações curriculares ligadas a uma Avaliação
        /// </summary>
        /// <param name="tud_id">ID da Turma Disciplina</param>
        /// <param name="tnt_id">ID da Turma Aula</param>
        /// <returns>Orientações curriculares ligadas a uma avaliação</returns>
        public DataTable SelecionaPorAvaliacao(long tud_id, int tnt_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaOrientacaoCurricular_SelecionaPorAvaliacao", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@tud_id";
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@tnt_id";
                Param.Value = tnt_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Metodos de Consulta

        #region Metodos de Save

        /// <summary>
        /// Salva os dados em lote.
        /// </summary>
        /// <param name="dtFrequenciaReuniaoResponsaveis">DataTable com os dados.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarEmLote(DataTable tbDados)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaNotaOrientacaoCurricular_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbDados";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaNotaOrientacaoCurricular";
                sqlParam.Value = tbDados;
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

        #endregion Metodos de Save

        #region Metodos SobreEscritos

        /// <summary>
        /// Inseri os valores da classe em um registro ja existente.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados.</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        protected override bool Alterar(CLS_TurmaNotaOrientacaoCurricular entity)
        {
            entity.toc_dataAlteracao = DateTime.Now;
            return base.Alterar(entity);
        }

        /// <summary>
        /// Inseri os valores da classe em um novo registro.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem inseridos.</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        protected override bool Inserir(CLS_TurmaNotaOrientacaoCurricular entity)
        {
            entity.toc_dataAlteracao = entity.toc_dataCriacao = DateTime.Now;
            return base.Inserir(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaNotaOrientacaoCurricular entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaNotaOrientacaoCurricular_UPDATE";
            base.ParamAlterar(qs, entity);

            qs.Parameters.Remove("toc_dataCriacao");
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados.</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaNotaOrientacaoCurricular entity)
        {
            if (entity != null & qs != null)
            {
                entity.toc_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.toc_id > 0);
            }

            return false;
        }

        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Carregar(CLS_TurmaNotaOrientacaoCurricular entity)
        // {
        //    return base.Carregar(entity);
        // }
        ///// <summary>
        ///// Exclui um registro do banco.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Delete(CLS_TurmaNotaOrientacaoCurricular entity)
        // {
        //    return base.Delete(entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamCarregar(QuerySelectStoredProcedure qs, CLS_TurmaNotaOrientacaoCurricular entity)
        // {
        //    base.ParamCarregar(qs, entity);
        // }
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir.
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure.</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        // protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaNotaOrientacaoCurricular entity)
        // {
        //    base.ParamInserir(qs, entity);
        // }
        ///// <summary>
        ///// Salva o registro no banco de dados.
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar.</param>
        ///// <returns>True - Operacao bem sucedida.</returns>
        // public override bool Salvar(CLS_TurmaNotaOrientacaoCurricular entity)
        // {
        //    return base.Salvar(entity);
        // }
        ///// <summary>
        ///// Realiza o select da tabela.
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela.</returns>
        // public override IList<CLS_TurmaNotaOrientacaoCurricular> Select()
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
        // public override IList<CLS_TurmaNotaOrientacaoCurricular> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        // {
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        // }
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade.
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido.</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados.</param>
        ///// <returns>Entidade preenchida.</returns>
        // public override CLS_TurmaNotaOrientacaoCurricular DataRowToEntity(DataRow dr, CLS_TurmaNotaOrientacaoCurricular entity)
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
        // public override CLS_TurmaNotaOrientacaoCurricular DataRowToEntity(DataRow dr, CLS_TurmaNotaOrientacaoCurricular entity, bool limparEntity)
        // {
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        // }

        #endregion Metodos SobreEscritos
    }
}