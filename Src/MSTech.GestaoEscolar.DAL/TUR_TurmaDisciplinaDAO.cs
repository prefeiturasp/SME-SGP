/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class TUR_TurmaDisciplinaDAO : Abstract_TUR_TurmaDisciplinaDAO
    {

        public DataTable SelectBy_tur_id
        (
            long tur_id
            , Guid ent_id
            , bool mostraFilhosRegencia
            , bool mostraRegencia
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_tur_id_combo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@mostraFilhosRegencia";
                Param.Size = 4;
                if (mostraFilhosRegencia)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraRegencia";
                Param.Size = 1;
                Param.Value = mostraRegencia;
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
        /// Seleciona as turmas e disciplinas que o docente leciona.
        /// </summary>
        /// <param name="doc_id">Id do docente</param>
        /// <returns>Turmas e disciplinas que o docente leciona.</returns>
        public DataTable SelecionaTurmaDisciplinaPorDocente
        (
            long doc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelecionaTurmaDisciplinaPorDocente", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            Param.Value = doc_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                return qs.Return;
            }

            return new DataTable();
        }

        /// <summary>
        /// Retorna as disciplinas de uma turma visíveis na efetivação. 
        /// Se for docente, traz as disciplinas que ele dá aula ou coordena.
        /// Se não for docente, traz todas as disciplinas da turma, independente se a turma 
        /// é de docente especialista e o lançamento for em conjunto.
        /// Na efetivação é considerado apenas o tipo de formato de avaliação, se for pra
        /// lançar por disciplina (Notas por disciplina ou Conceito global + notas por disciplina), 
        /// traz todas as disciplinas.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>                
        public DataTable Select_Efetivacao_By_TurmaDocente
        (
            long tur_id
            , long doc_id
            , int mtu_id
            , long alu_id
            , byte tdt_situacao
        )
        {
            string sProcedure = string.Empty;

            if (alu_id > 0)
            {
                sProcedure = "NEW_TUR_TurmaDisciplina_RetornaDisciplinasAluno";
            }
            else
            {
                sProcedure = "NEW_TUR_TurmaDisciplina_Select_Efetivacao_By_TurmaDocente";
            }

            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure(sProcedure, _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            if (alu_id > 0)
            {
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
            }
            else
            {
                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_situacao";
                Param.Size = 1;
                if (tdt_situacao > 0)
                {
                    Param.Value = tdt_situacao;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);
            }

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Utilizado nas telas do menu classe (EXCETO EFETIVAÇÃO).
        /// Quando informado o tur_id, carrega todas as disciplinas da turma.
        /// Quando informado o doc_id, carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina.
        /// Quando informado o tipo de lançamento de frequência, filtra as turmas
        /// que o formato de avaliação seja do mesmo tipo (somente quando for
        /// busca por docente.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>                
        /// <param name="fav_tipoLancamentoFrequencia">Tipo de lançamento de frequência do formato de avaliação da turma</param>
        /// <param name="tud_tipo">ID do tipo de disciplina que não será exibido</param>
        public DataTable SelectBy_TurmaDocente
        (
            long tur_id
            , long doc_id
            , byte fav_tipoLancamentoFrequencia
            , byte tud_tipo
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_TurmaDocente", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            if (tur_id > 0)
                Param.Value = tur_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_tipoLancamentoFrequencia";
            Param.Size = 1;
            if (fav_tipoLancamentoFrequencia > 0)
                Param.Value = fav_tipoLancamentoFrequencia;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tud_tipo";
            Param.Size = 1;
            if (tud_tipo > 0)
                Param.Value = tud_tipo;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                return qs.Return;
            }

            return new DataTable();
        }

        /// <summary>
        /// Quando informado o tur_id, carrega todas as disciplinas da turma.
        /// Quando informado o doc_id, carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina.
        /// Quando informado o tipo de lançamento de frequência, filtra as turmas
        /// que o formato de avaliação seja do mesmo tipo (somente quando for
        /// busca por docente. Não considera vigência do período.
        /// Não traz as disciplinas do tipo 13-Docente específico – complementação da regência.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>                
        /// <param name="fav_tipoLancamentoFrequencia">Tipo de lançamento de frequência do formato de avaliação da turma</param>
        public DataTable SelectBy_TurmaDocente_SemVigencia
        (
            long tur_id
            , long doc_id
            , byte fav_tipoLancamentoFrequencia
            , byte regencia
            , int tipoRegencia
            , bool filtroTurmasAtivas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_TurmaDocente_SemVigencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            if (tur_id > 0)
                Param.Value = tur_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_tipoLancamentoFrequencia";
            Param.Size = 1;
            if (fav_tipoLancamentoFrequencia > 0)
                Param.Value = fav_tipoLancamentoFrequencia;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            // Alterado para não passar DBNull.Value no parâmetro @regencia. A STP espera 0 ou 1.

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@regencia";
            Param.Size = 1;
            Param.Value = regencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tipoRegencia";
            Param.Size = 4;
            if (tipoRegencia > 0)
                Param.Value = tipoRegencia;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@filtroTurmasAtivas";
            Param.Size = 1;
            Param.Value = filtroTurmasAtivas;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                return qs.Return;
            }

            return new DataTable();
        }

        /// <summary>
        /// Carrega todas as disciplinas da turma.
        /// Não traz as disciplinas do tipo 13-Docente específico – complementação da regência.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="verificaDisciplinaPrincipal">Indica se irá verificar a disciplina principal</param>
        /// <param name="regencia">Indica se irá verificar a regência</param>
        /// <param name="tipoRegencia">Indica se irá verificar se é regência ou componente da regência.</param>
        /// <param name="filtroTurmasAtivas">Indica se irá filtrar apenas turmas ativas.</param>
        public DataTable SelectBy_Turma_SemVigencia
        (
            long tur_id
            , byte verificaDisciplinaPrincipal
            , byte regencia
            , int tipoRegencia
            , bool filtroTurmasAtivas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectClassesBy_Turma_SemVigencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@verificaDisciplinaPrincipal";
            Param.Size = 1;
            Param.Value = verificaDisciplinaPrincipal;
            qs.Parameters.Add(Param);

            // Alterado para não passar DBNull.Value no parâmetro @regencia. A STP espera 0 ou 1.
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@regencia";
            Param.Size = 1;
            Param.Value = regencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tipoRegencia";
            Param.Size = 4;
            if (tipoRegencia > 0)
                Param.Value = tipoRegencia;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@filtroTurmasAtivas";
            Param.Size = 1;
            Param.Value = filtroTurmasAtivas;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                return qs.Return;
            }

            return new DataTable();
        }

        /// <summary>
        /// Carrega as turmas e disciplinas que o docente dá aula, mais as que ele é coordenador de disciplina.
        /// Quando informado o tipo de lançamento de frequência, filtra as turmas
        /// que o formato de avaliação seja do mesmo tipo (somente quando for
        /// busca por docente. Não considera vigência do período.
        /// Não traz as disciplinas do tipo 13-Docente específico – complementação da regência.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>                
        /// <param name="fav_tipoLancamentoFrequencia">Tipo de lançamento de frequência do formato de avaliação da turma</param>
        /// <param name="regencia">Indica se irá verificar a regência</param>
        /// <param name="tipoRegencia">Indica se irá verificar se é regência ou componente da regência.</param>
        /// <param name="filtroTurmasAtivas">Indica se irá filtrar apenas turmas ativas.</param>
        public DataTable SelectBy_Docente_SemVigencia
        (
            long doc_id
            , byte fav_tipoLancamentoFrequencia
            , byte regencia
            , int tipoRegencia
            , bool filtroTurmasAtivas
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectClassesBy_Docente_SemVigencia", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            Param.Value = doc_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@fav_tipoLancamentoFrequencia";
            Param.Size = 1;
            if (fav_tipoLancamentoFrequencia > 0)
                Param.Value = fav_tipoLancamentoFrequencia;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            // Alterado para não passar DBNull.Value no parâmetro @regencia. A STP espera 0 ou 1.
            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@regencia";
            Param.Size = 1;
            Param.Value = regencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tipoRegencia";
            Param.Size = 4;
            if (tipoRegencia > 0)
                Param.Value = tipoRegencia;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@filtroTurmasAtivas";
            Param.Size = 1;
            Param.Value = filtroTurmasAtivas;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                return qs.Return;
            }

            return new DataTable();
        }


        /// <summary>
        /// Retorna as disciplinas de uma turma para lançamento de frequência
        /// Se existir disciplina principal no período carrega apenas ela
        /// </summary>
        /// <param name="tur_id">ID da turma</param>        
        public DataTable SelectBy_TurmaFrequencia
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_TurmaFrequencia", _Banco);
            try
            {
                #region PARAMETROS

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

                if (qs.Return.Rows.Count > 0)
                {
                    return qs.Return;
                }

                return new DataTable();
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
        /// Retorna os as disciplinas por turma e tipo de avaliacao
        /// </summary>                                        
        /// <param name="fav_tipoLancamentoFrequencia">Tipo de avaliação</param> 
        /// <param name="doc_id"></param>
        public DataTable SelectBy_DocenteETipoLancamento
        (
            int fav_tipoLancamentoFrequencia
            , long doc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_TurmaETipoLancamentoDocente", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@fav_tipoLancamentoFrequencia";
                Param.Size = 4;
                Param.Value = fav_tipoLancamentoFrequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
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
        /// Retorna os as disciplinas por turma e disciplina
        /// </summary>                
        /// <param name="tur_id">ID da turma</param>        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="cur_idAtual">ID do curso</param>
        /// <param name="crr_idAtual">ID do curriculo do curso</param>
        /// <param name="crp_idAtual">ID do período do currículo</param>
        /// <param name="crp_idAnterior">ID do período do currículo</param>
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        public DataTable SelectBy_TurmaDisciplina
        (
            long tur_id
            , long tud_id
            , int cur_idAtual
            , int crr_idAtual
            , int crp_idAtual
            , int crp_idAnterior
            , byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_TurmaDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_idAtual";
                Param.Size = 4;
                Param.Value = cur_idAtual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_idAtual";
                Param.Size = 4;
                Param.Value = crr_idAtual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_idAtual";
                Param.Size = 4;
                Param.Value = crp_idAtual;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_idAnterior";
                Param.Size = 4;
                Param.Value = crp_idAnterior;
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
        /// Retorna as disciplinas por turma disciplina e curriculo periodo
        /// </summary>                        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do periodo do curriculo</param>
        public DataTable SelectBy_TurmaDisciplinaCurriculoPeriodo
        (
            long tud_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_TurmaDisciplinaCurriculoPeriodo", _Banco);
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
        /// Retorna as entidades da TurmaDisciplina cadastradas na turma.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="retornaGlobal"></param>
        /// <returns></returns>
        public DataTable SelectBy_Turma
        (
            long tur_id
            , bool retornaGlobal
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_Turma", _Banco);
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@retornaGlobal";
                Param.Size = 1;
                Param.Value = retornaGlobal;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return qs.Return;
                }
                return new DataTable();
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
        /// Retorna as disciplinas por turma.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="retornaGlobal"></param>
        /// <returns></returns>
        public DataTable SelecionaDisciplinasPorTurma
        (
            long tur_id
            , bool retornaGlobal
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_DisciplinasTurma", _Banco);
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
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@retornaGlobal";
                Param.Size = 1;
                Param.Value = retornaGlobal;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                if (qs.Return.Rows.Count > 0)
                {
                    return qs.Return;
                }
                return new DataTable();
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
        /// Retorna as disciplinas por turma.
        /// </summary>
        /// <param name="tur_ids">Ids das turmas.</param>
        /// <returns>DataTable com as turmas e suas disciplinas.</returns>
        public DataTable SelecionaPorTurmas(string tur_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelecionaPorTurmas", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.AnsiString;
            Param.ParameterName = "@tur_ids";
            Param.Value = tur_ids;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();
            qs.Parameters.Clear();

            return qs.Return;

        }

        /// <summary>
        /// Retorna um datatable contendo todas as turmas disciplinas
        /// que não foram excluídas logicamente, filtrados por 
        /// id do curso, id do curriculo, id do curriculo periodo
        /// docente e codigo da turma.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo</param>
        /// <param name="crp_id">ID do curriculo periodo</param>
        /// <param name="paginado"></param>
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <param name="totalRecords"></param>
        /// <returns>DataTable com as turmas disciplinas</returns>
        public DataTable SelectBy_CurriculoDisciplina
        (
            long tur_id
           , int cur_id
           , int crr_id
           , int crp_id
           , bool paginado
           , int currentPage
           , int pageSize
           , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_CurriculoDisciplina", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cur_id";
                Param.Size = 4;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crr_id";
                Param.Size = 4;
                if (crr_id > 0)
                    Param.Value = crr_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@crp_id";
                Param.Size = 4;
                if (crp_id > 0)
                    Param.Value = crp_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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
        /// Retorna o tipo da disciplina da turma disciplina
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <returns></returns>
        public DataTable SelectBy_TipoDisciplina
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_TipoDisciplina", _Banco);
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

        public DataTable SelectBy_TurmaTipos
        (
            long tur_id
            , Guid ent_id
            , bool mostraFilhosRegencia
            , bool mostraRegencia
            , bool mostraExperiencia
            , bool mostraTerritorio
            , bool paginado
           , int currentPage
           , int pageSize
           , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_tur_id_combo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@mostraFilhosRegencia";
                Param.Size = 4;
                if (mostraFilhosRegencia)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraRegencia";
                Param.Size = 1;
                Param.Value = mostraRegencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraExperiencia";
                Param.Size = 1;
                Param.Value = mostraExperiencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraTerritorio";
                Param.Size = 1;
                Param.Value = mostraTerritorio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cap_id";
                Param.Size = 4;                
                Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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

        public DataTable SelectBy_tur_id
        (
            long tur_id
            , Guid ent_id
            , bool mostraFilhosRegencia
            , bool mostraRegencia
            , bool mostraExperiencia
            , bool mostraTerritorio
            , int cap_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_tur_id_combo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@mostraFilhosRegencia";
                Param.Size = 4;
                if (mostraFilhosRegencia)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraRegencia";
                Param.Size = 1;
                Param.Value = mostraRegencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraExperiencia";
                Param.Size = 1;
                Param.Value = mostraExperiencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraTerritorio";
                Param.Size = 1;
                Param.Value = mostraTerritorio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cap_id";
                Param.Size = 4;
                if (cap_id > 0)
                    Param.Value = cap_id;
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
        /// Selects the by_tur_id.
        /// </summary>
        /// <param name="tur_id">The tur_id.</param>
        /// <param name="ent_id">The ent_id.</param>
        /// <param name="mostraFilhosRegencia">if set to <see langword="true" /> [mostra filhos regencia].</param>
        /// <param name="doc_id">The doc_id.</param>
        /// <returns></returns>
        /// <author>juliano.real</author>
        /// <datetime>28/01/2014-12:11</datetime>
        public DataTable SelectBy_TurmaDocente
        (
            long tur_id
            , Guid ent_id
            , bool mostraFilhosRegencia
            , bool mostraRegencia
            , bool mostraExperiencia
            , bool mostraTerritorio
            , long doc_id
            , int cap_id
            , bool mostraCompartilhadas = false
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_Select_TurmaDocente", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                if (tur_id > 0)
                    Param.Value = tur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int16;
                Param.ParameterName = "@mostraFilhosRegencia";
                Param.Size = 4;
                if (mostraFilhosRegencia)
                    Param.Value = 1;
                else
                    Param.Value = 0;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraRegencia";
                Param.Size = 1;
                Param.Value = mostraRegencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraExperiencia";
                Param.Size = 1;
                Param.Value = mostraRegencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraTerritorio";
                Param.Size = 1;
                Param.Value = mostraTerritorio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@Doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cap_id";
                Param.Size = 4;
                if (cap_id > 0)
                    Param.Value = cap_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@mostraCompartilhadas";
                Param.Size = 1;
                Param.Value = mostraCompartilhadas;
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
        /// Utilizado para carregar uma lista de todas as disciplinas e curriculos da mesma turma
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        public DataTable SelecionarTurmaDisciplina_CurriculoDisciplina_By_Turma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_CurriculoDisciplina_SelectBy_Turma", _Banco);
            try
            {
                #region PARAMETROS

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
        /// Utilizado para carregar uma lista de com a disciplina e curriculo da turma multisseriada do docente
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        public DataTable SelecionarTurmaDisciplina_TurmaCurriculo_By_Turma
        (
            long tur_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_TurmaCurriculo_SelectBy_Turma", _Banco);
            try
            {
                #region PARAMETROS

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
        /// Retorna um datatable contendo todas as disciplinas
        /// que não foram excluídas logicamente, filtrados por 
        /// id da turma e docente.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns>DataTable com as disciplinas.</returns>
        public DataTable SelectBy_Turma
        (
            long tur_id
           , long doc_id
           , bool vigencia
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_tur_id", _Banco);
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
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@vigencia";
                Param.Value = vigencia;
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
        /// Retorna as disciplinas de uma turma para lançamento de frequência mensal. Se o tipo de apuração de frequencia for:
        /// DIA: traz apenas a disciplina principal, ou a disciplina global (ou caso não haja nenhuma dessas, faz verificação em tela e cria 
        /// uma coluna de conceito global).
        /// TEMPOS DE AULA: traz todas as disciplinas exceto a disciplina global (tambem verifica em tela qual o fav_tipo, sendo global 
        /// cria apenas a coluna de conceito global, sendo disciplina traz todas as disciplinas e sendo global + disciplina traz todas 
        /// as disciplinas e acrescenta a coluna de conceito global.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">Período do calendário.</param>
        /// <param name="regencia">Indica se os componentes da regência devem ser desconsiderados.</param>
        /// <param name="ValidaControleSemestral">Valida se a disciplina possui controle semestral. Se ela possui controle semestral checado na turma, 
        /// a disciplina não será avalida para o bimestre.</param>
        /// <returns>DataTable de disciplinas</returns>
        public DataTable SelectByTurma_LancamentoMensal(long tur_id, int tpc_id, bool regencia, bool ValidaControleSemestral)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectByTurma_LancamentoMensal", _Banco);

            #region PARAMETROS

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
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@regencia";
            Param.Size = 1;
            Param.Value = regencia;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@ValidaControleSemestral";
            Param.Size = 1;
            Param.Value = ValidaControleSemestral;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona o nome da disciplina  de acordo com o docente se este for uma disciplina especial. (Libras)
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="tdc_id">ID do tipo de docente.</param>
        /// <returns></returns>
        public string SelecionaNomePorTipoDocente(long tud_id, byte tdc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelecionaNomePorTipoDocente", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdc_id";
                Param.Size = 1;
                Param.Value = tdc_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? qs.Return.Rows[0]["tud_nome"].ToString() : string.Empty;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona turmas disciplinas por tud_ids
        /// </summary>
        /// <param name="tud_ids">tud_ids separados por ';'</param>
        /// <returns></returns>
        public List<TUR_TurmaDisciplina> SelecionaTurmaDisciplina(string tud_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelecionaTurmaDisciplina", _Banco);

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
                                     .Select(dr => DataRowToEntity(dr, new TUR_TurmaDisciplina()))
                                     .ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona dados relacionados pelo tud_id para evitar varias buscas.
        /// </summary>
        /// <param name="tud_id">Id da Turma Disciplina.</param>
        public DataTable SelecionaEntidadesControleTurmas
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_SelecionaEntidadesControleTurmasPor_Tud", _Banco);

            #region Parâmetros

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            if (qs.Return.Rows.Count > 0)
            {
                return qs.Return;
            }

            return new DataTable();
        }

        /// <summary>
        /// Retorna os parâmetros de configuração das disciplinas 
        /// </summary>                
        /// <param name="esc_id">ID da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crp_id">ID do período do currículo</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tds_id">ID da disciplina</param>
        /// <param name="tur_codigo">Código da turma</param> 
        /// <param name="paginado">Se é paginado ou não.</param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        public DataTable SelectBy_ConfiguracaoParametrosDisciplinas
        (
	        int esc_id 
	        , int cur_id
	        , int crp_id
	        , int crr_id
	        , int cal_id
	        , int tds_id
	        , string tur_codigo
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_BLN_ConfiguracaoParametrosDisciplinas_SelectBy_Filtros", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@cur_id";
                Param.Size = 8;
                if (cur_id > 0)
                    Param.Value = cur_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@crp_id";
                Param.Size = 8;
                Param.Value = crp_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@crr_id";
                Param.Size = 8;
                Param.Value = crr_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@cal_id";
                Param.Size = 8;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tds_id";
                Param.Size = 8;
                Param.Value = tds_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.String;
                Param.ParameterName = "@tur_codigo";
                Param.Size = 30;
                if (!string.IsNullOrEmpty(tur_codigo))
                    Param.Value = tur_codigo;
                else
                    Param.Value = DBNull.Value;                
                qs.Parameters.Add(Param);              

                #endregion

                if (paginado)
                    totalRecords = qs.Execute(currentPage, pageSize);
                else
                {
                    qs.Execute();
                    totalRecords = qs.Return.Rows.Count;
                }

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
        /// Retorna uma listagem de disciplinas por escola quando não for passado a data base
        /// apenas os registros ativos serão retornados, caso passe a data base serão retornados
        /// apenas os registros criados ou alterados apos esta data.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns>dataTable com as disciplinas</returns>
        public DataTable SelecionaDisciplinasPorEscola(int esc_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelecionaDisciplinasPorEscola", _Banco);
            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 1;

                if (dataBase.Equals(new DateTime()))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataBase;

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
        /// Seleciona as disciplinas do docente em determinada turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public DataTable SelecionaDisciplinasPorDocenteTurma(long tur_id, long doc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelecionaDisciplinasPorDocenteTurma", _Banco);

            try
            {
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

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                if (doc_id > 0)
                    Param.Value = doc_id;
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
        /// Seleciona disciplina por id e dados da turma e docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina.</param>
        /// <returns></returns>
        public DataTable SelecionaDisciplinaDadosDocenteTurma(long tud_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelecionaPorId", _Banco);

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

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
		/// Seleciona os TUR_TurmaDisciplina ativos associados à DIS_Disciplina <paramref name="dis_id"/>
		/// </summary>
		/// <param name="dis_id">Id da disciplina desejada</param>
		/// <returns></returns>
		public DataTable SelecionaPorDisciplina(int dis_id)
		{
			QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectBy_Disciplina", _Banco);

			try
			{
				#region Parâmetro

				Param = qs.NewParameter();
				Param.DbType = DbType.Int32;
				Param.ParameterName = "@dis_id";
				Param.Value = dis_id;
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
        /// Seleciona a disciplina relacionada com a disciplina compartilhada, vigente no momento.
        /// </summary>
        /// <param name="tud_id">ID da disciplina compartilhada.</param>
        /// <returns></returns>
        public DataTable SelectRelacionadaVigenteBy_DisciplinaCompartilhada(long tud_id, long doc_id)
        {
            // Só executa a chamada no BD se a disciplina da turma for informada
            if (tud_id > 0)
            { //DANIELLE
                QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectRelacionadaVigenteBy_DisciplinaCompartilhada", _Banco);

                try
                {
                    #region Parâmetro

                    Param = qs.NewParameter();
                    Param.DbType = DbType.Int64;
                    Param.ParameterName = "@tud_id";
                    Param.Size = 8;
                    Param.Value = tud_id;
                    qs.Parameters.Add(Param);

                    Param = qs.NewParameter();
                    Param.DbType = DbType.Int64;
                    Param.ParameterName = "@doc_id";
                    Param.Size = 8;
                    Param.Value = doc_id;
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
            else
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// Seleciona as disciplinas da turma, relacionando com as disciplinas de docência compartilhada.
        /// </summary>
        /// <param name="tud_id">ID da turma.</param>
        /// <returns></returns>
        public DataTable SelectRelacionadoDocenciaCompartilhadaBy_Turma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectRelacionadoDocenciaCompartilhadaBy_Turma", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
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
        /// Busca as turmas/disciplinas eletivas que não foram excluídas logicamente.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <returns></returns>
        public DataTable SelectEletivaAlunoBy_EscolaCalendario
        (
            int esc_id
            , int cal_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_TUR_TurmaDisciplina_SelectEletivaAlunoBy_EscolaCalendario", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 8;
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
        /// Atualiza os campos de configuração de parâmetros de disciplinas com base no tud_id.
        /// </summary>
        /// <param name="tud_id">ID TUR_TurmaDisciplina</param>
        /// <param name="tud_naoexibirfrequencia">Indica se a turma disciplina deve frequencia</param>
        /// <param name="tud_naoexibirnota">Indica se a turma disciplina deve exibir nota</param>
        /// <param name="tud_naolancarfrequencia">Indica se a turma disciplina deve lancar frequencia</param>
        /// <param name="tud_naolancarnota">Indica se a turma disciplina não deve lancar nota</param>
        /// <returns></returns>
        public bool Update_ConfiguracaoParametrosDisciplinas
        (
            Int64 tud_id
            , bool tud_naoexibirfrequencia
            , bool tud_naoexibirnota
            , bool tud_naolancarfrequencia
            , bool tud_naolancarnota
            , bool tud_naoexibirboletim
            , bool tud_naoLancarPlanejamento
            , bool tud_permitirLancarAbonoFalta
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_BLN_ConfiguracaoParametrosDisciplinas_UPDATE", _Banco);
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
                Param.ParameterName = "@tud_naoexibirfrequencia";
                Param.Size = 1;
                Param.Value = tud_naoexibirfrequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoexibirnota";
                Param.Size = 1;
                Param.Value = tud_naoexibirnota;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naolancarfrequencia";
                Param.Size = 1;
                Param.Value = tud_naolancarfrequencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naolancarnota";
                Param.Size = 1;
                Param.Value = tud_naolancarnota;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoexibirboletim";
                Param.Size = 1;
                Param.Value = tud_naoexibirboletim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tud_dataAlteracao";
                Param.Size = 1;
                Param.Value = DateTime.Now;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_naoLancarPlanejamento";
                Param.Size = 1;
                Param.Value = tud_naoLancarPlanejamento;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@tud_permitirLancarAbonoFalta";
                Param.Size = 1;
                Param.Value = tud_permitirLancarAbonoFalta;
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
        // FIM ALTERAÇÃO
        
        ///// <summary>
        ///// Inseri os valores da classe em um registro ja existente
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem modificados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Alterar(TUR_TurmaDisciplina entity)
        //{
        //    return base.Alterar(entity);
        //}
        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(TUR_TurmaDisciplina entity)
        //{
        //    return base.Inserir(entity);
        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(TUR_TurmaDisciplina entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Exclui um registro do banco
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem apagados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Delete(TUR_TurmaDisciplina entity)
        //{
        //    return base.Delete(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Alterar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamAlterar(QueryStoredProcedure qs, TUR_TurmaDisciplina entity)
        //{
        //    base.ParamAlterar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, TUR_TurmaDisciplina entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Deletar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamDeletar(QueryStoredProcedure qs, TUR_TurmaDisciplina entity)
        //{
        //    base.ParamDeletar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, TUR_TurmaDisciplina entity)
        //{
        //    base.ParamInserir(qs, entity);
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(TUR_TurmaDisciplina entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<TUR_TurmaDisciplina> Select()
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
        //public override IList<TUR_TurmaDisciplina> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, TUR_TurmaDisciplina entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override TUR_TurmaDisciplina DataRowToEntity(DataRow dr, TUR_TurmaDisciplina entity)
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
        //public override TUR_TurmaDisciplina DataRowToEntity(DataRow dr, TUR_TurmaDisciplina entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}
    }
}