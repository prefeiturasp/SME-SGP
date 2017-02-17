using System;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.ComponentModel;
using MSTech.Business.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.Data.Common;
using System.Collections.Generic;
using System.Linq;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Estruturas

    /// <summary>
    /// Dados do tipo periodo calendário.
    /// </summary>
    public struct sTipoPeriodoCalendario {
        public int tpc_id { get; set; }
        public string tpc_nome { get; set; }
        public string tpc_nomeAbreviado { get; set; }
        public int tpc_ordem { get; set; }
        public string tpc_situacao { get; set; }
        public string tpc_foraPeriodoLetivo { get; set; }
        public int cap_id { get; set; }
        public string cap_descricao { get; set; }
        public DateTime cap_dataInicio { get; set; }
        public DateTime cap_dataFim { get; set; }
        public int PeriodoAtual { get; set; }
        public string tpc_cap_id { get; set; }
    }

    [Serializable]
    public struct sComboPeriodoCalendario
    {
        public int tpc_id { get; set; }
        public string tpc_nome { get; set; }
        public string cap_descricao { get; set; }
        public bool PeriodoAtual { get; set; }
        public string tpc_cap_id { get; set; }
    }

    #endregion Estruturas

    public class ACA_TipoPeriodoCalendarioBO : BusinessBase<ACA_TipoPeriodoCalendarioDAO, ACA_TipoPeriodoCalendario>
    {
        public const string chaveCache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente = "Cache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente";

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(int cal_id, long tud_id, int tev_idEfetivacao, long tur_id, bool VerificaEscolaCalendarioPeriodo, long doc_id)
        {
            return string.Format(chaveCache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente + "_{0}_{1}_{2}_{3}_{4}_{5}", cal_id, tud_id, tev_idEfetivacao, tur_id, VerificaEscolaCalendarioPeriodo, doc_id);
        }

        /// <summary>
        /// Retorna os tipos de períodos do calendário que possuem alguma avaliação cadastrada para o tipo 
        /// período dele no formato de avaliação selecionado
        /// </summary>        
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="fav_id"></param>         
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaCalendarioComAvaliacao(int cal_id, int fav_id)
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelectByCalendarioComAvaliacao(cal_id, fav_id);
        }

        /// <summary>
        /// Retorna todos os tipos de período do calendário não excluídos logicamente
        /// Sem paginação
        /// </summary>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTipoPeriodoCalendario> SelecionaTipoPeriodoCalendario(int AppMinutosCacheLongo = 0, bool removerRecesso = true, Guid ent_id = new Guid())
        {
            List<sTipoPeriodoCalendario> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = "Cache_SelecionaTipoPeriodoCalendario";
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = (from dr in new ACA_TipoPeriodoCalendarioDAO().SelectBy_Pesquisa(false, 1, 1, out totalRecords).AsEnumerable()
                             select (sTipoPeriodoCalendario)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoPeriodoCalendario())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTipoPeriodoCalendario>)cache;
            }
            else
                lista = (from dr in new ACA_TipoPeriodoCalendarioDAO().SelectBy_Pesquisa(false, 1, 1, out totalRecords).AsEnumerable()
                         select (sTipoPeriodoCalendario)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoPeriodoCalendario())).ToList();

            if (removerRecesso && lista.Any())
            {
                return lista.Where(p => p.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id)).ToList();
            }

            return lista;
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente por calendário
        /// </summary>        
        /// <param name="cal_id">ID do calendário</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTipoPeriodoCalendario> SelecionaTipoPeriodoCalendarioPorCalendario
        (
            int cal_id
            , int AppMinutosCacheLongo = 0
        )
        {
            List<sTipoPeriodoCalendario> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = string.Format("Cache_SelecionaTipoPeriodoCalendarioPorCalendario_{0}", cal_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = (from dr in new ACA_TipoPeriodoCalendarioDAO().SelectBy_cal_id(0, cal_id).AsEnumerable()
                             select (sTipoPeriodoCalendario)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoPeriodoCalendario())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTipoPeriodoCalendario>)cache;
            }
            else
                lista = (from dr in new ACA_TipoPeriodoCalendarioDAO().SelectBy_cal_id(0, cal_id).AsEnumerable()
                         select (sTipoPeriodoCalendario)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoPeriodoCalendario())).ToList();

            return lista;
        }

        /// <summary>
        /// Seleciona os cal_ids ligados ao tpc_id informado para limpar cache
        /// </summary>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public static List<int> SelecionaCalendarioPorTipoPeriodoCalendario(int tpc_id)
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            DataTable dtDados = dao.SelecionaCalendarioPorTipoPeriodoCalendario(tpc_id);
            return (from DataRow dr in dtDados.Rows
                    select string.IsNullOrEmpty(dr["cal_id"].ToString()) ? -1 : Convert.ToInt32(dr["cal_id"])).ToList();
        }

        /// <summary>
        /// Retorna os tipos de período do calendário cadastrados no calendário.
        /// Quando informada a turma (tur_id), traz todas as marcações por tud_id
        /// em cada período do calendário.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_id">ID da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoPeriodoCalendarioPorCalendario_MarcacaoTurmas
        (
            int cal_id
            , long tur_id
        )
        {
            // Buscar do banco e guardar em cache.
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            DataTable dt = dao.SelectBy_Calendario_MarcacaoTurmas(cal_id, tur_id);

            return dt;
        }

        /// <summary>
        /// Carrega os tipos de período calendário não excluídos logicamente
        /// que estão em fechamento e o atual.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoPeriodoCalendarioAtualFechamentoPorCalendario(int cal_id)
        {
            // Buscar do banco e guardar em cache.
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            DataTable dt = dao.Select_AtualFechamento_By_Calendario(cal_id);

            return dt;
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente por calendário
        /// </summary>        
        /// <param name="tur_id">ID do calendário</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoPeriodoCalendario_Fav_Tur
        (
            long tur_id
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelectBy_FAV_Tur(tur_id);
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente por turma.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns></returns>
        public static List<sTipoPeriodoCalendario> SelecionaTipoPeriodoCalendario_Tur(long tur_id, int AppMinutosCacheLongo = 0)
        {
            List<sTipoPeriodoCalendario> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                string chave = string.Format("Cache_SelecionaTipoPeriodoCalendario_Tur_{0}", tur_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = (from dr in new ACA_TipoPeriodoCalendarioDAO().SelectBy_Tur(tur_id).AsEnumerable()
                             select (sTipoPeriodoCalendario)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoPeriodoCalendario())).ToList();

                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTipoPeriodoCalendario>)cache;
            }
            else
                lista = (from dr in new ACA_TipoPeriodoCalendarioDAO().SelectBy_Tur(tur_id).AsEnumerable()
                         select (sTipoPeriodoCalendario)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTipoPeriodoCalendario())).ToList();

            return lista;
        }
        
        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente por calendário
        /// </summary>        
        /// <param name="tpc_id">ID do tipo de período do calendário</param> 
        /// <param name="cal_id">ID do calendário</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoPeriodoCalendarioPorTipoPeriodoCalendario
        (
            int tpc_id
            , int cal_id
            , TalkDBTransaction banco = null
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            if (banco != null) dao._Banco = banco;
            return dao.SelectBy_cal_id(tpc_id, cal_id);
        }

        /// <summary>
        /// Retorna todos os tipos de período do calendário não excluídos logicamente até a data atual
        /// </summary>
        /// <param name="cal_id">ID do tipo de período do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable CarregarPeriodosAteDataAtual
        (
            int cal_id
            , long tud_id
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelecionarPeriodosAteDataAtual(cal_id, tud_id);
        }

        /// <summary>
        /// Retorna todos os tipos de período do calendário não excluídos logicamente até a data atual por turma
        /// </summary>
        /// <param name="cal_id">ID do tipo de período do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable CarregarPeriodosAteDataAtualPorTurma
        (
            int cal_id
            , long tur_id
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelecionarPeriodosAteDataAtualPorTurma(cal_id, tur_id);
        }

        /// <summary>
        /// Carrega os períodos do calendário de acordo com o calendário, e quando for
        /// disciplina eletiva ou optativa ou eletiva do aluno, somente os períodos que a disciplina oferece.
        /// </summary>
        /// <param name="cal_id">ID do tipo de período do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma - obrigatório</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTodosPor_EventoEfetivacao
        (
            int cal_id
            , long tud_id
            , long tur_id
            , Guid ent_id
            , TalkDBTransaction banco = null
            , long doc_id = -1
        )
        {
            // Tipo de evento de efetivação de notas, configurado nos parâmetros.
            int tev_idEfetivacao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);

            return banco == null ?
                new ACA_TipoPeriodoCalendarioDAO().SelecionaTodos_EventoEfetivacao(cal_id, tud_id, tev_idEfetivacao, tur_id, doc_id) :
                new ACA_TipoPeriodoCalendarioDAO { _Banco = banco }.SelecionaTodos_EventoEfetivacao(cal_id, tud_id, tev_idEfetivacao, tur_id, doc_id);
        }

        /// <summary>
        /// Carrega os períodos do calendário de acordo com o calendário, e quando for
        /// disciplina eletiva ou optativa ou eletiva do aluno, somente os períodos que a disciplina oferece.
        /// Traz períodos que estejam vigentes (período atual), ou se houver um evento de efetivação
        /// vigente ligado ao tpc_id.
        /// </summary>
        /// <param name="cal_id">ID do tipo de período do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma - obrigatório</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodoCalendario> SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente
        (
            int cal_id
            , long tud_id
            , long tur_id
            , Guid ent_id
            , bool VerificaEscolaCalendarioPeriodo
            , int appMinutosCacheLongo = 0
            , long doc_id = -1
        )
        {
            // Tipo de evento de efetivação de notas, configurado nos parâmetros.
            int tev_idEfetivacao = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id);
            List<sComboPeriodoCalendario> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(cal_id, tud_id, tev_idEfetivacao, tur_id, VerificaEscolaCalendarioPeriodo, doc_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
                        DataTable dtDados = dao.SelectBy_PeriodoVigente_EventoEfetivacaoVigente(cal_id, tud_id, tev_idEfetivacao, tur_id, VerificaEscolaCalendarioPeriodo, doc_id);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboPeriodoCalendario
                                 {
                                     tpc_id = string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? -1 : Convert.ToInt32(dr["tpc_id"]),
                                     tpc_nome = dr["tpc_nome"].ToString(),
                                     cap_descricao = dr["cap_descricao"].ToString(),
                                     tpc_cap_id = dr["tpc_cap_id"].ToString(),
                                     PeriodoAtual = string.IsNullOrEmpty(dr["PeriodoAtual"].ToString()) ? false : Convert.ToBoolean(dr["PeriodoAtual"])
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboPeriodoCalendario>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
                DataTable dtDados = dao.SelectBy_PeriodoVigente_EventoEfetivacaoVigente(cal_id, tud_id, tev_idEfetivacao, tur_id, VerificaEscolaCalendarioPeriodo, doc_id);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboPeriodoCalendario
                         {
                                     tpc_id = string.IsNullOrEmpty(dr["tpc_id"].ToString()) ? -1 : Convert.ToInt32(dr["tpc_id"]),
                                     tpc_nome = dr["tpc_nome"].ToString(),
                                     cap_descricao = dr["cap_descricao"].ToString(),
                                     tpc_cap_id = dr["tpc_cap_id"].ToString(),
                                     PeriodoAtual = string.IsNullOrEmpty(dr["PeriodoAtual"].ToString()) ? false : Convert.ToBoolean(dr["PeriodoAtual"])
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Carrega os períodos do calendário de acordo com o calendário, e quando for
        /// disciplina eletiva ou optativa ou eletiva do aluno, somente os períodos que a disciplina oferece.
        /// Traz períodos que estejam vigentes (período atual), ou se houver um evento de efetivação
        /// vigente ligado ao tpc_id.
        /// </summary>
        /// <param name="cal_id">ID do tipo de período do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tur_id">ID da turma - obrigatório</param>
        /// <param name="VerificaEscolaCalendarioPeriodo">Informa se irá selecionar todos os dados conforme os filtros (false) 
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// ou se irá selecionar apenas os dados que não estão na tabela ESC_EscolaCalendarioPeriodo (true)</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboPeriodoCalendario> SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente
        (
            int cal_id
            , long tud_id
            , long tur_id
            , Guid ent_id
            , int appMinutosCacheLongo = 0
            , long doc_id = -1
        )
        {
            return SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente(cal_id, tud_id, tur_id, ent_id, false, appMinutosCacheLongo, doc_id);
        }

        /// <summary>
        /// Retorna a data final do período do calendário atual
        /// </summary>
        /// <param name="cal_id">ID do tipo de período do calendário</param>                
        /// <param name="dataMov">Data que ocorre o evento</param>       
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DateTime SelecionaDataFinalPeriodoCalendarioAtual
        (
            int cal_id,
            DateTime dataMov
       
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelectBy_Calendario(cal_id,dataMov);
        }

        /// <summary>
        /// Verifica se já existe um tipo de período do calendário cadastrado com o mesmo nome
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param> 
        /// <param name="tpc_nome">Nome do tipo de período do calendário</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTipoPeriodoCalendarioExistente
        (
            int tpc_id
            , string tpc_nome
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelectBy_Nome(tpc_id, tpc_nome);
        }

        /// <summary>
        /// Verifica se já existe um tipo de período do calendário cadastrado com o mesmo nome
        /// </summary>
        /// <param name="tpc_id">ID do tipo de período do calendário</param> 
        /// <param name="tpc_nomeAbreviado">Nome abreviado do tipo de período do calendário</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaTipoPeriodoCalendarioExistenteAbreviado
        (
            int tpc_id
            , string tpc_nomeAbreviado
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelectBy_NomeAbreviado(tpc_id, tpc_nomeAbreviado);
        }

        /// <summary>
        /// Verifica o maior número de ordem cadastado de tipo de período do calendário
        /// </summary>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaMaiorOrdem()
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.Select_MaiorOrdem();
        }

        /// <summary>
        /// Retorna o tpc_id vigente, caso não exista o último.
        /// </summary>
        /// <returns>Tpc_id</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int SelecionaPeriodoVigente()
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelectBy_PeriodoVigente();
        }

        /// <summary>
        /// Retorna todos os tipos de períodos do calendário não excluídos logicamente até a data atual,
        /// com alunos matriculados no último dia do período.
        /// </summary>
        /// <param name="cal_id">ID do calendário</param>        
        /// <param name="tud_id">ID da disciplina</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarPeriodosComMatricula
        (
            int cal_id
            , long tud_id
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            return dao.SelecionarPeriodosComMatricula(cal_id, tud_id);
        }

        /// <summary>
        /// Altera a ordem do tipo de período do calendário
        /// </summary>
        /// <param name="entitySubir">Entidade do tipo de periodo do calendário</param>
        /// <param name="entityDescer">Entidade do tipo de periodo do calendário</param>        
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SaveOrdem
        (
            ACA_TipoPeriodoCalendario entityDescer
            , ACA_TipoPeriodoCalendario entitySubir
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();

            if (entityDescer.Validate())
                dao.Salvar(entityDescer);
            else
                throw new ValidationException(entityDescer.PropertiesErrorList[0].Message);

            if (entitySubir.Validate())
                dao.Salvar(entitySubir);
            else
                throw new ValidationException(entitySubir.PropertiesErrorList[0].Message);

            return true;
        }

        /// <summary>
        /// Inclui ou altera o tipo de período do calendário
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoPeriodoCalendario</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            ACA_TipoPeriodoCalendario entity
        )
        {
            if (entity.IsNew)
                entity.tpc_ordem = SelecionaMaiorOrdem() + 1;

            if (entity.Validate())
            {
                if (VerificaTipoPeriodoCalendarioExistente(entity.tpc_id, entity.tpc_nome))
                    throw new DuplicateNameException("Já existe um tipo de período calendário cadastrado com este nome.");

                if (!string.IsNullOrEmpty(entity.tpc_nomeAbreviado) && VerificaTipoPeriodoCalendarioExistenteAbreviado(entity.tpc_id, entity.tpc_nomeAbreviado))
                    throw new DuplicateNameException("Já existe um tipo de período calendário cadastrado com este nome abreviado.");

                List<int> lstCalIds = SelecionaCalendarioPorTipoPeriodoCalendario(entity.tpc_id);
                foreach (int cal in lstCalIds)
                    GestaoEscolarUtilBO.LimpaCache(string.Format(chaveCache_SelecionaPor_PeriodoVigente_EventoEfetivacaoVigente + "_{0}", cal.ToString()));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.Cache_SelecionaDisciplinaPorTurmaDocente);
                CacheManager.Factory.RemoveByPattern(ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_PATTERN_KEY);

                ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
                return dao.Salvar(entity);
            }

            throw new ValidationException(entity.PropertiesErrorList[0].Message);
        }

        /// <summary>
        /// Deleta logicamente um tipo de período do calendário
        /// </summary>
        /// <param name="entity">Entidade ACA_TipoPeriodoCalendario</param>        
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            ACA_TipoPeriodoCalendario entity
        )
        {
            ACA_TipoPeriodoCalendarioDAO dao = new ACA_TipoPeriodoCalendarioDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Verifica se o tipo de período do calendário pode ser deletado
                if (GestaoEscolarUtilBO.VerificarIntegridade("tpc_id", entity.tpc_id.ToString(), "ACA_TipoPeriodoCalendario,REL_AlunosSituacaoFechamento,MTR_MatriculasBoletim,MTR_MatriculasBoletimDisciplina,CLS_AlunoAvaliacaoTurmaDisciplinaMedia,CLS_AlunoFechamentoPendencia,CLS_CompensacaoAusencia", dao._Banco))
                    throw new ValidationException("Não é possível excluir o tipo de período do calendário pois possui outros registros ligados a ele.");
                
                //Deleta logicamente o tipo de período do calendário
                dao.Delete(entity);

                return true;
            }
            catch (Exception err)
            {
                dao._Banco.Close(err);
                throw;
            }
            finally
            {
                dao._Banco.Close();
            }
        }
    }
}
