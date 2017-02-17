/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
	
	public class CLS_TurmaDisciplinaPlanejamentoDAO : Abstract_CLS_TurmaDisciplinaPlanejamentoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna o planejamento da disciplina da turma que tenha
        /// o tipo de período calendário nulo.
        /// </summary>
        public DataTable SelecionaPorDisciplinaPermissaoDocente
        (
            long tud_id
            , byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SelecionaPorDisciplinaPermissaoDocente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
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



        /// <summary>
        /// Retorna o planejamento da disciplina da turma que tenha
        /// o tipo de período calendário nulo.
        /// </summary>
        public DataTable SelectBy_tud_id_tpc_id_null
        (
            long tud_id
            , byte tdt_posicao
        )
        {
            DataTable dt = new DataTable();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SELECTBY_tud_id_tpc_id_null", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                    dt = qs.Return;

                return dt;
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
        /// Busca o planejamento da turma.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public List<CLS_TurmaDisciplinaPlanejamento> BuscaPlanejamentoTurmaDisciplina
        (
           long tur_id
        )
        {
            List<CLS_TurmaDisciplinaPlanejamento> lt = new List<CLS_TurmaDisciplinaPlanejamento>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaPlanejamentoTurmaDisciplina", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;

                qs.Parameters.Add(Param);



                #endregion

                qs.Execute();

                lt = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new CLS_TurmaDisciplinaPlanejamento())).ToList<CLS_TurmaDisciplinaPlanejamento>();
                return lt;



            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca o planejamento da turma.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public DataTable BuscaPlanejamentoTurmaDisciplinaDT
        (
           string esc_ids, Int64 tur_id, DateTime syncDate
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_BuscaPlanejamentoTurmaDisciplinaPor_Turmas", _Banco);

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

        /// <summary>
        /// Busca o planejamento da turma.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public DataTable BuscaPlanejamentoOrientacaoCurricularDT
        (
           string esc_ids, Int64 tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("DCL_BuscaPlanejamentoOrientacaoCurricularPor_Escolas_Turmas", _Banco);

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
        /// Retorna os periodos da turma e os planejamentos cadastrados
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param> 
        public DataTable SelectBy_tud_id
        (
            long tud_id
        )
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SELECTBY_tud_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Retorna os planejamentos cadastrados
        /// </summary>                
        /// <param name="tud_id">ID da disciplina da turma</param> 
        public DataTable SelecionaPorTurmaDisciplina
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SelecionaPorTurmaDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
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
        /// Retorna o planejamento anual da turma e calendario passados
        /// </summary>                
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tud_id">ID da disciplina da turma</param>     
        public List<CLS_TurmaDisciplinaPlanejamento> SelecionaPorTurmaCalendario
        (
            long tur_id            
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SelecionaPorTurmDisciplinaCalendario", _Banco);

            try
            {
                List<CLS_TurmaDisciplinaPlanejamento> lt = new List<CLS_TurmaDisciplinaPlanejamento>();
                
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);
              
                #endregion

                qs.Execute();

                lt = qs.Return.Rows.Cast<DataRow>().Select(p => DataRowToEntity(p, new CLS_TurmaDisciplinaPlanejamento())).ToList<CLS_TurmaDisciplinaPlanejamento>();
                return lt;

                
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        

        /// <summary>
        /// Retorna os periodos da turma e os planejamentos cadastrados
        /// </summary>                
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tud_id">ID da disciplina da turma</param> 
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        public DataTable SelecionaPorDisciplinaCalendarioPermissaoDocente
        (
            int cal_id
            , long tud_id
            , byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SelecionaPorDisciplinaCalendarioPermissaoDocente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
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

        /// <summary>
        /// Retorna os periodos da turma e os planejamentos cadastrados
        /// </summary>                
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tud_id">ID da disciplina da turma</param> 
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        public DataTable SelectBy_tud_id_cal_id
        (
            int cal_id
            , long tud_id
            , byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SELECTBY_tud_id_cal_id", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                if (tud_id > 0)
                    Param.Value = tud_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
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
	    /// Verifica se o planejamento já foi lançado anteriormente
	    /// </summary>                        
	    /// <param name="tud_id">ID da disciplina da turma</param>
	    /// <param name="tpc_id">ID do período do calendário</param> 
        /// <param name="tdt_posicao">Posição do docente</param>
	    public int VerificaPlanejamentoExistente
        (            
            long tud_id
            , int tpc_id
            , byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_VerificaPlanejamentoExistente", _Banco);
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0]["tdp_id"].ToString()) : 0;
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
        /// Salva os dados do planejamento anual/bimestral. Considera a data de alteração do tablet.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SalvarSincronizacaoDiarioClasse(CLS_TurmaDisciplinaPlanejamento entity)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_UpdateDiarioClasse", _Banco);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tdp_id";
                Param.Size = 4;
                Param.Value = entity.tdp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (entity.tpc_id > 0)
                    Param.Value = entity.tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdp_planejamento";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tdp_planejamento))
                    Param.Value = entity.tdp_planejamento;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdp_diagnostico";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tdp_diagnostico))
                    Param.Value = entity.tdp_diagnostico;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdp_avaliacaoTrabalho";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tdp_avaliacaoTrabalho))
                    Param.Value = entity.tdp_avaliacaoTrabalho;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdp_recursos";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tdp_recursos))
                    Param.Value = entity.tdp_recursos;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdp_intervencoesPedagogicas";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tdp_intervencoesPedagogicas))
                {
                    Param.Value = entity.tdp_intervencoesPedagogicas;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdp_registroIntervencoes";
                Param.Size = 2147483647;
                if (!string.IsNullOrEmpty(entity.tdp_registroIntervencoes))
                {
                    Param.Value = entity.tdp_registroIntervencoes;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                Param.Value = entity.cur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                Param.Value = entity.crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = entity.crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdp_situacao";
                Param.Size = 1;
                Param.Value = entity.tdp_situacao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tdp_dataCriacao";
                Param.Size = 16;
                Param.Value = entity.tdp_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tdp_dataAlteracao";
                Param.Size = 16;
                Param.Value = entity.tdp_dataAlteracao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = entity.tdt_posicao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                if (entity.pro_id != Guid.Empty)
                    Param.Value = entity.pro_id;
                else
                    Param.Value = DBNull.Value;
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
        /// Seleciona as turmas em que o docente leciona no mesmo curso, curriculo,
        /// periodo, disciplina e posição da atribuição que está sendo salva.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo</param>
        /// <param name="crp_id">ID do periodo</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <returns></returns>
        public DataTable SelecionaOutrasTurmasDocente(Int64 tur_id, int cal_id, int cur_id, int crr_id, int crp_id, long tud_id, byte tdt_posicao)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaDisciplinaPlanejamento_SelecionaOutrasTurmasDocente", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;

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
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                Param.Value = crp_id;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;

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

        #region Métodos sobrescritos

        protected override string ConnectionStringName
        {
            get
            {
                return "GestaoEscolar";
            }
        }

	    /// <summary>
	    /// Recebe o valor do auto incremento e coloca na propriedade 
	    /// </summary>
	    /// <param name="qs">Objeto da Store Procedure</param>
	    /// <param name="entity"></param>
	    protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
        {
            entity.tdp_id = Convert.ToInt32(qs.Return.Rows[0][0]);
            return (entity.tdp_id > 0);
        }	

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tdp_planejamento"].DbType = DbType.AnsiString;
            qs.Parameters["@tdp_diagnostico"].DbType = DbType.AnsiString;

            qs.Parameters["@tdp_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tdp_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// Configura os parametros para alteracao
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@tdp_planejamento"].DbType = DbType.AnsiString;
            qs.Parameters["@tdp_diagnostico"].DbType = DbType.AnsiString;

            qs.Parameters.RemoveAt("@tdp_dataCriacao");
            qs.Parameters["@tdp_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override bool Alterar(CLS_TurmaDisciplinaPlanejamento entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaDisciplinaPlanejamento_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Configura os parametros para exclusao
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaDisciplinaPlanejamento entity)
        {
            base.ParamDeletar(qs, entity);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdp_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tdp_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Exclui logicamente
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Delete(CLS_TurmaDisciplinaPlanejamento entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaDisciplinaPlanejamento_UPDATE_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}