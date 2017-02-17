/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class CLS_AlunoPlanejamentoDAO : Abstract_CLS_AlunoPlanejamentoDAO
    {
        /// <summary>
        /// Seleciona o planejamento do aluno na turmadisciplina
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <returns></returns>
        public CLS_AlunoPlanejamento SelecionaPlanejamentoAluno(long alu_id, long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoPlanejamento_SelectBy_alu_tud_id", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    return DataRowToEntity(qs.Return.Rows[0], new CLS_AlunoPlanejamento());

                return new CLS_AlunoPlanejamento();
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
        /// Seleciona o planejamento e relacionados do aluno na turmadisciplina 
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <returns></returns>
        public DataTable SelecionaPlanejamentoAlunoRelacionados(long tud_id, DataTable dtTurmas)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoPlanejamentoRelacionado_SelectBy_tud_id", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@dtTurmas";
                sqlParam.TypeName = "TipoTabela_Turma";
                sqlParam.Value = dtTurmas;
                qs.Parameters.Add(sqlParam);

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
        /// Seleciona os alunos pelo turmadisciplina
        /// </summary>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <returns></returns>
        public DataTable SelecionaAlunosPorTud(long tud_id, bool documentoOficial, DataTable dtTurmas)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoPlanejamento_SelectAlunosPorTurmaDisciplina", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@documentoOficial";
                Param.Size = 1;
                Param.Value = documentoOficial;
                qs.Parameters.Add(Param);


                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@dtTurmas";
                sqlParam.TypeName = "TipoTabela_Turma";
                sqlParam.Value = dtTurmas;
                qs.Parameters.Add(sqlParam);

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
        /// Carrega as turmas que o aluno está atribuído (ou 
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="esc_id">ID da escola que está consultando</param>
        /// <param name="cal_ano">ID do calendário para pegar o ano das turmas que serão consultadas</param>
        /// <returns></returns>
        public DataTable SelecionarTurmasAluno(long alu_id, int esc_id, int cal_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoPlanejamento_SelecionarTurmasAluno", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
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
        /// Carrega os planejamentos do aluno na turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="alu_id">ID do aluno</param>
        /// <returns></returns>
        public DataTable SelecionaPlanejamentoTurmaAluno(long tur_id, long alu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoPlanejamento_SelecionaPlanejamentoTurmaAluno", _Banco);

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@alu_id";
                Param.Size = 8;
                Param.Value = alu_id;
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
        /// Carrega os planejamentos do aluno na turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        public List<CLS_AlunoPlanejamento> SelecionaPlanejamentoPorTurma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_AlunoPlanejamento_SelecionaPlanejamentoPorTurma", _Banco);
            List<CLS_AlunoPlanejamento> lt = new List<CLS_AlunoPlanejamento>();

            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);
                                
                #endregion

                qs.Execute();

                lt = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new CLS_AlunoPlanejamento())).ToList<CLS_AlunoPlanejamento>();
                return lt;

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

        #region Métodos sobrescritos

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_AlunoPlanejamento entity)
        {
            entity.apl_dataCriacao = entity.apl_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_AlunoPlanejamento entity)
        {
            entity.apl_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@apl_dataCriacao");
        }

        protected override bool Alterar(CLS_AlunoPlanejamento entity)
        {
            __STP_UPDATE = "NEW_CLS_AlunoPlanejamento_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_AlunoPlanejamento entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@apl_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@apl_situacao";
            Param.Size = 1;
            Param.Value = entity.apl_situacao;
            qs.Parameters.Add(Param);
        }

        public override bool Delete(CLS_AlunoPlanejamento entity)
        {
            __STP_DELETE = "NEW_CLS_AlunoPlanejamento_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_AlunoPlanejamento entity)
        {
            if (entity != null & qs != null)
            {
                entity.apl_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.apl_id > 0;
            }

            return false;
        }

        #endregion
    }
}