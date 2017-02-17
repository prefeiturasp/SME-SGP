using System;
using System.Collections.Generic;
using System.Linq;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Business.Common;
using MSTech.Validation.Exceptions;
using System.ComponentModel;
using System.Data;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estrutura

    /// <summary>
    /// Estrutura com período do calendário e seus dias não úteis
    /// </summary>
    public struct sCalendarioPeriodoEscolaDiasNaoUteis
    {
        public int esc_id { get; set; }
        public int uni_id { get; set; }
        public int cal_id { get; set; }
        public int tpc_id { get; set; }
        public DateTime cap_dataInicio { get; set; }
        public DateTime cap_dataFim { get; set; }
        public List<DateTime> listaDiaNaoUtil { get; set; }
    }

    /// <summary>
    /// Estrutura com períodos do calendário
    /// </summary>
    [Serializable]
    public struct Struct_CalendarioPeriodos
    {
        public int cal_id { get; set; }
        public int cap_id { get; set; }
        public string cap_descricao { get; set; }
        public DateTime cap_dataInicio { get; set; }
        public DateTime cap_dataFim { get; set; }
        public DateTime cal_dataInicio { get; set; }
        public DateTime cal_dataFim { get; set; }
        public int tpc_id { get; set; }
        public int tpc_ordem { get; set; }
        public string tpc_nomeAbreviado { get; set; }
        //public Guid ent_id { get; set; }
        //public int cal_ano { get; set; }
        //public string cal_descricao { get; set; }
        //public byte cal_situacao { get; set; }
        //public DateTime cal_dataCriacao { get; set; }
        //public DateTime cal_dataAlteracao { get; set; }
        //public DateTime cap_dataCriacao { get; set; }
        //public DateTime cap_dataAlteracao { get; set; }
        //public string tpc_nome { get; set; }
        //public bool tpc_foraPeriodoLetivo { get; set; }
        //public byte tpc_situacao { get; set; }
        //public DateTime tpc_dataCriacao { get; set; }
        //public DateTime tpc_dataAlteracao { get; set; }
        public string cap_periodo { get; set; }
        //public string qtidade_dnu { get; set; }
        public int cal_ano { get; set; }

    }

    /// <summary>
    /// Estrutura com as datas de início e fim de períodos do calendário e do calendário.
    /// </summary>
    public struct DatasPeriodosCalendario
    {
        public DateTime cap_dataInicio { get; set; }
        public DateTime cap_dataFim { get; set; }
        public DateTime cal_dataInicio { get; set; }
        public DateTime cal_dataFim { get; set; }
    }

    #endregion Estrutura

    public class ACA_CalendarioPeriodoBO : BusinessBase<ACA_CalendarioPeriodoDAO, ACA_CalendarioPeriodo>
    {
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
        /// <param name="avaliacaoTipo">Tipo da avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_dataInicio">Data de início do período</param>
        /// <param name="cap_dataFim">Data de fim do período</param>
        /// <returns></returns>
        public static void RetornaDatasPeriodoPor_FormatoAvaliacaoTurmaDisciplina
        (
            int tpc_id
            , string avaliacaoesRelacionadas
            , long tud_id
            , int fav_id
            , AvaliacaoTipo avaliacaoTipo
            , int cal_id
            , out DateTime cap_dataInicio
            , out DateTime cap_dataFim
        )
        {
            // Se for avaliação final, retorna a data de inicio e fim do calendário
            if (avaliacaoTipo == AvaliacaoTipo.Final || avaliacaoTipo == AvaliacaoTipo.ConselhoClasse)
            {
                ACA_CalendarioAnual cal = new ACA_CalendarioAnual { cal_id = cal_id };
                ACA_CalendarioAnualBO.GetEntity(cal);

                cap_dataInicio = cal.cal_dataInicio;
                cap_dataFim = cal.cal_dataFim;
            }
            // Se não for avaliação final, retorna a data de inicio e fim do periodo do calendario
            else
            {
                RetornaDatasPeriodoPor_FormatoAvaliacaoTurmaDisciplina(tpc_id, avaliacaoesRelacionadas, tud_id, fav_id, out cap_dataInicio, out cap_dataFim);
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
        /// <param name="cap_dataInicio">Data de início do período</param>
        /// <param name="cap_dataFim">Data de fim do período</param>
        /// <returns></returns>
        public static void RetornaDatasPeriodoPor_FormatoAvaliacaoTurmaDisciplina
        (
            int tpc_id
            , string avaliacaoesRelacionadas
            , long tud_id
            , int fav_id
            , out DateTime cap_dataInicio
            , out DateTime cap_dataFim
            , int appMinutosCacheLongo = 0
        )
        {

            DataTable dados = null;

            Func<DataTable> retorno = delegate()
            {
                ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
                dados = dao.SelectBy_FormatoAvaliacaoTurmaDisciplina(tpc_id, avaliacaoesRelacionadas, tud_id, fav_id);
                return dados;
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.CALENDARIO_ANUAL_FORMATOAVALIACAOTURMADISCIPLINA_MODEL_KEY, tpc_id, avaliacaoesRelacionadas, tud_id, fav_id);

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCacheLongo
                            );
            }
            else
            {
                dados = retorno();
            }

            if (dados.Rows.Count > 0)
            {
                cap_dataInicio = Convert.ToDateTime(dados.Rows[0]["cap_dataInicio"]);
                cap_dataFim = Convert.ToDateTime(dados.Rows[0]["cap_dataFim"]);
            }
            else
            {
                cap_dataInicio = new DateTime();
                cap_dataFim = new DateTime();
            }
           
        }

        /// <summary>
        /// Retorna a data de início e fim do período do calendário do tipo informado
        /// (quando informado o tpc_id). 
        /// Quando não informado o tpc_id, retorna o primeiro período de acordo
        /// com as avaliações relacionadas.
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <param name="cap_dataInicio">Data de início do período</param>
        /// <param name="cap_dataFim">Data de fim do período</param>
        /// <param name="UltimoPeriodo">Flag que indica se o período do calendário é o último cadastrado para o calendário</param>
        /// <returns></returns>
        public static void RetornaDatasPeriodoPor_FormatoAvaliacaoTurma
        (
            int tpc_id
            , long tur_id
            , int fav_id
            , TalkDBTransaction banco
            , out DateTime cap_dataInicio
            , out DateTime cap_dataFim
            , out bool UltimoPeriodo
        )
        {
            ACA_CalendarioPeriodoDAO dao = banco == null ? new ACA_CalendarioPeriodoDAO() : new ACA_CalendarioPeriodoDAO { _Banco = banco };

            DataTable dt = dao.SelectBy_FormatoAvaliacaoTurma(tpc_id, string.Empty, tur_id, fav_id);

            if (dt.Rows.Count > 0)
            {
                cap_dataInicio = Convert.ToDateTime(dt.Rows[0]["cap_dataInicio"]);
                cap_dataFim = Convert.ToDateTime(dt.Rows[0]["cap_dataFim"]);
                UltimoPeriodo = Convert.ToBoolean(dt.Rows[0]["UltimoPeriodo"]);
            }
            else
            {
                cap_dataInicio = new DateTime();
                cap_dataFim = new DateTime();
                UltimoPeriodo = false;
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
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="avaliacaoTipo">Tipo da avaliação</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_dataInicio">Data de início do período</param>
        /// <param name="cap_dataFim">Data de fim do período</param>
        /// <returns></returns>
        public static void RetornaDatasPeriodoPor_FormatoAvaliacaoTurma
        (
            int tpc_id
            , string avaliacaoesRelacionadas
            , long tur_id
            , int fav_id
            , AvaliacaoTipo avaliacaoTipo
            , int cal_id
            , out DateTime cap_dataInicio
            , out DateTime cap_dataFim
        )
        {
            // Se for avaliação final, retorna a data de inicio e fim do calendário
            if (avaliacaoTipo == AvaliacaoTipo.Final || avaliacaoTipo == AvaliacaoTipo.ConselhoClasse)
            {
                ACA_CalendarioAnual cal = new ACA_CalendarioAnual { cal_id = cal_id };
                ACA_CalendarioAnualBO.GetEntity(cal);

                cap_dataInicio = cal.cal_dataInicio;
                cap_dataFim = cal.cal_dataFim;
            }
            // Se não for avaliação final, retorna a data de inicio e fim do periodo do calendario
            else
            {
                RetornaDatasPeriodoPor_FormatoAvaliacaoTurma(tpc_id, avaliacaoesRelacionadas, tur_id, fav_id, out cap_dataInicio, out cap_dataFim);
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
        /// <param name="tur_id">ID da turma</param>
        /// <param name="fav_id">ID do formato de avaliação</param>
        /// <param name="cap_dataInicio">Data de início do período</param>
        /// <param name="cap_dataFim">Data de fim do período</param>
        /// <returns></returns>
        public static void RetornaDatasPeriodoPor_FormatoAvaliacaoTurma
        (
            int tpc_id
            , string avaliacaoesRelacionadas
            , long tur_id
            , int fav_id
            , out DateTime cap_dataInicio
            , out DateTime cap_dataFim
        )
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            DataTable dt = dao.SelectBy_FormatoAvaliacaoTurma(tpc_id, avaliacaoesRelacionadas, tur_id, fav_id);

            if (dt.Rows.Count > 0)
            {
                cap_dataInicio = Convert.ToDateTime(dt.Rows[0]["cap_dataInicio"]);
                cap_dataFim = Convert.ToDateTime(dt.Rows[0]["cap_dataFim"]);
            }
            else
            {
                cap_dataInicio = new DateTime();
                cap_dataFim = new DateTime();
            }
        }

        /// <summary>
        /// Seleciona os períodos do calendário anual. Caso o parâmentro idCalendarioAnual for menos que zero,
        /// seleciona o último calendário ativo para o ano atual.
        /// </summary>
        /// <param name="idCalendarioAnual">id do calendário anual(opcional)</param>
        /// <param name="idEntidade">id da entidade do usuário logado(obrigatório)</param>
        /// <returns>Lista de períodos do calendário.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IList<ACA_CalendarioPeriodo> SelecionaPeriodoPorCalendarioEntidade(int idCalendarioAnual, Guid idEntidade)
        {
            #region VALIDA PARAMETROS DE ENTRADA

            if (idEntidade.Equals(Guid.Empty))
                throw new ValidationException("Parâmetro idEntidade é obrigatório.");

            #endregion

            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.SelectByCalendarioEntidade(idCalendarioAnual, idEntidade);
        }

        /// <summary>
        /// Seleciona todos os calendários não excluídos logicamente.
        /// </summary>
        /// <returns>Lista de calendários</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodos()
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.SelecionaTodos();
        }

        /// <summary>
        /// Retorna os períodos do calendário, com as quantidades de aulas
        /// lançadas na disciplina.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <returns></returns>
        public static DataTable Seleciona_QtdeAulas_TurmaDiscplina(long tur_id, long tud_id, int cal_id, byte tdt_posicao = 0, long doc_id = 0)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.Select_QtdeAulas_TurmaDiscplina(tur_id, tud_id, cal_id, tdt_posicao, doc_id);
        }

        /// <summary>
        /// Retorna os períodos do calendário, com as quantidades de aulas lançadas nas disciplinas da turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns>DataTable com os dados selecionados.</returns>
        public static List<TUR_TurmaDisciplinaAulaPrevistaBO.QuantitativoTurmaDisciplina> Seleciona_QtdeAulas_Turma(long tur_id)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            DataTable dt = dao.Select_QtdeAulas_Turma(tur_id);
            List<TUR_TurmaDisciplinaAulaPrevistaBO.QuantitativoTurmaDisciplina> listaDados = dt.Rows.Cast<DataRow>().Select(dr =>
                        new TUR_TurmaDisciplinaAulaPrevistaBO.QuantitativoTurmaDisciplina
                        {
                            tud_id = Convert.ToInt64(dr["tud_id"]),
                            tud_nome = Convert.ToString(dr["tud_nome"]),
                            tpc_id = Convert.ToInt32(dr["tpc_id"]),
                            cap_descricao = Convert.ToString(dr["cap_descricao"]),
                            aulasPrevistas = Convert.ToInt32(dr["aulasPrevistas"]),
                            aulasDadas = Convert.ToInt32(dr["aulasDadas"]),
                            tud_tipo = Convert.ToByte(dr["tud_tipo"]),
                            experienciaVigente = Convert.ToBoolean(dr["experienciaVigente"])
                        }).ToList();

            return listaDados;
        }

        /// <summary>
        /// Retorna a data inicial e final do período do calendário
        /// por calendário e tipo de período do calendário
        /// </summary>                
        /// <param name="cal_id">ID do calendário</param> 
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTipoPeriodoCalendario(int cal_id, int tpc_id)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.SelectBy_cal_id_tpc_id(cal_id, tpc_id);
        }

        /// <summary>
        /// Retorna a data inicial e final do período do calendário e do calendário
        /// </summary>                
        /// <param name="cal_id">ID do calendário</param> 
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDatasCalendario(int cal_id, int tpc_id)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.SelecionaDatasCalendario(cal_id, tpc_id);
        }

        /// <summary>
        /// Retorna a data inicial e final do período do calendário e do calendário
        /// </summary>                
        /// <param name="cal_id">ID do calendário</param> 
        /// <param name="tpc_id">ID do tipo de período do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DatasPeriodosCalendario SelecionaDatasCalendario(int cal_id, int tpc_id, TalkDBTransaction banco)
        {
            DatasPeriodosCalendario ret = new DatasPeriodosCalendario();
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            if (banco != null)
                dao._Banco = banco;

            DataTable dt = dao.SelecionaDatasCalendario(cal_id, tpc_id);
            if (dt.Rows.Count > 0)
            {
                ret = (DatasPeriodosCalendario)GestaoEscolarUtilBO.DataRowToEntity(dt.Rows[0], new DatasPeriodosCalendario());
            }

            return ret;
        }

        /// <summary>
        /// Retorna os calendários da escola
        /// </summary>
        /// <param name="esc_id">ID da escola</param>        
        /// <returns></returns>
        public static DataTable BuscaCalendariosEscola(Int64 esc_id, DateTime syncDate)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.BuscaCalendariosEscola(esc_id, syncDate);
        }

        /// <summary>
        /// Retorna os calendários por entidade
        /// </summary>
        /// <param name="esc_id">Entidade</param>        
        /// <returns></returns>
        public static DataTable BuscaCalendariosEntidade(Guid Ent_id)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.BuscaCalendariosEntidade(Ent_id);
        }

        /// <summary>
        /// Retorna os tpc_id vigentes
        /// por calendário
        /// </summary>                
        /// <param name="cal_id">ID do calendário</param> 
        /// <param name="cap_datafim">Data fim da vigência</param>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoPeriodoPorCalendario(int cal_id, DateTime cap_datafim)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.SelectBy_cal_id_cap_datafim(cal_id, cap_datafim);
        }

        /// <summary>
        /// Retorna o período do calendário em que esteja a data informada.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public static void SelecionaPeriodoPorCalendarioData(int cal_id, DateTime data, out int tpc_id, out DateTime cap_dataInicio, out DateTime cap_dataFim)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            DataTable dt = dao.SelectBy_CalendarioData(cal_id, data);

            if (dt.Rows.Count > 0)
            {
                tpc_id = Convert.ToInt32(dt.Rows[0]["tpc_id"]);
                cap_dataInicio = Convert.ToDateTime(dt.Rows[0]["cap_dataInicio"]);
                cap_dataFim = Convert.ToDateTime(dt.Rows[0]["cap_dataFim"]);
            }
            else
            {
                tpc_id = 0;
                cap_dataInicio = new DateTime();
                cap_dataFim = new DateTime();
            }
        }

        /// <summary>
        /// Retorna o período do calendário referente ao tpc_id.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tpc_id">ID do tipo do período do calendário</param>
        /// <param name="AppMinutosCacheLongo">Quantidade de minutos da configuração de cache longo</param>
        /// <returns></returns>
        public static ACA_CalendarioPeriodo SelecionaPor_Calendario_TipoPeriodo(int cal_id, int tpc_id, int AppMinutosCacheLongo = 0)
        {
            ACA_CalendarioPeriodo entity = new ACA_CalendarioPeriodo();

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = String.Format(RetornaChaveCache_PeriodosCalendarioTipoCalendarioPeriodo()
                                             , cal_id
                                             , tpc_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();

                    var x = from DataRow dr in dao.SelectBy_Calendario(cal_id).Rows
                            where Convert.ToInt32(dr["tpc_id"]).Equals(tpc_id)
                            select dr;

                    if (x.Count() > 0)
                        entity = dao.DataRowToEntity(x.FirstOrDefault(), entity);

                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    entity = (ACA_CalendarioPeriodo)cache;
            }
            else
            {
                ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();

                var x = from DataRow dr in dao.SelectBy_Calendario(cal_id).Rows
                        where Convert.ToInt32(dr["tpc_id"]).Equals(tpc_id)
                        select dr;

                if (x.Count() > 0)
                    entity = dao.DataRowToEntity(x.FirstOrDefault(), entity);
            }

            return entity;
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_PeriodosCalendario(int cal_id)
        {
            return string.Format(ModelCache.PERIODO_CALENDARIO_POR_CALENDARIO_MODEL_KEY, cal_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_PeriodosCalendarioTipoCalendarioPeriodo()
        {
            return "Cache_SelecionaPeriodosCalendario_cal_id_{0}_tpc_id_{1}";
        }

        /// <summary>
        /// Retorna o período do calendário.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="AppMinutosCacheLongo">Quantidade de minutos da configuração de cache longo</param>
        /// <returns>List<Struct_CalendarioPeriodos></returns>
        public static List<Struct_CalendarioPeriodos> SelecionaPor_Calendario
        (
            int cal_id
            , int AppMinutosCacheLongo = 0
            , bool removerRecesso = true
            , Guid ent_id = new Guid()
        )
        {
            List<Struct_CalendarioPeriodos> dados = null;

            if (AppMinutosCacheLongo > 0)
            {
                string chave = RetornaChaveCache_PeriodosCalendario(cal_id);
                dados = CacheManager.Factory.Get
                            (
                                chave,
                                () =>
                                {
                                    return (from DataRow dr in new ACA_CalendarioPeriodoDAO().SelectBy_Calendario(cal_id).Rows
                                    select (Struct_CalendarioPeriodos)GestaoEscolarUtilBO.DataRowToEntity(dr, new Struct_CalendarioPeriodos())).ToList();
                                },
                                AppMinutosCacheLongo
                            );
            }
            else
                dados = (from DataRow dr in new ACA_CalendarioPeriodoDAO().SelectBy_Calendario(cal_id).Rows
                         select (Struct_CalendarioPeriodos)GestaoEscolarUtilBO.DataRowToEntity(dr, new Struct_CalendarioPeriodos())).ToList();

            if (removerRecesso && dados.Any())
            {
                return dados.Where(p => p.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id)).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna o período do calendário referente a turma disciplina.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina</param>
        /// <param name="cap_dataInicio"></param>
        /// <returns></returns>
        public static void SelecionaPor_Calendario_tud_id(long tud_id, out DateTime cap_dataInicio)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            DataTable dt = dao.SelectBy_tud_id(tud_id);

            cap_dataInicio = Convert.ToDateTime(dt.Rows[0]["cap_dataInicio"]);
        }

        /// <summary>
        /// Retorna os periodos do calendario que já foram efetivados
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matriculaTurma</param>
        /// <returns></returns>
        public static DataTable SelecionaPeriodosEfetivados(long alu_id, int mtu_id)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.SelecionaPeriodosEfetivados(alu_id, mtu_id);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable Seleciona_cal_id(int cal_id, bool paginado, int currentPage, int pageSize, bool removerRecesso = true, Guid ent_id = new Guid())
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            DataTable retorno = dao.SelectBy_cal_id(cal_id, paginado, currentPage / pageSize, pageSize, out totalRecords);

            if (removerRecesso && retorno.Rows.Count > 0)
            {
                retorno = retorno.AsEnumerable().Where(p => (int)p["tpc_id"] != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id)).CopyToDataTable();
            }

            return retorno;
        }

        /// <summary>
        /// Seleciona os períodos do(s) calendário(s), no intervalo de data informado.
        /// </summary>
        /// <param name="cal_ids">ids do calendário anual</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data de fim do periodo</param>
        /// <returns>Lista de períodos do calendário.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<int> SelecionaPeriodoCalendarioPorIntervaloData(string cal_ids, DateTime dataInicio, DateTime dataFim)
        {
            ACA_CalendarioPeriodoDAO dao = new ACA_CalendarioPeriodoDAO();
            return dao.SelecionaPeriodoCalendarioPorIntervaloData(cal_ids, dataInicio, dataFim);
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
        public static List<DateTime> SelecionaDiasNaoUteis(int esc_id, int uni_id, int cal_id, DateTime dataInicio, DateTime dataFim, Guid ent_id, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new ACA_CalendarioPeriodoDAO().SelecionaDiasNaoUteis(esc_id, uni_id, cal_id, dataInicio, dataFim, ent_id) :
                new ACA_CalendarioPeriodoDAO { _Banco = banco }.SelecionaDiasNaoUteis(esc_id, uni_id, cal_id, dataInicio, dataFim, ent_id);
        }

        /// <summary>
        ///// Remove do cache da consulta de recursos da aula
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tpc_id">ID do tipo do período do calendário</param>
        public static void RemoveCacheCalendarioPeriodo(int cal_id, int tpc_id)
        {
            if (cal_id > 0)
            {
                if (HttpContext.Current != null)
                {
                    string chave = RetornaChaveCache_PeriodosCalendario(cal_id);

                    if (HttpContext.Current.Cache[chave] != null)
                        HttpContext.Current.Cache.Remove(chave);

                    if (tpc_id > 0)
                    {
                        chave = string.Format(RetornaChaveCache_PeriodosCalendarioTipoCalendarioPeriodo(), cal_id, tpc_id);
                        
                        if (HttpContext.Current.Cache[chave] != null)
                            HttpContext.Current.Cache.Remove(chave);
                    }
                }
            }
        }

        /// <summary>
        /// Salva o período do calendário
        /// </summary>
        /// <param name="entityCalPeriodo">Entidade CalendarioPeriodo</param>
        /// <param name="banco">Transação</param>
        /// <returns>True em caso de sucesso</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save(ACA_CalendarioPeriodo entityCalPeriodo, TalkDBTransaction banco)
        {
            try
            {
                if (entityCalPeriodo.Validate())
                {
                    ACA_CalendarioPeriodoDAO CalPeriodoDAO = new ACA_CalendarioPeriodoDAO { _Banco = banco };

                    // Removendo o cache
                    RemoveCacheCalendarioPeriodo(entityCalPeriodo.cal_id, entityCalPeriodo.tpc_id);

                    CalPeriodoDAO.Salvar(entityCalPeriodo);
                }
                else
                {
                    throw new ValidationException(entityCalPeriodo.PropertiesErrorList[0].Message);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
