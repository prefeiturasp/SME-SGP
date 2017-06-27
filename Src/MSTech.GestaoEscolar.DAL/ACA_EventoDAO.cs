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

    /// <summary>
    /// 
    /// </summary>
    public class ACA_EventoDAO : Abstract_ACA_EventoDAO
    {
        /// <summary>
        /// torna os eventos cadastrados no calendário
        /// que estejam marcados como sem atividade discente.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <returns></returns>
        public DataTable Select_SemAtividadeDiscente_PorCalendario(Int32 cal_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Evento_Select_SemAtividadeDiscente_PorCalendario", _Banco);

            #region PARAMETROS

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evt_id"></param>
        /// <param name="tev_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="evt_nome"></param>
        /// <param name="evt_situacao"></param>
        /// <param name="ent_id"></param>
        /// <param name="evt_padrao"></param>
        /// <param name="paginado"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <param name="uad_idSuperior"></param>
        /// <returns></returns>
        public DataTable SelectBy_All(long evt_id, int tev_id, int esc_id, int uni_id, string evt_nome, byte evt_situacao, Guid ent_id, Guid @uad_idSuperior, Int16 evt_padrao, bool paginado, int currentPage, int pageSize, out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Evento_Select", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@evt_id";
                Param.Size = 4;
                if (evt_id > 0)
                    Param.Value = evt_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Size = 4;
                if (tev_id > 0)
                    Param.Value = tev_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@evt_nome";
                if (!String.IsNullOrEmpty(evt_nome))
                    Param.Value = evt_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@evt_situacao";
                Param.Size = 1;
                if (evt_situacao > 0)
                    Param.Value = evt_situacao;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@evt_padrao";
                Param.Value = evt_padrao;
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
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes. Retorna eventos 
        /// padrão ou que sejam da escola da turma informada.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="tur_id">Id da turma para filtrar as escolas do evento - Obrigatório</param>
        /// <returns>Tabela com os campos: cal_id, evt_id</returns>
        public DataTable Select_EventoEfetivacao(Int32 cal_id, Int64 tur_id)
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioEvento_Efeticao", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

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
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes. Retorna eventos 
        /// padrão ou que sejam da escola da turma informada.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="tur_id">Id da turma para filtrar as escolas do evento - Obrigatório</param>
        /// <param name="tpc_idFiltrar">ID do período do calendário para filtrar eventos - traz eventos relacionados à esse período do calendário.</param>
        /// <param name="tev_id"></param>
        /// <returns>Tabela com os campos: cal_id, evt_id</returns>
        public DataTable Select_EventoEfetivacaoTodos(Int32 cal_id, Int64 tur_id, String tev_id, int tpc_idFiltrar = -1)
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioEvento_Efeticao_Todos", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tev_id";
                Param.Value = tev_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                if (tpc_idFiltrar > 0)
                    Param.Value = tpc_idFiltrar;
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
        /// Busca os eventos ligados ao calendário seja padrao ou nao, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes, por escola
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="cap_id">Id do período do calendário</param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="tev_ids"></param>
        /// <returns>Tabela com os campos: cal_id, evt_id</returns>
        public DataTable Select_EventoEfetivacaoTodosPorPeriodoEscola(int cal_id, int cap_id, int esc_id, int uni_id, string tev_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioEvento_Efetivacao_PorPeriodoCalendarioEscola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cap_id";
                Param.Size = 4;
                Param.Value = cap_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tev_ids";
                Param.Value = tev_ids;
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


        public DataTable SelectBy_Busca
        (
            long evt_id
            , int tev_id
            , int esc_id
            , int uni_id
            , string evt_nome
            , int cal_id
            , byte evt_situacao
            , Guid ent_id
            , Guid uad_idSuperior
            , Int16 evt_padrao
            , Guid usu_id
            , Guid gru_id
            , bool adm
            , bool MostraCodigoEscola
            , out int totalRecords)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Evento_Select_Busca", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@evt_id";
                Param.Size = 4;
                if (evt_id > 0)
                    Param.Value = evt_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Size = 4;
                if (tev_id > 0)
                    Param.Value = tev_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@evt_nome";
                if (!String.IsNullOrEmpty(evt_nome))
                    Param.Value = evt_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cal_id";
                if (cal_id > 0)
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@evt_situacao";
                Param.Size = 1;
                if (evt_situacao > 0)
                    Param.Value = evt_situacao;
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@uad_idSuperior";
                Param.Size = 16;
                if (uad_idSuperior != Guid.Empty)
                    Param.Value = uad_idSuperior;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Byte;
                Param.ParameterName = "@evt_padrao";
                Param.Value = evt_padrao;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@MostraCodigoEscola";
                Param.Size = 1;
                Param.Value = MostraCodigoEscola;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                totalRecords = qs.Return.Rows.Count;

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
        /// 
        /// </summary>
        /// <param name="evt_nome"></param>
        /// <param name="evt_id"></param>
        /// <param name="ent_id"></param>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <returns></returns>
        public bool SelectBy_Name(string evt_nome, long evt_id, int esc_id, int uni_id, Guid ent_id, string cal_ids)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Evento_SelectBy_Nome", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@evt_nome";
                Param.Size = 200;
                if (!string.IsNullOrEmpty(evt_nome))
                    Param.Value = evt_nome;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@evt_id";
                Param.Size = 8;
                if (evt_id > 0)
                    Param.Value = evt_id;
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
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cal_ids";
                Param.Value = cal_ids;
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
        /// Seleciona a maior data entre os eventos para a escola, período de tipo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="tpc_id">ID do tipo de período do calendário.</param>
        /// <param name="tev_id">Tipos de eventos.</param>
        /// <returns></returns>
        public DateTime SelecionaMaiorDataPorTipoPeriodoEscola(int esc_id, int uni_id, int tpc_id, string tev_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Evento_SelecionaMaximaDataPorTipoPeriodoEscola", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                Param.Value = esc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                Param.Value = uni_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@tev_id";
                Param.Value = tev_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return Convert.ToDateTime(qs.Return.Rows[0]["data"].ToString());
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona o evento do calendário
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id">ID do tipo de período</param>
        /// <returns></returns>
        public DataTable Select_EventoLiberacao(Int32 cal_id, Int64 tur_id, Int32 tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioEvento_LiberacaoBoletimOnline", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

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
                if (tpc_id > 0)
                    Param.Value = tpc_id;
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
        /// Busca os eventos ligados ao calendário, que sejam do tipo definido
        /// no parâmetro como de efetivação, e que estejam vigentes. Retorna eventos 
        /// padrão ou que sejam da escola da turma informada.
        /// </summary>
        /// <param name="cal_id">Id do calendário - Obrigatório</param>
        /// <param name="tur_id">Id da turma para filtrar as escolas do evento - Obrigatório</param>
        /// <param name="tpc_id">ID do período do calendário para filtrar eventos - traz eventos relacionados à esse período do calendário.</param>
        /// <param name="tev_id"></param>
        /// <returns></returns>
        public DataTable Select_TodosEventosPorTipo_CalendarioPeriodo(Int32 cal_id, Int64 tur_id, int tev_id, int tpc_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioEvento_SelectAllBy_Tipo_CalendarioPeriodo", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@tur_id";
                Param.Size = 8;
                Param.Value = tur_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Size = 4;
                Param.Value = tev_id;
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
        /// Verifica se existem aulas criadas no calendário da escola entre as datas informadas
        /// </summary>
        /// <param name="uni_id">Lista com os ID's de calendário.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="data_ini">Data inicial</param>
        /// <param name="data_fim">Data final</param>
        /// <returns>Flag indicando se foram encontradas aulas.</returns>
        public bool VerificaAulaPorCalendarioEscolaData(string cal_id, int esc_id, DateTime data_ini, DateTime data_fim)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioEvento_VerificaAula", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cal_id";
                if (!String.IsNullOrEmpty(cal_id))
                    Param.Value = cal_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@data_ini";
                Param.Size = 16;
                Param.Value = data_ini;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@data_fim";
                Param.Size = 16;
                Param.Value = data_fim;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();

                return (qs.Return.Rows.Count > 0);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os eventos por calendários, unidades escolares e tipo de evento.
        /// </summary>
        /// <param name="cal_ids">Ids dos calendários anuais.</param>
        /// <param name="esc_id">Id da escola.</param>
        /// <param name="uni_id">Id da unidade escolar.</param>
        /// <param name="tev_id">Id do tipo de evento.</param>
        /// <returns></returns>
        public DataTable SelecionaPorCalendarioEscolaTipoEvento(string cal_ids, int esc_id, int uni_id, int tev_id, bool apenasVigentes)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Evento_SelecionaPorCalendarioEscolaTipoEvento", _Banco);

            try
            {
                #region Parâmetros

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cal_ids";
                Param.Value = cal_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@esc_id";
                Param.Size = 4;
                if (esc_id > 0)
                    Param.Value = esc_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@uni_id";
                Param.Size = 4;
                if (uni_id > 0)
                    Param.Value = uni_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Size = 4;
                if (tev_id > 0)
                    Param.Value = tev_id;
                else
                    Param.Value = DBNull.Value;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@Vigente";
                Param.Size = 1;
                Param.Value = apenasVigentes;
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
        /// Retorna se existem eventos vigentes de acordo com o tipo de evento e usuário.
        /// </summary>
        /// <param name="tev_id"></param>
        /// <param name="adm"></param>
        /// <param name="usu_id"></param>
        /// <param name="gru_id"></param>
        /// <param name="doc_id"></param>
        /// <param name="cal_ano"></param>
        /// <returns></returns>
        public string VerificaEventoVigentePorUsuario(int tev_id, bool adm, Guid usu_id, Guid gru_id, long doc_id, int cal_ano)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_Evento_VerificaEventoVigentePorUsuario", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@tev_id";
                Param.Size = 4;
                Param.Value = tev_id;
                qs.Parameters.Add(Param);                

                Param = qs.NewParameter();
                Param.DbType = DbType.Boolean;
                Param.ParameterName = "@adm";
                Param.Size = 1;
                Param.Value = adm;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@usu_id";
                Param.Size = 16;
                Param.Value = usu_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@gru_id";
                Param.Size = 16;
                Param.Value = gru_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@doc_id";
                Param.Size = 8;
                Param.Value = doc_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Int32;
                Param.ParameterName = "@cal_ano";
                Param.Size = 4;
                if (cal_ano > 0)
                {
                    Param.Value = cal_ano;
                }
                else
                {
                    Param.Value = DBNull.Value;
                }
                qs.Parameters.Add(Param);

                #endregion PARAMETROS

                qs.Execute();

                string retorno = string.Empty;
                foreach (DataRow evt in qs.Return.Rows)
                {
                    retorno += string.IsNullOrEmpty(retorno) ? evt["evt_id"].ToString() : string.Format(",{0}", evt["evt_id"].ToString());
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

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
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Evento entity)
        {
            base.ParamInserir(qs, entity);

            qs.Parameters["@evt_dataCriacao"].Value = DateTime.Now;
            qs.Parameters["@evt_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_Evento entity)
        {
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@evt_dataCriacao");
            qs.Parameters["@evt_dataAlteracao"].Value = DateTime.Now;
        }

        /// <summary>
        /// Inseri os valores da classe em um registro ja existente
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        protected override bool Alterar(ACA_Evento entity)
        {
            __STP_UPDATE = "NEW_ACA_Evento_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_Evento entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@evt_id";
            Param.Size = 4;
            Param.Value = entity.evt_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@evt_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@evt_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Exclui um registro do banco
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem apagados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        public override bool Delete(ACA_Evento entity)
        {
            __STP_DELETE = "NEW_ACA_Evento_UPDATEBy_Situacao";
            return base.Delete(entity);
        }

        #endregion

        #region Comentados

        ///// <summary>
        ///// Inseri os valores da classe em um novo registro
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem inseridos</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool Inserir(ACA_Evento entity)
        //{

        //}
        ///// <summary>
        ///// Carrega um registro da tabela usando os valores nas chaves
        ///// </summary>
        ///// <param name="entity">Entidade com os dados a serem carregados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Carregar(ACA_Evento entity)
        //{
        //    return base.Carregar(entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Carregar
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamCarregar(QuerySelectStoredProcedure qs, ACA_Evento entity)
        //{
        //    base.ParamCarregar(qs, entity);
        //}
        ///// <summary>
        ///// Configura os parametros do metodo de Inserir
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        //protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_Evento entity)
        //{
        //}
        ///// <summary>
        ///// Salva o registro no banco de dados
        ///// </summary>
        ///// <param name="entity">Entidade com os dados para preenchimento para inserir ou alterar</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //public override bool Salvar(ACA_Evento entity)
        //{
        //    return base.Salvar(entity);
        //}
        ///// <summary>
        ///// Realiza o select da tabela
        ///// </summary>
        ///// <returns>Lista com todos os registros da tabela</returns>
        //public override IList<ACA_Evento> Select()
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
        //public override IList<ACA_Evento> Select_Paginado(int currentPage, int pageSize, out int totalRecord)
        //{
        //    return base.Select_Paginado(currentPage, pageSize, out totalRecord);
        //}
        ///// <summary>
        ///// Recebe o valor do auto incremento e coloca na propriedade 
        ///// </summary>
        ///// <param name="qs">Objeto da Store Procedure</param>
        ///// <param name="entity">Entidade com os dados</param>
        ///// <returns>True - Operacao bem sucedida</returns>
        //protected override bool ReceberAutoIncremento(QuerySelectStoredProcedure qs, ACA_Evento entity)
        //{
        //    return base.ReceberAutoIncremento(qs, entity);
        //}
        ///// <summary>
        ///// Passa os dados de um datatable para uma entidade
        ///// </summary>
        ///// <param name="dr">DataRow do datatable preenchido</param>
        ///// <param name="entity">Entidade onde ser�o transferidos os dados</param>
        ///// <returns>Entidade preenchida</returns>
        //public override ACA_Evento DataRowToEntity(DataRow dr, ACA_Evento entity)
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
        //public override ACA_Evento DataRowToEntity(DataRow dr, ACA_Evento entity, bool limparEntity)
        //{
        //    return base.DataRowToEntity(dr, entity, limparEntity);
        //}

        #endregion
    }
}