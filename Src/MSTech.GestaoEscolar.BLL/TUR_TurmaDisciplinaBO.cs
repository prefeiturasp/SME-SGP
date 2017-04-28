using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Web;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumeradores

    /// <summary>
    /// Tipo de disciplina.
    /// </summary>
    public enum TurmaDisciplinaTipo : byte
    {
        Obrigatoria = 1
        ,
        Optativa = 3
        ,
        Eletiva = 4
        ,
        DisciplinaPrincipal = 5
        ,
        DocenteTurmaObrigatoria = 6
        ,
        DocenteTurmaEletiva = 7
        ,
        DependeDisponibilidadeProfessorObrigatoria = 8
        ,
        DependeDisponibilidadeProfessorEletiva = 9
        ,
        DisciplinaEletivaAluno = 10
        ,
        Regencia = 11
        ,
        ComponenteRegencia = 12
        ,
        DocenteEspecificoComplementacaoRegencia = 13
        ,
        Multisseriada = 14
        ,
        MultisseriadaDocente = 15
        ,
        MultisseriadaAluno = 16
        ,
        DocenciaCompartilhada = 17
        ,
        Experiencia = 18
        ,
        TerritorioSaber = 19
    }

    /// <summary>
    /// Modo de disciplina.
    /// </summary>
    public enum TurmaDisciplinaModo : byte
    {
        Normal = 1
        ,
        RegimeEspecial = 2
            , Excepcional = 3
    }

    /// <summary>
    /// Situação do registro da TurmaDisciplina.
    /// </summary>
    public enum TurmaDisciplinaSituacao : byte
    {
        Ativo = 1
        , Excluido = 3
    }

    #endregion

    #region Estruturas

    /// <summary>
    /// Escalas de avaliação do docente
    /// </summary>
    /// <author>juliano.real</author>
    /// <datetime>05/05/2014-11:44</datetime>
    [Serializable]
    public struct Avaliacao
    {
        public ACA_EscalaAvaliacao escalaAvaliacao;
        public ACA_EscalaAvaliacaoNumerica escalaAvaliacaoNumerica;
        public ACA_EscalaAvaliacaoParecer escalaAvaliacaoParecer;
        //private DataRow dataRow;

        public Avaliacao(DataRow dataRow, string ColumnPrefix = "")
        {
            //this.dataRow = dataRow;

            if (string.IsNullOrEmpty(dataRow[ColumnPrefix + "esa_id"].ToString()))
                escalaAvaliacao = new ACA_EscalaAvaliacao();
            else
                escalaAvaliacao = new ACA_EscalaAvaliacao
                {
                    esa_id = Convert.ToInt32(dataRow[ColumnPrefix + "esa_id"])
                    ,
                    ent_id = new Guid(dataRow[ColumnPrefix + "ent_id"].ToString())
                    ,
                    esc_id = string.IsNullOrEmpty(dataRow[ColumnPrefix + "esc_id"].ToString()) ? -1 : Convert.ToInt32(dataRow[ColumnPrefix + "esc_id"])
                    ,
                    uni_id = string.IsNullOrEmpty(dataRow[ColumnPrefix + "uni_id"].ToString()) ? -1 : Convert.ToInt32(dataRow[ColumnPrefix + "uni_id"])
                    ,
                    esa_padrao = Convert.ToBoolean(string.IsNullOrEmpty(dataRow[ColumnPrefix + "esa_padrao"].ToString()) ? 0 : dataRow[ColumnPrefix + "esa_padrao"])
                    ,
                    esa_tipo = Convert.ToByte(string.IsNullOrEmpty(dataRow[ColumnPrefix + "esa_tipo"].ToString()) ? 0 : dataRow[ColumnPrefix + "esa_tipo"])
                    ,
                    esa_nome = dataRow[ColumnPrefix + "esa_nome"].ToString()
                    ,
                    esa_situacao = Convert.ToByte(dataRow[ColumnPrefix + "esa_situacao"])
                    ,
                    esa_dataCriacao = Convert.ToDateTime(dataRow[ColumnPrefix + "esa_dataCriacao"].ToString())
                    ,
                    esa_dataAlteracao = Convert.ToDateTime(dataRow[ColumnPrefix + "esa_dataAlteracao"].ToString())
                    ,
                    IsNew = false
                };

            if (string.IsNullOrEmpty(dataRow[ColumnPrefix + "ean_situacao"].ToString()))
                escalaAvaliacaoNumerica = new ACA_EscalaAvaliacaoNumerica();
            else
                escalaAvaliacaoNumerica = new ACA_EscalaAvaliacaoNumerica
                {
                    esa_id = Convert.ToInt32(dataRow[ColumnPrefix + "esa_id"])
                    ,
                    ean_menorValor = Convert.ToDecimal(string.IsNullOrEmpty(dataRow[ColumnPrefix + "ean_menorValor"].ToString()) ? 0 : dataRow[ColumnPrefix + "ean_menorValor"])
                    ,
                    ean_maiorValor = Convert.ToDecimal(string.IsNullOrEmpty(dataRow[ColumnPrefix + "ean_maiorValor"].ToString()) ? 0 : dataRow[ColumnPrefix + "ean_maiorValor"])
                    ,
                    ean_variacao = Convert.ToDecimal(string.IsNullOrEmpty(dataRow[ColumnPrefix + "ean_variacao"].ToString()) ? 0 : dataRow[ColumnPrefix + "ean_variacao"])
                    ,
                    ean_situacao = Convert.ToByte(dataRow[ColumnPrefix + "ean_situacao"])
                    ,
                    IsNew = false
                };

            if (string.IsNullOrEmpty(dataRow[ColumnPrefix + "eap_id"].ToString()))
                escalaAvaliacaoParecer = new ACA_EscalaAvaliacaoParecer();
            else
                escalaAvaliacaoParecer = new ACA_EscalaAvaliacaoParecer
                {
                    esa_id = Convert.ToInt32(dataRow[ColumnPrefix + "esa_id"])
                    ,
                    eap_id = Convert.ToInt32(dataRow[ColumnPrefix + "eap_id"])
                    ,
                    eap_valor = dataRow[ColumnPrefix + "eap_valor"].ToString()
                    ,
                    eap_descricao = dataRow[ColumnPrefix + "eap_descricao"].ToString()
                    ,
                    eap_abreviatura = dataRow[ColumnPrefix + "eap_abreviatura"].ToString()
                    ,
                    eap_ordem = Convert.ToInt32(dataRow[ColumnPrefix + "eap_ordem"])
                    ,
                    eap_equivalenteInicio = Convert.ToDecimal(string.IsNullOrEmpty(dataRow[ColumnPrefix + "eap_equivalenteInicio"].ToString()) ? 0 : dataRow[ColumnPrefix + "eap_equivalenteInicio"])
                    ,
                    eap_equivalenteFim = Convert.ToDecimal(string.IsNullOrEmpty(dataRow[ColumnPrefix + "eap_equivalenteFim"].ToString()) ? 0 : dataRow[ColumnPrefix + "eap_equivalenteFim"])
                    ,
                    eap_situacao = Convert.ToByte(dataRow[ColumnPrefix + "eap_situacao"])
                    ,
                    eap_dataCriacao = Convert.ToDateTime(dataRow[ColumnPrefix + "eap_dataCriacao"].ToString())
                    ,
                    eap_dataAlteracao = Convert.ToDateTime(dataRow[ColumnPrefix + "eap_dataAlteracao"].ToString())
                    ,
                    IsNew = false
                };
        }
    }

    [Serializable]
    public struct sComboTurmaDisciplina
    {
        public string tur_tud_id { get; set; }
        public string tur_tud_nome { get; set; }
        public string tud_nome { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <author>juliano.real</author>
    /// <datetime>05/05/2014-11:44</datetime>
    [Serializable]
    public struct ControleTurmas
    {
        public TUR_Turma turma;
        public TUR_TurmaDisciplina turmaDisciplina;
        public ACA_Disciplina disciplina;
        public ESC_Escola escola;
        public ACA_Curriculo curriculo;
        public ACA_Curso curso;
        public ACA_FormatoAvaliacao formatoAvaliacao;
        public ACA_CalendarioAnual calendarioAnual;
        public ACA_CurriculoPeriodo curriculoPeriodo;
        public string tciIds;

        public Avaliacao escalaDocente;
        public Avaliacao escalaDiciplina;
        public Avaliacao escalaGlobal;
        public Avaliacao escalaGlobalAdicinal;
    }

    /// <summary>
    /// Lista com as turmas e suas disciplinas.
    /// </summary>
    public struct TurmaRelTurmaDisciplina
    {
        /// <summary>
        /// Dados da disciplina da turma.
        /// </summary>
        public struct DadosTurmaDisciplina
        {
            /// <summary>
            /// Id da TurmaDisciplina.
            /// </summary>
            public long tud_id;

            /// <summary>
            /// Id do tipo de disciplina
            /// </summary>
            public int tds_id;
        }

        /// <summary>
        /// Id da turma.
        /// </summary>
        public long tur_id;

        /// <summary>
        /// Lista com as disciplinas da turma.
        /// </summary>
        private List<DadosTurmaDisciplina> _ltTurmaDisciplina;
        public List<DadosTurmaDisciplina> ltTurmaDisciplina
        {
            get
            {
                return _ltTurmaDisciplina ?? new List<DadosTurmaDisciplina>();
            }
            set
            {
                _ltTurmaDisciplina = value;
            }
        }


    }

    /// <summary>
    /// Dados da disciplina da turma.
    /// </summary>
    [Serializable]
    public struct sTurmaDisciplina
    {
        public long tur_id { get; set; }
        public long tud_id { get; set; }
        public string tud_nome { get; set; }
        public int tud_tipo { get; set; }
        public bool tud_naoLancarFrequencia { get; set; }
        public bool tud_naoLancarNota { get; set; }
        public bool tud_permitirLancarAbonoFalta { get; set; }
    }

    /// <summary>
    /// Dados da disciplina da turma.
    /// </summary>
    [Serializable]
    public struct sTurmaDisciplinaRelacionada
    {
        public long tud_id { get; set; }
        public string tud_nome { get; set; }
        public long tdr_id { get; set; }
        public int tud_tipo { get; set; }
        public string DataValueFieldCombo {
            get {
                return string.Format("{0};{1}", tud_id, tdr_id);
            }
        }
        public byte tdr_situacao { get; set; }
    }	

    /// <summary>
    /// Estrutura que armzena informações sobre disciplina escola e calendário.
    /// </summary>
    public struct sTurmaDisciplinaEscolaCalendario
    {
        public long tur_id { get; set; }
        public long tud_id { get; set; }
        public byte tud_tipo { get; set; }
        public int esc_id { get; set; }
        public int uni_id { get; set; }
        public int cal_id { get; set; }
    }

    #endregion

    public class TUR_TurmaDisciplinaBO : BusinessBase<TUR_TurmaDisciplinaDAO, TUR_TurmaDisciplina>
    {
        public const string Cache_SelecionaDisciplinaPorTurmaDocente = "Cache_SelecionaDisciplinaPorTurmaDocente";
        public const string Cache_SelecionaPorDocenteControleTurma = "Cache_SelecionaPorDocenteControleTurma";
        public const string Cache_GetSelectBy_TurmaDocente = "Cache_GetSelectBy_TurmaDocente";
        public const string Cache_GetSelectBy_tur_id = "Cache_GetSelectBy_tur_id";


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaDisciplina> GetSelectBy_tur_id
        (
            long tur_id
            , Guid ent_id
            , bool mostraFilhosRegencia
            , bool mostraRegencia
            , int AppMinutosCacheLongo = 0
        )
        {
            List<sTurmaDisciplina> lista = null;

            if (AppMinutosCacheLongo > 0 && HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetSelectBy_tur_id(tur_id, ent_id, mostraFilhosRegencia, mostraRegencia);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                    lista = (from dr in dao.SelectBy_tur_id(tur_id, ent_id, mostraFilhosRegencia, mostraRegencia).AsEnumerable()
                             select (sTurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplina())).ToList();

                    // Adiciona cache com validade do tempo informado na configuração.
                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(AppMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                    lista = (List<sTurmaDisciplina>)cache;
            }
            else
            {
                TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                lista = (from dr in dao.SelectBy_tur_id(tur_id, ent_id, mostraFilhosRegencia, mostraRegencia).AsEnumerable()
                         select (sTurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplina())).ToList();
            }

            return lista;
        }

        /// <summary>
        /// Retorna a chave para guardar em cache a busca das disciplinas pela turma
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_tur_id(long tur_id, Guid ent_id, bool mostraFilhosRegencia, bool mostraRegencia)
        {
            return string.Format(Cache_GetSelectBy_tur_id + "_{0}_{1}_{2}_{3}", tur_id, ent_id, mostraFilhosRegencia, mostraRegencia);
        }


        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaDisciplinaPorTurmaDocente_SemVigencia(
            long tur_id, 
            long doc_id, 
            byte fav_tipoLancamentoFrequencia,
            byte regencia, 
            int tipoRegencia, 
            bool filtroTurmasAtivas
        )
        {
            return string.Format(
                ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMADOCENTE_SEM_VIGENCIA_MODEL_KEY, 
                tur_id, 
                doc_id, 
                fav_tipoLancamentoFrequencia, 
                regencia, 
                tipoRegencia, 
                filtroTurmasAtivas
            );
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de disciplinas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaDisciplinaPorTurma_SemVigencia(
            long tur_id,
            byte verificaDisciplinaPrincipal,
            byte regencia,
            int tipoRegencia,
            bool filtroTurmasAtivas
        )
        {
            return string.Format(
                ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_TURMA_SEM_VIGENCIA_MODEL_KEY,
                tur_id,
                verificaDisciplinaPrincipal,
                regencia,
                tipoRegencia,
                filtroTurmasAtivas
            );
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de disciplinas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaDisciplinaPorDocente_SemVigencia(
            long doc_id,
            byte fav_tipoLancamentoFrequencia,
            byte regencia,
            int tipoRegencia,
            bool filtroTurmasAtivas
        )
        {
            return string.Format(
                ModelCache.TURMA_DISCIPLINA_SELECIONA_DISCIPLINA_POR_DOCENTE_SEM_VIGENCIA_MODEL_KEY,
                doc_id,
                fav_tipoLancamentoFrequencia,
                regencia,
                tipoRegencia,
                filtroTurmasAtivas
            );
        }

        /// <summary>
        /// Retorna a chave para guardar em cache a busca por turma.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string RetornaChaveCache_SelectBy_Turma(long tur_id)
        {
            return string.Format("Cache_TUR_TurmaDisciplinaBO_GetSelectBy_Turma_{0}", tur_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de escolas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaDisciplinaPorTurmaDocente(long tur_id, long doc_id, byte tud_tipo)
        {
            return string.Format(Cache_SelecionaDisciplinaPorTurmaDocente + "_{0}_{1}_{2}", tur_id, doc_id, tud_tipo);
        }

        /// <summary>
        /// Retorna a chave para guardar em cache a busca das disciplinas relacionadas com a disciplina compartilhada.
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_SelectRelacionadaVigenteBy_DisciplinaCompartilhada(long tud_id, string doc_id)
        {
            return string.Format(ModelCache.TURMA_SELECIONA_RELACIONADA_VIGENTE_BY_DISCIPLINA_COMPARTILHADA_MODEL_KEY, tud_id, doc_id);
        }

        /// <summary>
        /// Retorna a chave para guardar em cache a busca das disciplinas pelo docente
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_TurmaDocente(long doc_id, Guid ent_id, long tur_id, bool mostraFilhosRegencia, bool mostraRegencia, bool mostraExperiencia, bool mostraTerritorio, int cap_id)
        {
            return string.Format(Cache_GetSelectBy_TurmaDocente + "_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}", doc_id, ent_id, tur_id, mostraFilhosRegencia, mostraRegencia, mostraExperiencia, mostraTerritorio, cap_id);
        }

        /// <summary>
        /// Retorna a chave para guardar em cache a busca das disciplinas pela turma
        /// </summary>
        /// <returns></returns>
        public static string RetornaChaveCache_GetSelectBy_tur_id(long tur_id, Guid ent_id, bool mostraFilhosRegencia, bool mostraRegencia, bool mostraExperiencia, bool mostraTerritorio, int cap_id)
        {
            return string.Format(Cache_GetSelectBy_tur_id + "_{0}_{1}_{2}_{3}_{4}_{5}_{6}", tur_id, ent_id, mostraFilhosRegencia, mostraRegencia, mostraExperiencia, mostraTerritorio, cap_id);
        }

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static TUR_TurmaDisciplina GetEntity(TUR_TurmaDisciplina entity, TalkDBTransaction banco = null)
        {
            string chave = RetornaChaveCache_GetEntity(entity);

            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            if (banco != null)
                dao._Banco = banco;

            GestaoEscolarUtilBO.CopiarEntity
            (
                CacheManager.Factory.Get
                    (
                        chave,
                        () =>
                        {
                            dao.Carregar(entity);
                            return entity;
                        },
                        GestaoEscolarUtilBO.MinutosCacheMedio
                    ),
                entity
            );

            return entity;
        }

        /// <summary>
        /// Remove do cache a entidade.
        /// </summary>
        /// <param name="entity"></param>
        private static void LimpaCache(TUR_TurmaDisciplina entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(TUR_TurmaDisciplina entity)
        {
            return string.Format(ModelCache.TURMA_DISCIPLINA_MODEL_KEY, entity.tud_id);
        }

        /// <summary>
        /// Seleciona as turmas e disciplinas que o docente leciona.
        /// </summary>
        /// <param name="doc_id">Id do docente.</param>
        /// <returns>Turmas e disciplinas que o docente leciona.</returns>
        public static DataTable SelecionaTurmaDisciplinaPorDocente(long doc_id)
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelecionaTurmaDisciplinaPorDocente(doc_id);
        }

        /// <summary>
        /// Retorna as entidades da TurmaDisciplina cadastradas na turma.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<CadastroTurmaDisciplina> GetSelectCadastradosBy_Turma
        (
            long tur_id
        )
        {
            List<CadastroTurmaDisciplina> lista = new List<CadastroTurmaDisciplina>();

            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            TUR_TurmaDisciplinaRelDisciplinaDAO daoRelDis = new TUR_TurmaDisciplinaRelDisciplinaDAO();
            TUR_TurmaDocenteDAO daoTurDoc = new TUR_TurmaDocenteDAO();

            DataTable dt = dao.SelectBy_Turma(tur_id, true);

            foreach (DataRow dr in dt.Rows)
            {
                TUR_TurmaDisciplina ent = new TUR_TurmaDisciplina();
                ent = dao.DataRowToEntity(dr, ent);

                TUR_TurmaDisciplinaRelDisciplina entRelDis = new TUR_TurmaDisciplinaRelDisciplina();
                entRelDis = daoRelDis.DataRowToEntity(dr, entRelDis);

                TUR_TurmaDocente entTurmaDocente = new TUR_TurmaDocente();
                entTurmaDocente = daoTurDoc.DataRowToEntity(dr, entTurmaDocente);

                List<TUR_TurmaDisciplinaCalendario> listCal =
                    TUR_TurmaDisciplinaCalendarioBO.GetSelectBy_TurmaDisciplina(ent.tud_id);

                lista.Add(new CadastroTurmaDisciplina
                {
                    entTurmaDisciplina = ent
                    ,
                    entTurmaDiscRelDisciplina = entRelDis
                    ,
                    entTurmaDocente = entTurmaDocente
                    ,
                    entTurmaCalendario = listCal
                });
            }

            return lista;
        }

        /// <summary>
        /// Utilizado na tela de lançamento de frequência por período.
        /// Carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina. 
        /// Só traz turmas que o formato de avaliação tenha o tipo de lançamento
        /// de frequência por período.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>  
        /// <param name="tur_id">ID da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinaPorTurmaDocente_FrequenciaPeriodo
        (
            long tur_id
            , long doc_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_TurmaDocente(tur_id, doc_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.Periodo, 0);
        }

        /// <summary>
        /// Utilizado na tela de lançamento de frequência por período.
        /// Carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina. 
        /// Só traz turmas que o formato de avaliação tenha o tipo de lançamento
        /// de frequência por aulas planejadas.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>  
        /// <param name="tur_id">ID da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinaPorTurmaDocente_FrequenciaAulas
        (
            long tur_id
            , long doc_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_TurmaDocente(tur_id, doc_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas, 0);
        }

        /// <summary>
        /// Utilizado na tela de lançamento de frequência por período.
        /// Carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina. 
        /// Só traz turmas que o formato de avaliação tenha o tipo de lançamento
        /// de frequência por aulas planejadas.
        /// Não traz as disciplinas do tipo 13-Docente específico – complementação da regência.
        /// </summary>
        /// <param name="doc_id">ID do docente</param>  
        /// <param name="tur_id">ID da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmaDisciplina> SelecionaDisciplinaPorTurmaDocente_FrequenciaAulas_SemVigencia
        (
            long tur_id
            , long doc_id
            , byte regencia
            , int tipoRegencia
            , bool filtroTurmasAtivas
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboTurmaDisciplina> dados = null;

            if (appMinutosCacheLongo > 0)
            {
                string chave;
                if (doc_id > 0)
                {
                    chave = RetornaChaveCache_SelecionaDisciplinaPorDocente_SemVigencia(
                        doc_id,
                        (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas,
                        regencia,
                        tipoRegencia,
                        filtroTurmasAtivas
                    );
                }
                else if (tur_id > 0)
                    {
                    chave = RetornaChaveCache_SelecionaDisciplinaPorTurma_SemVigencia(
                        tur_id,
                        (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas,
                        regencia,
                        tipoRegencia,
                        filtroTurmasAtivas
                    );
                }
                else
                    {
                    chave = RetornaChaveCache_SelecionaDisciplinaPorTurmaDocente_SemVigencia(
                        tur_id,
                        doc_id,
                        (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas,
                        regencia,
                        tipoRegencia,
                        filtroTurmasAtivas
                    );
                    }

                dados = CacheManager.Factory.Get(
                            chave,
                            () =>
                            {
                                using (
                                    DataTable dt = 
                                        (doc_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Docente_SemVigencia(doc_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas, regencia, tipoRegencia, filtroTurmasAtivas) :
                                        (tur_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Turma_SemVigencia(tur_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas, regencia, tipoRegencia, filtroTurmasAtivas) :
                                        new TUR_TurmaDisciplinaDAO().SelectBy_TurmaDocente_SemVigencia(tur_id, doc_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas, regencia, tipoRegencia, filtroTurmasAtivas)
                                )
                                {
                                    return dt.AsEnumerable().AsParallel().Select(p =>
                                            new sComboTurmaDisciplina
                                            {
                                                tur_tud_id = p["tur_tud_id"].ToString(),
                                                tur_tud_nome = p["tur_tud_nome"].ToString()
                }

                                        ).ToList();
                                }
                            },
                            appMinutosCacheLongo
                        );
            }
            else
            {
                using (
                    DataTable dt =
                        (doc_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Docente_SemVigencia(doc_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas, regencia, tipoRegencia, filtroTurmasAtivas) :
                        (tur_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Turma_SemVigencia(tur_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas, regencia, tipoRegencia, filtroTurmasAtivas) :
                        new TUR_TurmaDisciplinaDAO().SelectBy_TurmaDocente_SemVigencia(tur_id, doc_id, (byte)ACA_FormatoAvaliacaoTipoLancamentoFrequencia.AulasPlanejadas, regencia, tipoRegencia, filtroTurmasAtivas)
                )
                         {
                    dados = dt.AsEnumerable().AsParallel().Select(p =>
                        new sComboTurmaDisciplina
                        {
                            tur_tud_id = p["tur_tud_id"].ToString(),
                            tur_tud_nome = p["tur_tud_nome"].ToString()
            }

                    ).ToList();
                }
            }

            return dados;
        }

        /// <summary>
        /// Utilizado nas telas do menu classe (EXCETO EFETIVAÇÃO).
        /// Quando informado o tur_id, carrega todas as disciplinas da turma.
        /// Quando informado o doc_id, carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tud_tipo">ID do tipo de disciplina que não será exibido</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinaPorTurmaDocente
        (
            long tur_id
            , long doc_id
            , byte tud_tipo
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_TurmaDocente(tur_id, doc_id, 0, tud_tipo);
        }

        /// <summary>
        /// Utilizado nas telas do menu classe (EXCETO EFETIVAÇÃO).
        /// Quando informado o tur_id, carrega todas as disciplinas da turma.
        /// Quando informado o doc_id, carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tud_tipo">ID do tipo de disciplina que não será exibido</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmaDisciplina> SelecionaDisciplinaPorTurmaDocente
        (
            long tur_id
            , long doc_id
            , byte tud_tipo
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboTurmaDisciplina> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaDisciplinaPorTurmaDocente(tur_id, doc_id, tud_tipo);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                        DataTable dtDados = dao.SelectBy_TurmaDocente(tur_id, doc_id, 0, tud_tipo);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboTurmaDisciplina
                                 {
                                     tur_tud_id = dr["tur_tud_id"].ToString(),
                                     tur_tud_nome = dr["tur_tud_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboTurmaDisciplina>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                DataTable dtDados = dao.SelectBy_TurmaDocente(tur_id, doc_id, 0, tud_tipo);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboTurmaDisciplina
                         {
                             tur_tud_id = dr["tur_tud_id"].ToString(),
                             tur_tud_nome = dr["tur_tud_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Quando informado o tur_id, carrega todas as disciplinas da turma.
        /// Quando informado o doc_id, carrega as turmas e disciplinas que o docente 
        /// dá aula, mais as que ele é coordenador de disciplina.
        /// Não considera vigência do período
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="doc_id">ID do docente</param>  
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmaDisciplina> SelecionaDisciplinaPorTurmaDocente_SemVigencia
        (
            long tur_id
            , long doc_id
            , byte regencia
            , int tipoRegencia
            , bool filtroTurmasAtivas
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboTurmaDisciplina> dados = null;

            if (appMinutosCacheLongo > 0)
            {
                string chave;
                if (doc_id > 0)
                {
                    chave = RetornaChaveCache_SelecionaDisciplinaPorDocente_SemVigencia(
                        doc_id,
                        0,
                        regencia,
                        tipoRegencia,
                        filtroTurmasAtivas
                    );
                }
                else if (tur_id > 0)
                {
                    chave = RetornaChaveCache_SelecionaDisciplinaPorTurma_SemVigencia(
                        tur_id,
                        0,
                        regencia,
                        tipoRegencia,
                        filtroTurmasAtivas
                    );
                }
                else
                {
                    chave = RetornaChaveCache_SelecionaDisciplinaPorTurmaDocente_SemVigencia(
                        tur_id,
                        doc_id,
                        0,
                        regencia,
                        tipoRegencia,
                        filtroTurmasAtivas
                    );
                }

                dados = CacheManager.Factory.Get(
                            chave,
                            () =>
                            {
                                using (
                                    DataTable dt = 
                                        (doc_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Docente_SemVigencia(doc_id, 0, regencia, tipoRegencia, filtroTurmasAtivas) :
                                        (tur_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Turma_SemVigencia(tur_id, 0, regencia, tipoRegencia, filtroTurmasAtivas) :
                                        new TUR_TurmaDisciplinaDAO().SelectBy_TurmaDocente_SemVigencia(tur_id, doc_id, 0, regencia, tipoRegencia, filtroTurmasAtivas)
                                )
                    {
                                    return dt.AsEnumerable().AsParallel().Select(p =>
                                            new sComboTurmaDisciplina
                                 {
                                                tur_tud_id = p["tur_tud_id"].ToString(),
                                                tur_tud_nome = p["tur_tud_nome"].ToString(),
                                                tud_nome = p["tud_nome"].ToString()
                                            }

                                        ).ToList();
                                }
                            },
                            appMinutosCacheLongo
                        );
                    }
                    else
                    {
                using (
                    DataTable dt = 
                        (doc_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Docente_SemVigencia(doc_id, 0, regencia, tipoRegencia, filtroTurmasAtivas) :
                        (tur_id > 0) ? new TUR_TurmaDisciplinaDAO().SelectBy_Turma_SemVigencia(tur_id, 0, regencia, tipoRegencia, filtroTurmasAtivas) :
                        new TUR_TurmaDisciplinaDAO().SelectBy_TurmaDocente_SemVigencia(tur_id, doc_id, 0, regencia, tipoRegencia, filtroTurmasAtivas)
                )
                {
                    dados = dt.AsEnumerable().AsParallel().Select(p =>
                        new sComboTurmaDisciplina
                        {
                            tur_tud_id = p["tur_tud_id"].ToString(),
                            tur_tud_nome = p["tur_tud_nome"].ToString(),
                            tud_nome = p["tud_nome"].ToString()
                }
                    ).ToList();
            }
            }

            return dados;
        }

        /// <summary>
        /// Retorna as disciplinas de uma turma para lançamento de frequência
        /// Se existir disciplina principal no período carrega apenas ela
        /// </summary>
        /// <param name="tur_id">ID da turma</param>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinaPorTurmaFrequencia
        (
            long tur_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_TurmaFrequencia(tur_id);
        }

        /// <summary>
        /// Retorna as disciplinas por turma disciplina e curriculo periodo
        /// </summary>                
        /// <param name="tur_id">ID da turma</param>        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="cur_idAtual">ID do curso</param>
        /// <param name="crr_idAtual">ID do curriculo do curso</param>
        /// <param name="crp_idAtual">ID do período do currículo</param>
        /// <param name="crp_idAnterior">ID do período do currículo</param>
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinaPorTurmaTurmaDisciplina
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
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_TurmaDisciplina(tur_id, tud_id, cur_idAtual, crr_idAtual, crp_idAtual, crp_idAnterior, tdt_posicao);
        }

        /// <summary>
        /// Retorna os as disciplinas por turma e tipo de avaliacao
        /// </summary>                   
        /// <param name="fav_tipoLancamentoFrequencia">Tipo de avaliação</param>             
        /// <param name="doc_id"></param>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinaPorDocenteETipoLancamento
        (
            int fav_tipoLancamentoFrequencia
            , long doc_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_DocenteETipoLancamento(fav_tipoLancamentoFrequencia, doc_id);
        }

        /// <summary>
        /// Retorna as disciplinas por turma disciplina e curriculo periodo
        /// </summary>                        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do periodo do curriculo</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_TurmaDisciplinaCurriculoPeriodo
        (
            long tud_id
            , int cur_id
            , int crr_id
            , int crp_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_TurmaDisciplinaCurriculoPeriodo(tud_id, cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Retorna o tipo da disciplina da turma disciplina
        /// </summary>
        /// <param name="tud_id">ID da disciplina turma</param>        
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaTipoDisciplinaPorTurmaDisciplina
        (
            long tud_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_TipoDisciplina(tud_id);
        }

        /// <summary>
        /// Retorna as disciplinas por turma.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TUR_TurmaDisciplina> GetSelectBy_Turma
        (
            long tur_id
            , TalkDBTransaction banco = null
            , int appMinutosCacheLongo = 0
        )
        {

            List<TUR_TurmaDisciplina> dados = null;

            Func<List<TUR_TurmaDisciplina>> retorno = delegate()
                    {
                        TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                        dao._Banco = banco == null ? dao._Banco : banco;
                        DataTable dt = dao.SelecionaDisciplinasPorTurma(tur_id, true);
                        dados = new List<TUR_TurmaDisciplina>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            TUR_TurmaDisciplina ent = new TUR_TurmaDisciplina();
                            dados.Add(dao.DataRowToEntity(dr, ent));
                        }
                return dados;
            };

            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.TURMA_DISCIPLINA_TURMA_MODEL_KEY, tur_id);
                

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

            return dados;
           
        }

        /// <summary>
        /// Retorna as disciplinas por turma.
        /// </summary>
        /// <param name="tur_ids">Ids das turmas.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>Lista com as turmas e suas disciplinas.</returns>
        public static List<TurmaRelTurmaDisciplina> SelecionaPorTurmas(string tur_ids, TalkDBTransaction banco)
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO { _Banco = banco };

            DataTable dt = dao.SelecionaPorTurmas(tur_ids);

            var x = from turma in dt.Rows.Cast<DataRow>().GroupBy(dr => new { tur_id = Convert.ToInt64(dr["tur_id"]) }).Select(p => p.Key)
                    select new TurmaRelTurmaDisciplina
                    {
                        tur_id = turma.tur_id
                        ,
                        ltTurmaDisciplina = (from DataRow dr in dt.Rows
                                             where Convert.ToInt64(dr["tur_id"]) == turma.tur_id
                                             select new TurmaRelTurmaDisciplina.DadosTurmaDisciplina
                                             {
                                                 tud_id = Convert.ToInt64(dr["tud_id"])
                                                 ,
                                                 tds_id = Convert.ToInt32(dr["tds_id"])
                                             }).ToList()
                    };
            List<TurmaRelTurmaDisciplina> lista = x.ToList();
            return lista;
        }

        /// <summary>
        /// Retorna as disciplinas de uma turma visíveis na efetivação + a disciplina informada
        /// na disciplinaAdicional. 
        /// Se for docente, traz as disciplinas que ele dá aula ou coordena.
        /// Se não for docente, traz todas as disciplinas da turma, independente se a turma 
        /// é de docente especialista e o lançamento for em conjunto.
        /// Na efetivação é considerado apenas o tipo de formato de avaliação, se for pra
        /// lançar por disciplina (Notas por disciplina ou Conceito global + notas por disciplina), 
        /// traz todas as disciplinas.
        /// Só adiciona disciplina adicional se a turma for normal 
        /// (se for eletiva do aluno não dá pra efetivar sem tud_id).
        /// </summary>
        /// <param name="tur_id">id da turma disciplina</param>
        /// <param name="doc_id">id do docente</param>
        /// <param name="tur_codigo"></param>
        /// <param name="disciplinaAdicional">Disciplina a mais para retornar no dataTable (Global/Resultado final)</param>
        /// <param name="entTurma"></param>
        /// <param name="mtu_id">id da matricula na turma </param>                
        /// <param name="alu_id">id do aluno </param>                
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelect_Efetivacao_By_TurmaDisciplinaAdicional
        (
            long tur_id
            , long doc_id
            , string tur_codigo
            , string disciplinaAdicional
            , TUR_Turma entTurma
            , int mtu_id
            , long alu_id
            , byte tdt_situacao = (byte)TUR_TurmaDocenteSituacao.Ativo
            , byte tur_situacao = (byte)TUR_TurmaSituacao.Ativo
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            DataTable dt = dao.Select_Efetivacao_By_TurmaDocente(tur_id, doc_id, mtu_id, alu_id, tdt_situacao);

            // Só adiciona disciplina adicional se a turma for normal 
            // (se for eletiva do aluno não dá pra efetivar sem tud_id).
            if ((entTurma.tur_tipo == (byte)TUR_TurmaTipo.Normal) &&
                (!String.IsNullOrEmpty(disciplinaAdicional)))
            {
                var x = from DataRow dr in dt.Rows
                        where Convert.ToBoolean(dr["Permissao"])
                        select dr;

                // Adiciona a linha adicional somente se o usuário tem permissão de editar em 
                // pelo menos uma disciplina.
                if (x.Count() > 0)
                    dt.Rows.Add(RetornaLinhaDisciplinaAdicional(dt, tur_id, tur_codigo, disciplinaAdicional, tur_situacao));
            }

            if (dt.Rows.Count > 0 && tur_situacao == (byte)TUR_TurmaSituacao.Ativo)
            {
                bool contains = dt.AsEnumerable().Any(row => Convert.ToByte(row.Field<byte>("tur_situacao")) == 1);

                if (contains)
                {
                    string situacaoTurmaAtiva = Convert.ToString((byte)TUR_TurmaSituacao.Ativo);
                    dt = dt.Select("tur_situacao = " + situacaoTurmaAtiva).CopyToDataTable();
                }
            }

            return dt;
        }

        /// <summary>
        /// Retorna os tud_ids de uma turma para lançamento de frequência mensal. Se o tipo de apuração de frequencia for:
        /// DIA: traz apenas a disciplina principal, ou a disciplina global (ou caso não haja nenhuma dessas, faz verificação em tela e cria 
        /// uma coluna de conceito global).
        /// TEMPOS DE AULA: traz todas as disciplinas exceto a disciplina global (tambem verifica em tela qual o fav_tipo, sendo global 
        /// cria apenas a coluna de conceito global, sendo disciplina traz todas as disciplinas e sendo global + disciplina traz todas 
        /// as disciplinas e acrescenta a coluna de conceito global.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tpc_id"></param>
        /// <returns>DataTable de disciplinas</returns>
        public static List<long> SelecionaTud_ids_PorTurma_LancamentoMensal(long tur_id, int tpc_id)
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            DataTable dt = dao.SelectByTurma_LancamentoMensal(tur_id, tpc_id, true, true);

            return (from DataRow dr in dt.Rows
                    select Convert.ToInt64(dr["tud_id"])).ToList();
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
        /// <param name="tpc_id"></param>
        /// <param name="regencia">Indica se os componentes da regência devem ser desconsiderados.</param>
        /// <returns>DataTable de disciplinas</returns>
        public static DataTable SelecionaPorTurma_LancamentoMensal(long tur_id, int tpc_id, bool regencia)
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectByTurma_LancamentoMensal(tur_id, tpc_id, regencia, true);
        }

        /// <summary>
        /// Cria um datatable com uma única linha, para lançamento global (sem ser por disciplina).
        /// Verifica se o docente tem permissão pra editar pelo menos uma disciplina, se não
        /// tiver, retorna um DataTable vazio.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="doc_id">ID do docente logado no sistema</param>
        /// <returns></returns>
        public static DataTable CarregaLancamentoGlobal(long tur_id, string tur_codigo, long doc_id)
        {
            bool podeEditar = true;

            if (doc_id > 0)
            {
                // Verifica se o docente leciona pelo menos uma disciplina na turma.
                DataTable dtDocentes =
                    TUR_TurmaDocenteBO.SelecionaDocentesPorTurma(tur_id);

                var x = from DataRow dr in dtDocentes.Rows
                        where Convert.ToInt64(dr["doc_id"]) == doc_id
                        select dr;

                podeEditar = x.Count() > 0;
            }

            // Cria o dataTable com os campos necessários.
            DataTable dt = new DataTable();
            dt.Columns.Add("tur_id");
            dt.Columns.Add("tud_id");
            dt.Columns.Add("tds_id");
            dt.Columns.Add("tur_codigo");
            dt.Columns.Add("tud_nome");
            dt.Columns.Add("tur_tud_id");
            dt.Columns.Add("tur_tud_nome");
            dt.Columns.Add("tud_tipo");
            dt.Columns.Add("tur_situacao");

            if (podeEditar)
            {
                dt.Rows.Add(RetornaLinhaDisciplinaAdicional(dt, tur_id, tur_codigo, "Conceito global", (byte)TUR_TurmaSituacao.Ativo));
            }

            return dt;
        }

        /// <summary>
        /// Retorna uma linha de disciplina com o tud_id vazio, e com o nome passado por parâmetro.
        /// </summary>
        /// <param name="dt">Tabela</param>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="disciplinaAdicional">Nome da disciplina adicional</param>
        /// <returns></returns>
        private static DataRow RetornaLinhaDisciplinaAdicional(DataTable dt, long tur_id, string tur_codigo, string disciplinaAdicional, byte tur_situacao)
        {
            // Adiciona uma linha com a disciplina informada.
            DataRow dr = dt.NewRow();
            dr["tur_id"] = tur_id;
            dr["tud_id"] = 0;
            dr["tds_id"] = 0;
            dr["tur_codigo"] = tur_codigo;
            dr["tud_nome"] = disciplinaAdicional;
            // ID = [0]-tur_id; [1]-tud_id; [2]-permissao.
            dr["tur_tud_id"] = tur_id + ";" + "-1" + ";" + "1";
            dr["tur_tud_nome"] = tur_codigo + " / " + disciplinaAdicional;
            dr["tud_tipo"] = 0;
            dr["tur_situacao"] = tur_situacao;

            return dr;
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
        /// <param name="currentPage">Página atual do gridview</param>
        /// <param name="pageSize">Total de registros por página</param> 
        /// <param name="paginado">Indica se o datatable será paginado ou não</param>
        /// <returns>DataTable com as turmas disciplinas</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataTable GetSelect
        (
            long tur_id
            , int cur_id
            , int crr_id
            , int crp_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            totalRecords = 0;

            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_CurriculoDisciplina(tur_id, cur_id, crr_id, crp_id, paginado, currentPage / pageSize, pageSize, out totalRecords);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaDisciplina> GetSelectBy_tur_id
          (
              long tur_id
              , Guid ent_id
              , bool mostraFilhosRegencia
              , bool mostraRegencia
              , bool mostraExperiencia
              , bool mostraTerritorio
              , int cap_id
              , int AppMinutosCacheLongo = 0
          )
        {
            List<sTurmaDisciplina> lista = null;

            Func<List<sTurmaDisciplina>> retorno = delegate ()
            {
                TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                return (from dr in dao.SelectBy_tur_id(tur_id, ent_id, mostraFilhosRegencia, mostraRegencia, mostraExperiencia, mostraTerritorio, cap_id).AsEnumerable()
                        select (sTurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplina())).ToList();
            };

            if (AppMinutosCacheLongo > 0)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetSelectBy_tur_id(tur_id, ent_id, mostraFilhosRegencia, mostraRegencia, mostraExperiencia, mostraTerritorio, cap_id);
                lista = CacheManager.Factory.Get
                        (
                            chave,
                            retorno,
                            AppMinutosCacheLongo
                        );

            }
            else
            {
                lista = retorno();
            }

            return lista;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaDisciplina> GetSelectBy_TurmaTipos
        (
            long tur_id
            , Guid ent_id
            , bool mostraFilhosRegencia
            , bool mostraRegencia
            , bool mostraExperiencia
            , bool mostraTerritorio
            , int cap_id
            , bool paginado
            , int currentPage
            , int pageSize
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return (from dr in dao.SelectBy_TurmaTipos(tur_id, ent_id, mostraFilhosRegencia, mostraRegencia, mostraExperiencia, mostraTerritorio, paginado, currentPage, pageSize, out totalRecords).AsEnumerable()
                    select (sTurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplina())).ToList();
        }

        /// <summary>
        /// Retorna todas as disciplinas da turma em que o professor for docente e  que não foram excluídas logicamente.
        /// </summary>
        /// <param name="tur_id">Turma</param>
        /// <param name="ent_id">Entidade</param>
        /// <param name="mostraFilhosRegencia">Mostra componentes da regência</param>
        /// <param name="mostraRegencia">Mostra a disciplina Regência</param>
        /// <param name="doc_id">Docente</param>
        /// <param name="AppMinutosCacheLongo">Configuração do tempo de cache - caso seja 0 não usa o cache.</param>
        /// <param name="mostraCompartilhadas">Indica se traz as disciplinas compartilhadas</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaDisciplina> GetSelectBy_TurmaDocente
        (
            long tur_id
            , Guid ent_id
            , bool mostraFilhosRegencia
            , bool mostraRegencia
            , bool mostraExperiencia
            , bool mostraTerritorio
            , long doc_id
            , int cap_id
            , int AppMinutosCacheLongo = 0
            , bool mostraCompartilhadas = false
        )
        {
            List<sTurmaDisciplina> lista = null;

            Func<List<sTurmaDisciplina>> retorno = delegate ()
            {
                TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
                return (from dr in dao.SelectBy_TurmaDocente(tur_id, ent_id, mostraFilhosRegencia, mostraRegencia, mostraExperiencia, mostraTerritorio, doc_id, cap_id, mostraCompartilhadas).AsEnumerable()
                        select (sTurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplina())).ToList();
            };

            if (AppMinutosCacheLongo > 0)
            {
                string chave = RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, mostraFilhosRegencia, mostraRegencia, mostraExperiencia, mostraTerritorio, cap_id);
                lista = CacheManager.Factory.Get
                        (
                            chave,
                            retorno,
                            AppMinutosCacheLongo
                        );
            }
            else
            {
                lista = retorno();
            }

            return lista;
        }

        /// <summary>
        /// Utilizado para carregar uma lista de todas as disciplinas e curriculos da mesma turma
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarTurmaDisciplina_CurriculoDisciplina_By_Turma
        (
            long tur_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelecionarTurmaDisciplina_CurriculoDisciplina_By_Turma(tur_id);
        }

        /// <summary>
        /// Utilizado para carregar uma lista com  disciplina e curriculo da turma turma multisseriada do docente
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionarTurmaDisciplina_TurmaCurriculo_By_Turma
        (
            long tur_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelecionarTurmaDisciplina_TurmaCurriculo_By_Turma(tur_id);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as turmas disciplinas
        /// que não foram excluídas logicamente, filtrados por 
        /// id da turma e situacao        
        /// </summary>
        /// <param name="tur_id">ID do Turma</param>
        /// <param name="doc_id"></param>
        /// <param name="tud_situacao">Situação da Turma Disciplina</param>
        /// <returns>DataTable com as turmas disciplinas</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBY_Turmas
        (
            long tur_id
            , long doc_id
            , bool vigencia
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_Turma(tur_id, doc_id, vigencia);
        }

        /// <summary>
        /// Retorna um datatable contendo todas as disciplinas
        /// que não foram excluídas logicamente, filtrados por 
        /// id da turma e do docente.        
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns>DataTable com as disciplinas.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaDisciplinasDocentesPorTurma
        (
            long tur_id
            , long doc_id
            , bool vigencia = false
        )
        {
            return GetSelectBY_Turmas(tur_id, doc_id, vigencia);
        }

        /// <summary>
        /// Seleciona o nome da disciplina  de acordo com o docente se este for uma disciplina especial. (Libras)
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="tipoDocente">The tipo docente.</param>
        /// <param name="banco">The banco.</param>
        /// <returns></returns>
        public static string SelecionaNomePorTipoDocente(long tud_id, EnumTipoDocente tipoDocente, TalkDBTransaction banco = null)
        {
            TUR_TurmaDisciplinaDAO dao = banco == null ? new TUR_TurmaDisciplinaDAO() : new TUR_TurmaDisciplinaDAO { _Banco = banco };
            return dao.SelecionaNomePorTipoDocente(tud_id, (byte)tipoDocente);
        }

        /// <summary>
        /// Seleciona dados relacionados pelo tud_id para evitar varias buscas.
        /// </summary>
        /// <param name="tud_id">Id da Turma Disciplina.</param>
        /// <returns>Turmas e disciplinas que o docente leciona.</returns>
        public static ControleTurmas SelecionaEntidadesControleTurmas(long tud_id, int appMinutosCacheLongo = 0)
        {
            Func<ControleTurmas> retorno = delegate()
        {
            ControleTurmas controleTurmas = new ControleTurmas();

                using (DataTable dt = new TUR_TurmaDisciplinaDAO().SelecionaEntidadesControleTurmas(tud_id))
                {
            if (dt.Rows.Count > 0)
            {
                controleTurmas.turma = new TUR_TurmaDAO().DataRowToEntity(dt.Rows[0], new TUR_Turma());
                controleTurmas.turmaDisciplina = new TUR_TurmaDisciplinaDAO().DataRowToEntity(dt.Rows[0], new TUR_TurmaDisciplina());
                controleTurmas.disciplina = new ACA_DisciplinaDAO().DataRowToEntity(dt.Rows[0], new ACA_Disciplina());
                controleTurmas.escola = new ESC_EscolaDAO().DataRowToEntity(dt.Rows[0], new ESC_Escola());
                controleTurmas.curriculo = new ACA_CurriculoDAO().DataRowToEntity(dt.Rows[0], new ACA_Curriculo());
                controleTurmas.curso = new ACA_CursoDAO().DataRowToEntity(dt.Rows[0], new ACA_Curso());
                controleTurmas.curriculoPeriodo = new ACA_CurriculoPeriodoDAO().DataRowToEntity(dt.Rows[0], new ACA_CurriculoPeriodo());
                controleTurmas.formatoAvaliacao = new ACA_FormatoAvaliacaoDAO().DataRowToEntity(dt.Rows[0], new ACA_FormatoAvaliacao());
                controleTurmas.calendarioAnual = new ACA_CalendarioAnualDAO().DataRowToEntity(dt.Rows[0], new ACA_CalendarioAnual());
                controleTurmas.tciIds = dt.Rows[0]["tciIds"].ToString();

                controleTurmas.escalaDiciplina = new Avaliacao(dt.Rows[0], "Dis_");
                controleTurmas.escalaDocente = new Avaliacao(dt.Rows[0], "Doc_");
                controleTurmas.escalaGlobal = new Avaliacao(dt.Rows[0], "Global_");
                controleTurmas.escalaGlobalAdicinal = new Avaliacao(dt.Rows[0], "GlobalAdic_");
            }
                }

            return controleTurmas;
            };

            if (appMinutosCacheLongo > 0)
            {
                return CacheManager.Factory.Get
                                        (
                                            String.Format(ModelCache.CONTROLE_TURMA_SELECIONA_ENTIDADES_POR_TURMA_DISCIPLINA_MODEL_KEY, tud_id)
                                            ,
                                            retorno
                                            ,
                                            appMinutosCacheLongo
                                        );
            }
            else
            {
                return retorno();
            }
        }
       
        /// <summary>
        /// Valida dados necessários para salvar a turma eletiva do aluno, se esse for o tipo
        /// da turma.
        /// </summary>
        /// <param name="entTurma">Entidade da turma a ser salva</param>
        /// <param name="cad">Entidade de cadastro da disciplina</param>
        /// <param name="banco">Transação com banco</param>
        private static void ValidarDadosTurmaEletivaAluno(TUR_Turma entTurma, CadastroTurmaDisciplina cad, TalkDBTransaction banco)
        {
            if (entTurma.tur_tipo == (byte)TUR_TurmaTipo.EletivaAluno)
            {
                // Validar períodos do calendário para turmas eletivas do aluno.
                if (cad.entTurmaCalendario.Count != 2)
                {
                    throw new ValidationException("Devem ser selecionados somente dois períodos consecutivos do calendário para a turma de eletiva.");
                }

                ACA_TipoPeriodoCalendario entTpc1 = new ACA_TipoPeriodoCalendario
                {
                    tpc_id = cad.entTurmaCalendario[0].tpc_id
                };
                ACA_TipoPeriodoCalendarioBO.GetEntity(entTpc1, banco);

                ACA_TipoPeriodoCalendario entTpc2 = new ACA_TipoPeriodoCalendario
                {
                    tpc_id = cad.entTurmaCalendario[1].tpc_id
                };
                ACA_TipoPeriodoCalendarioBO.GetEntity(entTpc2, banco);

                if (entTpc1.tpc_ordem != (entTpc2.tpc_ordem - 1))
                {
                    // Os períodos devem ser seguidos (ex: 1º e 2º bimestre).
                    throw new ValidationException("Devem ser selecionados somente dois períodos consecutivos do calendário para a turma de eletiva.");
                }
            }
        }

        /// <summary>
        /// Seleciona turmas disciplinas por tud_ids
        /// </summary>
        /// <param name="tud_ids">tud_ids separados por ';'</param>
        /// <returns></returns>
        public static List<TUR_TurmaDisciplina> SelecionaTurmaDisciplina(string tud_ids, TalkDBTransaction banco = null)
        {
            TUR_TurmaDisciplinaDAO dao = banco == null ? new TUR_TurmaDisciplinaDAO() : new TUR_TurmaDisciplinaDAO { _Banco = banco };
            return dao.SelecionaTurmaDisciplina(tud_ids);
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
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_ConfiguracaoParametrosDisciplinas
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
        )
        {
            if (pageSize == 0)
                pageSize = 1;

            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return dao.SelectBy_ConfiguracaoParametrosDisciplinas(
                esc_id, cur_id, crp_id, crr_id, cal_id, tds_id, tur_codigo, paginado, currentPage / pageSize, pageSize, out totalRecords);

        }

        /// <summary>
        /// O método salva os parâmetros de configuracao das disciplinas.
        /// </summary>
        /// <param name="lista">TurmaDisciplinas a serem atualizadas</param>
        /// <returns>True se operação realizada.</returns>
        public static bool SalvarConfiguracaoParametrosDisciplinas(List<TUR_TurmaDisciplina> lista)
        {
            bool salvou = false;

            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                lista.ForEach(p => dao.Update_ConfiguracaoParametrosDisciplinas(p.tud_id, p.tud_naoExibirFrequencia, p.tud_naoExibirNota,
                                                                                p.tud_naoLancarFrequencia, p.tud_naoLancarNota, p.tud_naoExibirBoletim, p.tud_naoLancarPlanejamento, p.tud_permitirLancarAbonoFalta));
                salvou = true;
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.Cache_SelecionaPorDocenteControleTurma);
            }
            catch (Exception ex)
            {
                dao._Banco.Close(ex);
                throw;
            }
            finally
            {
                if (dao._Banco.ConnectionIsOpen)
                    dao._Banco.Close();
            }

            return salvou;
        }

        /// <summary>
        /// Retorna uma listagem de disciplinas por escola quando não for passado a data base
        /// apenas os registros ativos serão retornados, caso passe a data base serão retornados
        /// apenas os registros criados ou alterados apos esta data.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para seleção dos registros</param>
        /// <returns>dataTable com as disciplinas</returns>
        public static DataTable SelecionaDisciplinasPorEscola(int esc_id, DateTime dataBase)
        {
            return new TUR_TurmaDisciplinaDAO().SelecionaDisciplinasPorEscola(esc_id, dataBase);
        }

        /// <summary>
        /// Seleciona as disciplinas do docente em determinada turma.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        public static DataTable SelecionaDisciplinasPorDocenteTurma(long tur_id, long doc_id)
        {
            return new TUR_TurmaDisciplinaDAO().SelecionaDisciplinasPorDocenteTurma(tur_id, doc_id);
        }

        /// <summary>
        /// Seleciona disciplina por id e dados da turma e docente.
        /// </summary>
        /// <param name="tud_id">ID da disciplina.</param>
        /// <returns></returns>
        public static DataTable SelecionaDisciplinaDadosDocenteTurma(long tud_id)
        {
            return new TUR_TurmaDisciplinaDAO().SelecionaDisciplinaDadosDocenteTurma(tud_id);
        }

         /// <summary>
        /// Seleciona a disciplina relacionada com a disciplina compartilhada, vigente no momento.
        /// </summary>
        /// <param name="tud_id">ID da disciplina compartilhada.</param>
        /// <returns></returns>
        public static List<sTurmaDisciplinaRelacionada> SelectRelacionadaVigenteBy_DisciplinaCompartilhada
            (long tud_id, int AppMinutosCacheLongo, bool retornarComponentesRegencia, long doc_id)
        {
            return SelectRelacionadaVigenteBy_DisciplinaCompartilhada
                (tud_id, AppMinutosCacheLongo, retornarComponentesRegencia, doc_id, null);
        }

        /// <summary>
        /// Seleciona a disciplina relacionada com a disciplina compartilhada, vigente no momento.
        /// </summary>
        /// <param name="tud_id">ID da disciplina compartilhada.</param>
        /// <returns></returns>
        public static List<sTurmaDisciplinaRelacionada> SelectRelacionadaVigenteBy_DisciplinaCompartilhada  // DANIELLE
            (long tud_id, int AppMinutosCacheLongo = 0, bool retornarComponentesRegencia = false, long doc_id = 0
            , TalkDBTransaction banco = null)
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            if (banco != null) dao._Banco = banco;
        
            List<sTurmaDisciplinaRelacionada> lista = null;
            if (AppMinutosCacheLongo > 0)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, doc_id.ToString());

                lista = CacheManager.Factory.Get
                            (
                                chave,
                                () =>
                {
                                    return lista = (from dr in dao.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, doc_id).AsEnumerable()
                             select (sTurmaDisciplinaRelacionada)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplinaRelacionada())).ToList();
                                 },
                                AppMinutosCacheLongo
                              );   
                }  else  {
                    return lista = (from dr in dao.SelectRelacionadaVigenteBy_DisciplinaCompartilhada(tud_id, doc_id).AsEnumerable()
                         select (sTurmaDisciplinaRelacionada)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplinaRelacionada())).ToList();
            }

            if (lista != null && lista.Count > 0 && !retornarComponentesRegencia)
            {
                return lista.Where(p => p.tud_tipo != (byte)ACA_CurriculoDisciplinaTipo.ComponenteRegencia).ToList();
            }
            return lista;
        }


        /// <summary>
        /// Seleciona as disciplinas da turma, relacionando com as disciplinas de docência compartilhada.
        /// </summary>
        /// <param name="tud_id">ID da turma.</param>
        /// <returns></returns>
        public static DataTable SelectRelacionadoDocenciaCompartilhadaBy_Turma(long tur_id)
        {
            return new TUR_TurmaDisciplinaDAO().SelectRelacionadoDocenciaCompartilhadaBy_Turma(tur_id);
        }

        public static DataTable GetSelectBy_Disciplina(int dis_id, TalkDBTransaction banco = null)
        {
            var dao = new TUR_TurmaDisciplinaDAO();
            if (banco != null)
                dao._Banco = banco;

            return dao.SelecionaPorDisciplina(dis_id);
        }

        /// <summary>
        /// Busca as turmas/disciplinas eletivas que não foram excluídas logicamente.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sTurmaDisciplina> GetSelectEletivaAlunoBy_EscolaCalendario
        (
            int esc_id
            , int cal_id
        )
        {
            TUR_TurmaDisciplinaDAO dao = new TUR_TurmaDisciplinaDAO();
            return (from dr in dao.SelectEletivaAlunoBy_EscolaCalendario(esc_id, cal_id).AsEnumerable()
                         select (sTurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dr, new sTurmaDisciplina())).ToList();
        }

        /// <summary>
        /// Retorna as disciplinas com divergência entre aulas criadas e aulas previstas.
        /// </summary>
        /// <param name="tudIds">Lista de disciplinas para verificar</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<long> SelecionaDisciplinasDivergenciasAulasPrevistas(string tudIds)
        {
            return new TUR_TurmaDisciplinaDAO().SelecionaDisciplinasDivergenciasAulasPrevistas(tudIds);
        }
    }
}
