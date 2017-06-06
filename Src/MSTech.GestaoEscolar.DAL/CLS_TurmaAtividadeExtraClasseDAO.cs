/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using Data.Common;
    using MSTech.GestaoEscolar.DAL.Abstracts;
    using Entities;
    using System;
    using System.Data;
    using System.Data.SqlClient;/// <summary>
                                /// Description: .
                                /// </summary>
    public class CLS_TurmaAtividadeExtraClasseDAO : Abstract_CLS_TurmaAtividadeExtraClasseDAO
    {
        #region Métodos de verificação

        /// <summary>
        /// Verifica se já existe a atividade por disciplina, nome, tipo e bimestre.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tae_id"></param>
        /// <param name="tae_nome"></param>
        /// <param name="tav_id"></param>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public bool VerificaExistePorDisciplinaNomeTipoBimestre(long tud_id, int tae_id, string tae_nome, int tav_id, int tpc_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAtividadeExtraClasse_VerificaExistePorDisciplinaNomeTipoBimestre", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tae_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (tae_id > 0)
                    Param.Value = tae_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tae_nome";
                Param.DbType = DbType.String;
                Param.Size = 100;
                Param.Value = tae_nome;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tav_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (tav_id > 0)
                    Param.Value = tav_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tpc_id";
                Param.DbType = DbType.Int32;
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

        /// <summary>
        /// Verifica se a carga horaria da atividade extraclasse vai estourar o permitido pelo curso no ano letivo
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tae_id"></param>
        /// <param name="tae_cargaHoraria"></param>
        /// <param name="cur_cargaHorariaExtraClasse"></param>
        /// <param name="cargaAtividadeExtraTotal"></param>
        /// <returns></returns>
        public bool VerificaCargaHorariaCursoCalendario(long tud_id, int tae_id, decimal tae_cargaHoraria, out decimal dis_cargaHorariaExtraClasse, out decimal cargaAtividadeExtraTotal)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAtividadeExtraClasse_VerificaCargaHorariaCursoCalendario", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tae_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                if (tae_id > 0)
                    Param.Value = tae_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tae_cargaHoraria";
                Param.DbType = DbType.Decimal;
                Param.Value = tae_cargaHoraria;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                dis_cargaHorariaExtraClasse = qs.Return.Rows.Count > 0 ? Convert.ToDecimal(qs.Return.Rows[0]["dis_cargaHorariaExtraClasse"]) : 0;
                cargaAtividadeExtraTotal = qs.Return.Rows.Count > 0 ? Convert.ToDecimal(qs.Return.Rows[0]["cargaAtividadeExtraTotal"]) : 0;

                return cargaAtividadeExtraTotal > dis_cargaHorariaExtraClasse;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de verificação

        #region Métodos de consulta

        /// <summary>
        /// Retorna as todas as Atividades extraclasse(por disciplina) com as notas do aluno
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tpc_id"></param>
        /// <param name="dtTurmas"></param>
        /// <returns></returns>
        public DataTable SelecionaPorPeriodoDisciplina_Alunos(long tud_id, int tpc_id, bool usuario_superior, byte tdt_posicao, DataTable dtTurmas)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAtividadeExtraClasse_SelecionaPorPeriodoDisciplina_Alunos", _Banco);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@usuario_superior";
                Param.Size = 1;
                Param.Value = usuario_superior;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.ParameterName = "@dtTurmas";
                sqlParam.TypeName = "TipoTabela_Turma";
                sqlParam.Value = dtTurmas;
                qs.Parameters.Add(sqlParam);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de consulta     

        #region Métodos de exclusão

        /// <summary>
        /// Exclui a atividade extraclasse
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tae_id"></param>
        /// <returns></returns>
        public bool Deletar(long tud_id, int tae_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAtividadeExtraClasse_Excluir", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_id";
                Param.DbType = DbType.Int64;
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tae_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = tae_id;
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

        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            entity.tae_dataCriacao = entity.tae_dataAlteracao = DateTime.Now;
            base.ParamInserir(qs, entity);
        }

        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            entity.tae_dataAlteracao = DateTime.Now;
            base.ParamAlterar(qs, entity);
            qs.Parameters.RemoveAt("@tae_dataCriacao");
            qs.Parameters.RemoveAt("@tdt_posicao");
        }

        protected override bool Alterar(CLS_TurmaAtividadeExtraClasse entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaAtividadeExtraClasse_UPDATE";
            return base.Alterar(entity);
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            if (entity != null & qs != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = entity.tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tae_id";
                Param.Size = 4;
                Param.Value = entity.tae_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tae_situacao";
                Param.Size = 3;
                Param.Value = entity.tae_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tae_dataAlteracao";
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);
            }
        }

        public override bool Delete(CLS_TurmaAtividadeExtraClasse entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaAtividadeExtraClasse_UpdateSituacao";
            return base.Delete(entity);
        }

        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAtividadeExtraClasse entity)
        {
            if (entity != null & qs != null)
            {
                entity.tae_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.tae_id > 0;
            }

            return false;
        }

        #endregion Métodos sobrescritos
    }
}