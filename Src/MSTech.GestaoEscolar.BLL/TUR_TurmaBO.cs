using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using MSTech.Data.Common.Abstracts;
using System.IO;
using MSTech.GestaoEscolar.BLL.Caching;

namespace MSTech.GestaoEscolar.BLL
{
    #region Excessões

    /// <summary>
    /// Classe de excessão referente à entidade TUR_Turma.
    /// Utilizada nas telas de cadastro, para identificar se excedeu no número máximo de alunos na turma.
    /// </summary>
    public class ExcessoAlunosTurmaException : ValidationException
    {
        private string _mensagem;

        public ExcessoAlunosTurmaException()
        {
        }

        public ExcessoAlunosTurmaException(string Mensagem)
        {
            _mensagem = Mensagem;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_mensagem))
                    _mensagem = base.Message;

                return _mensagem;
            }
        }
    }

    public class NumAlunosExcedeCapacidadeDependencia : Exception
    {
        private string _mensagem;

        public NumAlunosExcedeCapacidadeDependencia()
        {
        }

        public NumAlunosExcedeCapacidadeDependencia(string Mensagem)
        {
            _mensagem = Mensagem;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_mensagem))
                    _mensagem = base.Message;

                return _mensagem;
            }
        }
    }

    public class CapacidadeTurmaExcedeCapacidadeDependencia : Exception
    {
        private string _mensagem;

        public CapacidadeTurmaExcedeCapacidadeDependencia()
        {
        }

        public CapacidadeTurmaExcedeCapacidadeDependencia(string Mensagem)
        {
            _mensagem = Mensagem;
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_mensagem))
                    _mensagem = base.Message;

                return _mensagem;
            }
        }
    }

    #endregion Excessões

    #region Estruturas

    /// <summary>
    /// Estrutura utilizada para armazenar as turmas por escola e calendário.
    /// </summary>
    [Serializable]
    public class Struct_MinhasTurmas
    {
        public Guid uad_idSuperior { get; set; }

        public int esc_id { get; set; }

        public int uni_id { get; set; }

        public string esc_nome { get; set; }

        public string lengendTitulo { get; set; }

        public int cal_id { get; set; }

        public int cal_ano { get; set; }

        public bool turmasAnoAtual { get; set; }

        public List<Struct_Turmas> Turmas { get; set; }

        [Serializable]
        public class Struct_Turmas
        {
            public string tur_codigo { get; set; }

            public string tud_nome { get; set; }

            public string tur_curso { get; set; }

            public string tur_turno { get; set; }

            public long tur_id { get; set; }

            public int fav_id { get; set; }

            public string tur_escolaUnidade { get; set; }

            public long tud_id { get; set; }

            public int tdt_posicao { get; set; }

            public int tdt_id { get; set; }
            public DateTime tdt_vigenciaInicio { get; set; }
            public DateTime tdt_vigenciaFim { get; set; }

            public byte crg_tipo { get; set; }

            public byte tdc_id { get; set; }

            public int cal_id { get; set; }

            public int esc_id { get; set; }

            public int uni_id { get; set; }

            public bool mostraPosicao { get; set; }

            public bool tud_naoLancarNota { get; set; }

            public bool tud_naoLancarFrequencia { get; set; }

            public bool tud_disciplinaEspecial { get; set; }

            public int tdt_situacao { get; set; }

            public int tci_id { get; set; }

            public string tci_nome { get; set; }

            public int cur_id { get; set; }

            public int crr_id { get; set; }

            public int crp_id { get; set; }

            public int tds_id { get; set; }

            public int tur_situacao { get; set; }

            public byte tur_tipo { get; set; }

            public int tci_ordem { get; set; }

            public string tur_calendario { get; set; }

            public int cal_ano { get; set; }

            public bool turmasAnoAtual { get; set; }

            public string tur_codigoNormal { get; set; }

            public string EscolaTurmaDisciplina
            {
                get
                {
                    return tur_codigo + " / " + tud_nome + " - " + tur_escolaUnidade;
                }
            }

            public string TurmaDisciplinaEscola
            {
                get
                {
                    return tud_tipo == (byte)TurmaDisciplinaTipo.MultisseriadaDocente && tur_tipo == (byte)TUR_TurmaTipo.Normal ?
                        cal_ano + " - " + "Turma " + tur_codigoNormal + " - " + tur_codigo + " - " + tud_nome + (mostraPosicao ? " - " + TipoDocencia : "") + " - " + tur_escolaUnidade :
                        cal_ano + " - " + "Turma " + tur_codigo + " - " + tud_nome + (mostraPosicao ? " - " + TipoDocencia : "") + " - " + tur_escolaUnidade;
                }
            }

            public string DataValueFieldCombo
            {
                get
                {
                    return tur_id + ";" + tud_id + ";" + cal_id + ";" + tdt_posicao + ";" + tud_tipo + ";" + tur_tipo + ";" + tur_idNormal + ";" + tud_idAluno + ";" + (fav_fechamentoAutomatico ? "true" : "false");
                }
            }

            public string TipoDocencia
            {
                get
                {
                    IEnumerable<EnumTipoDocente> tipoDocente = Enum.GetValues(typeof(EnumTipoDocente)).Cast<EnumTipoDocente>().Where(p => (byte)p == tdc_id);
                    if (tipoDocente.Any())
                    {
                        FieldInfo infoElemento = tipoDocente.First().GetType().GetField(tipoDocente.First().ToString());
                        DescriptionAttribute[] atributos = (DescriptionAttribute[])infoElemento.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (atributos.Length > 0)
                        {
                            if (atributos[0].Description != null)
                            {
                                return atributos[0].Description;
                            }
                        }
                    }

                    return "Titular";
                }
            }

            public bool aulasPrevistasPreenchida { get; set; }

            public DateTime tur_dataEncerramento { get; set; }

            public bool disciplinaAtiva { get; set; }

            public byte tud_tipo { get; set; }

            public string tciIds { get; set; }

            public long tud_idAluno { get; set; }

            public long tur_idNormal { get; set; }

            public List<int> lstIdTipoCiclo
            {
                get
                {
                    if (!String.IsNullOrEmpty(tciIds))
                    {
                        string[] vetTipoCiclo = tciIds.Split(',');
                        var elements = from element in vetTipoCiclo
                                       select Convert.ToInt32(element);
                        return elements.ToList();
                    }
                    else
                    {
                        return new List<int>();
                    }
                }
            }

            public bool fav_fechamentoAutomatico { get; set; }

            public bool divergenciasAulasPrevistas { get; set; }

            public int tne_id { get; set; }

            public int tme_id { get; set; }
        }
    }

    /// <summary>
    /// Estrutura para formar turmas no início do ano letivo
    /// </summary>
    public struct TUR_Turma_FormarTurmas
    {
        public TUR_Turma entity;
        public List<TUR_TurmaCurriculo> listaCurriculos;
        public List<CadastroTurmaDisciplina> listTurmaDisciplina;
        public List<TUR_TurmaCurriculoAvaliacao> listTurmaCurriculoAvaliacao;
    }

    /// <summary>
    /// Estrutura para formar turmas eletiva no início do ano letivo
    /// </summary>
    public struct TUR_Turma_FormarTurmasEletiva
    {
        public TUR_Turma entity;
        public List<TUR_TurmaCurriculo> listaCurriculos;
        public List<CadastroTurmaDisciplina> listTurmaDisciplina;
    }

    /// <summary>
    /// Estrutura para formar turmas eletiva no início do ano letivo
    /// </summary>
    [Serializable]
    public struct Struct_temposEfetivacaoSemestral
    {
        public int tpc_id;
        public int crp_qtdeTemposDia;
    }

    [Serializable]
    public struct sHistoricoDocente
    {
        public int esc_id { get; set; }

        public string esc_nome { get; set; }

        public long tur_id { get; set; }

        public String Turma { get; set; }

        public String tur_codigo { get; set; }

        public int cal_id { get; set; }

        public int tur_situacao { get; set; }

        public long tud_id { get; set; }

        public String tud_nome { get; set; }

        public int tud_situacao { get; set; }

        public int tdt_posicao { get; set; }

        public int tdt_id { get; set; }

        public byte crg_tipo { get; set; }

        public DateTime tdt_vigenciaInicio { get; set; }

        public DateTime tdt_vigenciaFim { get; set; }

        public int tci_id { get; set; }

        public String tci_nome { get; set; }

        public String tdc_nome { get; set; }

        public string docenciaCompartilhada { get; set; }

        public byte tud_tipo { get; set; }

        public int cal_ano { get; set; }

        public byte tdt_situacao { get; set; }

        public int tme_id { get; set; }
    }

    [Serializable]
    public struct sComboTurmas
    {
        public string tur_id { get; set; }

        public string tur_codigo { get; set; }

        public string tur_vagas { get; set; }

        public string tur_esc_nome { get; set; }

        public string tur_crp_ttn_id { get; set; }

        public string tur_cod_desc_nome { get; set; }

        public byte tur_situacao { get; set; }
    }
    
    /// <summary>
    /// Estrutura das turmas com evento de liberação do boletim online
    /// </summary>
    [Serializable]
    public struct TurmasBoletimLiberado
    {
        public int cal_id { get; set; }

        public long tur_id { get; set; }

        public int tpc_id { get; set; }
    }

    #endregion Estruturas

    #region Enumeradores

    /// <summary>
    /// Situações da turma
    /// </summary>
    public enum TUR_TurmaSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
        ,

        Cancelada = 4
        ,

        Encerrada = 5
        ,

        EmMatricula = 6
        ,

        Extinta = 7
        ,

        Aguardando = 8
    }

    /// <summary>
    /// Tipos de turma disponíveis.
    /// </summary>
    [Serializable]
    public enum TUR_TurmaTipo : byte
    {
        Normal = 1
        ,

        EletivaAluno = 2
        ,

        Multisseriada = 3
        ,

        MultisseriadaDocente = 4
        ,

        AtendimentoEducacionalEspecializado = 5
    }

    /// <summary>
    /// Duração da turma.
    /// </summary>
    public enum TUR_TurmaDuracao : byte
    {
        Anual = 1
        ,

        Semestral = 2
    }

    /// <summary>
    /// Tipo de teste ao salvar
    /// </summary>
    public enum TUR_TipoTeste : byte
    {
        AlunosMatriculados = 1
        ,

        CapacidadeTurma = 2
    }
    
    #endregion Enumeradores

    public class TUR_TurmaBO : BusinessBase<TUR_TurmaDAO, TUR_Turma>
    {
        #region Selects

        /// <summary>
        /// Override do GetEntity que guarda em Cache a entidade retornada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new static TUR_Turma GetEntity(TUR_Turma entity, TalkDBTransaction banco = null)
        {
            string chave = RetornaChaveCache_GetEntity(entity);

            TUR_TurmaDAO dao = new TUR_TurmaDAO();
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
                }
                    ,
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
        private static void LimpaCache(TUR_Turma entity)
        {
            CacheManager.Factory.Remove(RetornaChaveCache_GetEntity(entity));
            GestaoEscolarUtilBO.LimpaCache(string.Format("{0}", ModelCache.TIPO_TURNO_TURMA_MODEL_KEY, entity.tur_id));
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntity(TUR_Turma entity)
        {
            return string.Format(ModelCache.TURMA_MODEL_KEY, entity.tur_id);
        }

        /// <summary>
        /// Retorna as turmas de acordo com os filtros informados e com a permissão do usuário,
        /// traz somente turmas do tipo informado no parâmetro.
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="trn_id"></param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <param name="tur_tipo">Tipo de turma - obrigatório</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Pesquisa_Tipo
        (
            Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int trn_id
            , long doc_id
            , string tur_codigo
            , Guid ent_id
            , Guid uad_idSuperior
            , TUR_TurmaTipo tur_tipo
            , int LinhasPorPagina
            , int Pagina
            , int SortDirection
            , string SortExpression
        )
        {
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Pesquisa_Tipo
            (
                usu_id
                , gru_id
                , esc_id
                , uni_id
                , cal_id
                , cur_id
                , crr_id
                , crp_id
                , trn_id
                , doc_id
                , tur_codigo
                , ent_id
                , uad_idSuperior
                , (byte)tur_tipo
                , MostraCodigoEscola
                , LinhasPorPagina
                , Pagina
                , SortDirection
                , SortExpression
                , out totalRecords
            );
        }

        /// <summary>
        /// Retorna as turmas da escola, curso, período do curso e calendário.
        /// Somente turmas do tipo 1-Normal.
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_Escola_Periodo_Calendario
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Escola_Periodo_Calendario(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, usu_id, gru_id);
        }

        /// <summary>
        /// Retorna a entidade da turma passada
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public static Guid GetEntidadeByTurma(Int64 tur_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.GetEntidadeByTurma(tur_id);
        }

        /// <summary>
        /// Retorna todas as turmas que estão no ano e nas escolas informadas de acordo com o tipo
        /// </summary>
        /// <param name="cal_ano">Ano do calendario</param>
        /// <param name="tur_tipo">Tipos de turma</param>
        /// <param name="escolas">Id's das escola</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="banco">Transação</param>
        /// <returns></returns>
        public static DataTable GetSelectBy_AnoTipoTurma
        (
            int cal_ano
            , string tur_tipo
            , string escolas
            , Guid ent_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = banco };
            return dao.SelectBy_AnoTipoTurma(cal_ano, tur_tipo, escolas, ent_id);
        }

        /// <summary>
        /// Seleciona todos as turmas filtrando por escola, curso, currículo, período e calendário.
        /// Métodos do combo que usam: CarregarPorEscolaCalendarioEPeriodo.
        /// </summary>
        /// <param name="ent_id">ID da entidade.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do período do curso.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        public static List<sComboTurmas> SelecionarPorEscolaCalendarioEPeriodo
        (
            Guid ent_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , byte tur_situacao
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboTurmas> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_EscolaCalendarioEPeriodo(ent_id, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tur_situacao);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        TUR_TurmaDAO dao = new TUR_TurmaDAO();
                        DataTable dtDados = dao.SelecionarPorEscolaCalendarioEPeriodo(ent_id, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tur_situacao);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboTurmas
                                 {
                                     tur_id = dr["tur_id"].ToString(),
                                     tur_codigo = dr["tur_codigo"].ToString(),
                                     tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                                     tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboTurmas>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                TUR_TurmaDAO dao = new TUR_TurmaDAO();
                DataTable dtDados = dao.SelecionarPorEscolaCalendarioEPeriodo(ent_id, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tur_situacao);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboTurmas
                         {
                             tur_id = dr["tur_id"].ToString(),
                             tur_codigo = dr["tur_codigo"].ToString(),
                             tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                             tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// etorna as turmas de acordo com os filtros informados. 
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="adm">Informa se é usuário de visão administrador</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        /// <param name="appMinutosCacheLongo">Minutos do cache</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmas> GetSelectBy_Escola_Periodo_Situacao
        (
              Guid usu_id
            , Guid gru_id
            , bool adm
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_tipo
            , byte tur_situacao
        )
        {
            return GetSelectBy_Escola_Periodo_Situacao(usu_id, gru_id, adm, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_tipo, tur_situacao, 0);
        }

        /// <summary>
        /// etorna as turmas de acordo com os filtros informados. 
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="adm">Informa se é usuário de visão administrador</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        /// <param name="appMinutosCacheLongo">Minutos do cache</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmas> GetSelectBy_Escola_Periodo_Situacao
        (
              Guid usu_id
            , Guid gru_id
            , bool adm
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_tipo
            , byte tur_situacao
            , int appMinutosCacheLongo = 0
        )
        {
            return GetSelectBy_Escola_Periodo_Situacao(usu_id, gru_id, adm, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_tipo, tur_situacao, false, appMinutosCacheLongo);
        }

        /// <summary>
        /// Retorna as turmas de acordo com os filtros informados. 
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="adm">Informa se é usuário de visão administrador</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        /// <param name="appMinutosCacheLongo">Minutos do cache</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmas> GetSelectBy_Escola_Periodo_Situacao
        (
              Guid usu_id
            , Guid gru_id
            , bool adm
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_tipo
            , byte tur_situacao
            , bool mostraEletivas
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboTurmas> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_Escola_Periodo_Situacao(usu_id, gru_id, adm, esc_id, uni_id, cal_id,
                                                                             cur_id, crr_id, crp_id, ent_id, tur_tipo,
                                                                             tur_situacao, mostraEletivas);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        TUR_TurmaDAO dao = new TUR_TurmaDAO();
                        DataTable dtDados = dao.SelectBy_Escola_Periodo_Situacao(usu_id, gru_id, adm, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_tipo, tur_situacao, mostraEletivas);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboTurmas
                                 {
                                     tur_id = dr["tur_id"].ToString(),
                                     tur_codigo = dr["tur_codigo"].ToString(),
                                     tur_vagas = dr["tur_vagas"].ToString(),
                                     tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                                     tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString(),
                                     tur_situacao = Convert.ToByte(dr["tur_situacao"].ToString())
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboTurmas>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                TUR_TurmaDAO dao = new TUR_TurmaDAO();
                DataTable dtDados = dao.SelectBy_Escola_Periodo_Situacao(usu_id, gru_id, adm, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_tipo, tur_situacao, mostraEletivas);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboTurmas
                         {
                             tur_id = dr["tur_id"].ToString(),
                             tur_codigo = dr["tur_codigo"].ToString(),
                             tur_vagas = dr["tur_vagas"].ToString(),
                             tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                             tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString(),
                             tur_situacao = Convert.ToByte(dr["tur_situacao"].ToString())
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as turmas de acordo com os filtros informados. 
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade</param>
        /// <param name="cal_id">Id do calendário</param>
        /// <param name="tur_situacao">Situacao da turma</param>
        /// <param name="appMinutosCacheLongo">Minutos do cache</param>
        /// <returns>Turmas</returns>
        public static List<sComboTurmas> GetSelectBy_Escola_Calendario_Situacao(int esc_id, int uni_id, int cal_id, byte tur_situacao, int appMinutosCacheLongo = 0)
        {
            List<sComboTurmas> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_Escola_Calendario_Situacao(esc_id, uni_id, cal_id, tur_situacao);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        TUR_TurmaDAO dao = new TUR_TurmaDAO();
                        DataTable dtDados = dao.SelectBy_Escola_Calendario_Situacao(esc_id, uni_id, cal_id, tur_situacao);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboTurmas
                                 {
                                     tur_id = dr["tur_id"].ToString(),
                                     tur_codigo = dr["tur_codigo"].ToString(),
                                     tur_vagas = dr["tur_vagas"].ToString(),
                                     tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                                     tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString()
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboTurmas>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                TUR_TurmaDAO dao = new TUR_TurmaDAO();
                DataTable dtDados = dao.SelectBy_Escola_Calendario_Situacao(esc_id, uni_id, cal_id, tur_situacao);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboTurmas
                         {
                             tur_id = dr["tur_id"].ToString(),
                             tur_codigo = dr["tur_codigo"].ToString(),
                             tur_vagas = dr["tur_vagas"].ToString(),
                             tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                             tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString()
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        /// <param name="ttn_id">ID do tipo de turno</param>
        public static DataTable GetSelectBy_Escola_Periodo_MomentoAno
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int ttn_id = -1
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Escola_Periodo_MomentoAno(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, usu_id, gru_id, ttn_id);
        }

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno
        /// E também retornas as turmas de cursos equivalentes para o período informado
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        /// <param name="cal_id">ID do calendário</param>
        public static DataTable SelecionaTurmasCursoEquivalentes
        (
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
            , int cal_id = 0
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelecionaTurmasCursoEquivalentes(esc_id, uni_id, cur_id, crr_id, crp_id, ent_id, usu_id, gru_id, cal_id);
        }

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno
        /// e tenha o número da avaliação do currículo
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public static DataTable GetSelectBy_Escola_Periodo_MomentoAno_Avaliacao
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int tca_numeroAvaliacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Escola_Periodo_MomentoAno_Avaliacao(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tca_numeroAvaliacao, ent_id, usu_id, gru_id);
        }

        /// <summary>
        /// Retorna todas as turmas ativas (turmas de cursos equivalentes) e que seu calendario anual exista no MomentoAno
        /// e tenha o número da avaliação do currículo
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public static DataTable SelecionaTurmasCursosEquivalentesAvaliacao
        (
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int tca_numeroAvaliacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelecionaTurmasCursosEquivalentesAvaliacao(esc_id, uni_id, cur_id, crr_id, crp_id, tca_numeroAvaliacao, ent_id, usu_id, gru_id);
        }

        /// <summary>
        /// Retorna as turmas de acordo com os filtros, podendo ser inforados ou não.
        /// Traz turmas de todos os tipos (Normal e Eletiva do aluno).
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo do usuário - obrigatório</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="trn_id">ID do turno</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior à escola</param>
        /// <param name="adm"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Pesquisa_TodosTipos
        (
            Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int cal_id
            , int trn_id
            , string tur_codigo
            , long doc_id
            , bool adm
        )
        {
            totalRecords = 0;
            TUR_TurmaDAO dao = new TUR_TurmaDAO();

            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            return dao.SelectBy_Pesquisa_TodosTipos
            (
                usu_id,
                gru_id,
                ent_id,
                uad_idSuperior,
                esc_id,
                uni_id,
                cur_id,
                crr_id,
                cal_id,
                trn_id,
                tur_codigo,
                doc_id,
                adm,
                MostraCodigoEscola,
                out totalRecords);
        }

        /// <summary>
        /// Retorna as turmas do PEJA de acordo com os filtros, podendo ser informados ou não.
        /// Traz turmas de todos os tipos (Normal e Eletiva do aluno).
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo do usuário - obrigatório</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="trn_id">ID do turno</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade administrativa superior à escola</param>
        /// <param name="adm"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Pesquisa_PEJA
        (
            Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , Guid uad_idSuperior
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int cal_id
            , int trn_id
            , string tur_codigo
            , long doc_id
            , bool adm
        )
        {
            totalRecords = 0;
            TUR_TurmaDAO dao = new TUR_TurmaDAO();

            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            List<DataRow> lt = (from DataRow dr in dao.SelectBy_Pesquisa_TodosTipos
                                        (
                                            usu_id,
                                            gru_id,
                                            ent_id,
                                            uad_idSuperior,
                                            esc_id,
                                            uni_id,
                                            cur_id,
                                            crr_id,
                                            cal_id,
                                            trn_id,
                                            tur_codigo,
                                            doc_id,
                                            adm,
                                            MostraCodigoEscola,
                                            out totalRecords
                                        ).Rows
                                where Convert.ToByte(dr["crr_regimeMatricula"]) == 3
                                select dr).ToList();

            totalRecords = lt.Count;

            return lt.Count > 0 ? lt.CopyToDataTable() : new DataTable();
        }

        /// <summary>
        /// Retorna as turmas que o docente pode dar aula ou é coordenador
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="doc_id">ID do docente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Docente_Efetivacao_TodosTipos
        (
            Guid ent_id
            , long doc_id
        )
        {
            totalRecords = 0;
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Docente_Efetivacao_TodosTipos
            (
                ent_id,
                doc_id,
                out totalRecords);
        }

        /// <summary>
        /// Retorna as turmas que o docente pode dar aula ou é coordenador
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global, com exeção da posicao
        /// do docente passada no parametro.
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmas> GetSelectBy_Docente_TodosTipos_Posicao
        (
            Guid ent_id
            , long doc_id
            , int posicao
            , int esc_id
            , bool mostrarCodigoNome
            , bool turmasNormais = false
            , int appMinutosCacheLongo = 0
        )
        {
            return GetSelectBy_Docente_TodosTipos_Posicao(ent_id, doc_id, posicao, esc_id, 0, mostrarCodigoNome, turmasNormais, appMinutosCacheLongo);
        }

        /// <summary>
        /// Retorna as turmas que o docente pode dar aula ou é coordenador
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global, com exeção da posicao
        /// do docente passada no parametro.
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmas> GetSelectBy_Docente_TodosTipos_Posicao
        (
            Guid ent_id
            , long doc_id
            , int posicao
            , int esc_id
            , int cal_id
            , bool mostrarCodigoNome
            , bool turmasNormais = false
            , int appMinutosCacheLongo = 0
        )
        {
            return GetSelectBy_Docente_TodosTipos_Posicao(ent_id, doc_id, posicao, esc_id, cal_id, mostrarCodigoNome, turmasNormais, true, appMinutosCacheLongo);
        }

        /// <summary>
        /// Retorna as turmas que o docente pode dar aula ou é coordenador
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global, com exeção da posicao
        /// do docente passada no parametro.
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="mostrarCodigoNome">True - Exibe o código e nome da turma | False - Exibe apenas o código da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmas> GetSelectBy_Docente_TodosTipos_Posicao
        (
            Guid ent_id
            , long doc_id
            , int posicao
            , int esc_id
            , int cal_id
            , bool mostrarCodigoNome
            , bool turmasNormais = false
            , bool mostraEletivas = true
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboTurmas> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_Docente_TodosTipos_Posicao(ent_id, doc_id, posicao, esc_id, cal_id, mostrarCodigoNome, turmasNormais, mostraEletivas);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        TUR_TurmaDAO dao = new TUR_TurmaDAO();
                        totalRecords = 0;
                        DataTable dtDados = dao.SelectBy_Docente_TodosTipos_Posicao(ent_id, doc_id, posicao, esc_id, cal_id, mostrarCodigoNome, turmasNormais, mostraEletivas, out totalRecords);
                        dados = (from DataRow dr in dtDados.Rows
                                 select new sComboTurmas
                                 {
                                     tur_id = dr["tur_id"].ToString(),
                                     tur_codigo = dr["tur_codigo"].ToString(),
                                     tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                                     tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString(),
                                     tur_esc_nome = dr["tur_esc_nome"].ToString(),
                                 }).ToList();

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<sComboTurmas>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                TUR_TurmaDAO dao = new TUR_TurmaDAO();
                totalRecords = 0;
                DataTable dtDados = dao.SelectBy_Docente_TodosTipos_Posicao(ent_id, doc_id, posicao, esc_id, cal_id, mostrarCodigoNome, turmasNormais, mostraEletivas, out totalRecords);
                dados = (from DataRow dr in dtDados.Rows
                         select new sComboTurmas
                         {
                             tur_id = dr["tur_id"].ToString(),
                             tur_codigo = dr["tur_codigo"].ToString(),
                             tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                             tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString(),
                             tur_esc_nome = dr["tur_esc_nome"].ToString(),
                         }).ToList();
            }

            return dados;
        }

        /// <summary>
        /// Retorna as turmas PEJA que o docente pode dar aula ou é coordenador
        /// de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        /// Se for conceito global, traz as turmas apenas se estiver configurado
        /// que docentes pode efetivar notas do conceito global
        /// </summary>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="doc_id">ID do docente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Docente_Efetivacao_PEJA
        (
            Guid ent_id
            , long doc_id
        )
        {
            totalRecords = 0;
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            List<DataRow> lt = (from DataRow dr in dao.SelectBy_Docente_Efetivacao_TodosTipos
                                                    (
                                                        ent_id,
                                                        doc_id,
                                                        out totalRecords
                                                    ).Rows
                                where Convert.ToByte(dr["crr_regimeMatricula"]) == 3
                                select dr).ToList();

            totalRecords = lt.Count;

            return lt.Count > 0 ? lt.CopyToDataTable() : new DataTable();
        }

        /// <summary>
        /// Retorna as turmas da escola e ano do calendário.
        /// </summary>
        /// <param name="esc_id">id da escola da turma</param>
        /// <param name="uni_id">unid administrativa da escola</param>
        /// <param name="cal_ano">ano dos calendários</param>
        /// <param name="ent_id">Entidade</param>
        /// <param name="gru_id">Grupo do usuário</param>
        /// <param name="usu_id">Usuário</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Escola_Ano
        (
            int esc_id
            , int uni_id
            , int cal_ano
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
        )
        {
            if (cal_ano == 0)
                throw new ValidationException("Ano deve ser maior que zero e no formato AAAA.");

            totalRecords = 0;
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.Selectby_EscolaAno(ent_id, usu_id, gru_id, esc_id, uni_id, cal_ano, out totalRecords);
        }

        /// <summary>
        /// Retorna os alunos matriculados em turmas normais
        /// Para efetuar matricula nas turmas eletivas do aluno
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="dis_id">ID da disciplina eletiva do aluno</param>
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="adm">Indica se o usuário é administrador</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        public static DataTable SelecionaTurmasEletivasAluno
        (
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int dis_id
            , int cal_id
            , Guid ent_id
            , bool adm
            , Guid usu_id
            , Guid gru_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_TurmasEletivasAluno(esc_id, uni_id, cur_id, crr_id, dis_id, cal_id, ent_id, adm, usu_id, gru_id);
        }

        /// <summary>
        /// Retorna as turmas eletivas do aluno de acordo com os filtros informados e com a permissão do usuário
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo da turma</param>
        /// <param name="crp_id"></param>
        /// <param name="doc_id"></param>
        /// <param name="ttn_id"></param>
        /// <param name="dis_id">ID da disciplina</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable GetSelectBy_Pesquisa_TurmasEletivasAluno
        (
            Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int doc_id
            , int ttn_id
            , int dis_id
            , string tur_codigo
            , Guid ent_id
            , Guid uad_idSuperior
        )
        {
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            totalRecords = 0;
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Pesquisa_TurmasEletivasAluno
            (
                usu_id
                , gru_id
                , esc_id
                , uni_id
                , cal_id
                , cur_id
                , crr_id
                , crp_id
                , doc_id
                , ttn_id
                , dis_id
                , tur_codigo
                , ent_id
                , uad_idSuperior
                , MostraCodigoEscola
                , out totalRecords
            );
        }

        /// <summary>
        /// Retorna as turmas multisseriadas de acordo com os filtros informados e com a permissão do usuário
        /// </summary>
        /// <param name="usu_id">ID do usuário - obrigatório</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo da turma</param>
        /// <param name="crp_id"></param>
        /// <param name="doc_id"></param>
        /// <param name="ttn_id"></param>
        /// <param name="dis_id">ID da disciplina</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBy_Pesquisa_TurmasMultisseriadas
        (
            Guid usu_id
            , Guid gru_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int doc_id
            , int ttn_id
            , int dis_id
            , string tur_codigo
            , Guid ent_id
            , Guid uad_idSuperior
        )
        {
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            totalRecords = 0;
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Pesquisa_TurmasMultisseriadas
            (
                usu_id
                , gru_id
                , esc_id
                , uni_id
                , cal_id
                , cur_id
                , crr_id
                , crp_id
                , doc_id
                , ttn_id
                , dis_id
                , tur_codigo
                , ent_id
                , uad_idSuperior
                , MostraCodigoEscola
                , out totalRecords
            );
        }

        /// <summary>
        /// Faz busca das turmas. Traz somente turmas do tipo 1-Normal.
        /// </summary>
        /// <param name="usu_id">id do usuario</param>
        /// <param name="gru_id">id do grupo</param>
        /// <param name="tur_id">id da turma</param>
        /// <param name="esc_id">id da escola</param>
        /// <param name="uni_id">id da unidade administrativa</param>
        /// <param name="cal_id">id calendário</param>
        /// <param name="cur_id">id do curso</param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id">id do curriculo período</param>
        /// <param name="ent_id">id da entidade</param>
        /// <param name="tur_situacao"></param>
        /// <returns>retorna as turmas ativas</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable RetornaTurmas
        (
             Guid usu_id
            , Guid gru_id
            , long tur_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_situacao
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.RetornaTurmas(usu_id, gru_id, tur_id, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_situacao);
        }

        /// <summary>
        /// Verifica se já existe turma eletiva cadastrada com o mesmo codigo de turma
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="tur_codigoTurma">código da turma</param>
        /// <returns>True - caso encontre algum registro/Fase - caso não encontre nada</returns>
        public static bool VerificaExisteTurmaEletiva
        (
              int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , string tur_codigoTurma
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            DataTable dt = dao.VerificaExisteTurmaEletiva(esc_id, uni_id, cal_id, cur_id, crr_id, tur_codigoTurma);

            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// Busca turmas da escola, ano, curso e período informados.
        /// Considera os cursos equivalentes.
        /// Traz somente turmas do tipo 1-Normal.
        /// Somente turmas ativas.
        /// </summary>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo</param>
        /// <param name="crp_id">ID do período</param>
        /// <param name="cal_ano">Ano do calendário da turma</param>
        /// <returns></returns>
        public static DataTable SelecionaPor_Escola_Calendario_CursoPeriodo_Equivalentes
        (
             Guid usu_id
            , Guid gru_id
            , Guid ent_id
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int cal_ano
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelecionaPor_Escola_Calendario_CursoPeriodo_Equivalentes
                (usu_id, gru_id, ent_id, esc_id, uni_id, cur_id, crr_id, crp_id, cal_ano);
        }

        /// <summary>
        /// Faz busca das turmas. Traz somente turmas do tipo 1-Normal.
        /// </summary>
        /// <param name="usu_id">Id do usuário</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade administrativa</param>
        /// <param name="cal_id">Id calendário</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo</param>
        /// <param name="crp_id">Id do currículo período</param>
        /// <param name="cal_ano">Ano do calendário da turma</param>
        /// <param name="ent_id">Id da entidade</param>
        /// <param name="tur_situacao">Situação da turma</param>
        /// <returns>Retorna as turmas ativas confome os filtros.</returns>
        public static DataTable RetornaTurmasCalendario
        (
             Guid usu_id
            , Guid gru_id
            , long tur_id
            , int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int cal_ano
            , Guid ent_id
            , byte tur_situacao
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.RetornaTurmasCalendario(usu_id, gru_id, tur_id, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, cal_ano, ent_id, tur_situacao);
        }

        /// <summary>
        /// Retorna dados da turma e dados adicionais da turma para serem usados na tela de
        /// alunos na turma.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param> 
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelectBY_tur_id(long tur_id, Guid ent_id)
        {
            bool MostraCodigoEscola = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.ORDENAR_ESCOLAS_POR_CODIGO, ent_id);

            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_tur_id(tur_id, MostraCodigoEscola);
        }

        /// <summary>
        /// Retorna o formato de ensino da turma.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="bancoGestao">Transação com banco Gestão - obrigatório</param>
        /// <returns></returns>
        public static ACA_FormatoAvaliacao SelecionaFormatoAvaliacao
          (
              long tur_id
            , TalkDBTransaction bancoGestao = null
          )
        {
            TUR_TurmaDAO dao;
            if (bancoGestao != null)
            {
                dao = new TUR_TurmaDAO { _Banco = bancoGestao };
            }
            else
            {
                dao = new TUR_TurmaDAO();
            }

            ACA_FormatoAvaliacaoDAO daoFormatoAvaliacao = new ACA_FormatoAvaliacaoDAO();
            return
                dao.SelectBy_TurmaFormatoAvaliacao(tur_id).Rows.Cast<DataRow>().Select(
                    p => daoFormatoAvaliacao.DataRowToEntity(p, new ACA_FormatoAvaliacao())).FirstOrDefault();
        }

        /// <summary>
        /// Retorna as turmas da escola, curso, período do curso e calendário.
        /// Traz somente turmas do tipo 1-Normal, e com fav_tipoLancamentoFrequencia = 3 ou 4.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade.</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="crp_id">ID do currículo período.</param>
        /// <param name="ent_id">ID da entidade.</param>
        /// <param name="gru_id">ID do grupo.</param>
        /// <param name="usu_id">ID do usuário.</param>
        /// <returns>DataTable de turmas.</returns>
        public static DataTable SelecionaPorEscolaPeriodoCalendarioComFrequenciaMensal
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid gru_id
            , Guid usu_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectByEscolaPeriodoCalendarioComFrequenciaMensal(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, gru_id, usu_id);
        }

        /// <summary>
        /// Retorna uma turma dado um código, se existir.
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade de escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <returns>Entidade TUR_Turma</returns>
        public static TUR_Turma SelecionaPorCodigo
        (
            int tur_id
            , int esc_id
            , int uni_id
            , int cal_id
            , string tur_codigo
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            DataTable dt = dao.SelectBy_Codigo(tur_id, esc_id, uni_id, cal_id, tur_codigo);
            return dt.Rows.Count > 0 ? dao.DataRowToEntity(dt.Rows[0], new TUR_Turma()) : null;
        }

        /// <summary>
        /// Verifica se existe uma turma para escola que tenha avaliação recuperação final
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        public static Boolean VerificaExisteRecuperacaoFinal(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.VerificaExisteRecuperacaoFinal(esc_id, uni_id, cal_id, cap_id);
        }

        /// <summary>
        /// Retorna todas as turmas que não fecharam o COC.
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public static DataTable SelecionaTurmasSemFechamentoBimestre(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelecionaTurmasSemFechamentoBimestre(esc_id, uni_id, cal_id, cap_id);
        }

        /// <summary>
        /// Retorna todas as turmas que não fecharam o COC para avaliação recuperação final
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        public static DataTable SelecionaTurmasSemFechamentoRecuperaoFinal(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelecionaTurmasSemFechamentoRecuperacaoFinal(esc_id, uni_id, cal_id, cap_id);
        }

        /// <summary>
        /// Retorna as turmas que nunca foi salvo a frequencia de algum
        /// aluno na reunião de pais.
        /// </summary>
        /// <param name="esc_id">ID da escola da turma</param>
        /// <param name="uni_id">ID da unid administrativa da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public static DataTable SelecionaTurmasSemFrequenciaReuniaoPais(int esc_id, int uni_id, int cal_id, int cap_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelecionaTurmasSemFrequenciaReuniaoPais(esc_id, uni_id, cal_id, cap_id);
        }


        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="gru_id">Id do grupo do usuário.</param>
        /// <param name="usu_id">Id do usuário.</param>
        /// <returns></returns>
        public static DataTable SelecionaPorEscolaPeriodoMomentoAnoAcertoSituacao
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            return new TUR_TurmaDAO().SelecionaPorEscolaPeriodoMomentoAnoAcertoSituacao(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, usu_id, gru_id);
        }

        /// <summary>
        /// Retorna todas as turmas ativas e que seu calendario anual exista no MomentoAno
        /// e tenha o número da avaliação do currículo
        /// Utilizado no UserControl de Movimentação e na tela de Solicitação de Transferência
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curso do curriculo</param>
        /// <param name="crp_id">ID do curriculo do periodo</param>
        /// <param name="tca_numeroAvaliacao">Número da avaliação</param>
        /// <param name="ent_id">ID da entidade do usuário logado</param>
        /// <param name="usu_id">ID do usuário logado</param>
        /// <param name="gru_id">ID do grupo do usuário logado</param>
        /// <returns></returns>
        public static DataTable SelecionaPorEscolaPeriodoMomentoAnoAvaliacaoAcertoSituacao
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , int tca_numeroAvaliacao
            , Guid ent_id
            , Guid usu_id
            , Guid gru_id
        )
        {
            return new TUR_TurmaDAO().SelecionaPorEscolaPeriodoMomentoAnoAvaliacaoAcertoSituacao(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tca_numeroAvaliacao, ent_id, usu_id, gru_id);
        }

        /// <summary>
        /// Busca as turmas de SAAI – Sala de apoio e acompanhamento a inclusão e sala especial para os parametros passados.
        /// OBS: se o parametro doc_id for passado buscará as turmas daquele docente
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade</param>
        /// <param name="cal_id">ID do colendario</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns>Retorna uma lista com as turmas e turmas de sala especial com base nos parametros passados</returns>
        public static DataTable SelecionaTurmaSalaRecurso(int esc_id, int uni_id, int cal_id, long doc_id)
        {
            return new TUR_TurmaDAO().SelecionaTurmaSalaRecurso(esc_id, uni_id, cal_id, doc_id);
        }

        /// <summary>
        /// Seleciona as turmas ativas para configuração da matriz curricular de um curso.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo.</param>
        /// <param name="cal_ano">Ano do calendário.</param>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <returns></returns>
        public static DataTable SelecionaTurmasMatrizCurricular(int esc_id, int uni_id, int cur_id, int crr_id, int cal_ano, Guid ent_id)
        {
            return new TUR_TurmaDAO().SelecionaTurmasMatrizCurricular(esc_id, uni_id, cur_id, crr_id, cal_ano, ent_id);
        }

        /// <summary>
        /// Traz as turmas que o docente pode dar aula ou é coordenador
        ///	de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        ///	Se for conceito global, traz as turmas apenas se estiver configurado
        ///	que docentes pode efetivar notas do conceito global
        ///	Usada na tela de controle de turmas.
        /// </summary>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <param name="appMinutosCacheCurto">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <returns></returns>
        public static List<Struct_MinhasTurmas> SelecionaPorDocenteControleTurma(Guid ent_id, long doc_id, int appMinutosCacheCurto = 0, bool ativos = true)
        {
            List<Struct_MinhasTurmas> dados = SelecionaPorDocenteControleTurma(ent_id, doc_id, appMinutosCacheCurto);

            if (ativos)
            {
                List<Struct_MinhasTurmas> dadosAtivos;

                dadosAtivos = (from d in dados
                               where d.Turmas.Any(x => x.tdt_situacao == 1)
                               select new Struct_MinhasTurmas
                               {
                                   uad_idSuperior = d.uad_idSuperior
                            ,
                                   esc_id = d.esc_id
                            ,
                                   uni_id = d.uni_id
                            ,
                                   esc_nome = d.esc_nome
                            ,
                                   lengendTitulo = d.lengendTitulo
                            ,
                                   cal_id = d.cal_id
                            ,
                                   cal_ano = d.cal_ano
                            ,
                                   turmasAnoAtual = d.turmasAnoAtual
                            ,
                                   Turmas = (from drTurmas in d.Turmas
                                             where drTurmas.esc_id == d.esc_id
                                             orderby drTurmas.tur_codigo
                                             select new Struct_MinhasTurmas.Struct_Turmas
                                             {
                                                 tur_codigo = drTurmas.tur_codigo
                                                 ,
                                                 tud_nome = drTurmas.tud_nome
                                                 ,
                                                 tur_curso = drTurmas.tur_curso
                                                 ,
                                                 tur_turno = drTurmas.tur_turno
                                                 ,
                                                 tur_id = drTurmas.tur_id
                                                 ,
                                                 tur_escolaUnidade = drTurmas.tur_escolaUnidade
                                                 ,
                                                 tud_id = drTurmas.tud_id
                                                 ,
                                                 tdt_posicao = drTurmas.tdt_posicao
                                                 ,
                                                 tdc_id = drTurmas.tdc_id
                                                 ,
                                                 cal_id = drTurmas.cal_id
                                                 ,
                                                 esc_id = drTurmas.esc_id
                                                 ,
                                                 uni_id = drTurmas.uni_id
                                                 ,
                                                 mostraPosicao = true
                                                 ,
                                                 tud_naoLancarNota = drTurmas.tud_naoLancarNota
                                                 ,
                                                 tud_naoLancarFrequencia = drTurmas.tud_naoLancarFrequencia
                                                 ,
                                                 tud_disciplinaEspecial = drTurmas.tud_disciplinaEspecial
                                                 ,
                                                 tdt_situacao = drTurmas.tdt_situacao
                                                 ,
                                                 aulasPrevistasPreenchida = drTurmas.aulasPrevistasPreenchida
                                                 ,
                                                 tur_situacao = drTurmas.tur_situacao
                                                 ,
                                                 tur_calendario = drTurmas.tur_calendario
                                                 ,
                                                 tds_id = drTurmas.tds_id
                                                 ,
                                                 tci_ordem = drTurmas.tci_ordem
                                                 ,
                                                 disciplinaAtiva = drTurmas.disciplinaAtiva
                                                 ,
                                                 tud_tipo = drTurmas.tud_tipo
                                                 ,
                                                 cal_ano = drTurmas.cal_ano
                                                 ,
                                                 turmasAnoAtual = drTurmas.turmasAnoAtual
                                                 ,
                                                 tciIds = drTurmas.tciIds
                                                 ,
                                                 tur_tipo = drTurmas.tur_tipo
                                                 ,
                                                 tur_idNormal = drTurmas.tur_idNormal
                                                 ,
                                                 tud_idAluno = drTurmas.tud_idAluno
                                                 ,
                                                 fav_fechamentoAutomatico = drTurmas.fav_fechamentoAutomatico
                                                        ,
                                                 fav_id = drTurmas.fav_id
                                                 ,
                                                 tdt_id = drTurmas.tdt_id
                                                 ,
                                                 tdt_vigenciaInicio = drTurmas.tdt_vigenciaInicio
                                                 ,
                                                 tdt_vigenciaFim = drTurmas.tdt_vigenciaFim
                                                 ,
                                                 crg_tipo = drTurmas.crg_tipo
                                                 ,
                                                 tne_id = drTurmas.tne_id
                                             }).ToList()
                               }).ToList();

                dadosAtivos.OrderBy(g => g.cal_ano);

                return dadosAtivos;
            }
            return dados;
        }

        /// <summary>
        /// Remove do cache a consulta de turmas do docente, da tela de controle de turmas.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="doc_id">ID do docente</param>
        public static void RemoveCacheDocenteControleTurma(Guid ent_id, long doc_id)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_DocenteControleTurmas(ent_id, doc_id);
                HttpContext.Current.Cache.Remove(chave);
            }
        }

        public static void RemoveCacheDocente_TurmaDisciplina(long tur_id, long doc_id, Guid ent_id)
        {
            TUR_Turma tur = new TUR_Turma { tur_id = tur_id };
            TUR_TurmaBO.GetEntity(tur);
            List<Struct_CalendarioPeriodos> lstCap = ACA_CalendarioPeriodoBO.SelecionaPor_Calendario(tur.cal_id, GestaoEscolarUtilBO.MinutosCacheLongo);
            // Chave padrão do cache - nome do método + parâmetros.
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, true, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, true, false, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, false, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, false, false, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, true, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, true, false, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, false, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, false, false, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, true, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, true, false, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, false, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, false, false, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, true, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, true, false, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, false, true, 0));
            GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, false, false, 0));

            foreach (Struct_CalendarioPeriodos cap in lstCap)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, true, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, true, false, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, false, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, true, false, false, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, true, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, true, false, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, false, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, true, false, false, false, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, true, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, true, false, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, false, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, true, false, false, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, true, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, true, false, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, false, true, cap.cap_id));
                GestaoEscolarUtilBO.LimpaCache(TUR_TurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDocente(doc_id, ent_id, tur_id, false, false, false, false, cap.cap_id));
            }
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para a consulta de turmas do docente no
        /// controle de turmas.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="doc_id">ID do docente</param>
        /// <returns></returns>
        public static string RetornaChaveCache_DocenteControleTurmas(Guid ent_id, long doc_id)
        {
            return string.Format("Cache_SelecionaPorDocenteControleTurma_{0}_{1}", ent_id, doc_id > 0 ? doc_id.ToString() : "");
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar a tela de Minhas Turmas.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPorFiltrosMinhasTurmas(int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, Guid ent_id, string tur_codigo, int tci_id)
        {
            return string.Format("Cache_SelecionaPorFiltrosMinhasTurmas_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}", esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de turmas da tela de Minhas Turmas.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <returns></returns>
        private static string RetornaChaveCache_MinhasTurmasCombo(long tur_id)
        {
            return string.Format("Cache_MinhasTurmasCombo_{0}", tur_id);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de turmas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_Escola_Periodo_Situacao(Guid usu_id, Guid gru_id, bool adm, int esc_id, int uni_id,
                                                                             int cal_id, int cur_id, int crr_id, int crp_id, Guid ent_id,
                                                                             byte tur_tipo, byte tur_situacao, bool mostraEletivas)
        {
            return string.Format("Cache_GetSelectBy_Escola_Periodo_Situacao_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}",
                                 usu_id, gru_id, adm, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_tipo, tur_situacao, mostraEletivas);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de turmas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_Escola_Calendario_Situacao(int esc_id, int uni_id, int cal_id, byte tur_situacao)
        {
            return string.Format("Cache_GetSelectBy_Escola_Calendario_Situacao_{0}_{1}_{2}_{3}", esc_id, uni_id, cal_id, tur_situacao);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de turmas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_Docente_TodosTipos_Posicao(Guid ent_id, long doc_id, int posicao, int esc_id, int cal_id, bool mostrarCodigoNome, bool turmasNormais, bool mostraEletivas)
        {
            return string.Format("Cache_GetSelectBy_Docente_TodosTipos_Posicao_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}",
                                 ent_id, doc_id, posicao, esc_id, cal_id, mostrarCodigoNome, turmasNormais, mostraEletivas);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de turmas
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_EscolaCalendarioEPeriodo(Guid ent_id, int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, byte tur_situacao)
        {
            return string.Format("Cache_SelecionarPorEscolaCalendarioEPeriodo_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}",
                                 ent_id, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tur_situacao);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar o combo de turmas equivalentes
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPorEscolaPeriodoSituacaoEquivalentes(Guid usu_id, Guid gru_id, bool adm, int esc_id, int uni_id,
                                                                                                int cur_id, int crr_id, int crp_id, Guid ent_id,
                                                                                                byte tur_situacao)
        {
            return string.Format("Cache_SelecionaPorEscolaPeriodoSituacaoEquivalentes_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}",
                                 usu_id, gru_id, adm, esc_id, uni_id, cur_id, crr_id, crp_id, ent_id, tur_situacao);
        }

        /// <summary>
        /// Retorna a chave do cache utilizada para carregar a tela de Planejamento Semanal.
        /// </summary>
        /// <returns></returns>
        private static string RetornaChaveCache_SelecionaPorFiltrosPlanejamentoSemanal(int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, Guid ent_id, string tur_codigo, int tci_id, byte tud_tipo)
        {
            return string.Format("Cache_SelecionaPorFiltrosPlanejamentoSemanal_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}", esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id, tud_tipo);
        }

        /// <summary>
        /// Traz as turmas que o docente pode dar aula ou é coordenador
        ///	de alguma disciplina da turma, de todos os tipos (Normal e Eletiva do aluno).
        ///	Se for conceito global, traz as turmas apenas se estiver configurado
        ///	que docentes pode efetivar notas do conceito global
        ///	Usada na tela de controle de turmas.
        /// </summary>
        /// <param name="ent_id">ID da entidade do usuário logado.</param>
        /// <param name="doc_id">ID do docente.</param>
        /// <returns></returns>
        private static List<Struct_MinhasTurmas> SelecionaPorDocenteControleTurma(Guid ent_id, long doc_id, int appMinutosCacheCurto)
        {
            List<Struct_MinhasTurmas> dados;
            // Se não retornou os dados do cache, carrega do banco.

            if (appMinutosCacheCurto > 0)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_SelecionaPorDocenteControleTurma(ent_id.ToString(), doc_id.ToString());

                dados = CacheManager.Factory.Get
                            (
                                chave,
                                () =>
                                {
                                    return dados = (from DataRow dr in new TUR_TurmaDAO().SelecionaPorDocenteControleTurma(ent_id, doc_id).Rows
                                                    group dr by new { escola = Convert.ToInt32(dr["esc_id"]), calendario = Convert.ToInt32(dr["cal_id"]) } into g
                                                    select new Struct_MinhasTurmas
                                                    {
                                                        uad_idSuperior = new Guid(string.IsNullOrEmpty(g.FirstOrDefault()["uad_idSuperior"].ToString()) ? Guid.Empty.ToString() : g.FirstOrDefault()["uad_idSuperior"].ToString())
                                                        ,
                                                        esc_id = g.Key.escola
                                                        ,
                                                        uni_id = Convert.ToInt32(g.FirstOrDefault()["uni_id"])
                                                        ,
                                                        esc_nome = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                                                        ,
                                                        lengendTitulo = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                                                                        + "<br />Ano letivo: " + g.FirstOrDefault()["cal_ano"].ToString()
                                                        ,
                                                        cal_id = g.Key.calendario
                                                        ,
                                                        cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"].ToString())
                                                        ,
                                                        turmasAnoAtual = Convert.ToBoolean(g.FirstOrDefault()["turmasAnoAtual"].ToString())
                                                        ,
                                                        Turmas = (from DataRow drTurmas in g
                                                                  where Convert.ToInt32(drTurmas["esc_id"]) == g.Key.escola
                                                                  && Convert.ToInt32(drTurmas["cal_id"]) == g.Key.calendario
                                                                  orderby drTurmas["tur_codigo"].ToString()
                                                                  select new Struct_MinhasTurmas.Struct_Turmas
                                                                  {
                                                                      tur_codigo = drTurmas["tur_codigo"].ToString()
                                                                      ,
                                                                      tud_nome = drTurmas["tud_nome"].ToString()
                                                                      ,
                                                                      tur_curso = drTurmas["tur_curso"].ToString()
                                                                      ,
                                                                      tur_turno = drTurmas["tur_turno"].ToString()
                                                                      ,
                                                                      tur_id = Convert.ToInt64(drTurmas["tur_id"])
                                                                      ,
                                                                      fav_id = Convert.ToInt32(drTurmas["fav_id"])
                                                                      ,
                                                                      tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                                                                      ,
                                                                      tud_id = Convert.ToInt64(drTurmas["tud_id"])
                                                                      ,
                                                                      tdt_posicao = Convert.ToInt32(drTurmas["tdt_posicao"])
                                                                      ,
                                                                      tdc_id = Convert.ToByte(drTurmas["tdc_id"])
                                                                      ,
                                                                      cal_id = Convert.ToInt32(drTurmas["cal_id"])
                                                                      ,
                                                                      esc_id = Convert.ToInt32(drTurmas["esc_id"])
                                                                      ,
                                                                      uni_id = Convert.ToInt32(drTurmas["uni_id"])
                                                                      ,
                                                                      mostraPosicao = true
                                                                      ,
                                                                      tud_naoLancarNota = Convert.ToBoolean(drTurmas["tud_naoLancarNota"])
                                                                      ,
                                                                      tud_naoLancarFrequencia = Convert.ToBoolean(drTurmas["tud_naoLancarFrequencia"])
                                                                      ,
                                                                      tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                                                                      ,
                                                                      tdt_situacao = Convert.ToInt32(drTurmas["tdt_situacao"])
                                                                      ,
                                                                      aulasPrevistasPreenchida = Convert.ToBoolean(drTurmas["AulasPrevistasPreenchida"])
                                                                      ,
                                                                      tur_dataEncerramento = (string.IsNullOrEmpty(drTurmas["tur_dataEncerramento"].ToString()) ? new DateTime() : Convert.ToDateTime(drTurmas["tur_dataEncerramento"]))
                                                                      ,
                                                                      tur_situacao = Convert.ToInt32(drTurmas["tur_situacao"])
                                                                      ,
                                                                      tur_calendario = drTurmas["tur_calendario"].ToString()
                                                                      ,
                                                                      tds_id = Convert.ToInt32(drTurmas["tds_id"])
                                                                      ,
                                                                      disciplinaAtiva = Convert.ToBoolean(drTurmas["disciplinaAtiva"])
                                                                      ,
                                                                      tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                                                                      ,
                                                                      cal_ano = Convert.ToInt32(drTurmas["cal_ano"].ToString())
                                                                      ,
                                                                      turmasAnoAtual = Convert.ToBoolean(drTurmas["turmasAnoAtual"].ToString())
                                                                      ,
                                                                      tciIds = drTurmas["tciIds"].ToString()
                                                                      ,
                                                                      tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                                                                      ,
                                                                      tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                                                                      ,
                                                                      tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                                                                      ,
                                                                      fav_fechamentoAutomatico = Convert.ToBoolean(drTurmas["fav_fechamentoAutomatico"])
                                                                      ,
                                                                      tdt_id = Convert.ToInt32(drTurmas["tdt_id"].ToString())
                                                                      ,
                                                                      tdt_vigenciaInicio = Convert.ToDateTime(drTurmas["tdt_vigenciaInicio"].ToString())
                                                                      ,
                                                                      tdt_vigenciaFim =
                                                                          drTurmas["tdt_vigenciaFim"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaFim"].ToString())
                                                                          : new DateTime()
                                                                      ,
                                                                      crg_tipo = Convert.ToByte(drTurmas["crg_tipo"].ToString())
                                                                      ,
                                                                      tne_id = drTurmas["tne_id"] != DBNull.Value ?
                                                                          Convert.ToInt32(drTurmas["tne_id"].ToString())
                                                                          : -1
                                                                  }).ToList()
                                                    }).ToList();
                                },
                                appMinutosCacheCurto
                            );
            }
            else
            {
                dados = (from DataRow dr in new TUR_TurmaDAO().SelecionaPorDocenteControleTurma(ent_id, doc_id).Rows
                         group dr by new { escola = Convert.ToInt32(dr["esc_id"]), calendario = Convert.ToInt32(dr["cal_id"]) } into g
                         select new Struct_MinhasTurmas
                         {
                             uad_idSuperior = new Guid(string.IsNullOrEmpty(g.FirstOrDefault()["uad_idSuperior"].ToString()) ? Guid.Empty.ToString() : g.FirstOrDefault()["uad_idSuperior"].ToString())
                             ,
                             esc_id = g.Key.escola
                             ,
                             uni_id = Convert.ToInt32(g.FirstOrDefault()["uni_id"])
                             ,
                             esc_nome = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                             ,
                             lengendTitulo = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                                + "<br />" + g.FirstOrDefault()["tur_calendario"].ToString()
                             ,
                             cal_id = g.Key.calendario
                             ,
                             cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"].ToString())
                             ,
                             turmasAnoAtual = Convert.ToBoolean(g.FirstOrDefault()["turmasAnoAtual"].ToString())
                             ,
                             Turmas = (from DataRow drTurmas in g
                                       where Convert.ToInt32(drTurmas["esc_id"]) == g.Key.escola
                                       && Convert.ToInt32(drTurmas["cal_id"]) == g.Key.calendario
                                       orderby drTurmas["tur_codigo"].ToString()
                                       select new Struct_MinhasTurmas.Struct_Turmas
                                       {
                                           tur_codigo = drTurmas["tur_codigo"].ToString()
                                           ,
                                           tud_nome = drTurmas["tud_nome"].ToString()
                                           ,
                                           tur_curso = drTurmas["tur_curso"].ToString()
                                           ,
                                           tur_turno = drTurmas["tur_turno"].ToString()
                                           ,
                                           tur_id = Convert.ToInt64(drTurmas["tur_id"])
                                           ,
                                           fav_id = Convert.ToInt32(drTurmas["fav_id"])
                                              ,
                                           tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                                           ,
                                           tud_id = Convert.ToInt64(drTurmas["tud_id"])
                                           ,
                                           tdt_posicao = Convert.ToInt32(drTurmas["tdt_posicao"])
                                           ,
                                           tdc_id = Convert.ToByte(drTurmas["tdc_id"])
                                           ,
                                           cal_id = Convert.ToInt32(drTurmas["cal_id"])
                                           ,
                                           esc_id = Convert.ToInt32(drTurmas["esc_id"])
                                           ,
                                           uni_id = Convert.ToInt32(drTurmas["uni_id"])
                                           ,
                                           mostraPosicao = true
                                           ,
                                           tud_naoLancarNota = Convert.ToBoolean(drTurmas["tud_naoLancarNota"])
                                           ,
                                           tud_naoLancarFrequencia = Convert.ToBoolean(drTurmas["tud_naoLancarFrequencia"])
                                           ,
                                           tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                                           ,
                                           tdt_situacao = Convert.ToInt32(drTurmas["tdt_situacao"])
                                           ,
                                           aulasPrevistasPreenchida = Convert.ToBoolean(drTurmas["AulasPrevistasPreenchida"])
                                           ,
                                           tur_dataEncerramento = (string.IsNullOrEmpty(drTurmas["tur_dataEncerramento"].ToString()) ? new DateTime() : Convert.ToDateTime(drTurmas["tur_dataEncerramento"]))
                                           ,
                                           tur_situacao = Convert.ToInt32(drTurmas["tur_situacao"])
                                           ,
                                           tur_calendario = drTurmas["tur_calendario"].ToString()
                                           ,
                                           tds_id = Convert.ToInt32(drTurmas["tds_id"])
                                           ,
                                           disciplinaAtiva = Convert.ToBoolean(drTurmas["disciplinaAtiva"])
                                           ,
                                           tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                                           ,
                                           cal_ano = Convert.ToInt32(drTurmas["cal_ano"].ToString())
                                           ,
                                           turmasAnoAtual = Convert.ToBoolean(drTurmas["turmasAnoAtual"].ToString())
                                           ,
                                           tciIds = drTurmas["tciIds"].ToString()
                                           ,
                                           tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                                           ,
                                           tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                                           ,
                                           tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                                           ,
                                           fav_fechamentoAutomatico = Convert.ToBoolean(drTurmas["fav_fechamentoAutomatico"])
                                           ,
                                           tdt_id = Convert.ToInt32(drTurmas["tdt_id"].ToString())
                                           ,
                                           tdt_vigenciaInicio = Convert.ToDateTime(drTurmas["tdt_vigenciaInicio"].ToString())
                                           ,
                                           tdt_vigenciaFim = drTurmas["tdt_vigenciaFim"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaFim"].ToString())
                                                                          : new DateTime()
                                           ,
                                           crg_tipo = Convert.ToByte(drTurmas["crg_tipo"].ToString())
                                           ,
                                           tne_id = drTurmas["tne_id"] != DBNull.Value ?
                                                                          Convert.ToInt32(drTurmas["tne_id"].ToString())
                                                                          : -1
                                       }).ToList()
                         }).ToList();
            }

            dados.OrderBy(g => g.cal_ano);

            return dados;
        }

        /// <summary>
        /// Retorna a capacidade das turmas e a quantidade de alunos matriculados ativos, para
        /// a turma informada.
        /// </summary>
        /// <param name="tur_id">IDs das turmas separados por ","</param>
        /// <param name="Capacidade">Capacidade da turma</param>
        /// <param name="qtMatriculados">Quantidade de matrículas ativas na turma</param>
        /// <returns></returns>
        public static void RetornaVagasMatriculadosPor_Turma(long tur_id, out int Capacidade, out int qtMatriculados)
        {
            int QtDeficientes;
            string tur_codigo;
            RetornaVagasMatriculadosPor_Turma(tur_id, out Capacidade, out qtMatriculados, null, out QtDeficientes, out tur_codigo);
        }

        /// <summary>
        /// Retorna a capacidade das turmas e a quantidade de alunos matriculados ativos, para
        /// a turma informada.
        /// </summary>
        /// <param name="tur_id">IDs das turmas separados por ","</param>
        /// <param name="Capacidade">Capacidade da turma</param>
        /// <param name="qtMatriculados">Quantidade de matrículas ativas na turma</param>
        /// <param name="banco">Transação com banco de dados</param>
        /// <param name="QtDeficientes">Retorna a quantidade de deficientes matriculados na turma</param>
        /// <param name="tur_codigo">Retorna o código da turma</param>
        /// <returns></returns>
        public static void RetornaVagasMatriculadosPor_Turma
            (long tur_id, out int Capacidade, out int qtMatriculados, TalkDBTransaction banco, out int QtDeficientes, out string tur_codigo)
        {
            Capacidade = 0;
            qtMatriculados = 0;
            QtDeficientes = 0;
            tur_codigo = "";

            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            if (banco != null)
            {
                dao._Banco = banco;
            }

            DataTable dt = dao.SelecionaVagasMatriculadosPor_Turmas(tur_id.ToString());

            if (dt.Rows.Count > 0)
            {
                Capacidade = Convert.ToInt32(dt.Rows[0]["Capacidade"]);
                qtMatriculados = Convert.ToInt32(dt.Rows[0]["QtMatriculasAtivas"]);
                QtDeficientes = Convert.ToInt32(dt.Rows[0]["QtDeficientes"]);
                tur_codigo = dt.Rows[0]["tur_codigo"].ToString();
            }
        }

        /// <summary>
        /// Retorna a capacidade das turmas e a quantidade de alunos matriculados ativos, pra
        /// cada turma.
        /// </summary>
        /// <param name="tur_ids">IDs das turmas separados por ","</param>
        /// <returns></returns>
        public static DataTable RetornaVagasMatriculadosPor_Turma(string tur_ids, TalkDBTransaction banco)
        {
            if (string.IsNullOrEmpty(tur_ids))
                return new DataTable();
            return new TUR_TurmaDAO { _Banco = banco }.SelecionaVagasMatriculadosPor_Turmas(tur_ids);
        }

        /// <summary>
        /// Retorna a capacidade das turmas e a quantidade de alunos matriculados ativos, pra
        /// cada turma.
        /// </summary>
        /// <param name="tur_ids">IDs das turmas separados por ","</param>
        /// <returns></returns>
        public static DataTable RetornaVagasMatriculadosPor_Turma(string tur_ids)
        {
            if (string.IsNullOrEmpty(tur_ids))
                return new DataTable();
            return new TUR_TurmaDAO().SelecionaVagasMatriculadosPor_Turmas(tur_ids);
        }

        /// <summary>
        /// Retorna as informações do turno referente a turma.
        /// </summary>
        /// <param name="tur_id">Id da turma.</param>
        /// <param name="bancoGestao">Transação do banco.</param>
        /// <returns>Informações do turno referente a turma.</returns>
        public static DataTable SelecionaTurnoPorTurma
        (
            long tur_id,
            TalkDBTransaction bancoGestao
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = bancoGestao };
            return dao.SelecionaTurnoPorTurma(tur_id);
        }

        /// <summary>
        /// Gerar o código da turma normal automaticamente
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="numeroTurma">Indica o número da turma que será gerada</param>
        /// <param name="pft">Entidade parâmetros de formação de turmas</param>
        /// <returns>Novo código da turma normal</returns>
        public static string GerarCodigoTurmaNormal
        (
            int esc_id
            , int uni_id
            , int numeroTurma
            , MTR_ParametroFormacaoTurma pft
            , Guid ent_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();

            string ultimoCodigoTurma = VerificaUltimoCodigoTurmaCadastrado(esc_id, uni_id, pft.cal_id, pft.cur_id, pft.crr_id, pft.crp_id, pft.pft_prefixoCodigoTurma);
            string codigoTurma = calculaCodigoTurmaNormal(ultimoCodigoTurma, numeroTurma, pft, ent_id);
            string codigo = pft.pft_prefixoCodigoTurma + codigoTurma;

            bool verifica = dao.VerificaPrefixoCodigoPorEscolaCalendario(esc_id, uni_id, pft.cal_id, codigo);

            while (verifica)
            {
                codigoTurma = calculaCodigoTurmaNormal(codigo, numeroTurma, pft, ent_id);
                codigo = pft.pft_prefixoCodigoTurma + codigoTurma;

                verifica = dao.VerificaPrefixoCodigoPorEscolaCalendario(esc_id, uni_id, pft.cal_id, codigo);
            }

            return codigo;
        }

        public static string calculaCodigoTurmaNormal
        (
            string ultimoCodigoTurma
            , int numeroTurma
            , MTR_ParametroFormacaoTurma pft
            , Guid ent_id
        )
        {
            int novoSufixo;
            string codigoTurma = string.Empty;

            if ((MTR_ParametroFormacaoTurmaTipoDigito)pft.pft_tipoDigitoCodigoTurma == MTR_ParametroFormacaoTurmaTipoDigito.Alfabetico)
            {
                char ultimoSufixo;

                if (!string.IsNullOrEmpty(pft.pft_prefixoCodigoTurma))
                    ultimoSufixo = string.IsNullOrEmpty(ultimoCodigoTurma) ? '0' : Convert.ToChar(ultimoCodigoTurma.Substring(pft.pft_prefixoCodigoTurma.Length, 1));
                else
                    ultimoSufixo = string.IsNullOrEmpty(ultimoCodigoTurma) ? '0' : Convert.ToChar(ultimoCodigoTurma.Substring(1, 1));

                novoSufixo = Convert.ToInt32(ultimoSufixo);
                novoSufixo = novoSufixo == 48 ? novoSufixo + 16 + numeroTurma : novoSufixo + numeroTurma;

                //testa se já passou do 'Z' na tabela acii e entrou nos caracteres especiais.
                if (novoSufixo > 90)
                {
                    //São permitidas no máximo 26 turmas por ‘agrupamento’ quando o tipo de código de turma é alfabético.
                    throw new ValidationException("São permitidas no máximo 26 turmas por " +
                                                  GestaoEscolarUtilBO.nomePadraoPeriodo(ent_id).ToLower() +
                                                  " quando o tipo de código da turma é alfabético.");
                }

                codigoTurma = Convert.ToChar(novoSufixo).ToString();
            }
            else if ((MTR_ParametroFormacaoTurmaTipoDigito)pft.pft_tipoDigitoCodigoTurma == MTR_ParametroFormacaoTurmaTipoDigito.Numerico)
            {
                string ultimoSufixo;

                if (!string.IsNullOrEmpty(pft.pft_prefixoCodigoTurma))
                    ultimoSufixo = string.IsNullOrEmpty(ultimoCodigoTurma) ? "0" : Convert.ToString(ultimoCodigoTurma.Substring(pft.pft_prefixoCodigoTurma.Length));
                else
                    ultimoSufixo = string.IsNullOrEmpty(ultimoCodigoTurma) ? "0" : Convert.ToString(ultimoCodigoTurma);

                int.TryParse(ultimoSufixo, out novoSufixo);
                novoSufixo = novoSufixo + numeroTurma;

                codigoTurma = novoSufixo.ToString().PadLeft(pft.pft_qtdDigitoCodigoTurma, '0');
            }

            return codigoTurma;
        }

        /// <summary>
        /// Gerar o código da turma eletiva automaticamente
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="uni_id">Id da unidade da escola</param>
        /// <param name="cur_id">Id do curso</param>
        /// <param name="crr_id">Id do currículo do curso</param>
        /// <param name="dis_id">Id da disciplina eletiva</param>
        /// <param name="numeroTurma">Indica o número da turma que será gerada</param>
        /// <param name="ptf">Entidade parâmetros de formação de turmas</param>
        /// <param name="banco">Transação (Opcional)</param>
        /// <returns>Novo código da turma eletiva</returns>
        public static string GerarCodigoTurmaEletiva
        (
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int dis_id
            , int numeroTurma
            , MTR_ParametroFormacaoTurma ptf
            , TalkDBTransaction banco
        )
        {
            string codigoTurma = string.Empty;
            string tea_sigla;

            ACA_Disciplina dis = new ACA_Disciplina { dis_id = dis_id };
            if (banco == null)
                ACA_DisciplinaBO.GetEntity(dis);
            else
                ACA_DisciplinaBO.GetEntity(dis, banco);

            DataTable dtaSigla = ACA_DisciplinaMacroCampoEletivaAlunoBO.SelecionaMacroCampoDisciplina(dis_id, banco);
            if (dtaSigla.Rows.Count > 0)
            {
                DataRow drNomeSigla = (from DataRow row in dtaSigla.Rows
                                       select row).FirstOrDefault();

                tea_sigla = drNomeSigla["tea_sigla"] + dis.dis_codigo;
            }
            else
            {
                throw new ValidationException(String.Format("O(a) " + CustomResource.GetGlobalResourceObject("Mensagens", "MSG_DISCIPLINA") + " {0} não possui macro-campo(s) associado(s).", dis.dis_nome));
            }

            string ultimoCodigoTurma = VerificaUltimoCodigoTurmaEletivaCadastrado(esc_id, uni_id, ptf.cal_id, cur_id, crr_id, tea_sigla, banco);

            if (!string.IsNullOrEmpty(ultimoCodigoTurma))
            {
                string ultimoSufixo = Convert.ToString(ultimoCodigoTurma.Substring((ultimoCodigoTurma.Length) - 2));

                int novoSufixo;
                int.TryParse(ultimoSufixo, out novoSufixo);
                novoSufixo = novoSufixo + numeroTurma;

                codigoTurma = tea_sigla + novoSufixo.ToString().PadLeft(2, '0');
            }
            else if (!string.IsNullOrEmpty(tea_sigla))
            {
                codigoTurma = tea_sigla + numeroTurma.ToString().PadLeft(2, '0');
            }

            return codigoTurma;
        }

        /// <summary>
        /// Retorna a Quantidade de Vagas levando em consideração os parâmetros de deficientes.
        /// </summary>
        /// <param name="entTurma">Turma</param>
        /// <returns>Quantidade de Vagas</returns>
        public static int QuantidadeVagas(TUR_Turma entTurma)
        {
            // Verificação de Quantidade de Vagas por Deficientes/Incluídos
            TUR_TurmaCurriculo turmaCurriculo = TUR_TurmaCurriculoBO.GetSelectBy_Turma(entTurma.tur_id, GestaoEscolarUtilBO.MinutosCacheLongo).FirstOrDefault();
            if (turmaCurriculo != null)
            {
                ESC_Escola esc = new ESC_Escola { esc_id = entTurma.esc_id };
                ESC_EscolaBO.GetEntity(esc);

                MTR_ParametroFormacaoTurma parametroFormacaoTurma = MTR_ParametroFormacaoTurmaBO.SelecionaParametroPorAnoCursoPeriodo(
                                                                        turmaCurriculo.cur_id, turmaCurriculo.crr_id, turmaCurriculo.crp_id, esc.ent_id);
                if (parametroFormacaoTurma == null)
                    return entTurma.tur_vagas;

                switch (parametroFormacaoTurma.pft_tipoControleCapacidade)
                {
                    case (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.SemControle:
                        return entTurma.tur_vagas;

                    case (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormal:
                        DataTable dtAlunosDeficientes2 = MTR_MatriculaTurmaBO.RetornaAlunoDeficienteTurma(entTurma.tur_id);
                        if (dtAlunosDeficientes2.Rows.Count == 0)
                        {
                            return entTurma.tur_vagas;
                        }

                        return parametroFormacaoTurma.pft_capacidadeComDeficiente;

                    case (byte)MTR_ParametroFormacaoTurmaTipoControleCapacidade.CapacidadeNormalIndividual:
                        DataTable dtAlunosDeficientes = MTR_MatriculaTurmaBO.RetornaAlunoDeficienteTurma(entTurma.tur_id);
                        if (dtAlunosDeficientes.Rows.Count == 0)
                        {
                            return entTurma.tur_vagas;
                        }

                        List<MTR_ParametroFormacaoTurmaCapacidadeDeficiente> listParametroFormacaoTurmaCapacidadeDeficiente =
                            MTR_ParametroFormacaoTurmaCapacidadeDeficienteBO.SelecionaPorProcessoParametro(parametroFormacaoTurma.pfi_id, parametroFormacaoTurma.pft_id);

                        MTR_ParametroFormacaoTurmaCapacidadeDeficiente parametroFormacaoTurmaCapacidadeDeficiente =
                            listParametroFormacaoTurmaCapacidadeDeficiente.Where(p => p.pfc_qtdDeficiente.Equals(dtAlunosDeficientes.Rows.Count)).FirstOrDefault();
                        if (parametroFormacaoTurmaCapacidadeDeficiente == null)
                        {
                            if (listParametroFormacaoTurmaCapacidadeDeficiente.Count == 0)
                            {
                                return entTurma.tur_vagas;
                            }

                            return listParametroFormacaoTurmaCapacidadeDeficiente.OrderByDescending(p => p.pfc_qtdDeficiente).First().pfc_capacidadeComDeficiente;
                        }

                        return parametroFormacaoTurmaCapacidadeDeficiente.pfc_capacidadeComDeficiente;

                    default:
                        return entTurma.tur_vagas;
                }
            }

            return entTurma.tur_vagas;
        }

        /// <summary>
        /// Retorna uma lista de minhas turmas mediante aos filtros informados
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        /// <param name="appMinutosCacheCurto">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <returns>Lista</returns>
        public static List<Struct_MinhasTurmas> SelecionaPorFiltrosMinhasTurmas(
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int tci_id = 0
            , int appMinutosCacheCurto = 0
        )
        {
            return SelecionaPorFiltrosMinhasTurmas(esc_id, uni_id, 0, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id, appMinutosCacheCurto);
        }

        /// <summary>
        /// Retorna uma datatable mediante os filtros informados na tela de Busca
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="appMinutosCacheCurto">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        /// <returns>DataTable</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorFiltrosMinhasTurmasPaginado(
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int appMinutosCacheCurto = 0
            , int tci_id = 0
        )
        {
            return SelecionaPorFiltrosMinhasTurmasPaginado(esc_id, uni_id, 0, cur_id, crr_id, crp_id, ent_id, tur_codigo, appMinutosCacheCurto, tci_id);
        }

        /// <summary>
        /// Retorna uma datatable mediante os filtros informados na tela de Busca
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="appMinutosCacheCurto">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        /// <returns>DataTable</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorFiltrosMinhasTurmasPaginado(
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int appMinutosCacheCurto = 0
            , int tci_id = 0
        )
        {
            List<Struct_MinhasTurmas> dados = SelecionaPorFiltrosMinhasTurmas(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id, appMinutosCacheCurto);

            if (dados.Count <= 0)
            {
                totalRecords = 0;
                return null;
            }

            totalRecords = (dados[0]).Turmas.Count;
            return ConvertToDataTable((dados[0]).Turmas);
        }

        /// <summary>
        /// Retorna uma datatable mediante os filtros informados na tela de Busca
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="appMinutosCacheCurto">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        /// <param name="tud_tipo">Tipo da disciplina</param>
        /// <returns>DataTable</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorFiltrosPlanejamentoSemanalPaginado(
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , byte tud_tipo
            , int appMinutosCacheCurto = 0
            , int tci_id = 0
        )
        {
            List<Struct_MinhasTurmas> dados = SelecionaPorFiltrosPlanejamentoSemanal(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id, tud_tipo, appMinutosCacheCurto);

            if (dados.Count <= 0)
            {
                totalRecords = 0;
                return null;
            }

            totalRecords = (dados[0]).Turmas.Count;
            return ConvertToDataTable((dados[0]).Turmas);
        }

        /// <summary>
        /// Retorna uma lista de planejamento semanal mediante aos filtros informados
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="appMinutosCacheCurto">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        /// <param name="tud_tipo">Tipo da disciplina</param>
        /// <returns>Lista</returns>
        public static List<Struct_MinhasTurmas> SelecionaPorFiltrosPlanejamentoSemanal(
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int tci_id
            , byte tud_tipo
            , int appMinutosCacheCurto = 0
            )
        {
            List<Struct_MinhasTurmas> dados;
            if (appMinutosCacheCurto > 0 && HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_SelecionaPorFiltrosPlanejamentoSemanal(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id, tud_tipo);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    // Se não retornou os dados do cache, carrega do banco.
                    DataTable dt = new TUR_TurmaDAO().SelecionaPorFiltrosPlanejamentoSemanal(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id, tud_tipo);

                    dados = (from DataRow dr in dt.Rows
                             group dr by Convert.ToInt32(dr["esc_id"]) into g
                             select new Struct_MinhasTurmas
                             {
                                 esc_id = g.Key
                                 ,
                                 uni_id = Convert.ToInt32(g.FirstOrDefault()["uni_id"])
                                 ,
                                 lengendTitulo = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                                    + "<br />" + g.FirstOrDefault()["tur_calendario"].ToString()
                                 ,
                                 cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"].ToString())
                                 ,
                                 turmasAnoAtual = Convert.ToBoolean(g.FirstOrDefault()["turmasAnoAtual"].ToString())
                                 ,
                                 Turmas = (from DataRow drTurmas in g
                                           where Convert.ToInt32(drTurmas["esc_id"]) == g.Key
                                           orderby drTurmas["tur_codigo"].ToString()
                                           select new Struct_MinhasTurmas.Struct_Turmas
                                           {
                                               tur_codigo = drTurmas["tur_codigo"].ToString()
                                               ,
                                               tud_nome = drTurmas["tud_nome"].ToString()
                                               ,
                                               tur_curso = drTurmas["tur_curso"].ToString()
                                               ,
                                               tur_turno = drTurmas["tur_turno"].ToString()
                                               ,
                                               tur_id = Convert.ToInt64(drTurmas["tur_id"])
                                               ,
                                               tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                                               ,
                                               tud_id = Convert.ToInt64(drTurmas["tud_id"])
                                               ,
                                               tdt_posicao = Convert.ToInt32(drTurmas["tdt_posicao"])
                                               ,
                                               tdc_id = Convert.ToByte(drTurmas["tdc_id"])
                                               ,
                                               cal_id = Convert.ToInt32(drTurmas["cal_id"])
                                               ,
                                               esc_id = Convert.ToInt32(drTurmas["esc_id"])
                                               ,
                                               uni_id = Convert.ToInt32(drTurmas["uni_id"])
                                               ,
                                               mostraPosicao = false // não mostrar a posição quando for o admin
                                               ,
                                               tud_naoLancarNota = Convert.ToBoolean(drTurmas["tud_naoLancarNota"])
                                               ,
                                               tud_naoLancarFrequencia = Convert.ToBoolean(drTurmas["tud_naoLancarFrequencia"])
                                               ,
                                               tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                                               ,
                                               tdt_situacao = 1
                                               //,
                                               //aulasPrevistasPreenchida = Convert.ToBoolean(drTurmas["AulasPrevistasPreenchida"])
                                               ,
                                               tur_calendario = drTurmas["tur_calendario"].ToString()
                                               ,
                                               tds_id = Convert.ToInt32(drTurmas["tds_id"])
                                               ,
                                               disciplinaAtiva = true
                                               ,
                                               tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                                               ,
                                               tur_dataEncerramento = Convert.ToDateTime(drTurmas["tur_dataEncerramento"] == DBNull.Value ? new DateTime() : drTurmas["tur_dataEncerramento"])
                                               ,
                                               cal_ano = Convert.ToInt32(drTurmas["cal_ano"].ToString())
                                               ,
                                               turmasAnoAtual = Convert.ToBoolean(drTurmas["turmasAnoAtual"].ToString())
                                               ,
                                               tciIds = drTurmas["tciIds"].ToString()
                                               ,
                                               tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                                               ,
                                               tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                                               ,
                                               tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                                               ,
                                               tur_codigoNormal = drTurmas["tur_codigoNormal"].ToString()
                                               ,
                                               fav_id = Convert.ToInt32(drTurmas["fav_id"])
                                               ,
                                               tdt_id = drTurmas["tdt_id"] != DBNull.Value ?
                                                                          Convert.ToInt32(drTurmas["tdt_id"].ToString())
                                                                          : 0
                                                                      ,
                                               tdt_vigenciaInicio = drTurmas["tdt_vigenciaInicio"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaInicio"].ToString())
                                                                          : new DateTime()
                                                                      ,
                                               tdt_vigenciaFim =
                                                                          drTurmas["tdt_vigenciaFim"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaFim"].ToString())
                                                                          : new DateTime()
                                                                      ,
                                               crg_tipo = drTurmas["crg_tipo"] != DBNull.Value ?
                                                                          Convert.ToByte(drTurmas["crg_tipo"].ToString())
                                                                          : (byte)0
                                           }).ToList()
                             }).ToList();
                    // Adiciona cache com validade do tempo informado na configuração.
                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheCurto), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<Struct_MinhasTurmas>)cache;
                }
            }
            else
            {
                // Se não retornou os dados do cache, carrega do banco.
                DataTable dt = new TUR_TurmaDAO().SelecionaPorFiltrosPlanejamentoSemanal(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id, tud_tipo);

                dados = (from DataRow dr in dt.Rows
                         group dr by Convert.ToInt32(dr["esc_id"]) into g
                         select new Struct_MinhasTurmas
                         {
                             esc_id = g.Key
                             ,
                             uni_id = Convert.ToInt32(g.FirstOrDefault()["uni_id"])
                             ,
                             lengendTitulo = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                                + "<br />" + g.FirstOrDefault()["tur_calendario"].ToString()
                             ,
                             cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"].ToString())
                             ,
                             turmasAnoAtual = Convert.ToBoolean(g.FirstOrDefault()["turmasAnoAtual"].ToString())
                             ,
                             Turmas = (from DataRow drTurmas in g
                                       where Convert.ToInt32(drTurmas["esc_id"]) == g.Key
                                       orderby drTurmas["tur_codigo"].ToString()
                                       select new Struct_MinhasTurmas.Struct_Turmas
                                       {
                                           tur_codigo = drTurmas["tur_codigo"].ToString()
                                           ,
                                           tud_nome = drTurmas["tud_nome"].ToString()
                                           ,
                                           tur_curso = drTurmas["tur_curso"].ToString()
                                           ,
                                           tur_turno = drTurmas["tur_turno"].ToString()
                                           ,
                                           tur_id = Convert.ToInt64(drTurmas["tur_id"])
                                           ,
                                           tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                                           ,
                                           tud_id = Convert.ToInt64(drTurmas["tud_id"])
                                           ,
                                           tdt_posicao = Convert.ToInt32(drTurmas["tdt_posicao"])
                                           ,
                                           tdc_id = Convert.ToByte(drTurmas["tdc_id"])
                                           ,
                                           cal_id = Convert.ToInt32(drTurmas["cal_id"])
                                           ,
                                           esc_id = Convert.ToInt32(drTurmas["esc_id"])
                                           ,
                                           uni_id = Convert.ToInt32(drTurmas["uni_id"])
                                           ,
                                           mostraPosicao = false // não mostrar a posição quando for o admin
                                           ,
                                           tud_naoLancarNota = Convert.ToBoolean(drTurmas["tud_naoLancarNota"])
                                           ,
                                           tud_naoLancarFrequencia = Convert.ToBoolean(drTurmas["tud_naoLancarFrequencia"])
                                           ,
                                           tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                                           ,
                                           tdt_situacao = 1
                                           //,
                                           //aulasPrevistasPreenchida = Convert.ToBoolean(drTurmas["AulasPrevistasPreenchida"])
                                           ,
                                           tur_calendario = drTurmas["tur_calendario"].ToString()
                                           ,
                                           tds_id = Convert.ToInt32(drTurmas["tds_id"])
                                           ,
                                           disciplinaAtiva = true
                                           ,
                                           tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                                           ,
                                           tur_dataEncerramento = Convert.ToDateTime(drTurmas["tur_dataEncerramento"] == DBNull.Value ? new DateTime() : drTurmas["tur_dataEncerramento"])
                                           ,
                                           cal_ano = Convert.ToInt32(drTurmas["cal_ano"].ToString())
                                           ,
                                           turmasAnoAtual = Convert.ToBoolean(drTurmas["turmasAnoAtual"].ToString())
                                           ,
                                           tciIds = drTurmas["tciIds"].ToString()
                                           ,
                                           tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                                           ,
                                           tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                                           ,
                                           tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                                           ,
                                           tur_codigoNormal = drTurmas["tur_codigoNormal"].ToString()
                                           ,
                                           tdt_id = Convert.ToInt32(drTurmas["tdt_id"].ToString())
                                                                      ,
                                           tdt_vigenciaInicio = Convert.ToDateTime(drTurmas["tdt_vigenciaInicio"].ToString())
                                                                      ,
                                           tdt_vigenciaFim =
                                                                          drTurmas["tdt_vigenciaFim"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaFim"].ToString())
                                                                          : new DateTime()
                                                                      ,
                                           crg_tipo = Convert.ToByte(drTurmas["crg_tipo"].ToString())
                                       }).ToList()
                         }).ToList();
            }
            return dados;
        }

        /// <summary>
        /// Retorna uma lista de minhas turmas mediante aos filtros informados
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        /// <returns>Lista</returns>
        public static List<Struct_MinhasTurmas> SelecionaPorFiltrosMinhasTurmas(
            int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int tci_id
            )
        {
            return SelecionaPorFiltrosMinhasTurmas(esc_id, uni_id, 0, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id);
        }

        /// <summary>
        /// Retorna uma lista de minhas turmas mediante aos filtros informados
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="ent_id">ID da entidade - obrigatório</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="appMinutosCacheCurto">Minutos configurados para guardar a consulta em cache (caso não informado, não utiliza cache)</param>
        /// <param name="tci_id">ID do tipo de ciclo</param>
        /// <returns>Lista</returns>
        public static List<Struct_MinhasTurmas> SelecionaPorFiltrosMinhasTurmas(
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , string tur_codigo
            , int tci_id
            , int appMinutosCacheCurto = 0
            )
        {
            List<Struct_MinhasTurmas> dados;
            if (appMinutosCacheCurto > 0 && HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_SelecionaPorFiltrosMinhasTurmas(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    // Se não retornou os dados do cache, carrega do banco.
                    DataTable dt = new TUR_TurmaDAO().SelecionaPorFiltrosMinhasTurmas(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id);

                    dados = (from DataRow dr in dt.Rows
                             group dr by Convert.ToInt32(dr["esc_id"]) into g
                             select new Struct_MinhasTurmas
                             {
                                 esc_id = g.Key
                                 ,
                                 uni_id = Convert.ToInt32(g.FirstOrDefault()["uni_id"])
                                 ,
                                 lengendTitulo = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                                    + "<br />" + g.FirstOrDefault()["tur_calendario"].ToString()
                                 ,
                                 cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"].ToString())
                                 ,
                                 turmasAnoAtual = Convert.ToBoolean(g.FirstOrDefault()["turmasAnoAtual"].ToString())
                                 ,
                                 Turmas = (from DataRow drTurmas in g
                                           where Convert.ToInt32(drTurmas["esc_id"]) == g.Key
                                           orderby drTurmas["tur_codigo"].ToString()
                                           select new Struct_MinhasTurmas.Struct_Turmas
                                           {
                                               tur_codigo = drTurmas["tur_codigo"].ToString()
                                               ,
                                               tud_nome = drTurmas["tud_nome"].ToString()
                                               ,
                                               tur_curso = drTurmas["tur_curso"].ToString()
                                               ,
                                               tur_turno = drTurmas["tur_turno"].ToString()
                                               ,
                                               tur_id = Convert.ToInt64(drTurmas["tur_id"])
                                               ,
                                               tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                                               ,
                                               tud_id = Convert.ToInt64(drTurmas["tud_id"])
                                               ,
                                               tdt_posicao = Convert.ToInt32(drTurmas["tdt_posicao"])
                                               ,
                                               tdc_id = Convert.ToByte(drTurmas["tdc_id"])
                                               ,
                                               cal_id = Convert.ToInt32(drTurmas["cal_id"])
                                               ,
                                               esc_id = Convert.ToInt32(drTurmas["esc_id"])
                                               ,
                                               uni_id = Convert.ToInt32(drTurmas["uni_id"])
                                               ,
                                               mostraPosicao = false // não mostrar a posição quando for o admin
                                               ,
                                               tud_naoLancarNota = Convert.ToBoolean(drTurmas["tud_naoLancarNota"])
                                               ,
                                               tud_naoLancarFrequencia = Convert.ToBoolean(drTurmas["tud_naoLancarFrequencia"])
                                               ,
                                               tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                                               ,
                                               tdt_situacao = 1
                                               ,
                                               aulasPrevistasPreenchida = Convert.ToBoolean(drTurmas["AulasPrevistasPreenchida"])
                                               ,
                                               tur_calendario = drTurmas["tur_calendario"].ToString()
                                               ,
                                               tds_id = Convert.ToInt32(drTurmas["tds_id"])
                                               ,
                                               disciplinaAtiva = true
                                               ,
                                               tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                                               ,
                                               tur_dataEncerramento = Convert.ToDateTime(drTurmas["tur_dataEncerramento"] == DBNull.Value ? new DateTime() : drTurmas["tur_dataEncerramento"])
                                               ,
                                               cal_ano = Convert.ToInt32(drTurmas["cal_ano"].ToString())
                                               ,
                                               turmasAnoAtual = Convert.ToBoolean(drTurmas["turmasAnoAtual"].ToString())
                                               ,
                                               tciIds = drTurmas["tciIds"].ToString()
                                               ,
                                               tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                                               ,
                                               tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                                               ,
                                               tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                                               ,
                                               tur_codigoNormal = drTurmas["tur_codigoNormal"].ToString()
                                               ,
                                               fav_id = Convert.ToInt32(drTurmas["fav_id"])
                                               ,
                                               tdt_id = drTurmas["tdt_id"] != DBNull.Value ?
                                                                          Convert.ToInt32(drTurmas["tdt_id"].ToString())
                                                                          : 0
                                               ,
                                               tdt_vigenciaInicio = drTurmas["tdt_vigenciaInicio"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaInicio"].ToString())
                                                                          : new DateTime()
                                               ,
                                               tdt_vigenciaFim =
                                                                          drTurmas["tdt_vigenciaFim"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaFim"].ToString())
                                                                          : new DateTime()
                                               ,
                                               crg_tipo = drTurmas["crg_tipo"] != DBNull.Value ?
                                                                          Convert.ToByte(drTurmas["crg_tipo"].ToString())
                                                                          : (byte)0
                                                ,
                                                tne_id = drTurmas["tne_id"] != DBNull.Value ?
                                                                          Convert.ToInt32(drTurmas["tne_id"].ToString())
                                                                          : -1
                                           }).ToList()
                             }).ToList();
                    // Adiciona cache com validade do tempo informado na configuração.
                    HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheCurto), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    dados = (List<Struct_MinhasTurmas>)cache;
                }
            }
            else
            {
                // Se não retornou os dados do cache, carrega do banco.
                DataTable dt = new TUR_TurmaDAO().SelecionaPorFiltrosMinhasTurmas(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, ent_id, tur_codigo, tci_id);

                dados = (from DataRow dr in dt.Rows
                         group dr by Convert.ToInt32(dr["esc_id"]) into g
                         select new Struct_MinhasTurmas
                         {
                             esc_id = g.Key
                             ,
                             uni_id = Convert.ToInt32(g.FirstOrDefault()["uni_id"])
                             ,
                             lengendTitulo = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                                + "<br />" + g.FirstOrDefault()["tur_calendario"].ToString()
                             ,
                             cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"].ToString())
                             ,
                             turmasAnoAtual = Convert.ToBoolean(g.FirstOrDefault()["turmasAnoAtual"].ToString())
                             ,
                             Turmas = (from DataRow drTurmas in g
                                       where Convert.ToInt32(drTurmas["esc_id"]) == g.Key
                                       orderby drTurmas["tur_codigo"].ToString()
                                       select new Struct_MinhasTurmas.Struct_Turmas
                                       {
                                           tur_codigo = drTurmas["tur_codigo"].ToString()
                                           ,
                                           tud_nome = drTurmas["tud_nome"].ToString()
                                           ,
                                           tur_curso = drTurmas["tur_curso"].ToString()
                                           ,
                                           tur_turno = drTurmas["tur_turno"].ToString()
                                           ,
                                           tur_id = Convert.ToInt64(drTurmas["tur_id"])
                                           ,
                                           tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                                           ,
                                           tud_id = Convert.ToInt64(drTurmas["tud_id"])
                                           ,
                                           tdt_posicao = Convert.ToInt32(drTurmas["tdt_posicao"])
                                           ,
                                           tdc_id = Convert.ToByte(drTurmas["tdc_id"])
                                           ,
                                           cal_id = Convert.ToInt32(drTurmas["cal_id"])
                                           ,
                                           esc_id = Convert.ToInt32(drTurmas["esc_id"])
                                           ,
                                           uni_id = Convert.ToInt32(drTurmas["uni_id"])
                                           ,
                                           mostraPosicao = false // não mostrar a posição quando for o admin
                                           ,
                                           tud_naoLancarNota = Convert.ToBoolean(drTurmas["tud_naoLancarNota"])
                                           ,
                                           tud_naoLancarFrequencia = Convert.ToBoolean(drTurmas["tud_naoLancarFrequencia"])
                                           ,
                                           tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                                           ,
                                           tdt_situacao = 1
                                           ,
                                           aulasPrevistasPreenchida = Convert.ToBoolean(drTurmas["AulasPrevistasPreenchida"])
                                           ,
                                           tur_calendario = drTurmas["tur_calendario"].ToString()
                                           ,
                                           tds_id = Convert.ToInt32(drTurmas["tds_id"])
                                           ,
                                           disciplinaAtiva = true
                                           ,
                                           tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                                           ,
                                           tur_dataEncerramento = Convert.ToDateTime(drTurmas["tur_dataEncerramento"] == DBNull.Value ? new DateTime() : drTurmas["tur_dataEncerramento"])
                                           ,
                                           cal_ano = Convert.ToInt32(drTurmas["cal_ano"].ToString())
                                           ,
                                           turmasAnoAtual = Convert.ToBoolean(drTurmas["turmasAnoAtual"].ToString())
                                           ,
                                           tciIds = drTurmas["tciIds"].ToString()
                                           ,
                                           tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                                           ,
                                           tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                                           ,
                                           tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                                           ,
                                           tur_codigoNormal = drTurmas["tur_codigoNormal"].ToString()
                                           ,
                                           tdt_id = Convert.ToInt32(drTurmas["tdt_id"].ToString())
                                           ,
                                           tdt_vigenciaInicio = Convert.ToDateTime(drTurmas["tdt_vigenciaInicio"].ToString())
                                           ,
                                           tdt_vigenciaFim =
                                                                          drTurmas["tdt_vigenciaFim"] != DBNull.Value ?
                                                                          Convert.ToDateTime(drTurmas["tdt_vigenciaFim"].ToString())
                                                                          : new DateTime()
                                           ,
                                           crg_tipo = Convert.ToByte(drTurmas["crg_tipo"].ToString())
                                           ,
                                           tne_id = drTurmas["tne_id"] != DBNull.Value ?
                                                                          Convert.ToInt32(drTurmas["tne_id"].ToString())
                                                                          : -1
                                       }).ToList()
                         }).ToList();
            }
            return dados;
        }

        /// <summary>
        /// Seleciona histórico do docente
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="doc_id"></param>
        /// <returns></returns>
        public static List<sHistoricoDocente> SelecionaHistoricoPorDocenteControleTurma(int esc_id, long doc_id, Guid ent_id)
        {
            List<sHistoricoDocente> dados;
            // Se não retornou os dados do cache, carrega do banco.
            DataTable dt = new TUR_TurmaDAO().SelecionaHistoricoPorDocenteControleTurma(esc_id, doc_id, ent_id);

            dados = (from DataRow dr in dt.Rows
                     group dr by Convert.ToInt32(dr["tud_id"]) into g
                     select new sHistoricoDocente
                     {
                         esc_id = Convert.ToInt32(g.FirstOrDefault()["esc_id"])
                         ,
                         esc_nome = g.FirstOrDefault()["esc_nome"].ToString()
                         ,
                         tur_id = Convert.ToInt64(g.FirstOrDefault()["tur_id"]) //g.Key
                         ,
                         Turma = g.FirstOrDefault()["Turma"].ToString()
                         ,
                         tur_codigo = g.FirstOrDefault()["tur_codigo"].ToString()
                         ,
                         cal_id = Convert.ToInt32(g.FirstOrDefault()["cal_id"])
                         ,
                         tur_situacao = Convert.ToInt32(g.FirstOrDefault()["tur_situacao"])
                         ,
                         tud_id = Convert.ToInt64(g.FirstOrDefault()["tud_id"])
                         ,
                         tud_nome = g.FirstOrDefault()["tud_nome"].ToString()
                         ,
                         tud_situacao = Convert.ToInt32(g.FirstOrDefault()["tud_situacao"])
                         ,
                         tdt_posicao = Convert.ToInt32(g.FirstOrDefault()["tdt_posicao"])
                         ,
                         tci_id = Convert.ToInt32(g.FirstOrDefault()["tci_id"])
                         ,
                         tci_nome = g.FirstOrDefault()["tci_nome"].ToString()
                         ,
                         tdc_nome = g.FirstOrDefault()["tdc_nome"].ToString()
                         ,
                         tdt_vigenciaInicio = Convert.ToDateTime(g.FirstOrDefault()["tdt_vigenciaInicio"])
                         ,
                         tdt_vigenciaFim = (!string.IsNullOrEmpty(g.FirstOrDefault()["tdt_vigenciaFim"].ToString()) ? Convert.ToDateTime(g.FirstOrDefault()["tdt_vigenciaFim"]) : new DateTime())
                         ,
                         docenciaCompartilhada = g.FirstOrDefault()["docenciaCompartilhada"].ToString()
                         ,
                         tud_tipo = Convert.ToByte(g.FirstOrDefault()["tud_tipo"])
                         ,
                         cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"])
                         ,
                         tdt_situacao = Convert.ToByte(g.FirstOrDefault()["tdt_situacao"])
                         ,
                         tdt_id = Convert.ToInt32(g.FirstOrDefault()["tdt_id"])
                         ,
                         crg_tipo = Convert.ToByte(g.FirstOrDefault()["crg_tipo"])
                         ,
                         tme_id = Convert.ToInt32(g.FirstOrDefault()["tme_id"] != DBNull.Value ? g.FirstOrDefault()["tme_id"] : "-1")
                     }).ToList();
            return dados;
        }

        /// <summary>
        /// Retorna lista de turmas ativas
        /// </summary>
        /// <param name="lstMinhasTurmas"></param>
        /// <param name="esc_id">Filtro caso precise selecionar turmas apenas de uma escola</param>
        /// <param name="docenciaCompartilhadaAtiva">Filtro para mostrar disciplina de docencia compartilhada, apenas se tiver alguma disciplina relacionada atribuida.</param>
        /// <returns></returns>
        public static List<Struct_MinhasTurmas.Struct_Turmas> SelecionaTurmasAtivasDocente(List<Struct_MinhasTurmas> lstMinhasTurmas, int esc_id, int cal_id = -1, bool docenteAtivo = true, bool docenciaCompartilhadaAtiva = true)
        {
            List<Struct_MinhasTurmas> lstMinhasTurmasByEscola = new List<Struct_MinhasTurmas>();

            if (esc_id > 0)
            {
                lstMinhasTurmasByEscola = lstMinhasTurmas.Where(x => x.esc_id == esc_id).ToList();
            }
            else
            {
                lstMinhasTurmasByEscola = lstMinhasTurmas;
            }

            List<Struct_MinhasTurmas.Struct_Turmas> lista = new List<Struct_MinhasTurmas.Struct_Turmas>();

            lstMinhasTurmasByEscola.All(p =>
            {
                lista.AddRange(p.Turmas.Where(t => (!docenteAtivo || t.tdt_situacao == 1) && t.tur_situacao == (byte)TUR_TurmaSituacao.Ativo && (cal_id <= 0 || t.cal_id == cal_id) && (t.disciplinaAtiva || !docenciaCompartilhadaAtiva)));
                return true;
            });

            lista = lista.GroupBy(x => new { x.tud_id, x.tur_idNormal }).Select(g => g.First()).ToList();

            return lista;
        }

        /// <summary>
        /// Carrega combo turmas da tela Minhas turmas
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns></returns>
        public static List<Struct_MinhasTurmas.Struct_Turmas> SelecionaMinhasTurmasComboPorTurId(long tur_id)
        {
            List<Struct_MinhasTurmas.Struct_Turmas> dados;
            // Se não retornou os dados do cache, carrega do banco.
            DataTable dt = new TUR_TurmaDAO().SelecionaMinhasTurmasComboPorTurId(tur_id);

            dados = (from DataRow drTurmas in dt.Rows
                     orderby drTurmas["tur_codigo"].ToString()
                     select new Struct_MinhasTurmas.Struct_Turmas
                     {
                         tur_codigo = drTurmas["tur_codigo"].ToString()
                         ,
                         tud_nome = drTurmas["tud_nome"].ToString()
                         ,
                         tur_curso = string.Empty
                         ,
                         tur_turno = string.Empty
                         ,
                         tur_id = Convert.ToInt64(drTurmas["tur_id"])
                         ,
                         tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                         ,
                         tud_id = Convert.ToInt64(drTurmas["tud_id"])
                         ,
                         tdt_posicao = Convert.ToInt32(drTurmas["tdt_posicao"])
                         ,
                         tdc_id = 0
                         ,
                         cal_id = Convert.ToInt32(drTurmas["cal_id"])
                         ,
                         esc_id = Convert.ToInt32(drTurmas["esc_id"])
                         ,
                         uni_id = Convert.ToInt32(drTurmas["uni_id"])
                         ,
                         mostraPosicao = false
                         ,
                         tud_naoLancarNota = false
                         ,
                         tud_naoLancarFrequencia = false
                         ,
                         tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                         ,
                         tdt_situacao = 1
                         ,
                         tds_id = Convert.ToInt32(drTurmas["tds_id"])
                         ,
                         cal_ano = Convert.ToInt32(drTurmas["cal_ano"])
                         ,
                         tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                         ,
                         tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                         ,
                         tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                         ,
                         tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                         ,
                         tur_codigoNormal = drTurmas["tur_codigoNormal"].ToString()
                         ,
                         fav_fechamentoAutomatico = Convert.ToBoolean(drTurmas["fav_fechamentoAutomatico"])
                     }).ToList();

            return dados;
        }

        /// <summary>
        /// Carrega combo turmas da tela Minhas turmas
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="appMinutosCacheCurto"></param>
        /// <returns></returns>
        public static List<Struct_MinhasTurmas.Struct_Turmas> SelecionaMinhasTurmasComboPorTurId(long tur_id, int appMinutosCacheCurto = 0)
        {
            List<Struct_MinhasTurmas.Struct_Turmas> lista = new List<Struct_MinhasTurmas.Struct_Turmas>();

            if (appMinutosCacheCurto > 0 && HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_MinhasTurmasCombo(tur_id);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    lista = SelecionaMinhasTurmasComboPorTurId(tur_id);

                    // Adiciona cache com validade do tempo informado na configuração.
                    HttpContext.Current.Cache.Insert(chave, lista, null, DateTime.Now.AddMinutes(appMinutosCacheCurto), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    lista = (List<Struct_MinhasTurmas.Struct_Turmas>)cache;
                }
            }
            else
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                lista = SelecionaMinhasTurmasComboPorTurId(tur_id);
            }

            return lista;
        }

        public static string RetornaChaveCache_GestorMinhaEscola(Guid ent_id, int esc_id)
        {
            return string.Format("Cache_SelecionaPorUnidadeGestorMinhaEscola_{0}_{1}", ent_id, esc_id);
        }


        public static List<Struct_MinhasTurmas> SelecionaPorGestorMinhaEscola(Guid ent_id, int esc_id, int appMinutosCacheCurto = 0)
        {
            List<Struct_MinhasTurmas> dados = null;
            if (appMinutosCacheCurto > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_GestorMinhaEscola(ent_id, esc_id);
                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        dados = SelecionaPorUnidadeGestorMinhaEscola(ent_id, esc_id);

                        // Adiciona cache com validade do tempo informado na configuração.
                        HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheCurto), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        dados = (List<Struct_MinhasTurmas>)cache;
                    }
                }
            }

            if (dados == null)
            {
                // Se não carregou pelo cache, seleciona os dados do banco.
                dados = SelecionaPorUnidadeGestorMinhaEscola(ent_id, esc_id);
            }

            return dados;
        }

        private static List<Struct_MinhasTurmas> SelecionaPorUnidadeGestorMinhaEscola(Guid ent_id, int esc_id)
        {
            List<Struct_MinhasTurmas> dados;
            // Se não retornou os dados do cache, carrega do banco.
            DataTable dt = new TUR_TurmaDAO().SelecionaPorUnidadeGestorMinhaEscola(ent_id, esc_id);

            dados = (from DataRow dr in dt.Rows
                     group dr by new { escola = Convert.ToInt32(dr["esc_id"]), calendario = Convert.ToInt32(dr["cal_id"]) } into g
                     select new Struct_MinhasTurmas
                     {
                         uad_idSuperior = new Guid(string.IsNullOrEmpty(g.FirstOrDefault()["uad_idSuperior"].ToString()) ? Guid.Empty.ToString() : g.FirstOrDefault()["uad_idSuperior"].ToString())
                         ,
                         esc_id = g.Key.escola
                         ,
                         uni_id = Convert.ToInt32(g.FirstOrDefault()["uni_id"])
                         ,
                         esc_nome = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                         ,
                         lengendTitulo = g.FirstOrDefault()["tur_escolaUnidade"].ToString()
                            + "<br />Ano letivo: " + g.FirstOrDefault()["cal_ano"].ToString()
                         ,
                         cal_id = g.Key.calendario
                         ,
                         cal_ano = Convert.ToInt32(g.FirstOrDefault()["cal_ano"].ToString())
                         ,
                         turmasAnoAtual = Convert.ToBoolean(g.FirstOrDefault()["turmasAnoAtual"].ToString())
                         ,
                         Turmas = (from DataRow drTurmas in g
                                   where Convert.ToInt32(drTurmas["esc_id"]) == g.Key.escola
                                    && Convert.ToInt32(drTurmas["cal_id"]) == g.Key.calendario
                                   orderby drTurmas["tur_codigo"].ToString()
                                   select new Struct_MinhasTurmas.Struct_Turmas
                                   {
                                       tur_codigo = drTurmas["tur_codigo"].ToString()
                                       ,
                                       tud_nome = drTurmas["tud_nome"].ToString()
                                       ,
                                       tur_curso = drTurmas["tur_curso"].ToString()
                                       ,
                                       tur_turno = drTurmas["tur_turno"].ToString()
                                       ,
                                       tur_id = Convert.ToInt64(drTurmas["tur_id"])
                                       ,
                                       fav_id = Convert.ToInt32(drTurmas["fav_id"])
                                       ,
                                       tur_escolaUnidade = drTurmas["tur_escolaUnidade"].ToString()
                                       ,
                                       tud_id = Convert.ToInt64(drTurmas["tud_id"])
                                       ,
                                       tdt_posicao = 1
                                       ,
                                       tdc_id = 0
                                       ,
                                       cal_id = Convert.ToInt32(drTurmas["cal_id"])
                                       ,
                                       esc_id = Convert.ToInt32(drTurmas["esc_id"])
                                       ,
                                       uni_id = Convert.ToInt32(drTurmas["uni_id"])
                                       ,
                                       mostraPosicao = true
                                       ,
                                       tud_naoLancarNota = Convert.ToBoolean(drTurmas["tud_naoLancarNota"])
                                       ,
                                       tud_naoLancarFrequencia = Convert.ToBoolean(drTurmas["tud_naoLancarFrequencia"])
                                       ,
                                       tud_disciplinaEspecial = Convert.ToBoolean(drTurmas["tud_disciplinaEspecial"])
                                       ,
                                       tdt_situacao = 1
                                       ,
                                       aulasPrevistasPreenchida = Convert.ToBoolean(drTurmas["AulasPrevistasPreenchida"])
                                       ,
                                       tur_dataEncerramento = Convert.ToDateTime(drTurmas["tur_dataEncerramento"] == DBNull.Value ? new DateTime() : drTurmas["tur_dataEncerramento"])
                                       ,
                                       tci_id = Convert.ToInt32(drTurmas["tci_id"])
                                       ,
                                       tci_nome = drTurmas["tci_nome"].ToString()
                                       ,
                                       cur_id = Convert.ToInt32(drTurmas["cur_id"])
                                       ,
                                       crr_id = Convert.ToInt32(drTurmas["crr_id"])
                                       ,
                                       crp_id = Convert.ToInt32(drTurmas["crp_id"])
                                       ,
                                       tds_id = Convert.ToInt32(drTurmas["tds_id"])
                                       ,
                                       tur_situacao = Convert.ToInt32(drTurmas["tur_situacao"])
                                       ,
                                       tur_tipo = Convert.ToByte(drTurmas["tur_tipo"])
                                       ,
                                       tci_ordem = Convert.ToInt32(drTurmas["tci_ordem"])
                                       ,
                                       tur_calendario = drTurmas["tur_calendario"].ToString()
                                       ,
                                       cal_ano = Convert.ToInt32(drTurmas["cal_ano"].ToString())
                                       ,
                                       turmasAnoAtual = Convert.ToBoolean(drTurmas["turmasAnoAtual"].ToString())
                                       ,
                                       tciIds = drTurmas["tciIds"].ToString()
                                       ,
                                       tud_tipo = Convert.ToByte(drTurmas["tud_tipo"])
                                       ,
                                       tud_idAluno = Convert.ToInt64(drTurmas["tud_idAluno"] == DBNull.Value ? "-1" : drTurmas["tud_idAluno"])
                                       ,
                                       tur_idNormal = Convert.ToInt64(drTurmas["tur_idNormal"] == DBNull.Value ? "-1" : drTurmas["tur_idNormal"])
                                       ,
                                       fav_fechamentoAutomatico = Convert.ToBoolean(drTurmas["fav_fechamentoAutomatico"])
                                       ,
                                       tne_id = drTurmas["tne_id"] != DBNull.Value ? Convert.ToInt32(drTurmas["tne_id"].ToString()) : -1
                                       ,
                                       tme_id = drTurmas["tme_id"] != DBNull.Value ? Convert.ToInt32(drTurmas["tme_id"].ToString()) : -1
                                   }).ToList()
                     }).ToList();

            dados.OrderBy(g => g.cal_ano);

            return dados;
        }

        /// <summary>
        /// Seleciona as turmas por escola e grupamento e as turmas dos grupamentos equivalentes.
        /// </summary>
        /// <param name="usu_id">ID do usuário.</param>
        /// <param name="gru_id">ID do grupo do usuário.</param>
        /// <param name="adm">Flag que indica se o usuário tem permissão de administrador.</param>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do grupamento.</param>
        /// <param name="ent_id">ID da entidade do usuário.</param>
        /// <param name="tur_situacao">Situação das turmas.</param>
        /// <returns></returns>
        public static List<sComboTurmas> SelecionaPorEscolaPeriodoSituacaoEquivalentes
        (
            Guid usu_id
            , Guid gru_id
            , bool adm
            , int esc_id
            , int uni_id
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
            , byte tur_situacao
            , int appMinutosCacheLongo = 0
        )
        {
            List<sComboTurmas> dados = null;
            if (appMinutosCacheLongo > 0)
            {
                if (HttpContext.Current != null)
                {
                    // Chave padrão do cache - nome do método + parâmetros.
                    string chave = RetornaChaveCache_SelecionaPorEscolaPeriodoSituacaoEquivalentes(usu_id, gru_id, adm, esc_id, uni_id,
                                                                             cur_id, crr_id, crp_id, ent_id, tur_situacao);

                    object cache = HttpContext.Current.Cache[chave];

                    if (cache == null)
                    {
                        using (DataTable dt = new TUR_TurmaDAO().SelecionaPorEscolaPeriodoSituacaoEquivalentes(usu_id, gru_id, adm, esc_id, uni_id, cur_id, crr_id, crp_id, ent_id, tur_situacao))
                        {
                            dados = (from DataRow dr in dt.Rows
                                     select new sComboTurmas
                                     {
                                         tur_id = dr["tur_id"].ToString(),
                                         tur_codigo = dr["tur_codigo"].ToString(),
                                         tur_vagas = dr["tur_vagas"].ToString(),
                                         tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                                         tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString()
                                     }).ToList();

                            // Adiciona cache com validade do tempo informado na configuração.
                            HttpContext.Current.Cache.Insert(chave, dados, null, DateTime.Now.AddMinutes(appMinutosCacheLongo), System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }
                    else
                    {
                        dados = (List<sComboTurmas>)cache;
                    }
                }
            }

            if (dados == null)
            {
                using (DataTable dt = new TUR_TurmaDAO().SelecionaPorEscolaPeriodoSituacaoEquivalentes(usu_id, gru_id, adm, esc_id, uni_id, cur_id, crr_id, crp_id, ent_id, tur_situacao))
                {
                    dados = (from DataRow dr in dt.Rows
                             select new sComboTurmas
                             {
                                 tur_id = dr["tur_id"].ToString(),
                                 tur_codigo = dr["tur_codigo"].ToString(),
                                 tur_vagas = dr["tur_vagas"].ToString(),
                                 tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                                 tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString()
                             }).ToList();
                }
            }

            return dados;
        }


        /// <summary>
        /// Seleciona as turmas de origem dos alunos matriculados na turma multisseriada.
        /// </summary>
        /// <param name="tur_idMultisseriada">ID da turma multisseriada.</param>
        /// <returns></returns>
        public static DataTable SelecionaTurmasNormaisMatriculaMutisseriada(long tur_idMultisseriada)
        {
            return new TUR_TurmaDAO().SelecionaTurmasNormaisMatriculaMutisseriada(tur_idMultisseriada, (byte)TurmaDisciplinaTipo.MultisseriadaDocente, (byte)TurmaDisciplinaTipo.MultisseriadaAluno);
        }

        /// <summary>
        /// Seleciona turmas por escola, curso, período e calendário mínimo.
        /// </summary>
        /// <param name="esc_id">ID da escola.</param>
        /// <param name="uni_id">ID da unidade de escola.</param>
        /// <param name="cur_id">ID do curso.</param>
        /// <param name="crr_id">ID do currículo do curso.</param>
        /// <param name="crp_id">ID do período do currículo.</param>
        /// <param name="cal_ano">Ano limite mínimo.</param>
        /// <param name="appMinutosCacheLongo"></param>
        /// <returns></returns>
        public static List<sComboTurmas> SelecionaPorEscolaCursoPeriodoCalendarioMinimo(int esc_id, int uni_id, int cur_id, int crr_id, int crp_id, int cal_ano, int appMinutosCacheLongo = 0)
        {
            Func<List<sComboTurmas>> retorno = delegate
            {
                using (DataTable dt = new TUR_TurmaDAO().SelecionaPorEscolaCursoPeriodoCalendarioMinimo(esc_id, uni_id, cur_id, crr_id, crp_id, cal_ano))
                {
                    return (from DataRow dr in dt.Rows
                            select new sComboTurmas
                            {
                                tur_id = dr["tur_id"].ToString(),
                                tur_codigo = dr["tur_codigo"].ToString(),
                                tur_vagas = dr["tur_vagas"].ToString(),
                                tur_crp_ttn_id = dr["tur_crp_ttn_id"].ToString(),
                                tur_cod_desc_nome = dr["tur_cod_desc_nome"].ToString()
                            }).ToList();
                }
            };

            if (appMinutosCacheLongo > 0)
            {
                return CacheManager.Factory.Get
                    (
                        String.Format(ModelCache.TURMA_ESCOLA_CURSO_PERIODO_CALENDARIO_MINIMO, esc_id, uni_id, cur_id, crr_id, crp_id, cal_ano)
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
        /// Retorna as turmas ativas de acordo com a permissão do usuário e as configurações da sondagem.
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<sComboTurmas> GetSelectBy_SondagemAgendamento
        (
            int snd_id
            , int sda_id
            , int esc_id
            , int cal_id
            , int cur_id
            , int crr_id
            , long doc_id
            , Guid gru_id
            , Guid usu_id
            , bool adm
            , Guid ent_id
        )
        {
            using (DataTable dt = new TUR_TurmaDAO().SelectBy_SondagemAgendamento(snd_id, sda_id, esc_id, cal_id, cur_id, crr_id, doc_id, gru_id, usu_id, adm, ent_id))
            {
                return (from DataRow dr in dt.Rows
                        select new sComboTurmas
                        {
                            tur_id = dr["tur_id"].ToString(),
                            tur_codigo = dr["tur_codigo"].ToString(),
                        }).ToList();
            }
        }

        #endregion Selects

        #region Saves

        /// <summary>
        /// Atualiza a situação da turma de acordo com o fechamento de matrícula
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool Atualiza_Situacao_Fechamento_Matricula
        (
            TUR_Turma entity
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = banco };

            return dao.Update_Situacao_FechamentoMatricula(entity);
        }
        
        /// <summary>
        /// Verifica se o curso tem regime de matrícula seriado por avaliações e valida dados
        /// do formato de avaliação e do calendário selecionado para a turma.
        /// </summary>
        /// <param name="banco">Transação com banco</param>
        /// <param name="entTurma">Entidade da turma a ser salva</param>
        /// <param name="esc">Entidade da escola</param>
        /// <param name="crr">Entidade do currículo do curso</param>
        /// <param name="crp_id">ID do currículo período</param>
        private static void ValidaDadosFormato_Calendario(TalkDBTransaction banco, TUR_Turma entTurma, ESC_Escola esc, ACA_Curriculo crr, int crp_id)
        {
            if ((ACA_CurriculoRegimeMatricula)crr.crr_regimeMatricula ==
                ACA_CurriculoRegimeMatricula.SeriadoPorAvaliacoes)
            {
                // Verifica se o formato de avaliação, é um formato válido para o curso selecionado
                DataTable dtFormato = ACA_FormatoAvaliacaoBO.
                    SelecionaPor_RegrasCurriculoPeriodo_SeriadoAvaliacoes
                    (entTurma.fav_id, crr.crr_qtdeAvaliacaoProgressao, crr.cur_id, crr.crr_id, crp_id,
                     entTurma.tur_docenteEspecialista, esc.ent_id, banco);
                var xFormato = from DataRow dr in dtFormato.Rows
                               where dr["fav_id"].ToString() == entTurma.fav_id.ToString()
                               select dr["fav_id"].ToString();

                if (xFormato.Count() <= 0)
                    throw new ValidationException(
                        "Não é permitido cadastrar turma com esse formato de avaliação.");

                // Verifica se o calendário escolar, é um calendário válido para o curso selecionado
                DataTable dtCalendario =
                    ACA_CalendarioAnualBO.SelecionaCalendarioAnualPorCursoQtdePeriodos(crr.cur_id,
                                                                                       crr.crr_qtdeAvaliacaoProgressao,
                                                                                       esc.ent_id,
                                                                                       banco);
                var xCalendario = from DataRow dr in dtCalendario.Rows
                                  where dr["cal_id"].ToString() == entTurma.cal_id.ToString()
                                  select dr["cal_id"].ToString();

                if (xCalendario.Count() <= 0)
                    throw new ValidationException(
                        "Não é permitido cadastrar turma com esse calendário escolar.");
            }
            else
            {
                // Verifica se o formato de avaliação, é um formato válido de acordo com as regras do curso.
                DataTable dtFormato = ACA_FormatoAvaliacaoBO.SelecionaPor_RegrasCurriculoPeriodo
                    (entTurma.fav_id, crr.cur_id, crr.crr_id, crp_id, entTurma.tur_docenteEspecialista, esc.ent_id, banco);

                var xFormato = from DataRow dr in dtFormato.Rows
                               where dr["fav_id"].ToString() == entTurma.fav_id.ToString()
                               select dr["fav_id"].ToString();

                if (xFormato.Count() <= 0)
                    throw new ValidationException(
                        "Não é permitido cadastrar turma com esse formato de avaliação.");
            }
        }
        
        /// <summary>
        /// Atualiza a situação das turmas do ano informado
        /// </summary>
        /// <param name="cal_ano">Ano do calendário</param>
        /// <param name="tur_situacao">Nova situação</param>
        /// <param name="escolas">Id's das escolas</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        /// <param name="banco">Transação</param>
        /// <returns>true = sucesso | false = fracasso</returns>
        public static bool AtualizaSituacaoPorAno(int cal_ano, TUR_TurmaSituacao tur_situacao, string escolas, Guid ent_id, TalkDBTransaction banco)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = banco };
            return dao.AtualizaSituacaoPorAno(cal_ano, (byte)tur_situacao, escolas, ent_id);
        }

        #endregion Saves

        #region Validações
        
        /// <summary>
        ///  Verifica a turma pelo código, escola, unidade, currículo, curso e período,
        ///  e preenche a entidade turma.
        /// </summary>
        /// <param name="entity">Entidade TUR_Turma.</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do curr. período.</param>
        /// <param name="ent_id">Id da entidade.</param>
        /// <returns>True = Encontrou a turma / False = Não encontrou.</returns>
        public static bool VerificaPorTurmaCurriculo
        (
            TUR_Turma entity
            , int cur_id
            , int crr_id
            , int crp_id
            , Guid ent_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_TurmaCurriculo(entity, cur_id, crr_id, crp_id, ent_id);
        }

        /// <summary>
        ///  Verifica se existe alguma turma pelo calendario, curso, serie e periodo
        /// </summary>
        /// <param name="cal_id">Id do calendario</param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="crp_id">Id do curr. período</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <returns>True = Encontrou a turma / False = Não encontrou.</returns>
        public static bool VerificaExisteTurmaParametroFormacao(int cal_id, int cur_id, int crr_id, int crp_id, TUR_TurmaTipo tur_tipo)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_VerificaExisteTurmaParametroFormacao(cal_id, cur_id, crr_id, crp_id, (byte)tur_tipo);
        }

        /// <summary>
        /// Verifica quantos registros relacionados a turma existem.
        /// </summary>
        /// <param name="tur_id">Id da Turma</param>
        /// <returns>Retorna verdadeiro se houverem registros relacionados com a turma.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaRegistrosAssociados
        (
            long tur_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.VerificaRegistrosAssociados(tur_id);
        }

        /// <summary>
        /// Verifica quantos registros relacionados a turma existem.
        /// </summary>
        /// <param name="tur_id">Id da Turma</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>Retorna verdadeiro se houverem registros relacionados com a turma.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaRegistrosAssociados
        (
            long tur_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = banco };
            return dao.VerificaRegistrosAssociados(tur_id);
        }

        /// <summary>
        /// Verifica a existencia de turmas associadas a um turno
        /// </summary>
        /// <param name="trn_id">Id do turmo para filtrar turmas associadas</param>
        /// <returns>TRUE - se existir turmas</returns>
        public static bool VerificaTurmaAssociada(int trn_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();

            if (trn_id == 0)
                throw new ArgumentException("Não é possível verificar turmas associadas.");

            if (dao.SelectTurma_ByTurno(trn_id).Rows.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Verifica se a turma está sendo utilizada na matrícula turma
        /// </summary>
        /// <param name="tur_id"></param>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool Existe_MatriculaTurma
        (
            long tur_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_VerificaMatriculaTurma(tur_id);
        }

        /// <summary>
        /// Verifica se a turma está sendo utilizada na matrícula turma
        /// </summary>
        /// <param name="tur_id"></param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>True/False</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool Existe_MatriculaTurma
        (
            long tur_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = banco };
            return dao.SelectBy_VerificaMatriculaTurma(tur_id);
        }

        /// <summary>
        /// Retorno booleano na qual verifica se existe uma Turma com o mesmo código
        /// cadastrado no banco com situação diferente de Excluido, para uma mesma escola / unidade e calendário.
        /// </summary>
        /// <param name="entity">Entidade da Turma</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public static bool VerificaTurmaExistente(TUR_Turma entity, TalkDBTransaction banco)
        {
            // Caso a turma estiver cancelada ou excluída desconsidera a validação
            if (entity.tur_situacao == (byte)TUR_TurmaSituacao.Cancelada ||
                entity.tur_situacao == (byte)TUR_TurmaSituacao.Excluido)
                return false;

            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = banco };

            DataTable dtTeste = dao.SelectBy_Codigo(entity.tur_id, entity.esc_id, entity.uni_id, entity.cal_id, entity.tur_codigo);
            return dtTeste.Rows.Count > 0;
        }

        /// <summary>
        /// Retorno booleano na qual verifica se existe uma Turma com o mesmo código
        /// cadastrado no banco com situação diferente de Excluido, para uma mesma escola / unidade e calendário.
        /// </summary>
        /// <param name="entity">Entidade da Turma</param>
        /// <param name="banco">Transação com banco - obrigatório</param>
        /// <returns>True - caso encontre algum registro no select/False - caso não encontre nada no select</returns>
        public static bool VerificaTurmaExistente(TUR_Turma entity)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Codigo(entity);
        }

        /// <summary>
        /// Retorna o último código de turma cadastrada de acordo com os parâmetros
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendario</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="crp_id">ID do período do currpiculo</param>
        /// <param name="tur_codigoPrefixo">Prefixo do código da turma</param>
        public static string VerificaUltimoCodigoTurmaCadastrado
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , int crp_id
            , string tur_codigoPrefixo
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_PrefixoCodigo(esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, tur_codigoPrefixo);
        }

        /// <summary>
        /// Retorna o último código de turma eletiva cadastrada de acordo com os parâmetros
        /// </summary>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendario</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo do curso</param>
        /// <param name="tur_codigoTurma">Código da turma</param>
        /// <param name="banco">Transação com banco - não obrigatório</param>
        public static string VerificaUltimoCodigoTurmaEletivaCadastrado
        (
            int esc_id
            , int uni_id
            , int cal_id
            , int cur_id
            , int crr_id
            , string tur_codigoTurma
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_PrefixoCodigoTurmaEletiva(esc_id, uni_id, cal_id, cur_id, crr_id, tur_codigoTurma);
        }

        /// <summary>
        /// Verifica se a turma possui uma disciplina de determinado tipo.
        /// </summary>
        /// <param name="tur_id">ID da turma.</param>
        /// <param name="tud_tipo">Tipo de disciplina a ser buscado.</param>
        /// <param name="appMinutosCacheLongo">Minutos do cache longo</param>
        /// <param name="banco">Transação</param>
        /// <returns></returns>
        public static bool VerificaPossuiDisciplinaPorTipo(long tur_id, TurmaDisciplinaTipo tud_tipo, int appMinutosCacheLongo = 0, TalkDBTransaction banco = null)
        {
            Func<bool> retorno = delegate ()
        {
            TUR_TurmaDAO dao = banco == null ?
                new TUR_TurmaDAO() :
                new TUR_TurmaDAO { _Banco = banco };

            return dao.VerificaPossuiDisciplinaPorTipo(tur_id, (byte)tud_tipo);
        };

            if (appMinutosCacheLongo > 0)
                return CacheManager.Factory.Get
                            (
                                String.Format(ModelCache.TURMA_POSSUI_DISCIPLINA_POR_TIPO_MODEL_KEY, tur_id, (byte)tud_tipo),
                                retorno,
                                appMinutosCacheLongo
                            );


            return retorno();
        }

        /// <summary>
        /// Verifica se existem turmas do PEJA por escola e calendario.
        /// </summary>
        /// <param name="esc_id">Id da escola</param>
        /// <param name="cal_id">Id do calendario</param>
        /// <returns>True = Encontrou turmas / False = Não encontrou.</returns>
        public static bool VerificaExistenciaTurmasPejaPorEscolaCalendario(int esc_id, int cal_id)
        {
            return new TUR_TurmaDAO().VerificaExistenciaTurmasPejaPorEscolaCalendario(esc_id, cal_id);
        }
        
        #endregion Validações

        #region Métodos RS

        /// <summary>
        /// Retorna as turmas de acordo com os filtros informados e com a permissão do usuário,
        /// traz somente turmas do tipo informado no parâmetro.
        /// </summary>
        /// <param name="ent_id">ID da entidade</param>
        /// <param name="usu_id">ID do usuário</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="uad_idSuperior">ID da unidade superior a escola</param>
        /// <param name="esc_id">ID da escola</param>
        /// <param name="uni_id">ID da unidade da escola</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do currículo do curso</param>
        /// <param name="crp_id">ID do currículoPeríodo</param>
        /// <param name="trn_id">ID do turno</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="tur_codigo">Código da turma</param>
        /// <param name="tur_tipo">Tipo de turma</param>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <returns>Turmas ativas</returns>
        public static DataTable SelecionaTurmasPorTipo(Guid ent_id, Guid usu_id, Guid gru_id, Guid uad_idSuperior, int esc_id, int uni_id, int cal_id, int cur_id, int crr_id, int crp_id, int trn_id, long doc_id, string tur_codigo, TUR_TurmaTipo tur_tipo, bool paginado, int currentPage, int pageSize)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.SelectBy_Tipo(ent_id, usu_id, gru_id, uad_idSuperior, esc_id, uni_id, cal_id, cur_id, crr_id, crp_id, trn_id, doc_id, tur_codigo, (byte)tur_tipo, paginado, currentPage, pageSize, out totalRecords);
        }

        /// <summary>
        /// Converte uma lista de objeto em um datatable
        /// </summary>
        /// <param name="data">Lista</param>
        /// <returns>Datatable</returns>
        private static DataTable ConvertToDataTable(List<Struct_MinhasTurmas.Struct_Turmas> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Struct_MinhasTurmas.Struct_Turmas));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (Struct_MinhasTurmas.Struct_Turmas item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        #endregion Métodos RS

        #region Consultas

        /// <summary>
        /// Lista as turmas por escola
        /// </summary>
        /// <param name="esc_id">Id da escola</param>       
        /// <returns>Lista de turmas</returns>
        public static DataTable BuscaTurmasAPI(Int64 tur_id, Int32 esc_id, Int64 doc_id, Int64 tud_id, DateTime dataBase)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.BuscaTurmasAPI(tur_id, esc_id, doc_id, tud_id, dataBase);
        }

        /// <summary>
        /// Lista as turmas por escola, calendario, curso, curriculo e periodo
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="cal_id"></param>
        /// <param name="cur_id"></param>
        /// <param name="crr_id"></param>
        /// <param name="crp_id"></param>
        /// <returns></returns>
        public static List<TUR_Turma> BuscaTurmasPorEscolaCalendarioCursoCurriculoPeriodo
        (
            int esc_id
          , int cal_id
          , int cur_id
          , int crr_id
          , int crp_id
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.BuscaTurmasPorEscolaCalendarioCursoCurriculoPeriodo(esc_id, cal_id, cur_id, crr_id, crp_id);
        }

        /// <summary>
        /// Retorna todos os dados das turmas informadas.
        /// </summary>
        /// <param name="tur_id">IDs das turmas</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<TUR_Turma> SelecionaDadosPor_Turmas
        (
            string tur_id
            , TalkDBTransaction banco
        )
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO { _Banco = banco };
            DataTable dt = dao.SelecionaDadosPor_Turmas(tur_id);

            return (from DataRow dr in dt.Rows
                    select dao.DataRowToEntity(dr, new TUR_Turma())).ToList();
        }

        /// <summary>
        /// Lista com a quantidade de tempos por bimestre.
        /// Apenas para cursos que possuem efetivação semestral.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <returns>Lista de quantidade de tempos</returns>
        public static List<Struct_temposEfetivacaoSemestral> SelecionaTemposDiaEfetivacaoSemestral(long tur_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            DataTable dt = dao.SelecionaTemposDiaEfetivacaoSemestral(tur_id);

            return (from DataRow dr in dt.Rows
                    select new Struct_temposEfetivacaoSemestral
                    {
                        crp_qtdeTemposDia = Convert.ToInt32(dr["crp_qtdeTemposDia"]),
                        tpc_id = Convert.ToInt32(dr["tpc_id"])
                    }).ToList();
        }

        /// <summary>
        /// Verifica se a turma deve ter acesso ao controle semestral ao alterar os dados da turma.
        /// </summary>
        /// <param name="tur_id">Id da turma</param>
        /// <param name="cal_ano">Ano</param>
        /// <returns>Se possui acesso ou não</returns>
        public static bool VerificaAcessoControleSemestral(long tur_id)
        {
            TUR_TurmaDAO dao = new TUR_TurmaDAO();
            return dao.VerificaAcessoControleSemestral(tur_id);
        }

        /// <summary>
        /// Seleciona os dados da turma da tela de VisualizaConteudo
        /// </summary>
        /// <param name="parametro1">Nome OU código da escola</param>
        /// <param name="parametro2">Ano letivo</param>
        /// <param name="parametro3">Código da turma</param>
        /// <returns>Retorna dados da turma</returns>
        public static DataSet VisualizaConteudo(string parametro1, string parametro2, string parametro3)
        {
            return new TUR_TurmaDAO().SelecionaVisualizaConteudo(parametro1, parametro2, parametro3);
        }

        #endregion Consultas
        
        #region Cache

        /// <summary>
        /// Retorna a chave do cache utilizada para guardar os eventos da efetivação
        /// </summary>
        /// <returns>Chave</returns>
        public static string RetornaChaveCache_SelecionaPorDocenteControleTurma(string ent_id, string doc_id)
        {
            return string.Format(ModelCache.TURMA_SELECIONA_POR_DOCENTE_CONTROLE_TURMA_MODEL_KEY, ent_id, doc_id);
        }

        #endregion

    }
}