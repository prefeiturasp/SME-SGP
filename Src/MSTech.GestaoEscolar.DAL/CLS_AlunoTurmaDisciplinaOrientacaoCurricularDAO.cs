/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Linq;
    using System.Data.SqlClient;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO : Abstract_CLS_AlunoTurmaDisciplinaOrientacaoCurricularDAO
    {
        #region Consultas

        /// <summary>
        /// Seleciona os alunos matriculados na turma para lançamento de alcance das habilidades na disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <param name="tpc_id">ID do tipo de período do calendário.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosPorTurmaDisciplinaPeriodo
        (
            long tud_id,
            long ocr_id,
            int tpc_id,
            int cal_id,
            DataTable dtTurma
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoTurmaDisciplinaOrientacaoCurricular_SelecionaAlunosPorTurmaDisciplinaPeriodo", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = ocr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@dtTurmas";
                sqlParam.TypeName = "TipoTabela_Turma";
                sqlParam.Value = dtTurma;
                qs.Parameters.Add(sqlParam);
                
                #endregion

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os alunos com lançamento de alcance por turma disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <returns></returns>
        public List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> SelecionaAlunosPorTurmaDisciplina(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoTurmaDisciplinaOrientacaoCurricular_SelecionaAlunosPorTurmaDisciplina", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new CLS_AlunoTurmaDisciplinaOrientacaoCurricular())).ToList() :
                    new List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular>();
            }
            finally
            {

            }
        }

        #region Diario Classe

        /// <summary>
        /// Busca o planejamento da turma.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public DataTable BuscaAlunoOrientacaoCurricular
        (
           string esc_ids, Int64 tur_id, DateTime syncDate
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_AlunoTurmaDisciplinaOrientacaoCurricularPor_Turmas", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@esc_ids";
                Param.Size = 500;
                if (!string.IsNullOrEmpty(esc_ids))
                    Param.Value = esc_ids;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 16;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@syncDate";
                Param.Size = 16;
                if (syncDate != new DateTime())
                    Param.Value = syncDate;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return;



            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #endregion

        #region Validações

        /// <summary>
        /// O método verifica se já existe um registro salvo para o aluno para a mesma disciplina, orientação e período.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="alu_id">ID do aluno.</param>
        /// <param name="mtu_id">ID da matrícula turma do aluno.</param>
        /// <param name="mtd_id">ID da matrícula turma disciplina do aluno.</param>
        /// <param name="ocr_id">ID da orientação curricular.</param>
        /// <param name="aha_id">ID do lançamento de alcance.</param>
        /// <param name="tpc_id">ID do tipo de período no calendário.</param>
        /// <returns></returns>
        public bool VerificaExistentePorOrientacaoPeriodoDisciplina
        (
            long tud_id,
            long alu_id,
            int mtu_id,
            int mtd_id,
            long ocr_id,
            int aha_id,
            int tpc_id
        )
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoTurmaDisciplinaOrientacaoCurricular_VerificaExistentePorOrientacaoPeriodoDisciplina", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtd_id";
                Param.Size = 4;
                Param.Value = mtd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = ocr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@aha_id";
                Param.Size = 4;
                if (aha_id > 0)
                    Param.Value = aha_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Saves

        /// <summary>
        /// Salva os dados de alcance de alunos no planejamento. Considera a data de alteração do tablet.
        /// </summary>
        /// <param name="entity">Entidade CLS_AlunoTurmaDisciplinaOrientacaoCurricular</param>
        /// <returns></returns>
        public bool SalvarSincronizacaoDiarioClasse(CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_AlunoTurmaDisciplinaOrientacaoCurricular_UpdateDiarioClasse", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = entity.alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtu_id";
                Param.Size = 4;
                Param.Value = entity.mtu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@mtd_id";
                Param.Size = 4;
                Param.Value = entity.mtd_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@ocr_id";
                Param.Size = 8;
                Param.Value = entity.ocr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@aha_id";
                Param.Size = 4;
                Param.Value = entity.aha_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = entity.tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@aha_alcancada";
                Param.Size = 1;
                Param.Value = entity.aha_alcancada;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@aha_efetivada";
                Param.Size = 1;
                Param.Value = entity.aha_efetivada;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@aha_situacao";
                Param.Size = 1;
                Param.Value = entity.aha_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@aha_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.aha_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@aha_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.aha_dataAlteracao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos sobrescritos

        /// <summary>
        /// Configura os parametros do metodo de Inserir.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity)
        {
            entity.aha_dataCriacao = entity.aha_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Alterar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity)
        {
            entity.aha_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@aha_dataCriacao");
        }

        /// <summary>
        /// Configura a procedure para alterar.
        /// </summary>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns></returns>
        protected override bool Alterar(CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoTurmaDisciplinaOrientacaoCurricular_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@aha_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@aha_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Configura a procedure para deletar.
        /// </summary>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns></returns>
        public override bool Delete(CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity)
        {
            __STP_DELETE = "NEW_CLS_AlunoTurmaDisciplinaOrientacaoCurricular_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoTurmaDisciplinaOrientacaoCurricular entity)
        {
            entity.aha_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return entity.aha_id > 0;
        }

        #endregion
    }
}