/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL.Abstracts;
using MSTech.GestaoEscolar.Entities;

namespace MSTech.GestaoEscolar.DAL
{
    using System.Data.SqlClient;

    /// <summary>
    ///
    /// </summary>
    public class CLS_TurmaAulaDAO : Abstract_CLS_TurmaAulaDAO
    {

        /// <summary>
        /// Retorna as aulas criadas pelo cargo especificado (atribuiçao esporádica).
        /// </summary>
        /// <param name="col_id"></param>
        /// <param name="crg_id"></param>
        /// <param name="coc_id"></param>
        /// <returns></returns>
        public DataTable PesquisaPor_AtribuicaoEsporadica
        (
            long col_id, int crg_id, int coc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_PesquisaPor_AtribuicaoEsporadica", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@col_id";
            Param.Size = 8;
            Param.Value = col_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@crg_id";
            Param.Size = 4;
            Param.Value = crg_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@coc_id";
            Param.Size = 4;
            Param.Value = coc_id;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna as informações de planos de aula para o listão.
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tpc_id">ID do periodo do calendario</param>
        /// <returns></returns>
        public DataTable SelectBy_Disciplina
        (
            Int64 tud_id
            , int tpc_id
            , Guid usu_id
            , byte tdt_posicao
            , bool usuario_superior
            , long tud_idRelacionada
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_Disciplina", _Banco);

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
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@tdt_posicao";
            Param.Size = 1;
            if (tdt_posicao > 0)
                Param.Value = tdt_posicao;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Guid;
            Param.ParameterName = "@usu_id";
            Param.Size = 16;
            Param.Value = usu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@usuario_superior";
            Param.Value = usuario_superior;
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

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Seleciona o id da aula que o protocolo passado por parâmetro gerou.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da aula.</param>
        /// <param name="pro_id">ID do protocolo.</param>
        /// <returns></returns>
        public int SelecionaIdAulaPorProcotolo(long tud_id, Guid pro_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAula_SelecionaIdAulaPorProtocolo", _Banco);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@pro_id";
                Param.Size = 16;
                Param.Value = pro_id;
                qs.Parameters.Add(Param);

                #endregion Parametros

                qs.Execute();

                return qs.Return;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as aulas, com informação de atividades e registros ligados no bimestre, para as aulas passado por parâmetro.
        /// </summary>
        /// <param name="dtAulas"></param>
        /// <param name="tud_tipoRegencia"></param>
        /// <param name="tud_tipoComponente"></param>
        /// <returns></returns>
        public DataSet SelecionaDadosPorAulas(DataTable dtAulas, byte tud_tipoRegencia, byte tud_tipoComponente)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelecionaDadosPorAulas", _Banco);
            qs.TimeOut = 0;

            try
            {
                #region Parâmetros

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaBusca";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaBusca";
                sqlParam.Value = dtAulas;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_tipoRegencia";
                Param.DbType = DbType.Byte;
                Param.Size = 1;
                Param.Value = tud_tipoRegencia;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.ParameterName = "@tud_tipoComponente";
                Param.DbType = DbType.Byte;
                Param.Size = 1;
                Param.Value = tud_tipoComponente;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna as aulas, com informação de atividades e registros ligados
        /// no bimestre, para as disciplinas e a posição do docente naquela disciplina.
        /// </summary>
        /// <param name="tud_id">IDs das disciplinas</param>
        /// <param name="tpc_id">ID do bimestre</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        public DataTable SelectBy_DisicplinasDocentePeriodo
        (
            string tud_id
            , int tpc_id
            , long doc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_DisciplinasDocente_Periodo", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.String;
            Param.ParameterName = "@tud_id";
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
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            if (doc_id > 0)
                Param.Value = doc_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        ///	Retorna as aulas da disciplina da turma e do período do calendário
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        public DataTable SelectBy_tud_id_tpc_id
        (
            long tud_id
            , int tpc_id
            , Guid ent_id
            , Guid usu_id
            , byte tdt_posicao
            , bool usuario_superior
            , long tud_idRelacionada
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_tud_id_tpc_id", _Banco);

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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@usuario_superior";
                Param.Value = usuario_superior;
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

                #endregion PARAMETROS

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
        ///	Retorna as aulas da disciplina da turma e do período do calendário
        ///	mais as atividades que não estejam ligada as aulas
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="tud_idsComponentes">ID das disciplinas componentes da regência, se houver</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        public DataTable SelectBy_AulaAtividadeAvaliativa
        (
            long tud_id
            , int tpc_id
            , string tud_idsComponentes
            , Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_AulaAtividadeAvaliativa", _Banco);

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tud_idsComponentes";
                if (!string.IsNullOrEmpty(tud_idsComponentes))
                    Param.Value = tud_idsComponentes;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// retorna os dados de aula por turma e data base, retornando registros ativos e excluidos.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="dataBase">data base para a consulta</param>
        /// <returns></returns>
        public DataSet BuscarAulasPorTurmaDataBase(Int64 tur_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_TurmaDataBase", _Banco);

            try
            {
                #region parametros
                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 16;

                if (new DateTime().Equals(dataBase))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataBase;

                qs.Parameters.Add(Param);
                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// retorna os dados de aula por escola e data base, retornando registros ativos e excluidos.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para a consulta</param>
        /// <returns></returns>
        public DataSet BuscarAulasPorEscolaDataBase(Int32 esc_id, DateTime dataBase)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_EscolaDataBase", _Banco);

            try
            {
                #region parametros
                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataBase";
                Param.Size = 16;

                if (new DateTime().Equals(dataBase))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataBase;

                qs.Parameters.Add(Param);
                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// returna uma dataset com diversos datatable referente a dados da aula por um determinado periodo
        /// </summary>
        /// <param name="tud_id">id da turma disciplina</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data fim do periodo</param>
        /// <param name="usu_id">ID do usuário que criou a aula</param>
        /// <returns></returns>
        public DataSet BuscarAulasPorTurmaDisciplinaPeriodo(Int64 tud_id, byte tdt_posicao, DateTime dataInicio, DateTime dataFim, Guid usu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_DisciplinaPeriodo", _Banco);

            try
            {
                #region parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicio";
                Param.Size = 16;

                if (new DateTime().Equals(dataInicio))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataInicio;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFim";
                Param.Size = 16;

                if (new DateTime().Equals(dataFim))
                    Param.Value = DBNull.Value;
                else
                    Param.Value = dataFim;

                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                if (!usu_id.Equals(Guid.Empty))
                    Param.Value = usu_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }


        /// <summary>
        /// returna uma dataset com diversos datatable referente a dados de uma aula
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>       
        /// <returns></returns>
        public DataSet BuscarAula(Int64 tud_id, Int32 tau_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_IDs", _Banco);

            try
            {
                #region parametros
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
                Param.Value = tau_id;
                qs.Parameters.Add(Param);
                #endregion

                return qs.Execute_DataSet();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Busca as ultimas aulas do tud_id.
        /// </summary>
        /// <param name="tud_id">Disciplina</param>
        /// <param name="tur_id">Turma</param>
        /// <param name="diasTras">Quantidade de dias para tras</param>
        /// <param name="diasFrente">Quantidade de dias para frente</param>
        /// <param name="primeiraSincronizacao">Indica se é a primeira sincronização do tablet (caso for, só traz ativos, se não for, traz excluídos também)</param>
        /// <returns></returns>
        public DataSet BuscaUltimasAulasPorTurmaDisciplina
        (
            Int64 tud_id,
            Int64 tur_id,
            Int32 paraTras,
            Int32 paraFrente,
            bool primeiraSincronizacao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaAulasPorTurmaDisciplina", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@paraTras";
            Param.Size = 4;
            if (paraTras == 0)
                Param.Value = DBNull.Value;
            else
                Param.Value = paraTras;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@paraFrente";
            Param.Size = 4;
            if (paraFrente == 0)
                Param.Value = DBNull.Value;
            else
                Param.Value = paraFrente;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Boolean;
            Param.ParameterName = "@primeiraSincronizacao";
            Param.Size = 1;
            Param.Value = primeiraSincronizacao;
            qs.Parameters.Add(Param);

            #endregion PARAMETROS

            return qs.Execute_DataSet();
        }

        /// <summary>
        /// Verifica se já existe uma aula da disciplina da turma cadastrada com o mesmo número da aula
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_id">ID da aula da disciplina da turma</param>
        /// <param name="tau_sequencia">Número da aula da disciplina da turma</param>
        public bool SelectBy_Nome
        (
            long tud_id
            , int tau_id
            , int tau_sequencia
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_Nome", _Banco);
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tau_sequencia";
                Param.Size = 4;
                Param.Value = tau_sequencia;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Retorna a aula por disciplina, data e posição do docente
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="tau_data">Data da aula da disciplina da turma</param>
        /// <param name="tau_id">ID da aula</param>
        public DataTable SelectBy_DataPosicaoDocente
        (
            long tud_id
            , byte tdt_posicao
            , DateTime tau_data
            , int tau_id = -1
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_DataPosicaoDocente", _Banco);
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
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tau_data";
                Param.Size = 16;
                Param.Value = tau_data;
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

                #endregion PARAMETROS

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
        /// Verifica a quantidade de aulas por semana cadastrada para a disciplina da turma em uma determina semana
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="dataInicial">Data do início da semana</param>
        /// <param name="dataFinal">Data do fim da semana</param>
        public int SelectBy_Semana
        (
            long tud_id
            , byte tdt_posicao
            , DateTime dataInicial
            , DateTime dataFinal
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_Semana", _Banco);
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
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                if (tdt_posicao > 0)
                    Param.Value = tdt_posicao;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicial";
                Param.Size = 16;
                Param.Value = dataInicial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFinal";
                Param.Size = 16;
                Param.Value = dataFinal;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(qs.Return.Rows[0][0].ToString()) : 0;
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
        /// Verifica e retorna a última sequência de aula cadastrada
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        public int SelectBy_tud_id_top_one
        (
            long tud_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_tud_id_top_one", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                return qs.Return.Rows.Count > 0 ? Convert.ToInt32(String.IsNullOrEmpty(qs.Return.Rows[0]["tau_sequencia"].ToString()) ? "0" : qs.Return.Rows[0]["tau_sequencia"].ToString()) : 0;
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
        ///	Retorna todas as anotações por aluno e periodo do calendário
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tpc_ids">String de ids do Período do calendário</param>
        public DataTable SelectBy_AlunoPeriodoCalendario
        (
            long alu_id
            , string tpc_ids
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAulaAluno_SelectBy_AlunoPeriodoCalendario", _Banco);

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tpc_ids";
                Param.Value = tpc_ids;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

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
        /// Verifica se já existe uma aula da disciplina da turma cadastrada na mesma data e retorna a entidade como resultado
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_data">Data da aula da disciplina da turma</param>
        public DataTable DiarioSelectBy_Data
        (
            long tud_id
            , DateTime tau_data
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelectBy_Data", _Banco);
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
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@tau_data";
                Param.Size = 16;
                Param.Value = tau_data;
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                //Para este caso específico a entidade é retornada para que possamos obter a informação do Tau_id referente
                //à aula
                return qs.Return;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona as aulas por período do calendário, turma disciplina e dia da semana.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="tpc_id">ID do tipo de período do calendário.</param>
        /// <param name="dataInicial">Data inicial.</param>
        /// <param name="dataFinal">Data final.</param>
        /// <param name="diaSemana">Dia da semana.</param>
        /// <param name="tdt_posicao">Posição do docente.</param>
        /// <returns></returns>
        public List<CLS_TurmaAula> SelecionaPorPeriodoCalendarioDisciplinaDiaSemana
        (
            long tud_id,
            int tpc_id,
            DateTime dataInicial,
            DateTime dataFinal,
            byte diaSemana,
            byte tdt_posicao
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelecionaPorPeriodoCalendarioDisciplinaDiaSemana", _Banco);

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
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicial";
                Param.Size = 16;
                Param.Value = dataInicial;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFinal";
                Param.Size = 16;
                Param.Value = dataFinal;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@diaSemana";
                Param.Size = 1;
                Param.Value = diaSemana;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tdt_posicao";
                Param.Size = 1;
                Param.Value = tdt_posicao;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new CLS_TurmaAula())).ToList() :
                    new List<CLS_TurmaAula>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona a lista com as aulas de acordo com os tau_ids informados.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da aula.</param>
        /// <param name="tau_ids">IDs das aulas.</param>
        /// <returns>Lista com as aulas</returns>
        public List<CLS_TurmaAula> SelecionarListaAulasPorIds(long tud_id, string tau_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelecionaPorTudTauIds", _Banco);

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
                Param.ParameterName = "@tau_ids";
                Param.Value = tau_ids;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Count > 0 ?
                    qs.Return.Rows.Cast<DataRow>().Select(dr => DataRowToEntity(dr, new CLS_TurmaAula())).ToList() :
                    new List<CLS_TurmaAula>();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Retorna a quantidade de aulas dadas no bimestre e a quantidade de aulas de reposicação.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tdc_ids"></param>
        /// <returns></returns>
        public DataTable SelecionaQuantidadeAulasDadas(long tud_id, int tpc_id, string tdc_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_SelecionaQuantidadeAulasDadas", _Banco);

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
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tdc_ids";
                Param.Value = tdc_ids;
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
        /// Atualiza a sequência das aulas da disciplina, ordenando-as por data.
        /// </summary>
        /// <param name="tud_id">ID da turma disicplina.</param>
        /// <returns></returns>
        public bool AtualizarSequenciaAulasPorTurmaDisciplina(long tud_id)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAula_AtualizaSequenciaAulasPorTurmaDisciplina", _Banco);

            try
            {
                #region Parametros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Size = 8;
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                #endregion Parametros

                qs.Execute();

                return qs.Return > 0;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Altera/Insere as aulas passadas por parâmetro na tabela.
        /// </summary>
        /// <param name="tbTurmaAula">Tabela de aulas</param>
        /// <returns></returns>
        public bool SalvarAulas(DataTable tbTurmaAula)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAula_SalvaAulas", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAula";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAula";
                sqlParam.Value = tbTurmaAula;
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
        /// Salva uma tabela de aulas e todos os dados ligados a elas.
        /// </summary>
        /// <param name="tud_tipoRegencia"></param>
        /// <param name="dtProtocolo"></param>
        /// <param name="dtTurmaAula"></param>
        /// <param name="dtTurmaAulaDisciplinaRelacionada"></param>
        /// <param name="dtTurmaAulaAluno"></param>
        /// <param name="dtTurmaAulaRecurso"></param>
        /// <param name="dtTurmaAulaRegencia"></param>
        /// <param name="dtTurmaAulaRecursoRegencia"></param>
        /// <param name="dtTurmaNota"></param>
        /// <param name="dtTurmaNotaAluno"></param>
        /// <returns></returns>
        public bool SalvaAulaFrequenciaAtividadeNota
        (
            byte tud_tipoRegencia,
            DataTable dtProtocolo,
            DataTable dtTurmaAula,
            DataTable dtTurmaAulaDisciplinaRelacionada,
            DataTable dtTurmaAulaAluno,
            DataTable dtTurmaAulaRecurso,
            DataTable dtTurmaAulaRegencia,
            DataTable dtTurmaAulaRecursoRegencia,
            DataTable dtTurmaNota,
            DataTable dtTurmaNotaRegencia,
            DataTable dtTurmaNotaAluno,
            DataTable dtTurmaAulaPlanoDisciplina,
            DataTable dtTurmaAulaAlunoTipoAnotacao,
            DataTable dtTurmaAulaOrientacaoCurricular,
            DataTable dtLogAula,
            DataTable dtLogNota,
            DataTable dtTurmaAulaTerritorio,
            bool alteraAnotacao
        )
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAula_SalvaAulaFrequenciaAtividadeNota", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@tud_tipoRegencia";
                Param.Size = 1;
                Param.Value = tud_tipoRegencia;
                qs.Parameters.Add(Param);

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbProtocolo";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_Protocolo";
                sqlParam.Value = dtProtocolo;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAula";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAula";
                sqlParam.Value = dtTurmaAula;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaDisciplinaRelacionada";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaDisciplinaRelacionada";
                sqlParam.Value = dtTurmaAulaDisciplinaRelacionada;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaAluno";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaAluno";
                sqlParam.Value = dtTurmaAulaAluno;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaRecurso";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaRecurso";
                sqlParam.Value = dtTurmaAulaRecurso;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaRegencia";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaRegencia";
                sqlParam.Value = dtTurmaAulaRegencia;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaRecursoRegencia";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaRecursoRegencia";
                sqlParam.Value = dtTurmaAulaRecursoRegencia;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaNota";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaNota";
                sqlParam.Value = dtTurmaNota;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaNotaRegencia";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaNotaRegencia";
                sqlParam.Value = dtTurmaNotaRegencia;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaNotaAluno";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaNotaAluno";
                sqlParam.Value = dtTurmaNotaAluno;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaPlanoDisciplina";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaPlanoDisciplina";
                sqlParam.Value = dtTurmaAulaPlanoDisciplina;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaAlunoTipoAnotacao";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaAlunoTipoAnotacao";
                sqlParam.Value = dtTurmaAulaAlunoTipoAnotacao;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaOrientacaoCurricular";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAulaOrientacaoCurricular";
                sqlParam.Value = dtTurmaAulaOrientacaoCurricular;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbLogAula";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_LOG_TurmaAula_Alteracao";
                sqlParam.Value = dtLogAula;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbLogNota";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_LOG_TurmaNota_Alteracao";
                sqlParam.Value = dtLogNota;
                qs.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTurmaAulaTerritorio";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAula";
                sqlParam.Value = dtTurmaAulaTerritorio;
                qs.Parameters.Add(sqlParam);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@alteraAnotacao";
                Param.Value = alteraAnotacao;
                qs.Parameters.Add(Param);

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
        /// Atualiza o campo tau_efetivado das aulas.
        /// </summary>
        /// <param name="dtTurmaAula">DataTable das aulas.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool AtualizarEfetivado(DataTable dtTurmaAula)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAula_AtualizarEfetivado", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTipoTabela_TurmaAula";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAula";
                sqlParam.Value = dtTurmaAula;
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
        /// Atualiza o campo tau_statusAtividadeAvaliativa das aulas.
        /// </summary>
        /// <param name="dtTurmaAula">DataTable das aulas.</param>
        /// <returns>True em caso de sucesso.</returns>
        public bool AtualizarStatusAtividadeAvaliativa(DataTable dtTurmaAula)
        {
            QueryStoredProcedure qs = new QueryStoredProcedure("NEW_CLS_TurmaAula_AtualizarStatusAtividadeAvaliativa", _Banco);

            try
            {
                #region Parâmetro

                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@tbTipoTabela_TurmaAula";
                sqlParam.SqlDbType = SqlDbType.Structured;
                sqlParam.TypeName = "TipoTabela_TurmaAula";
                sqlParam.Value = dtTurmaAula;
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
        /// Verifica se existe aula criada para Experiência (Território do Saber) de acordo com a vigência da Experiência
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do período do calendário (bimestre)</param>
        /// <returns>False: Não tem pendência | True: Tem pendência</returns>
        public bool VerificaPendenciaCadastroAulaExperiencia(long tud_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_CLS_TurmaAula_VerificaPendenciaCadastroAulaExperiencia", _Banco);

            try
            {
                #region Parâmetro

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tud_id";
                Param.Value = tud_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetro

                qs.Execute();

                return string.IsNullOrEmpty(qs.Return.Rows[0][0].ToString()) ? true : Convert.ToBoolean(qs.Return.Rows[0][0].ToString());
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }



        public override bool Carregar(CLS_TurmaAula entity)
        {
            __STP_LOAD = "NEW_CLS_TurmaAula_LOAD";
            return base.Carregar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, CLS_TurmaAula entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@tau_data"].DbType = DbType.DateTime;
            qs.Parameters["@tau_planoAula"].DbType = DbType.String;
            qs.Parameters["@tau_diarioClasse"].DbType = DbType.String;
            qs.Parameters["@tau_conteudo"].DbType = DbType.String;

            qs.Parameters["@tau_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@tau_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }

            if (qs.Parameters["@usu_id"].Value.ToString() == Guid.Empty.ToString())
                qs.Parameters["@usu_id"].Value = DBNull.Value;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamAlterar(QueryStoredProcedure qs, CLS_TurmaAula entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters["@tau_data"].DbType = DbType.DateTime;
            qs.Parameters["@tau_planoAula"].DbType = DbType.String;
            qs.Parameters["@tau_diarioClasse"].DbType = DbType.String;
            qs.Parameters["@tau_conteudo"].DbType = DbType.String;

            qs.Parameters.RemoveAt("@tau_dataCriacao");
            qs.Parameters["@tau_dataAlteracao"].Value = DateTime.Now;

            if (qs.Parameters["@pro_id"].Value.ToString() == Guid.Empty.ToString())
            {
                qs.Parameters["@pro_id"].Value = DBNull.Value;
            }

            if (qs.Parameters["@usu_id"].Value.ToString() == Guid.Empty.ToString())
                qs.Parameters["@usu_id"].Value = DBNull.Value;
        }

        /// <summary>
        /// Método alterado para que o update não faça a alteração da data de criação
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        protected override bool Alterar(CLS_TurmaAula entity)
        {
            __STP_UPDATE = "NEW_CLS_TurmaAula_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a exclusão lógica.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamDeletar(QueryStoredProcedure qs, CLS_TurmaAula entity)
        {
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
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tau_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@tau_dataAlteracao";
            Param.Size = 8;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Método alterado para que o delete não faça exclusão física e sim lógica (update).
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public override bool Delete(CLS_TurmaAula entity)
        {
            __STP_DELETE = "NEW_CLS_TurmaAula_Update_Situacao";
            return base.Delete(entity);
        }

        /// <summary>
        /// Recebe o valor do auto incremento e coloca na propriedade.
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure.</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros.</param>
        /// <returns>TRUE - Se entity.ParametroId > 0</returns>
        protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, CLS_TurmaAula entity)
        {
            if (entity != null & qs != null)
            {
                entity.tau_id = Convert.ToInt32(qs.Return.Rows[0][0]);
                return entity.tau_id > 0;
            }

            return false;
        }
    }
}