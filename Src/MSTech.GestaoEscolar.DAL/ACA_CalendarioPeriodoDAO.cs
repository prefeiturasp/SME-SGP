/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Data;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL.Abstracts;
using System.Linq;

namespace MSTech.GestaoEscolar.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ACA_CalendarioPeriodoDAO : Abstract_ACA_CalendarioPeriodoDAO
    {
        #region Métodos

        /// <summary>
        /// Retorna os períodos do calendário, com as quantidades de aulas
        /// lançadas na disciplina.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <returns></returns>
        public DataTable Select_QtdeAulas_TurmaDiscplina
        (
            long tur_id
            , long tud_id
            , int cal_id
            , byte tdt_posicao
            , long doc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_Seleciona_QtdeAulas_TurmaDiscplina", _Banco);
            
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
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@doc_id";
            Param.Size = 8;
            Param.Value = doc_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna os períodos do calendário, com as quantidades de aulas lançadas nas disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns>DataTable com os dados selecionados.</returns>
        public DataTable Select_QtdeAulas_Turma(long tur_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_Seleciona_QtdeAulas_Turma", _Banco);

            #region PARAMETROS
            
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

        /// <summary>
        /// Retorna o período do calendário em que esteja a data informada.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public DataTable SelectBy_CalendarioData
        (
            int cal_id
            , DateTime data
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_CalendarioData", _Banco);
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
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@data";
                Param.Size = 16;
                Param.Value = data;
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
        /// Retorna os calendários da escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>        
        /// <returns></returns>
        public DataTable BuscaCalendariosEscola
        (
            Int64 esc_id,
            DateTime syncDate
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_DCL_BuscaCalendariosEscola", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.Int64;
                Param.ParameterName = "@esc_id";
                Param.Size = 8;
                Param.Value = esc_id;
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
        /// Retorna os calendários da escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>        
        /// <returns></returns>
        public DataTable BuscaCalendariosEntidade
        (
            Guid ent_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("API_BuscaCalendariosEscola_Por_Entidade", _Banco);
            try
            {
                #region PARAMETROS

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
        /// Retorna a data de início e fim do período do calendário do tipo informado
        /// (quando informado o tpc_id). 
        /// Quando não informado o tpc_id, retorna o primeiro período de acordo
        /// com as avaliações relacionadas.
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="avaliacaoesRelacionadas">IDs das avaliações relacionadas (separadas por ",")</param>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <returns></returns>
        public DataTable SelectBy_FormatoAvaliacaoTurmaDisciplina
        (
            int tpc_id
            , string avaliacaoesRelacionadas
            , long tud_id
            , int fav_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_FormatoAvaliacaoTurmaDisciplina", _Banco);

            #region PARAMETROS

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
            Param.DbType = DbType.String;
            Param.ParameterName = "@avaliacaoesRelacionadas";
            if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
                Param.Value = DBNull.Value;
            else
                Param.Value = avaliacaoesRelacionadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tud_id";
            Param.Size = 8;
            Param.Value = tud_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a data de início e fim do período do calendário do tipo informado
        /// (quando informado o tpc_id). 
        /// Quando não informado o tpc_id, retorna o primeiro período de acordo
        /// com as avaliações relacionadas.
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="avaliacaoesRelacionadas">IDs das avaliações relacionadas (separadas por ",")</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <returns></returns>
        public DataTable SelectBy_FormatoAvaliacaoTurma
        (
            int tpc_id
            , string avaliacaoesRelacionadas
            , long tur_id
            , int fav_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_FormatoAvaliacaoTurma", _Banco);

            #region PARAMETROS

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
            Param.DbType = DbType.String;
            Param.ParameterName = "@avaliacaoesRelacionadas";
            if (string.IsNullOrEmpty(avaliacaoesRelacionadas))
                Param.Value = DBNull.Value;
            else
                Param.Value = avaliacaoesRelacionadas;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@tur_id";
            Param.Size = 8;
            Param.Value = tur_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@fav_id";
            Param.Size = 4;
            Param.Value = fav_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna a data de início e fim do período do calendário da turma disciplina informada
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <returns></returns>
        public DataTable SelectBy_tud_id
        (
            long tud_id   
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_tud_id", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@tud_id";
            Param.Size = 4;
            if (tud_id > 0)
                Param.Value = tud_id;
            else
                Param.Value = DBNull.Value;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }


        /// <summary>
        /// Retorna os periodos do calendario que já foram efetivados
        /// </summary>
        /// <param name="tur_id">ID da Turma</param>
        /// <param name="dataMov">Data de matricula</param>
        /// <returns></returns>
        public DataTable SelecionaPeriodosEfetivados(long alu_id, int mtu_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_RetornaPeriodosEfetivados", _Banco);

            #region PARAMETROS

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@alu_id";
            Param.Size = 8;
            Param.Value = alu_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int64;
            Param.ParameterName = "@mtu_id";
            Param.Size = 4;
            Param.Value = mtu_id;
            qs.Parameters.Add(Param);

            #endregion

            qs.Execute();

            return qs.Return;
        }

        /// <summary>
        /// Retorna uma lista de ACA_CurriculoPeriodo filtrado por entidade e calendário.
        /// Caso o parâmetro cal_id seja nulo retorna os dados calendário ativo para o ano atual 
        /// se houver.
        /// MÉTODO(S) DEPENDENTE(S):
        /// 1 - Classe: ACA_CalendarioPeriodoBO; Método: SelecionaPeriodoPorCalendarioEntidade
        /// </summary>
        /// <param name="cal_id">id do calendário anual</param>
        /// <param name="ent_id">id da entidade</param>
        /// <returns>List com objetos ACA_CurriculoPeriodo</returns>
        public IList<ACA_CalendarioPeriodo> SelectByCalendarioEntidade(int cal_id, Guid ent_id)
        {
            IList<ACA_CalendarioPeriodo> lt = new List<ACA_CalendarioPeriodo>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_CalendarioEntidade", _Banco);
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
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                foreach (DataRow dr in qs.Return.Rows)
                {
                    ACA_CalendarioPeriodo entity = new ACA_CalendarioPeriodo();
                    lt.Add(DataRowToEntity(dr, entity));
                }
                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }
        
        /// <summary>
        /// Seleciona todos os calendários não excluídos logicamente.
        /// </summary>
        /// <returns>Lista de calendários</returns>
        public DataTable SelecionaTodos()
        {            
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_All", _Banco);

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

        /// <summary>
        /// Retorna a data inicial e final do período do calendário
        /// por calendário e tipo de período do calendário
        /// </summary>                
        /// <param name="cal_id">ID do calendário</param> 
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        public DataTable SelectBy_cal_id_tpc_id
        (
            int cal_id
            , int tpc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_cal_id_tpc_id", _Banco);
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
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
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
        /// Retorna a data inicial e final do período do calendário e do calendário
        /// </summary>                
        /// <param name="cal_id">ID do calendário</param> 
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        public DataTable SelecionaDatasCalendario
        (
            int cal_id
            , int tpc_id
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelecionaDatasCalendario", _Banco);
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
                Param.ParameterName = "@tpc_id";
                Param.Size = 4;
                Param.Value = tpc_id;
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
        /// Retorna a data inicial e final do período do calendário
        /// por calendário e tipo de período do calendário
        /// </summary>                
        /// <param name="cal_id">ID do calendário</param>         
        /// <param name="cap_dataFim"></param>
        public DataTable SelectBy_cal_id_cap_datafim
        (
            int cal_id
            , DateTime cap_dataFim
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_cal_id_cap_datafim", _Banco);
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
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@cap_dataFim";
                Param.Size = 16;
                Param.Value = cap_dataFim;
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
        /// Retorna os períodos do calendário.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <returns></returns>
        public DataTable SelectBy_Calendario
           (
               int cal_id
           )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_cal_id", _Banco);

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

        public DataTable SelectBy_cal_id
        (
            int cal_id
            , bool paginado
            , int currentPage
            , int pageSize
            , out int totalRecords
        )
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_cal_id", _Banco);
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
        /// Seleciona os dias não úteis de um período do calendário anual.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cal_id">ID do calendário anual.</param>
        /// <param name="dataInicio">Data de ínicio.</param>
        /// <param name="dataFim">Data fim.</param>
        /// <param name="ent_id">Entidade do usuário logado.</param>
        /// <returns></returns>
        public List<DateTime> SelecionaDiasNaoUteis(int esc_id, int uni_id, int cal_id, DateTime dataInicio, DateTime dataFim, Guid ent_id)
        {
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelecionaDiasNaoUteis", _Banco);

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
                Param.ParameterName = "@cal_id";
                Param.Size = 4;
                Param.Value = cal_id;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicio";
                Param.Size = 16;
                Param.Value = dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFim";
                Param.Size = 16;
                Param.Value = dataFim;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.Guid;
                Param.ParameterName = "@ent_id";
                Param.Size = 16;
                Param.Value = ent_id;
                qs.Parameters.Add(Param);

                #endregion Parâmetros

                qs.Execute();

                return qs.Return.Rows.Cast<DataRow>().Select(dr => Convert.ToDateTime(dr["Dia"])).ToList();
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        /// <summary>
        /// Seleciona os períodos do(s) calendário(s), no intervalo de data informado.
        /// </summary>
        /// <param name="cal_ids">ids do calendário anual</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data de fim do periodo</param>
        /// <returns>Lista de períodos do calendário.</returns>
        public List<int> SelecionaPeriodoCalendarioPorIntervaloData(string cal_ids, DateTime dataInicio, DateTime dataFim)
        {
            List<int> lt = new List<int>();
            QuerySelectStoredProcedure qs = new QuerySelectStoredProcedure("NEW_ACA_CalendarioPeriodo_SelectBy_IntervaloData", _Banco);
            try
            {
                #region PARAMETROS

                Param = qs.NewParameter();
                Param.DbType = DbType.AnsiString;
                Param.ParameterName = "@cal_ids";
                Param.Value = cal_ids;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataInicio";
                Param.Size = 16;
                Param.Value = dataInicio;
                qs.Parameters.Add(Param);

                Param = qs.NewParameter();
                Param.DbType = DbType.DateTime;
                Param.ParameterName = "@dataFim";
                Param.Size = 16;
                Param.Value = dataFim;
                qs.Parameters.Add(Param);

                #endregion

                qs.Execute();
                foreach (DataRow dr in qs.Return.Rows)
                {
                    lt.Add((int)dr["tpc_id"]);
                }
                return lt;
            }
            finally
            {
                qs.Parameters.Clear();
            }
        }

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Parâmetros para efetuar a inclusão preservando a data de criação
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="entity"></param>
        protected override void ParamInserir(QuerySelectStoredProcedure qs, ACA_CalendarioPeriodo entity)
        {
            entity.cap_dataCriacao = DateTime.Now;
            entity.cap_dataAlteracao = DateTime.Now;

            base.ParamInserir(qs, entity);
        }

        /// <summary>
        /// Inseri os valores da classe em um novo registro
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem inseridos</param>
        /// <returns>True - Operacao bem sucedida</returns>
        protected override bool Inserir(ACA_CalendarioPeriodo entity)
        {
            __STP_INSERT = "STP_ACA_CalendarioPeriodo_INSERT";
            return base.Inserir(entity);
        }

        /// <summary>
        /// Parâmetros para efetuar a alteração preservando a data de criação
        /// </summary>
        protected override void ParamAlterar(QueryStoredProcedure qs, ACA_CalendarioPeriodo entity)
        {
            entity.cap_dataAlteracao = DateTime.Now;
            
            base.ParamAlterar(qs, entity);

            qs.Parameters.RemoveAt("@cap_dataCriacao");
        }
        /// <summary>
        /// Inseri os valores da classe em um registro ja existente
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem modificados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        protected override bool Alterar(ACA_CalendarioPeriodo entity)
        {
            __STP_UPDATE = "NEW_ACA_CalendarioPeriodo_UPDATE";
            return base.Alterar(entity);
        }

        /// <summary>
        /// Configura os parametros do metodo de Deletar
        /// </summary>
        /// <param name="qs">Objeto da Store Procedure</param>
        /// <param name="entity">Entidade com os dados para preenchimento dos parametros</param>
        protected override void ParamDeletar(QueryStoredProcedure qs, ACA_CalendarioPeriodo entity)
        {
            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cal_id";
            Param.Size = 4;
            Param.Value = entity.cal_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Int32;
            Param.ParameterName = "@cap_id";
            Param.Size = 4;
            Param.Value = entity.cap_id;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.Byte;
            Param.ParameterName = "@cap_situacao";
            Param.Size = 1;
            Param.Value = 3;
            qs.Parameters.Add(Param);

            Param = qs.NewParameter();
            Param.DbType = DbType.DateTime;
            Param.ParameterName = "@cap_dataAlteracao";
            Param.Size = 16;
            Param.Value = DateTime.Now;
            qs.Parameters.Add(Param);
        }

        /// <summary>
        /// Exclui um registro do banco
        /// </summary>
        /// <param name="entity">Entidade com os dados a serem apagados</param>
        /// <returns>True - Operacao bem sucedida</returns>
        public override bool Delete(ACA_CalendarioPeriodo entity)
        {
            __STP_DELETE = "NEW_ACA_CalendarioPeriodo_UPDATEBy_Situacao";
            return base.Delete(entity);
        }

        #endregion
    }
}