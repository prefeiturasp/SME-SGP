/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Data;
    using System.Data.SqlClient;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_TurmaNotaAlunoOrientacaoCurricularDAO : AbstractCLS_TurmaNotaAlunoOrientacaoCurricularDAO
    {
        #region Metodos de consulta

        /// <summary>
        /// Seleciona as Orientações curriculares ligadas a uma avaliação e aluno
        /// </summary>
        /// <param name="tud_id">ID da Turma Disciplina</param>
        /// <param name="tnt_id">ID da Turma Aula</param>
        /// <param name="alu_id">ID do Aluno</param>
        /// <param name="mtu_id">ID da Matricula do aluno na turma</param>
        /// <param name="mtd_id">ID da Matricula do aluno na disciplina</param>
        /// <returns>Orientações curriculares ligadas a uma avaliação e aluno</returns>
        public DataTable SelecionaPorAvaliacaoAluno(long tud_id, int tnt_id, long alu_id, int mtu_id, int mtd_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaAlunoOrientacaoCurricular_SelecionaPorAvaliacaoAluno", _Banco);

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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.ParameterName = "@alu_id";
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@mtu_id";
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.ParameterName = "@mtd_id";
                Param.Value = mtd_id;
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

        /// <summary>
        /// Valida se existe habilidade marcada para algum aluno na avaliação, que não esteja mais selecionada na avaliação.
        /// </summary>
        /// <param name="dtFrequenciaReuniaoResponsaveis">DataTable com os dados.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool ValidarHabilidadesAvaliacao(DataTable tbDados)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNotaAlunoOrientacaoCurricular_ValidarHabilidadesAvaliacao", _Banco);

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

                return qs.Return.Rows.Count <= 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Metodos de consulta

        #region Metodos de Save

        /// <summary>
        /// Salva os dados em lote.
        /// </summary>
        /// <param name="dtFrequenciaReuniaoResponsaveis">DataTable com os dados.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool SalvarEmLote(DataTable tbDados)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaNotaAlunoOrientacaoCurricular_SalvarEmLote", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbDados";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaNotaAlunoOrientacaoCurricular";
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
        protected override bool Alterar(CLS_TurmaNotaAlunoOrientacaoCurricular entity)
        {
            entity.aoc_dataAlteracao = DateTime.Now;
            return base.Alterar(entity);
        }

        /// <summary>
        /// Inseri os valores da classe em um novo registro.
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem inseridos.</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        protected override bool Inserir(CLS_TurmaNotaAlunoOrientacaoCurricular entity)
        {
            entity.aoc_dataAlteracao = entity.aoc_dataCriacao = DateTime.Now;
            return base.Inserir(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaNotaAlunoOrientacaoCurricular entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaNotaAlunoOrientacaoCurricular_UPDATE";
            base.ParamAlterar(qs, entity);

            qs.Parameters.Remove("aoc_dataCriacao");
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados.</param>
        /// <returns>True - Operacao bem sucedida.</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaNotaAlunoOrientacaoCurricular entity)
        {
            if (entity != null & qs != null)
            {
                entity.aoc_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return (entity.aoc_id > 0);
            }

            return false;
        }

        #endregion Metodos SobreEscritos
	}
}