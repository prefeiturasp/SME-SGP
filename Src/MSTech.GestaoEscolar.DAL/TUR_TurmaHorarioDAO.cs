/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.DAL
{
    using System;
    using System.Linq;
    using System.Data;
    using MSTech.Data.Common;
	using MSTech.GestaoEscolar.DAL.Abstracts;
    using MSTech.GestaoEscolar.Entities;
    using System.Collections.Generic;    
	
	/// <summary>
	/// Description: .
	/// </summary>
	public class TUR_TurmaHorarioDAO : Abstract_TUR_TurmaHorarioDAO
	{
        #region Métodos de validação

        /// <summary>
        /// Verifica se existe um registro de turma horário cadastrado para o turno
        /// </summary>
        /// <param name="trn_id">ID do turno.</param>
        /// <returns></returns>
        public bool VerificaExistePorTurno(int trn_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_TUR_TurmaHorario_VerificaExistePorTurno", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.ParameterName = "@trn_id";
                Param.DbType = DbType.Int32;
                Param.Size = 4;
                Param.Value = trn_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de validação

        #region Métodos de consulta

        /// <summary>
        /// Seleciona os tempos para as aulas, nas disciplinas que possuem aula no dia informado, por turma
        /// </summary>
        /// <returns></returns>
        public DataTable SelecionaAulas_Por_Turma_Data(long tur_id, int tpc_id, DateTime tau_data)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaHorario_SelecionaAulas_Por_Turma_Data", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tau_data";
                Param.Value = tau_data;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona turma horário por turma
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns></returns>
        public DataTable SelecionaPorTuma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaHorario_SelecionaPorTurma", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public List<TUR_TurmaHorario> SelecionaPorTurmaDisciplinasVigentes(string tud_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaHorario_SelecionaPorTurmaDisciplinasVigentes", _Banco);

            try
            {
                #region Parametro

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tud_ids";
                Param.Value = tud_ids;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>()
                                     .Select(dr => DataRowToEntity(dr, new TUR_TurmaHorario()))
                                     .ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        public List<TUR_TurmaHorario> SelecionaPorTurmaDisciplinasVigenciaInicio(string tud_ids, DateTime thr_vigenciaInicio)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaHorario_SelecionaPorTurmaDisciplinasVigenciaInicio", _Banco);

            try
            {
                #region Parametro

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tud_ids";
                Param.Value = tud_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@thr_vigenciaInicio";
                Param.Size = 16;
                Param.Value = thr_vigenciaInicio;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>()
                                     .Select(dr => DataRowToEntity(dr, new TUR_TurmaHorario()))
                                     .ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion Métodos de consulta

        #region Métodos Sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaHorario entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@thr_vigenciaInicio"].Value = entity.thr_vigenciaInicio == new DateTime() ? DateTime.Now : entity.thr_vigenciaInicio;
            qs.Parameters["@thr_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@thr_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaHorario entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@thr_dataCriacao");
            qs.Parameters.RemoveAt("@thr_registroExterno");

            qs.Parameters["@thr_dataAlteracao"].Value = DateTime.Now;
        }

        protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaHorario entity)
        {
            base.ParamDeletar(qs, entity);

            if (qs != null && entity != null)
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@thr_situacao";
                Param.Size = 1;
                Param.Value = 3;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@thr_dataAlteracao";
                Param.Size = 16;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);
            }
        }

        /// <summary>
        /// Méotodo de update alterado para que não modificasse o valor do campo data da criação;
        /// </summary>
        /// <param name="entity">Entidade com dados preenchidos</param>
        /// <returns>True para alteração realizado com sucesso.</returns>
        protected override bool Alterar(TUR_TurmaHorario entity)
        {
            __STP_UPDATE = "NEW_TUR_TurmaHorario_Update";
            return base.Alterar(entity);
        }

        public override bool Delete(TUR_TurmaHorario entity)
        {
            __STP_DELETE = "NEW_TUR_TurmaHorario_UpdateSituacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaHorario entity)
        {
            entity.thr_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.thr_id > 0);
        }

        #endregion

        /// <summary>
        /// Retorna a TUR_TurmaHorario pelo thr_id
        /// </summary>
        /// <param name="tud_id">Id da turma horário.</param>
        /// <returns>TUR_TurmaHorario</returns>
        public TUR_TurmaHorario SelecionarTurmaHorarioPorId(int trn_id, int trh_id, int thr_id, long tud_id)
        {
            try
            {
                TUR_TurmaHorarioDAO dao = new TUR_TurmaHorarioDAO();

                TUR_TurmaHorario turmaHorario = new TUR_TurmaHorario {
                                                                        trn_id = trn_id,
                                                                        trh_id = trh_id,
                                                                        thr_id = thr_id,
                                                                        tud_id = tud_id
                                                                     };

                dao.Carregar(turmaHorario);

                return turmaHorario.IsNew ? null : turmaHorario;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>  
        public DataTable Seleciona()
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaHorario_SELECT", _Banco);
            try
            {
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

        public List<TUR_TurmaHorario> SelecionaVigentes(DateTime dataVigente)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaHorario_SelecionaVigentes", _Banco);
            try
            {
                #region Parametro

                Param = qs.NewParameter();
                Param.DbType = DbType.Date;
                Param.ParameterName = "@dataVigente";
                if (dataVigente == new DateTime())
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataVigente;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.OfType<DataRow>().Select(row => DataRowToEntity(row, new TUR_TurmaHorario())).ToList();
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
    }
}