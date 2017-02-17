/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.Data.SqlClient;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class CLS_TurmaNotaDAO : Abstract_CLS_TurmaNotaDAO
    {
        /// <summary>
        ///	Retorna as atividades da disciplina da turma e do período do calendário
        /// </summary>        
        /// <param name="tud_id">ID da disciplina da turma</param>
        public DataTable SelectBy_tud_id(long tud_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectBy_tud_id", _Banco);

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
        ///	Retorna as atividades da disciplina da turma e do período do calendário
        /// </summary>        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public DataTable SelectBy_tud_id_tpc_id
        (
            long tud_id
            , int tpc_id
            , int tau_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectBy_tud_id_tpc_id", _Banco);

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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_id > 0)
                    Param.Value = tpc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                if (tau_id > 0)
                    Param.Value = tau_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                if (!ent_id.Equals(Guid.Empty))
                    Param.Value = ent_id;
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
        ///	Retorna as atividades da turma e do período do calendário
        /// </summary>        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="ent_id">Entidade do usuário logado</param>        
        public DataTable SelectByTurmaPeriodo
        (
            long tud_id
            , int tpc_id
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectByTurmaPeriodo", _Banco);

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
                Param.Value = tpc_id;
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

        /// <summary>
        /// Verifica se já existe uma atividade da disciplina da turma cadastrada com o mesmo nome
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tnt_id">ID da atividade da disciplina da turma</param>      
        /// <param name="tnt_nome">Nome da atividade da disciplina da turma</param>      
        public bool SelectBy_Nome
        (
            long tud_id
            , int tnt_id
            , string tnt_nome
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectBy_Nome", _Banco);
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
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                if (tnt_id > 0)
                    Param.Value = tnt_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tnt_nome";
                Param.Size = 100;
                Param.Value = tnt_nome;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
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
        /// Utilizado para atualizar a data da atividade de acordo com a data da aula
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_id">ID da aula da disciplina da turma</param>      
        /// <param name="tau_data">Data da aula da disciplina da turma</param>      
        public bool UpdateBy_Aula
        (
            long tud_id
            , int tau_id
            , DateTime tau_data
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_Update_Data_By_Aula", _Banco);
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
                Param.ParameterName = "@tau_id";
                Param.Size = 4;
                if (tau_id > 0)
                    Param.Value = tau_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tau_data";
                Param.Size = 8;
                if (tau_data != new DateTime())
                    Param.Value = tau_data;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tnt_dataAlteracao";
                Param.Size = 8;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return true;
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
        /// Busca as atividades de uma aula atravéz da TurmaDisciplina e Data
        /// </summary>
        /// <param name="tud_id">ID da TurmaDisciplina</param>
        /// <param name="tnt_data">Data da aula</param>
        /// <returns>DataTable</returns>
        public DataTable BuscaAtividadesDaAula
        (
            long tud_id
            , int tau_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaAtividadesDaAula", _Banco);

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
            Param.Size = 8;

            if (tau_id == 0)
            {
                Param.Value = DBNull.Value;
            }
            else
            {
                Param.Value = tau_id;
            }

            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Busca as atividades das disicplinas componentes da regência na turma, para a data informada.
        /// </summary>
        /// <param name="tnt_data">Dia da atividade</param>
        /// <param name="tud_idRegencia">ID da disciplina da regência</param>
        /// <param name="tud_tipoComponente">Tipo de disciplina componente</param>
        /// <returns>DataTable</returns>
        public DataTable BuscaAtividadesDoDia(DateTime tnt_data, long tud_idRegencia, byte tud_tipoComponente)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaAtividadesPorDataAula", _Banco);

            #region Parametros
            
            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tnt_data";
            Param.Size = 10;
            Param.Value = tnt_data;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_idRegencia";
            Param.Size = 8;
            Param.Value = tud_idRegencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tud_tipoComponente";
            Param.Size = 1;
            Param.Value = tud_tipoComponente;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();
            return qs.Return;
        }


        /// <summary>
        /// Retorna as atividades (TurmaNota) para a TurmaDisciplina e período informados, junto com 
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="usu_id">ID do docente logado </param>
        public DataTable SelectBy_TurmaDisciplina_Periodo_NotaAluno
        (
            long tud_id
            , int tpc_id
            , Guid usu_id
            , byte tdt_posicao
            , DataTable dtTurmas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAluno", _Banco);

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
            Param.Size = 5;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (usu_id != Guid.Empty)
            {
                Param.Value = usu_id;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            if (tdt_posicao > 0)
                Param.Value = tdt_posicao;
            else
                Param.Value = DBNull.Value;
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

        /// <summary>
        /// Retorna as atividades (TurmaNota) para a TurmaDisciplina e período informados, junto com 
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// Filtrando os alunos com ou sem deficiência, dependendo do docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tdc_id">ID do tipo de docente</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        public DataTable SelectBy_TurmaDisciplina_Periodo_NotaAlunoFiltroDeficiencia
        (
            long tud_id
            , int tpc_id
            , byte tdc_id
            , Guid usu_id
            , byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAlunoFiltroDeficiencia", _Banco);

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
            Param.Size = 5;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdc_id";
            Param.Size = 1;
            Param.Value = tdc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (usu_id != Guid.Empty)
            {
                Param.Value = usu_id;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            if (tdt_posicao > 0)
            {
                Param.Value = tdt_posicao;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }


        /// <summary>
        /// Retorna as atividades(disciplina e secretaria) para a TurmaDisciplina no período informado, junto com 
        /// os alunos matriculados naquela disciplina e suas notas nas avaliações, quando tiver.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="usu_id">ID do docente logado </param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="tud_idRelacionada">Disciplina relacionada</param>
        /// <param name="usuario_superior">Indica se é um usuário superior</param>
        /// <param name="trazerAvaSecretaria">Indica se deve trazer avaliação da secretaria</param>
        /// <param name="ausenteTurmaNota">Indica se está ausente</param>
        public DataTable SelectBy_TurmaDisciplina_Periodo_NotaAlunoTodos
         (
             long tud_id
             , int tpc_id
             , Guid usu_id
             , byte tdt_posicao
             , long tud_idRelacionada
             , bool usuario_superior
             , bool trazerAvaSecretaria
             , bool ausenteTurmaNota
             , DataTable dtTurmas
         )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectBy_Periodo_NotaAlunoTodos", _Banco);

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
            Param.Size = 5;
            Param.Value = tpc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            if (usu_id != Guid.Empty)
            {
                Param.Value = usu_id;
            }
            else
            {
                Param.Value = DBNull.Value;
            }
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            if (tdt_posicao > 0)
                Param.Value = tdt_posicao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_idRelacionada";
            Param.Size = 8;
            if (tud_idRelacionada > 0)
                Param.Value = tud_idRelacionada;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@trazerAvaSecretaria";
            Param.Size = 1;
            Param.Value = trazerAvaSecretaria;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@ausenteTurmaNota";
            Param.Size = 1;
            Param.Value = ausenteTurmaNota;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.Size = 1;
            Param.ParameterName = "@usuario_superior";
            Param.Value = usuario_superior;
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

        /// <summary>
        /// Salva uma tabela de atividades.
        /// </summary>
        /// <param name="dtTurmaNota"></param>
        /// <returns></returns>
        public bool SalvarAtividades(DataTable dtTurmaNota)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaNota_SalvaAtividades", _Banco);

            try
            {
                #region Parametro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaNota";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaNota";
                sqlParam.Value = dtTurmaNota;
                qs.Parameters.Add(sqlParam);

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
        /// Calcula as notas automaticas dos alunos.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>     
        /// <param name="tpc_id">ID do tipo periodo calendario</param> 
        /// <param name="esa_idDocente">ID da escala de avaliacao</param>
        /// <param name="aluno">Alunos para o calculo das notas</param> 
        /// <returns>Lista de avaliacoes automaticas</returns>      
        public DataTable CalculaNotaAlunos
        (
            long tud_id
            , int tpc_id
            , int esa_idDocente
            , DataTable alunos
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_CalculaNotaAutomaticaAlunos", _Banco);
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
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esa_idDocente";
                Param.Size = 4;
                Param.Value = esa_idDocente;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbAlunos";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_AlunoMatriculaTurmaDisciplina";
                sqlParam.Value = alunos;
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
        /// Seleciona uma avaliacao unica para o tipo de atividade, disciplina e periodo.
        /// </summary>
        /// <param name="tudId">Id da turma disciplina</param>
        /// <param name="tpcId">Id do periodo do calendario</param>
        /// <param name="tavId">Id do tipo de atividade</param>
        /// <returns></returns>
        public CLS_TurmaNota GetSelectByTipoAtividade(long tudId, int tpcId, int tavId)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectBy_TipoAtividade", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tudId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpcId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tav_id";
                Param.Size = 4;
                Param.Value = tavId;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return this.DataRowToEntity(qs.Return.Rows[0], new CLS_TurmaNota());
                }
                else
                {
                    return new CLS_TurmaNota();
                }
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

        public bool ValidaQuantidadeMaxima(long tudId, int qatId, int tpcId, int qtdMax)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_ValidaQuantidadeMaxima", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tudId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@qat_id";
                Param.Size = 4;
                Param.Value = qatId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpcId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@max_quantidade";
                Param.Size = 4;
                Param.Value = qtdMax;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return Convert.ToBoolean(qs.Return.Rows[0][0]);
                }
                else
                {
                    return false;
                }
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
        /// Seleciona a avaliacao filho relacionada.
        /// </summary>
        /// <param name="tudId"></param>
        /// <param name="tntId"></param>
        /// <returns></returns>
        public CLS_TurmaNota GetSelectRelacionadaFilho(long tudId, int tntId)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectRelacionadaFilho", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tudId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                Param.Value = tntId;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return this.DataRowToEntity(qs.Return.Rows[0], new CLS_TurmaNota());
                }
                else
                {
                    return new CLS_TurmaNota();
                }
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
        /// Seleciona a avaliacao pai relacionada.
        /// </summary>
        /// <param name="tudId"></param>
        /// <param name="tntId"></param>
        /// <returns></returns>
        public CLS_TurmaNota GetSelectRelacionadaPai(long tudId, int tntId)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelectRelacionadaPai", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tudId;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tnt_id";
                Param.Size = 4;
                Param.Value = tntId;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return this.DataRowToEntity(qs.Return.Rows[0], new CLS_TurmaNota());
                }
                else
                {
                    return new CLS_TurmaNota();
                }
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
        /// Seleciona a lista com as atividades de acordo com os tnt_ids informados.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da atividade.</param>
        /// <param name="tnt_ids">IDs das atividades.</param>
        /// <returns>Lista com as atividades.</returns>
        public List<CLS_TurmaNota> SelecionarListaAtividadesPorIds(long tud_id, string tnt_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaNota_SelecionaPorTudTntIds", _Banco);

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tnt_ids";
                Param.Value = tnt_ids;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new CLS_TurmaNota())).ToList() :
                    new List<CLS_TurmaNota>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaNota entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tnt_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tnt_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@usu_id"].Value.ToString() == Guid.Empty.ToString())
                qs.Parameters["@usu_id"].Value = DBNull.Value;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
                qs.Parameters["@pro_id"].Value = DBNull.Value;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaNota entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@tnt_dataCriacao");
            qs.Parameters["@tnt_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@usu_id"].Value.ToString() == Guid.Empty.ToString())
                qs.Parameters["@usu_id"].Value = DBNull.Value;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
                qs.Parameters["@pro_id"].Value = DBNull.Value;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNota</param>
        /// <returns>true = sucesso | false = fracasso</returns> 
        protected override bool Alterar(CLS_TurmaNota entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaNota_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaNota entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = entity.tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tnt_id";
            Param.Size = 4;
            Param.Value = entity.tnt_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tnt_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tnt_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaNota</param>
        /// <returns>true = sucesso | false = fracasso</returns>        
        public override bool Delete(CLS_TurmaNota entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaNota_Update_Situacao";
            return base.Delete(entity);
        }

        #endregion Métodos Sobrescritos
    }
}