using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.UI;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.Data.Common;
using MSTech.Data.Common.Abstracts;
using MSTech.GestaoEscolar.CustomResourceProviders;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Util;
using MSTech.Security.Cryptography;
using MSTech.Validation.Exceptions;
using MSTech.GestaoEscolar.BLL.Caching;
using MSTech.CoreSSO.DAL;
using System.Runtime.CompilerServices;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.BLL
{
    #region Métodos de extensão

    public static class MetodosExtensao
    {
        /// <summary>
        /// Retorna nome da pessoa formatado (nome social, nome registro ou ambos)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="exibicaoNome"></param>
        /// <returns></returns>
        public static string NomeFormatado(this PES_Pessoa entity, eExibicaoNomePessoa exibicaoNome)
        {
            switch (exibicaoNome)
            {
                case eExibicaoNomePessoa.NomeRegistro | eExibicaoNomePessoa.NomeSocial:
                    return entity.pes_nome + (string.IsNullOrEmpty(entity.pes_nomeSocial) ? string.Empty : string.Format(" ({0})", entity.pes_nomeSocial));
                case eExibicaoNomePessoa.NomeSocial:
                    return string.IsNullOrEmpty(entity.pes_nomeSocial) ? entity.pes_nome : entity.pes_nomeSocial;
                case eExibicaoNomePessoa.NomeRegistro:
                    return entity.pes_nome;
            }

            return entity.pes_nome;
        }
    }

    #endregion Métodos de extensão

    #region Enumeradores

    /// <summary>
    /// Enumerador das possíveis opções de menu da tela de Minhas turmas.
    /// </summary>
    public enum eOpcaoAbaMinhasTurmas : byte
    {
        PlanejamentoAnual = 1,
        DiarioClasse = 2,
        Listao = 3,
        Efetivacao = 4,
        Alunos = 5,
        Periodo = 6,
        Frequencia = 7,
        Avaliacao = 8
    }

    public enum PaginaGestao
    {
        Alunos = 1
        ,

        Escolas
        ,

        CalendarioAnual
        ,

        Curso
        ,

        Evento
        ,

        EscalaAvaliacao
        ,

        FormatoAvaliacao
        ,

        Turno
        ,

        Docentes
        ,

        Colaboradores
        ,

        Cargos
        ,

        Funcoes
        ,

        CargaHoraria
        ,

        CoordenadorDisciplina
        ,

        ProjetoReforco
        ,

        Usuarios
        ,

        Merenda_Produtos
        ,

        Merenda_Pratos
        ,

        Merenda_Cardapios
        ,

        Merenda_MapaCardapio
        ,

        Merenda_CardapiosEscola
        ,

        Biblioteca_Autores
        ,

        Biblioteca_Bibliotecas
        ,

        Biblioteca_Exemplares
        ,

        Biblioteca_Obras
        ,

        Biblioteca_Devolucoes
        ,

        Biblioteca_Renovacoes
        ,

        Biblioteca_Consulta
        ,

        Biblioteca_ConsultaAlunos
        ,

        Biblioteca_Periodicos
        ,

        Turma
        ,

        TurmaEletiva
        ,

        TurmaMultisseriada
        ,

        DisciplinaEletiva
        ,

        DocumentosAluno
        ,

        Planejamento
        ,

        Lancamento_Avaliacoes
        ,

        Lancamento_Frequencia
        ,

        Lancamento_FrequenciaMensal
        ,

        Reunioes_Responsaveis_Frequencia
        ,

        Efetivacao
        ,

        AtividadeProjetoReforco
        ,

        MatriculaProjetoReforco
        ,

        ParametroMovimentacao
        ,

        SolicitacaoTransferencia
        ,

        CertificadoHabilitacao_Busca
        ,

        CorrecaoGeral
        ,

        ConfiguracaoServicoPendencia
        ,

        Duplicados
        ,

        AcaoRetroativa
        ,

        EfetivacaoNotas
        ,

        MigracaoRio_ConsultaAlunos
        ,

        MigracaoRio_ConsultaDocentes
        ,

        MigracaoRio_RelatorioAvaliacao
        ,

        MigracaoRio_RelatorioHistorico
        ,

        MigracaoRio_RelatorioMovimentacao
        ,

        MigracaoRio_RelatorioDadosDocente
        ,

        MigracaoRio_RelatorioDadosAluno
        ,

        ExportacaoSCA
        ,

        ProcessoFechamentoInicio
        ,

        ParametroRemanejamento
        ,

        ControleRendimentoEscolar
        ,

        Movimentacao
        ,

        Movimentacao_Retroativa
        ,

        SalaRecurso
        ,

        ConsultaEquipamentos
        ,

        TurmaSalaRecurso
        ,

        FormatacaoRelatorio
        ,

        LancamentoFrequenciaSalaRecurso
        ,

        AvaliacaoSalaRecurso
        ,

        TurmaAlunos
        ,

        RelatorioProjetoProgramaEspecial
        ,

        LiberacaoBoletimOnline
        ,

        DeParaCurriculoPeriodo
        ,

        DeParaJustificativaFalta
        ,

        ManutencaoMovimentacao
        ,

        MovimentacoesDesfeitas
        ,

        EfetivacaoNotasDocente
        ,

        EstatisticaGeralPorEscola
        ,

        MatriculaDigitarConsulta
        ,

        AcertoSituacaoAluno
        ,

        AlunosMatriculados
        ,

        ProgressaoPEJA
        ,

        SolicitacaoTransferenciaUASuperiorOrigem
        ,

        QuadroAuxiliarPlanejamentoMatricula
        ,

        LancamentoNotaFrequenciaAnoAnterior
        ,

        ConvocacaoResponsaveis
        ,

        AvisoTextoGeral
        ,

        Informativo
        ,

        HorarioEntradaSaida
        ,

        Carteirinha
        ,

        AprovacaoDados
        ,

        TipoDesempenhoAprendizado
        ,

        CompensacaoAusencia
        ,

        AtribuicaoDocentes
        ,

        LoteFechamento
        ,

        ParametrizacaoDisciplinas
        ,

        MinhasTurmas
        ,

        TipoPeriodoCurso
        ,

        HistoricoEscolarPedagogico
        ,

        AtaFinalResultado
        ,

        AtaFinalEnriquecimentoCurricular
        ,

        MinhaEscola
        ,

        AlocarDocente
        ,

        DocumentosDocente
        ,

        GraficoComponenteCurricularTurmas
        ,

        GraficoConsolidadoAtividadeAvaliativa
        ,

        AtaSinteseResultadosAvaliacao
        ,

        AtaSinteseEnriquecimentoCurricular
        ,

        AtaFinalResultadosEnriquecimentoCurricular
        ,

        GraficoTurmaMatrizCurricular
        ,

        DadosAlunosBaixaFrequencia
        ,
        DadosAlunosJustificativaFalta
        ,

        Turmas_ComponentesFinalizados
        ,

        GraficoIndividualNotasConceito
        ,

        EfetivacaoGestor
        ,

        AlunosPendenciaEfetivacao
        ,

        FrequenciaProjeto
        ,

        AprovacaoIntencaoTransferencia
        ,

        JustificativaPendencia
        ,

        AtribuicaoEsporadica
        ,

        BuscaDestinatarios
        ,

        ConselhoMunicipal
        ,

        IndicadorFrequencia
        ,

        AulasSemPlanoAula
        ,

        AberturaTurmasAnosAnteriores
        ,

        JustificativaAbonoFalta
        ,

        DivergenciasRematriculas
        ,

        LancamentoFrequenciaExterna
        ,

        ObjetoAprendizagem
        ,

        RelatorioObjetoAprendizagem
        ,

        DivergenciasAulasPrevistas
        ,

        PlanejamentoSemanal
        ,

        Sondagem
        ,

        RelatorioAnaliseSondagem
        ,

        LancamentoSondagem
        ,

        FrequenciaMensal
        ,

        RelatorioSugestoesCurriculo
        ,

        QuantitativoSugestoes
        ,
        RelatorioAEE
    }

    [Serializable]
    public enum LoteStatus
    {
        Sucess,
        Error
    }

    public enum DataState
    {
        Unchanged
        ,

        Added
        ,

        Modified
        ,

        Deleted
    }

    public enum tipoRelatorio
    {
        Relatorio
        ,

        Documento
    }

    /// <summary>
    /// Enumerador para indicar o se a observação em edição é para a turma ou disciplina.
    /// </summary>
    public enum eTipoObservacao : byte
    {
        ConselhoPedagogico = 1
        ,

        Disciplina = 2
    }

    /// <summary>
    /// Enum com Ids e nomes dos documentos customizados para os clientes do gestão escolar.
    /// </summary>
    public enum ReportNameDocumentos
    {
        BoletimEscolar = 40,
        DeclaracaoMatricula = 41,
        DeclaracaoMatriculaExAluno = 42,
        DeclaracaoPedidoTransferencia = 43,
        DeclaracaoConclusaoCurso = 44,
        DeclaracaoExAlunoUnidadeEscolar = 45,
        DeclaracaoMatriculaPeriodo = 46,
        FichaIndividualAluno = 47,
        FichaCadastralAluno = 48,
        AutorizacaoPasseio = 49,
        ControleRecebimentoAPM = 50,
        TermoCompromisso = 51,
        DeclaracaoSolicitacaoVaga = 53,
        DeclaracaoSolicitacaoComparecimento = 54,
        DeclaracaoEscolaridade = 55,
        HistoricoEscolar = 61,
        CertificadoHabilitacao = 69,
        CertificadoConclusaoCurso = 70,
        ConviteReuniao = 104,
        RegistroRendimentoEscolar = 126,
        TermoCompromissoSalaRecurso = 141,
        RespostaSalaRecurso = 142,
        ComprovanteMatricula = 145,
        DeclaracaoSolicitacaoTransferencia = 158,
        DeclaracaoTrabalho = 174,
        EncaminhamentoAlunoRemanejado = 200,
        CertidaoEscolaridadeAnterior1972 = 219,
        FichaIndividualAvaliacaoPeriodica = 237,
        HistoricoEscolarPedagogico = 271,
        DocAluCartaoIdentificacao = 289,
        DocAluCarteirinhaEstudante = 290,
        DocAluDeclaracaoComparecimento = 293,
        DocAluFichaMatricula = 303
    }

    /// <summary>
    /// Enum com Ids e nomes dos relatórios do sistema de gestão acadêmica.
    /// </summary>
    public enum ReportNameGestaoAcademica
    {
        QuadroEstatisticoAlunosFalta = 62,
        QuadroEstatisticoFormacaoTurma = 63,
        QuadroEstatisticoQuantitativoAluno = 64,
        GestaoAcademicaEfetivacaoNotas = 71,
        QuadroEstatisticoMovimentosSerieSexo = 81,
        GestaoAcademicaRegistroAlunoReprovado = 94,
        ProgSocial_DeclaracaoComparecimentoReuniao = 121,
        GestaoAcademicaAnotacoes = 124,
        GestaoAcademicaPlanejamento = 129,
        ProjetoProgramaEspecialAlunos = 144,
        QuadroEstatisticoEstruturacaoTurmas = 157,
        RelatorioTotalAlunosBolsaFamiliaPorEscola = 179,
        QuadroEstatisticoResultadoMatricula = 205,
        Consolidado = 236,
        AlunoDispensaDisciplina = 239,
        PlanejamentoAnualOrientacoesCurriculares = 241,
        GraficoConsolidadoAtividadeAvaliativa = 252,
        AlunosBaixaFrequencia = 253,
        TurmaDisciplinaEfetivada = 260,
        GraficoIndividualNotas = 261,
        GraficoSinteseResultadosAvaliacaoTurmaMatrizCurricular = 262,
        GraficoSinteseResultadosAvaliacaoComponenteCurricularTurmas = 263,
        SitAluAprovadosReprovados = 264,
        SitAluMelhoresAlunos = 265,
        SitAluAcimaMedia = 266,
        SitAluAbaixoMedia = 267,
        SitAluSemAvaliacao = 268,
        GraficoIndividualNotaComponente = 270,
        AtaResultadoFinal = 273,
        AtaEnriquecimentoCurricular = 274,
        AtaResultadoFinalEnriquecimentoCurricular = 279,
        AlunoConselhoDeClasse = 280,
        IndicadorFrequenciaDRE = 314,
        IndicadorFrequenciaPeriodoCurso = 315,
        AulasSemPlanoAula = 316,
        DivergenciasRematriculas = 317,
        RelatorioObjetoAprendizagem = 318,
        AlunosJustificativaFalta = 319,
        DivergenciasAulasPrevistas = 320,
        AnaliseSondagem = 321,
        FrequenciaMensal = 322,
        RelatorioSugestoesCurriculo = 323,
        QuantitativoSugestoes = 324
    }

    /// <summary>
    /// Enum com Ids e nomes dos documentos do docente do sistema de gestão acadêmica.
    /// </summary>
    public enum ReportNameGestaoAcademicaDocumentosDocente
    {
        DocDctDiarioClasseFrequencia = 242,
        DocDctDiarioClasseAvaliacao = 243,
        DocDctGraficoAtividadeAvaliativa = 244,
        DocDctRelAtividadeAvaliativa = 245,
        DocDctRelTarjetaBimestral = 246,
        DocDctRelFrequenciaBimestral = 247,
        DocDctRelSinteseAula = 248,
        DocDctRelDadosPlanejamento = 249,
        DocDctRelOrientacaoAlcancada = 250,
        DocDctRelAnotacoesAula = 251,
        DocDctRelSinteseEnriquecimentoCurricular = 264,
        DocDctPlanoAnual = 281,
        DocDctPlanoCicloSerie = 282,
        DocDctAlunosPendenciaEfetivacao = 283
    }

    /// <summary>
    /// Flag de configuração da exibição de nome da pessoa
    /// </summary>
    [Flags]
    public enum eExibicaoNomePessoa
    {
        NomeRegistro = 1
        ,
        NomeSocial = 2
    }

    #endregion Enumeradores

    #region Estruturas

    public struct UnidadeAdministrativa
    {
        public int esc_id;
        public Guid uad_idSuperior;
    }

    [Serializable]
    public struct BuscaGestao
    {
        /// <summary>
        /// Página que o usuário efetuou a busca.
        /// </summary>
        public PaginaGestao PaginaBusca;

        private string filtros;

        /// <summary>
        /// Filtros utilizados na busca.
        /// </summary>
        public Dictionary<String, String> Filtros
        {
            get
            {
                try
                {
                    Dictionary<String, String> ret = new Dictionary<String, String>();

                    if (!String.IsNullOrEmpty(filtros))
                    {
                        string[] items = filtros.Split(';');

                        foreach (string s in items)
                        {
                            string[] k = s.Split('=');
                            ret.Add(k[0], k[1].Replace("#$12$#", ";"));
                        }
                    }

                    return ret;
                }
                catch
                {
                    return new Dictionary<String, String>();
                }
            }
            set
            {
                string res = "";

                foreach (KeyValuePair<String, String> item in value)
                {
                    string valor = item.Value.Replace(";", "#$12$#");

                    if (!String.IsNullOrEmpty(res))
                        res += ";";

                    res += item.Key + "=" + valor;
                }

                filtros = res;
            }
        }
    }

    /// <summary>
    /// Estrutura utilizada para armazenar os dados da permissão em cache.
    /// </summary>
    [Serializable]
    public struct sSYS_Grupo
    {
        public SYS_GrupoPermissao grupoPermissao { get; set; }

        public SYS_Modulo modulo { get; set; }
    }

    #endregion Estruturas

    public static class GestaoEscolarUtilBO
    {
        #region CONSTANTES

        public const string DevReportName = "DevReportName";

        /// <summary>
        /// Minutos de cache para a configuração de cache curta.
        /// </summary>
        public const int MinutosCacheCurto = 60;

        /// <summary>
        /// Minutos de cache para a configuração de cache médio.
        /// </summary>
        public const int MinutosCacheMedio = 360;

        /// <summary>
        /// Minutos de cache para a configuração de cache longo.
        /// </summary>
        public const int MinutosCacheLongo = 720;

        #endregion CONSTANTES

        #region Métodos para verificar integridade

        /// <summary>
        /// Verifica a existência da chave informada (1 campo) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave - obrigatório</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave - obrigatório</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por "," - obrigatório)</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public static bool VerificarIntegridade
        (
            string campo1
            , string valorCampo1
            , string tabelasNaoVerificar
            , TalkDBTransaction banco
        )
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                return dao.Select_VerificarIntegridade(campo1, valorCampo1, tabelasNaoVerificar);
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica a existência da chave informada (2 campos) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave - obrigatório</param>
        /// <param name="campo2">Nome da coluna 2 da chave - obrigatório</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave - obrigatório</param>
        /// <param name="valorCampo2">Valor da coluna 2 da chave - obrigatório</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por "," - obrigatório)</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public static bool VerificaIntegridadaChaveDupla
        (
            string campo1
            , string campo2
            , string valorCampo1
            , string valorCampo2
            , string tabelasNaoVerificar
            , TalkDBTransaction banco
        )
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                return dao.VerificaIntegridadaChaveDupla(campo1, campo2, valorCampo1, valorCampo2, tabelasNaoVerificar);
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Verifica a existência da chave informada (3 campos) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave - obrigatório</param>
        /// <param name="campo2">Nome da coluna 2 da chave - obrigatório</param>
        /// <param name="campo3">Nome da coluna 3 da chave - obrigatório</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave - obrigatório</param>
        /// <param name="valorCampo2">Valor da coluna 2 da chave - obrigatório</param>
        /// <param name="valorCampo3">Valor da coluna 3 da chave - obrigatório</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por "," - obrigatório)</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public static bool VerificaIntegridadaChaveTripla
        (
            string campo1
            , string campo2
            , string campo3
            , string valorCampo1
            , string valorCampo2
            , string valorCampo3
            , string tabelasNaoVerificar
            , TalkDBTransaction banco
        )
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                return dao.VerificaIntegridadaChaveTripla(campo1, campo2, campo3, valorCampo1, valorCampo2, valorCampo3, tabelasNaoVerificar);
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        #endregion Métodos para verificar integridade

        /// <summary>
        /// Limpa os caches que possuem a chave informada
        /// </summary>
        /// <param name="chave">Chave do cache (pode ser apenas um trecho da chave)</param>
        public static void LimpaCache(string chave)
        {
            System.Collections.IDictionaryEnumerator myCache = HttpContext.Current.Cache.GetEnumerator();

            myCache.Reset();

            while (myCache.MoveNext())
                if (myCache.Key.ToString().Contains(chave))
                    HttpContext.Current.Cache.Remove(myCache.Key.ToString());

            List<string> listaChaves = CacheManager.Factory.GetAllKey().Where(p => p.Key.Contains(chave)).Select(p => p.Key).ToList();

            listaChaves.ForEach(p => CacheManager.Factory.Remove(p));
        }

        /// <summary>
        /// Limpa os caches que possuem a chave informada.
        /// </summary>
        /// <param name="chave">Chave do cache (pode ser apenas um trecho da chave).</param>
        /// <param name="valor">Valor de algum item do cache.</param>
        public static void LimpaCache(string chave, string valor)
        {
            System.Collections.IDictionaryEnumerator myCache = HttpContext.Current.Cache.GetEnumerator();

            myCache.Reset();

            while (myCache.MoveNext())
            {
                string chaveCache = myCache.Key.ToString();
                if (chaveCache.Contains(chave))
                {
                    string[] valoresChave = chaveCache.Split('_');
                    foreach (string valorChave in valoresChave)
                    {
                        if (valorChave == valor)
                        {
                            HttpContext.Current.Cache.Remove(myCache.Key.ToString());
                            break;
                        }
                    }
                }
            }

            List<string> listaChaves = CacheManager.Factory.GetAllKey().Where(p => p.Key.Contains(chave)).Select(p => p.Key).ToList();

            foreach (string ch in listaChaves)
            {
                string[] valoresChave = ch.Split('_');
                foreach (string valorChave in valoresChave)
                {
                    if (valorChave == valor)
                    {
                        CacheManager.Factory.Remove(ch);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Copia as propriedades de uma entidade para a outra, usando a entity como referência.
        /// </summary>
        /// <param name="entityCarregada"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Abstract_Entity CopiarEntity(object entityCarregada, Abstract_Entity entity)
        {
            Type tp = entity.GetType();
            PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] propertiesCarregada = entityCarregada.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                PropertyInfo propCarregada = propertiesCarregada.ToList().Find(p => p.Name == prop.Name);
                if (propCarregada != null)
                {
                    prop.SetValue(entity, propCarregada.GetValue(entityCarregada), null);
                }
            }
            return entity;
        }

        public static List<UnidadeAdministrativa> RetornaUadPermissao(Guid usu_id, Guid gru_id)
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();
            DataTable dt = dao.RetornaUadPermissao(usu_id, gru_id);

            List<UnidadeAdministrativa> list = new List<UnidadeAdministrativa>();

            return new ESC_EscolaDAO().RetornaUadPermissao(usu_id, gru_id).Rows.Cast<DataRow>().Select(dr =>
                new UnidadeAdministrativa
                {
                    esc_id = Convert.ToInt32(dr["esc_id"].ToString()),
                    uad_idSuperior = new Guid(dr["uad_idSuperior"].ToString())
                }).ToList();
        }

        /// <summary>
        /// Retorna um objeto com os valores iguais ao da origem.
        /// Utilizado para clonar um Entity para um DTO com os mesmos campos para a API.
        ///
        /// PS.: o retorno do Entity desfigura o jSon na API, por isso a necessidade do DTO.
        /// </summary>
        /// <param name="objectOrigin"></param>
        /// <param name="objectClone"></param>
        /// <returns></returns>
        public static object Clone(object objectOrigin, object objectClone)
        {
            Type tp = objectOrigin.GetType();
            Type tpClone = objectClone.GetType();

            PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                PropertyInfo propClone;
                if (tpClone.BaseType == tp)
                    propClone = tpClone.BaseType.GetProperty(prop.Name);
                else
                    propClone = tpClone.GetProperty(prop.Name);

                if (propClone != null && prop.CanWrite)
                {
                    var value = prop.GetValue(objectOrigin, null);

                    if (value != null)
                    {
                        if (propClone.PropertyType == typeof(string))
                        {
                            if (value.GetType() == typeof(DateTime))
                            {
                                DateTime date = Convert.ToDateTime(value);
                                propClone.SetValue(objectClone, date.ToString(DateUtil.DATA_HORA_SEGUNDOS_MILIS), null);
                            }
                            else
                            {
                                propClone.SetValue(objectClone, value.ToString(), null);
                            }
                        }
                        else
                        {
                            propClone.SetValue(objectClone, value, null);
                        }
                    }
                }
            }

            return objectClone;
        }


        /// <summary>
        /// Retorna a linha da tabela com as propriedades carregadas.
        /// </summary>
        /// <param name="dr">Linha do dataTable</param>
        /// <param name="entity">Entidade utilizada para carregar os valores</param>
        /// <returns>Uma entidade carregada com os valores</returns>
        public static DataRow EntityToDataRow(DataTable dt, object entity)
        {
            try
            {
                DataRow drRetorno = dt.NewRow();

                DataColumnCollection columnList = dt.Columns;
                Type tp = entity.GetType();
                // Preenche propriedades.
                PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                // Preenche variáveis.
                FieldInfo[] fields = tp.GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (DataColumn coluna in columnList)
                {
                    PropertyInfo prop = properties.ToList().Find(p => p.Name == coluna.ColumnName);
                    if (prop != null)
                    {
                        drRetorno[coluna] = prop.GetValue(entity);
                    }

                    FieldInfo field = fields.ToList().Find(p => p.Name == coluna.ColumnName);
                    if (field != null)
                    {
                        drRetorno[coluna] = field.GetValue(entity);
                    }
                }


                return drRetorno;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna um objeto carregado de acordo com os valores do dataTable.
        /// </summary>
        /// <param name="dr">Linha do dataTable</param>
        /// <param name="entity">Entidade utilizada para carregar os valores</param>
        /// <returns>Uma entidade carregada com os valores</returns>
        public static object DataRowToEntity(DataRow dr, object entity)
        {
            try
            {
                Type tp = entity.GetType();

                // Preenche propriedades.
                PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in properties)
                {
                    if (dr.Table.Columns.Contains(prop.Name))
                    {
                        if (dr[prop.Name] != DBNull.Value)
                        {
                            var value = dr[prop.Name];

                            if (prop.PropertyType == typeof(string))
                            {
                                if (value.GetType() == typeof(DateTime))
                                {
                                    DateTime date = Convert.ToDateTime(value);
                                    prop.SetValue(entity, date.ToString(DateUtil.DATA_HORA_SEGUNDOS_MILIS), null);
                                }
                                else
                                {
                                    prop.SetValue(entity, value.ToString(), null);
                                }
                            }
                            else
                            {
                                prop.SetValue(entity, value, null);
                            }
                        }
                    }
                }

                // Preenche variáveis.
                FieldInfo[] fields = tp.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (dr.Table.Columns.Contains(field.Name))
                    {
                        if (dr[field.Name] != DBNull.Value)
                            field.SetValue(entity, dr[field.Name]);
                    }
                }

                return entity;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Converte o texto passado para maiúsculo, caso não seja nulo.
        /// </summary>
        /// <param name="texto">Texto a ser convertido.</param>
        /// <returns>Texto convertido para maiúsculo.</returns>
        public static string ConverterParaMaiusculo(string texto)
        {
            return (texto ?? string.Empty).ToUpper();
        }

        /// <summary>
        /// Grava o erro na tabela do CoreSSO_Log
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void GravarErro(Exception ex)
        {
            try
            {
                LOG_Erros entity = new LOG_Erros();

                IDictionary<string, ICFG_Configuracao> configuracao;
                CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
                entity.sis_id = string.IsNullOrEmpty(configuracao["appSistemaID"].cfg_valor) ? 0 : Convert.ToInt32(configuracao["appSistemaID"].cfg_valor);
                entity.err_descricao = GetErrorMessage(ex);
                entity.err_erroBase = ex.GetBaseException().Message;
                entity.err_tipoErro = ex.GetBaseException().GetType().FullName;
                entity.err_dataHora = DateTime.Now;
                entity.err_machineName = Environment.MachineName;

                string strHostName;
                string clientIPAddress = "";
                try
                {
                    strHostName = Dns.GetHostName();
                    clientIPAddress = Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                }
                catch
                {
                }
                entity.err_ip = String.IsNullOrEmpty(clientIPAddress) ? "0.0.0.0" : clientIPAddress;

                LOG_ErrosBO.Save(entity);
            }
            catch { }
        }

        /// <summary>
        /// Transforma uma tabela em uma string para no formato de arquivo csv
        /// </summary>
        /// <param name="dt">Tabela</param>
        /// <returns>StringBuilder com os dados da tabela sepadados por ;</returns>
        public static StringBuilder ConverterTabelaParaArquivoCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();

            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(";", fields));
            }
            return sb;
        }

        /// <summary>
        /// Retorna a exception formatada
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Exception formatada</returns>
        private static string GetErrorMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendFormat("** {0} **\r\n", DateTime.Now);
                sb.AppendFormat("Exception Type: {0}\r\n", ex.GetType());

                sb.AppendFormat("Exception: {0}\r\n", ex.Message);
                sb.AppendFormat("Source: {0}\r\n", ex.Source);

                if (ex.StackTrace != null)
                {
                    sb.AppendFormat("Stack Trace: {0}\r\n\r\n", ex.StackTrace);
                }

                Exception exTemp = null;
                if (ex.InnerException != null)
                    exTemp = ex.InnerException;
                while (exTemp != null)
                {
                    sb.AppendFormat("Inner Exception Type: {0}\r\n", exTemp.GetType());
                    sb.AppendFormat("Inner Exception: {0}\r\n", exTemp.Message);
                    sb.AppendFormat("Inner Source: {0}\r\n", exTemp.Source);
                    if (exTemp.StackTrace != null)
                    {
                        sb.AppendFormat("Inner Stack Trace: {0}\r\n\r\n", exTemp.StackTrace);
                    }
                    exTemp = exTemp.InnerException;
                }
            }
            catch
            {
            }
            return sb.ToString();
        }

        /// <summary>
        /// Verifica a existência da chave informada (4 campos) nas tabelas do sistema, exceto nas tabelas
        /// que estiverem no parâmetro tabelasNaoVerificar. Retorna true se a chave estiver sendo usada.
        /// </summary>
        /// <param name="campo1">Nome da coluna 1 da chave - obrigatório</param>
        /// <param name="campo2">Nome da coluna 2 da chave - obrigatório</param>
        /// <param name="campo3">Nome da coluna 3 da chave - obrigatório</param>
        /// <param name="campo3">Nome da coluna 4 da chave - obrigatório</param>
        /// <param name="valorCampo1">Valor da coluna 1 da chave - obrigatório</param>
        /// <param name="valorCampo2">Valor da coluna 2 da chave - obrigatório</param>
        /// <param name="valorCampo3">Valor da coluna 3 da chave - obrigatório</param>
        /// <param name="valorCampo3">Valor da coluna 4 da chave - obrigatório</param>
        /// <param name="tabelasNaoVerificar">Tabelas que não serão verificadas (separadas por "," - obrigatório)</param>
        /// <param name="banco">Conexão aberta com o banco de dados/Null para uma nova conexão</param>
        /// <returns>Flag que indica se chave está sendo usada em outros lugares</returns>
        public static bool VerificaIntegridadaChaveTetra
        (
            string campo1
            , string campo2
            , string campo3
            , string campo4
            , string valorCampo1
            , string valorCampo2
            , string valorCampo3
            , string valorCampo4
            , string tabelasNaoVerificar
            , TalkDBTransaction banco
        )
        {
            ESC_EscolaDAO dao = new ESC_EscolaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                return dao.VerificaIntegridadaChaveTetra(campo1, campo2, campo3, campo4, valorCampo1, valorCampo2, valorCampo3, valorCampo4, tabelasNaoVerificar);
            }
            catch (Exception err)
            {
                if (banco == null)
                    dao._Banco.Close(err);

                throw;
            }
            finally
            {
                if (banco == null)
                    dao._Banco.Close();
            }
        }

        /// <summary>
        /// Retorna se existe um conflito entre 2 períodos de data.
        /// p1 = Período 1.
        /// p2 = Período 2.
        /// Se a data de fim do período for vazias, passar DateTime.MaxValue.
        /// </summary>
        /// <param name="p1Inicio">Início do período 1</param>
        /// <param name="p1Fim">Fim do período 1</param>
        /// <param name="p2Inicio">Início do Período 2</param>
        /// <param name="p2Fim">Fim do período 2</param>
        /// <returns></returns>
        public static bool ExisteConflitoDatas(DateTime p1Inicio, DateTime p1Fim, DateTime p2Inicio, DateTime p2Fim)
        {
            p1Inicio = p1Inicio.Date;
            p1Fim = p1Fim.Date;
            p2Inicio = p2Inicio.Date;
            p2Fim = p2Fim.Date;

            return ((p1Inicio == p2Inicio) ||
                    (p1Inicio > p2Inicio ? p1Inicio <= p2Fim : p2Inicio <= p1Fim));
        }

        /// <summary>
        /// Calcula a diferença entre as datas por extenso
        /// </summary>
        /// <param name="dataInicial">Data inicial (menor)</param>
        /// <param name="dataFinal">Data final (maior)</param>
        /// <returns></returns>
        public static string DiferencaDataExtenso(DateTime dataInicial, DateTime dataFinal, string nomeDataInicial = "", string nomeDataFinal = "")
        {
            nomeDataFinal = string.IsNullOrEmpty(nomeDataFinal) ? "Data final" : nomeDataFinal;
            nomeDataInicial = string.IsNullOrEmpty(nomeDataInicial) ? "data inicial" : nomeDataInicial;
            if (dataFinal < dataInicial)
                throw new ValidationException(nomeDataFinal + " não pode ser maior que " + nomeDataInicial + ".");
            if (dataFinal == dataInicial)
                return "0 dias.";

            int anos = 0; int meses = 0; int dias = 0; int dif = 0;
            int controle = 1;

            if (dataInicial.Day == 29 && dataInicial.Month == 2)
                dif = 1;

            while (controle != 0)
            {
                switch (controle)
                {
                    case 1:
                        if (dataInicial.AddYears(anos + 1).AddDays(dif) > dataFinal)
                        {
                            controle = 2;
                            dataInicial = dataInicial.AddYears(anos);
                            if (DateTime.IsLeapYear(dataInicial.Year) && dataInicial.Month > 1)
                                dif = 0;
                        }
                        else
                        {
                            anos++;
                        }
                        break;
                    case 2:
                        if (dataInicial.AddMonths(meses + 1).AddDays(dif) > dataFinal)
                        {
                            controle = 3;
                            dataInicial = dataInicial.AddMonths(meses);
                            if (DateTime.IsLeapYear(dataInicial.Year) && dataInicial.Month > 1)
                                dif = 0;
                        }
                        else
                        {
                            meses++;
                        }
                        break;
                    case 3:
                        if (dataInicial.AddDays(dias + 1 + dif) > dataFinal)
                        {
                            dataInicial = dataInicial.AddDays(dias + dif);
                            controle = 0;
                        }
                        else
                        {
                            dias++;
                        }
                        break;
                }
            }

            if (anos <= 0 && meses <= 0 && dias <= 0)
                return "";

            return (anos > 0 ? anos.ToString() + " ano" + (anos > 1 ? "s" : "") : "") +
                   (meses > 0 ? (anos > 0 ? (dias > 0 ? ", " : " e ") : "") + meses.ToString() + (meses > 1 ? " meses" : " mês") : "") +
                   (dias > 0 ? (anos > 0 || meses > 0 ? " e " : "") + dias.ToString() + " dia" + (dias > 1 ? "s" : "") : "") + ".";
        }

        /// <summary>
        /// Retorna se o periodo 2 está fora do periodo 1
        /// p1 = Período 1.
        /// p2 = Período 2.
        /// Se a data de fim dos períodos for vazia, passar DateTime.MaxValue.
        /// </summary>
        /// <param name="p1Inicio">Início do período 1</param>
        /// <param name="p1Fim">Fim do período 1</param>
        /// <param name="p2Inicio">Início do Período 2</param>
        /// <param name="p2Fim">Fim do período 2</param>
        /// <returns>true/false</returns>
        public static bool DataForaPeriodo(DateTime p1Inicio, DateTime p1Fim, DateTime p2Inicio, DateTime p2Fim)
        {
            p1Inicio = p1Inicio.Date;
            p1Fim = p1Fim.Date;
            p2Inicio = p2Inicio.Date;
            p2Fim = p2Fim.Date;

            bool isValidInicio = (p2Inicio.Date >= p1Inicio.Date &&
                           ((p1Fim.Date == new DateTime()) || (p2Inicio.Date <= p1Fim.Date)));

            bool isValidFim = ((p2Fim.Date == new DateTime().Date) || (p2Fim.Date >= p1Inicio.Date &&
                           ((p2Fim.Date <= p1Fim.Date))));

            return ((!isValidFim) || (!isValidInicio));
        }

        /// <summary>
        /// Retorna um IEnumerable do objeto T
        /// </summary>
        /// <typeparam name="T">Classe do Tipo para ser retornado</typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IEnumerable<T> MapToEnumerable<T>(DataTable dataTable) where T : class, new()
        {
            foreach (DataRow row in dataTable.Rows)
            {
                T entity = new T();

                yield return (T)GestaoEscolarUtilBO.DataRowToEntity(row, entity);
            }
        }

        /// <summary>
        /// Valida a entidade, e se não estiver válida, dispara um ValidationException.
        /// </summary>
        /// <param name="entity">Entidade a ser validada</param>
        public static void ValidaEntidade(Data.Common.Abstracts.Abstract_Entity entity)
        {
            if (!entity.Validate())
                throw new ValidationException(ErrosValidacao(entity));
        }

        /// <summary>
        /// Retorna a lista de erros de validação dos campos
        /// </summary>
        /// <param name="Entidade">Entidade que deseja verificar a lista de validações</param>
        /// <returns>Lista com os erros separados pela tag br</returns>
        public static string ErrosValidacao(Data.Common.Abstracts.Abstract_Entity Entidade)
        {
            string listaErros = "";

            foreach (Validation.ValidationErrors erro in Entidade.PropertiesErrorList)
            {
                erro.Message = TrocaParametroMensagem(erro.Message);
                listaErros += erro.Message + "<br />";
            }

            return listaErros;
        }

        /// <summary>
        /// Metodo que realiza a substituição das chaves de parametro de mensagem pelo valor.
        /// </summary>
        /// <param name="msg">Texto que contém as chaves para substituição</param>
        /// <returns></returns>
        public static string TrocaParametroMensagem(string msg)
        {
            var Parametros = (from CFG_ParametroMensagem pms in CFG_ParametroMensagemBO.ParametrosMSG
                              where pms.pms_situacao != 3
                              && msg.Contains(pms.pms_chave)
                              select
                              new
                              {
                                  pms.pms_chave
                                    ,
                                  pms.pms_valor
                              }).ToList();

            bool alterou = false;

            Parametros.ForEach
                (pms =>
                {
                    msg = SubstituiValorMensagem(msg, pms.pms_chave, pms.pms_valor, ref alterou);
                }
                );

            return msg;
        }

        /// <summary>
        /// Troca o valor do parâmetro de mensagem no texto enviado, busca valor exatamente igual (considerando
        /// maiúsculas).
        /// </summary>
        /// <param name="texto">Texto a ser substituído</param>
        /// <param name="pms_chave">Chave do parâmetro</param>
        /// <param name="pms_valor">Valor que será substituído</param>
        /// <returns></returns>
        public static string SubstituiValorMensagem(string texto, string pms_chave, string pms_valor, ref bool alterou)
        {
            if (!string.IsNullOrEmpty(texto) && texto.Contains(pms_chave) && !string.IsNullOrEmpty(pms_valor))
            {
                alterou = true;
                texto = texto.Replace(pms_chave, pms_valor).ToLower();
                string c = texto.Substring(0, 1);
                return texto.Remove(0, 1).Insert(0, c.ToUpper());
            }
            return texto;
        }

        /// <summary>
        /// Calcula o aniversário em anos, meses e dias.
        /// </summary>
        /// <param name="dtVerificar">Data a comparar</param>
        /// <param name="dtNasc">Data de nascimento</param>
        /// <param name="anos">Retorno de anos</param>
        /// <param name="meses">Retorno de meses</param>
        /// <param name="dias">Retorno de dias</param>
        public static void CalculaAniversarioCompleto(DateTime dtVerificar, DateTime dtNasc, out int anos, out int meses, out int dias)
        {
            // compute & return the difference of two dates,
            // returning years, months & days
            // d1 should be the larger (newest) of the two dates
            // we want d1 to be the larger (newest) date
            // flip if we need to
            if (dtVerificar < dtNasc)
            {
                DateTime d3 = dtNasc;
                dtNasc = dtVerificar;
                dtVerificar = d3;
            }

            // compute difference in total months
            meses = 12 * (dtVerificar.Year - dtNasc.Year) + (dtVerificar.Month - dtNasc.Month);

            // based upon the 'days',
            // adjust months & compute actual days difference
            if (dtVerificar.Day < dtNasc.Day)
            {
                meses--;
                dias = DateTime.DaysInMonth(dtNasc.Year, dtNasc.Month) - dtNasc.Day + dtVerificar.Day;
            }
            else
            {
                dias = dtVerificar.Day - dtNasc.Day;
            }

            // compute years & actual months
            anos = meses / 12;
            meses -= anos * 12;
        }

        /// <summary>
        /// Retorna a string com anos e meses do aniversário.
        /// </summary>
        /// <param name="anos"></param>
        /// <param name="meses"></param>
        /// <returns></returns>
        public static string RetornaStringAniversario(int anos, int meses)
        {
            string ret = "" + (anos > 0 ? (anos + (anos > 1 ? " anos " : " ano ")) : "");

            if ((anos > 0) && (meses > 0))
                ret += " e ";

            ret += (meses > 0 ? (meses + (meses > 1 ? " meses " : " mês ")) : (anos > 0 ? "" : "0 meses"));

            return ret;
        }

        /// <summary>
        /// Verifica se campo foto se esta nulo e retorna uma mensagem de alerta.
        /// </summary>
        /// <param name="Foto"></param>
        /// <returns></returns>
        public static string VerificarFoto(string Foto)
        {
            if (!string.IsNullOrEmpty(Foto))
                return "É necessário incluir a foto novamente.";

            return string.Empty;
        }

        /// <summary>
        /// Armazena o nome padrao do período para o sistema
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static string nomePadraoPeriodo_Calendario(Guid ent_id)
        {
            string nomePeriodo_Calendario = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_PERIODO_CALENDARIO, ent_id);
            if (string.IsNullOrEmpty(nomePeriodo_Calendario))
                nomePeriodo_Calendario = "Período do calendário";

            return nomePeriodo_Calendario;
        }

        /// <summary>
        /// Armazena o nome padrao do curso para o sistema
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static string nomePadraoCurso(Guid ent_id)
        {
            string nomeCurso = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_CADASTRO_CURSO, ent_id);
            if (string.IsNullOrEmpty(nomeCurso))
                nomeCurso = "Curso";

            return nomeCurso;
        }

        /// <summary>
        /// Armazena o nome padrao do período para o sistema
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static string nomePadraoPeriodo(Guid ent_id)
        {
            string nomePeriodo = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_PERIODO_CURSO, ent_id);
            if (string.IsNullOrEmpty(nomePeriodo))
                nomePeriodo = "Período do curso";

            return nomePeriodo;
        }

        /// <summary>
        /// Armazena o nome padrao do tipo currículo período para o sistema
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        public static string nomePadraoTipoCurrPeriodo(Guid ent_id)
        {
            string nomePeriodo = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_TIPO_CURRICULO_PERIODO, ent_id);
            if (string.IsNullOrEmpty(nomePeriodo))
                nomePeriodo = "Tipo currículo período";

            return nomePeriodo;
        }

        /// <summary>
        /// Retorna o nome padrao da avaliação do período, caso o curso seja seriado por avaliação (peja)
        /// </summary>
        public static string nomePadraoPeriodoAvaliacao(string nomeAvaliacao)
        {
            if (string.IsNullOrEmpty(nomeAvaliacao))
                nomeAvaliacao = "Avaliação";

            return nomeAvaliacao;
        }

        /// <summary>
        /// Armazena o nome padrao da matricula estadual
        /// </summary>
        public static string nomePadraoMatriculaEstadual(Guid ent_id)
        {
            string nomeMatriculaEstadual = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.MATRICULA_ESTADUAL, ent_id);
            if (string.IsNullOrEmpty(nomeMatriculaEstadual))
                nomeMatriculaEstadual = "Número de matrícula";

            return nomeMatriculaEstadual;
        }

        /// <summary>
        /// Armazena o nome padrao da designação
        /// </summary>
        public static string nomePadraoDesignacao(Guid ent_id)
        {
            string nomeDesignacao = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.DESIGNACAO, ent_id);
            if (string.IsNullOrEmpty(nomeDesignacao))
                nomeDesignacao = "Designação";

            return nomeDesignacao;
        }

        /// <summary>
        /// Armazena o nome padrao do combo para tipo de deficiencia.
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// </summary>
        public static string nomePadraoTipoDeficiencia(Guid ent_id)
        {
            string nomeTipoDeficiencia = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.CAMPO_TIPO_DEFICIENCIA_ALUNO, ent_id);
            if (string.IsNullOrEmpty(nomeTipoDeficiencia))
                nomeTipoDeficiencia = "Tipo de deficiência";

            return nomeTipoDeficiencia;
        }

        /// <summary>
        /// Armazena o nome padrão da atividade avaliativa para o sistema
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns></returns>
        public static string nomePadraoAtividadeAvaliativa(Guid ent_id)
        {
            string nomeAtvidade = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.NOME_ATIVIDADE, ent_id);
            if (string.IsNullOrEmpty(nomeAtvidade))
                nomeAtvidade = "Atividade avaliativa";

            return nomeAtvidade;
        }

        /// <summary>
        /// Retorna o nome do tipo de unidade administrativa que filtra escolas.
        /// </summary>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <returns>Nome do tipo de unidade administrativa</returns>
        public static string nomeUnidadeAdministrativaFiltroEscola(Guid ent_id)
        {
            SYS_TipoUnidadeAdministrativa entityTipoUnidadeAdministrativa = new SYS_TipoUnidadeAdministrativa();

            string tua_id = ACA_ParametroAcademicoBO.ParametroValorPorEntidade(eChaveAcademico.TIPO_UNIDADE_ADMINISTRATIVA_FILTRO_ESCOLA, ent_id);
            if (!string.IsNullOrEmpty(tua_id))
            {
                entityTipoUnidadeAdministrativa.tua_id = new Guid(tua_id);
                SYS_TipoUnidadeAdministrativaBO.GetEntity(entityTipoUnidadeAdministrativa);
            }

            string nomeUnidadeAdministrativa = entityTipoUnidadeAdministrativa.tua_nome;
            if (string.IsNullOrEmpty(nomeUnidadeAdministrativa))
                nomeUnidadeAdministrativa = "Unidade Administrativa";

            return nomeUnidadeAdministrativa;
        }

        #region Arquivos .csv

        /// <summary>
        /// Verifica a estrutura do arquivo de importação
        /// </summary>
        /// <param name="line"></param>
        /// <param name="columns"></param>
        /// <param name="separator"></param>
        public static bool VerificarEstruturaRegistro(string line, int columns, out char separator)
        {
            // -> Validação do separador utilizado no arquivo
            if (line.Contains(';'))
                separator = ';';
            else
                throw new ArgumentException
                    ("O registro não está formatado corretamente, todos os campos devem estar separados por ponto e vírgula.");

            // -> Validação da quantidade de colunas do arquivo
            if (line.Split(separator).Length != columns)
                throw new ArgumentException
                    ("O registro possui campos divergentes.");

            return true;
        }

        /// <summary>
        /// Verifica o arquivo de importação
        /// </summary>
        /// <param name="PostedFile"></param>
        public static bool VerificarArquivo(HttpPostedFile PostedFile)
        {
            // -> Valida upload do arquivo
            bool upload = false;
            if (PostedFile != null)
                upload = (!String.IsNullOrEmpty(PostedFile.FileName));

            if (upload)
            {
                // -> Valida a extensão do arquivo
                if (!Path.GetExtension(PostedFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    throw new ValidationException("O arquivo selecionado não possui um formato válido para a importação.");

                // -> Valida quantidade de linhas do arquivo
                Regex regex = new Regex("\r\n");
                StreamReader streamReader = new StreamReader(PostedFile.InputStream);
                String stringReader = streamReader.ReadToEnd();

                // Reposiciona o cursor virtual no ínico do arquivo
                PostedFile.InputStream.Position = 0;

                long qtdeLinhas = (regex.Matches(stringReader).Count - 1);
                if (qtdeLinhas > 10000)
                    throw new ValidationException("O arquivo selecionado possui " + qtdeLinhas + " registros, selecione um arquivo com até 10000 registros.");
            }
            else
            {
                throw new ValidationException("Não foi possível efetuar upload do arquivo selecionado.");
            }

            return true;
        }

        #endregion Arquivos .csv

        /// <summary>
        /// retorna uma mesagem de alerta caso o formato da data esteja fora do padrão ou inexistente.
        /// </summary>
        /// <param name="msgValidacaoData"></param>
        /// <returns></returns>
        public static string RetornaMsgValidacaoData(string msgValidacaoData)
        {
            msgValidacaoData += " não está no formato dd/mm/aaaa ou é inexistente.";

            return msgValidacaoData;
        }

        /// <summary>
        /// retorna uma mesagem de alerta caso o formato do ano esteja fora do padrão ou inexistente.
        /// </summary>
        /// <param name="msgValidacaoAno"></param>
        /// <returns></returns>
        public static string RetornaMsgValidacaoAno(string msgValidacaoAno)
        {
            msgValidacaoAno += " não está no formato AAAA (4 dígitos) ou é inexistente.";

            return msgValidacaoAno;
        }

        /// <summary>
        /// Valida se a data está no formato DD/MM/AAAA.
        /// </summary>
        /// <param name="data">String com a data.</param>
        /// <returns>True= Está no formato. | False = Não está no formato.</returns>
        public static bool ValidaFormatoData(string data)
        {
            Regex Data = new Regex(@"(((0[1-9]|[12][0-9]|3[01])([/])(0[13578]|10|12)([/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([/])(0[469]|11)([/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([/])(02)([/])(\d{4}))|((29)(\.|-|\/)(02)([/])([02468][048]00))|((29)([/])(02)([/])([13579][26]00))|((29)([/])(02)([/])([0-9][0-9][0][48]))|((29)([/])(02)([/])([0-9][0-9][2468][048]))|((29)([/])(02)([/])([0-9][0-9][13579][26])))$");
            return Data.IsMatch(data);
        }

        /// <summary>
        /// Verifica se o objeto possui algum valor numérico.
        /// </summary>
        /// <param name="Expression">Expressão a ser analisada.</param>
        /// <returns>True = É numérico | False = Não é numérico.</returns>
        public static bool IsNumeric(object Expression)
        {
            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // Variable to collect the Return value of the TryParse method.
            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        /// <summary>
        /// Verifica se o ano digitado pelo usuário é bissexto ou não
        /// </summary>
        /// <param name="ano">Ano informado</param>
        /// <returns>True:é ano bissexto/False: é ano normal</returns>
        public static bool VerificaAnoBissexto(int ano)
        {
            return (ano % 4 == 0 && (ano % 100 != 0 || ano % 400 == 0));
        }

        /// <summary>
        /// Busca o nome do módulo de acordo com a url passada.
        /// </summary>
        /// <param name="caminho">Caminho da url.</param>
        /// <param name="textoPadrao">Texto do nome caso o parâmetro esteja vazio.</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <returns>Retorna o nome do módulo.</returns>
        public static string BuscaNomeModulo(string caminho, string textoPadrao, Guid gru_id)
        {
            SYS_GrupoPermissao grupoPermissao = SYS_GrupoBO.GetGrupoPermissaoBy_UrlNaoAbsoluta
            (
                gru_id
                ,
                caminho
            );

            SYS_Modulo modulo = new SYS_Modulo
            {
                mod_id = grupoPermissao.mod_id
                ,
                sis_id = grupoPermissao.sis_id
            };
            modulo = GetEntityModuloCache(modulo);

            return string.IsNullOrEmpty(modulo.mod_nome) ? textoPadrao : modulo.mod_nome;
        }

        /// <summary>
        /// Da um GetEntity na tabela de módulo e guarda no cache.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static SYS_Modulo GetEntityModuloCache(SYS_Modulo entity)
        {
            if (HttpContext.Current != null)
            {
                // Chave padrão do cache - nome do método + parâmetros.
                string chave = RetornaChaveCache_GetEntityModulo(entity);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    entity = SYS_ModuloBO.GetEntity(entity);
                    // Adiciona cache com validade de 1h.
                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddHours(1)
                        , System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    entity = (SYS_Modulo)cache;
                }

                return entity;
            }

            return SYS_ModuloBO.GetEntity(entity);
        }

        /// <summary>
        /// Retorna a chave para guardar em cache o GetEntity da classe.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string RetornaChaveCache_GetEntityModulo(SYS_Modulo entity)
        {
            return string.Format("SYS_Modulo_GetEntity_{0}_{1}", entity.sis_id, entity.mod_id);
        }

        /// <summary>
        /// Verifica se o usuário tem permissão para acessar a url informada
        /// </summary>
        /// <param name="caminho">Caminho da url.</param>
        /// <param name="gru_id">Id do grupo</param>
        /// <returns>Retorna se o usuário tem permissão ou não</returns>
        public static bool VerificaPermissaoModuloPorUrl(string caminho, Guid gru_id)
        {
            SYS_GrupoPermissao grupoPermissao = SYS_GrupoBO.GetGrupoPermissaoBy_UrlNaoAbsoluta
            (
                gru_id
                ,
                caminho
            );

            return grupoPermissao.grp_alterar || grupoPermissao.grp_consultar || grupoPermissao.grp_inserir || grupoPermissao.grp_excluir;
        }

        /// <summary>
        /// Retorna o nome do mês referente ao número
        /// </summary>
        /// <param name="mes">Número do mês</param>
        /// <returns>String com o nome do mês referente ao número</returns>
        public static string RetornaNomeMes(int mes)
        {
            switch (mes)
            {
                case 1:
                    return "Janeiro";

                case 2:
                    return "Fevereiro";

                case 3:
                    return "Março";

                case 4:
                    return "Abril";

                case 5:
                    return "Maio";

                case 6:
                    return "Junho";

                case 7:
                    return "Julho";

                case 8:
                    return "Agosto";

                case 9:
                    return "Setembro";

                case 10:
                    return "Outubro";

                case 11:
                    return "Novembro";

                case 12:
                    return "Dezembro";
            }

            return string.Empty;
        }

        /// <summary>
        /// Retorna apenas os números da string informada
        /// </summary>
        /// <param name="expr">String para extrair apenas os números</param>
        /// <returns>Apenas os números da string informada</returns>
        public static string RetornarApenasNumeros(string expr)
        {
            return string.Join(null, Regex.Split(expr, "[^\\d]"));
        }

        /// <summary>
        /// Retorna apenas os números da string informada
        /// </summary>
        /// <param name="expr">String para extrair apenas os números</param>
        /// <returns>Apenas os números da string informada</returns>
        public static string RetornarNomeArquivoValido(string expr)
        {
            return string.Join(null, Regex.Split(expr, "[\\\\/:\\*\\?\"<>\\|]"));
        }

        /// <summary>
        /// Verifica se a string é um Guid
        /// </summary>
        /// <param name="s">String a converter</param>
        /// <param name="result">Variavel para colocar o resultado quando é valido</param>
        /// <returns></returns>
        public static bool GuidTryParse(string s, out Guid result)
        {
            result = Guid.Empty;

            if (String.IsNullOrEmpty(s) || s.Equals(Guid.Empty.ToString()))
                return false;

            Regex format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

            Match match = format.Match(s);

            if (match.Success)
            {
                result = new Guid(s);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Busca o cabecalho
        /// </summary>
        /// <param name="esc_id"></param>
        /// <param name="uni_id"></param>
        /// <param name="telefone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static DataTable SelecionaPorEscolaCurso(int esc_id, int uni_id, Guid telefone, Guid email)
        {
            ACA_AvisoTextoGeralDAO dao = new ACA_AvisoTextoGeralDAO();
            return dao.SelecionaCabecalho(esc_id, uni_id, telefone, email);
        }

        #region Diario de classe

        /// <summary>
        /// Método para verificar a existência de um período relacionado com a turma. Se existir retorna o ID do período.
        /// <param name="Tur_ID">ID da turma</param>
        /// <param name="data">Data da aula</param>
        /// <returns>Retorna o ID do período em questão</returns>
        /// </summary>
        public static void RetornaPerido(int Tur_ID, DateTime data, out int tpc_id, out DateTime cap_dataInicio, out DateTime cap_dataFim)
        {
            //Carregando entidade Turma
            TUR_Turma turma = new TUR_Turma()
            {
                tur_id = Tur_ID
            };
            TUR_TurmaBO.GetEntity(turma);

            // Obtendo dados do período a partir do cal_id da turma e da Data da aula.
            ACA_CalendarioPeriodoBO.SelecionaPeriodoPorCalendarioData(turma.cal_id, data, out tpc_id, out cap_dataInicio, out cap_dataFim);
        }

        /// <summary>
        /// Exclui todos os registros de atividades associados a uma aula específica
        /// <param name="Tud_ID">ID da disciplina</param>
        /// <param name="Tpc_ID">ID do Período</param>
        /// <param name="Ent_ID">ID da Entidade</param>
        /// <returns>Retorna sucesso ou não no processo</returns>
        /// </summary>
        public static bool ExcluirTurmaNota(int Tud_ID, int Tpc_ID)
        {
            int Tnt_ID;
            bool sucesso = false;
            try
            {
                DataTable ExisteAtividade = new DataTable();
                //Excluindo todas as atividades referentes à aula
                do
                {
                    //Se existe a atividade, buscaremos por seus Tnt_ID e junto ao Tud_ID a excluiremos
                    ExisteAtividade = CLS_TurmaNotaBO.GetSelectBy_TurmaDisciplina_Periodo(Tud_ID, Tpc_ID);

                    Tnt_ID = Convert.ToInt32(ExisteAtividade.Rows[0]["tnt_id"]);

                    CLS_TurmaNota entJaExistente = new CLS_TurmaNota
                    {
                        tud_id = Tud_ID
                            ,
                        tnt_id = Tnt_ID
                    };

                    entJaExistente = CLS_TurmaNotaBO.GetEntity(entJaExistente);

                    sucesso = CLS_TurmaNotaBO.Delete(entJaExistente);
                }
                while (ExisteAtividade.Rows.Count != 0);
                return sucesso;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica se a aula está dentro do perídodo de matrícula do aluno
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="mtu_id">ID da matrícula do aluno na Turma</param>
        /// <param name="mtd_id">ID da matrícula do aluno na Disciplina</param>
        /// <param name="taa_data"></param>
        /// <returns>Retorna sucesso caso a aula esteja dentro do período de matrícula do aluno</returns>
        public static bool ValidarMatriculaAluno(int alu_id, int mtu_id, int mtd_id, DateTime taa_data)
        {
            MTR_MatriculaTurmaDisciplina ent = new MTR_MatriculaTurmaDisciplina
            {
                alu_id = alu_id,
                mtu_id = mtu_id,
                mtd_id = mtd_id
            };

            MTR_MatriculaTurmaDisciplinaBO.GetEntity(ent);
            if (!ent.IsNew)
            {
                //null date. Matrícula sem período final
                if (ent.mtd_dataSaida == new DateTime())
                {
                    if (taa_data < ent.mtd_dataMatricula)
                    {
                        return false;
                    }
                }
                else
                {
                    if (taa_data < ent.mtd_dataMatricula || taa_data > ent.mtd_dataSaida)
                    {
                        return false;
                    }
                }

                //Aula dentro do período de matricula
                return true;
            }

            //entity MTR_MatriculaTurmaDisciplina not found
            return false;
        }

        #endregion Diario de classe

        /// <summary>
        /// Gets or sets the current dev report.
        /// </summary>
        /// <value>
        /// The current dev report.
        /// </value>
        /// <author>juliano.real</author>
        /// <datetime>24/10/2013-11:58</datetime>
        public static XtraReport CurrentDevReport
        {
            get
            {
                XtraReport DevReport = HttpContext.Current.Session[DevReportName] as XtraReport;
                if (DevReport != null)
                {
                    return DevReport;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[DevReportName] = value;
            }
        }

        /// <summary>
        /// Carrega a session corrente do usuário com as informações para a geração do relatório.
        /// </summary>
        /// <param name="reportID">id do relatório.</param>
        /// <param name="parameters">Parâmetros do relatório.</param>
        public static void SendParametersToReport(XtraReport dxReport)
        {
            #region VALIDA PARAMETROS DE ENTRADA

            if (dxReport == null)
                throw new ValidationException("O report é obrigatório.");

            #endregion VALIDA PARAMETROS DE ENTRADA

            CurrentDevReport = dxReport;
        }

        /// <summary>
        /// Remover os valores da session com parâmetros para geração dos relatórios.
        /// </summary>
        public static void ClearSessionReportParameters()
        {
            HttpContext.Current.Session.Remove(DevReportName);
        }

        /// <summary>
        /// Busca o valor do parametro disciplina pela chave.
        /// </summary>
        /// <param name="pms_chave">Chave de parametro mensagem</param>
        /// <returns></returns>
        public static string BuscaValorParametroMensagemDisciplina(string pms_chave)
        {
            return CFG_ParametroMensagemBO.BuscaValoraPorChave(pms_chave);
        }

        /// <summary>
        /// Preenche o combo de acordo com o enumerador.
        /// </summary>
        /// <typeparam name="T">Enumerador.</typeparam>
        /// <param name="cbo">Combo a ser populado.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Método genérico para carregar o combo de acordo com um numerador T")]
        public static void CarregarComboEnum<T>(DropDownList cbo)
        {
            if (cbo != null)
            {
                Type objType = typeof(T);
                FieldInfo[] propriedades = objType.GetFields();
                foreach (FieldInfo objField in propriedades)
                {
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attributes.Length > 0)
                    {
                        cbo.Items.Add(new ListItem(CustomResource.GetGlobalResourceObject("Enumerador", attributes[0].Description), Convert.ToString(objField.GetRawConstantValue())));
                    }
                }
            }
        }

        /// <summary>
        /// Preenche o combo de acordo com o enumerador.
        /// </summary>
        /// <typeparam name="T">Enumerador.</typeparam>
        /// <param name="cbo">Combo a ser populado.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Método genérico para carregar o combo de acordo com um numerador T")]
        public static void CarregarComboEnum<T>(ListItemCollection Items, bool ordenar = false)
        {
            if (Items != null)
            {
                ListItem[] lstItems = EnumToArrayListItem<T>();
                if (ordenar)
                {
                    lstItems = lstItems.OrderBy(p => p.Value).ToArray();
                }
                Items.AddRange(lstItems);
            }
        }

        /// <summary>
        /// Retorna uma array[] de ListItem de acordo com o enumerador.
        /// </summary>
        /// <typeparam name="T">Enumerador.</typeparam>
        /// <returns ListItem[]></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Método genérico para carregar o combo de acordo com um numerador T")]
        public static ListItem[] EnumToArrayListItem<T>()
        {
            List<ListItem> lst = new List<ListItem>();

            Type objType = typeof(T);
            FieldInfo[] propriedades = objType.GetFields();
            foreach (FieldInfo objField in propriedades)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    lst.Add(new ListItem(CustomResource.GetGlobalResourceObject("Enumerador", attributes[0].Description), Convert.ToString(objField.GetRawConstantValue())));
                }
            }

            return lst.ToArray();
        }

        /// <summary>
        /// Retorna uma array[] de ListItem de acordo com o enumerador.
        /// </summary>
        /// <typeparam name="T">Enumerador.</typeparam>
        /// <returns ListItem[]></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Método genérico para carregar o combo de acordo com um numerador T")]
        public static string GetEnumDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return CustomResource.GetGlobalResourceObject("Enumerador", attributes[0].Description);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Convert uma entidade para uma tabela com suas propriedades
        /// populando a tabela com a lista de entidades
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="entity">Objeto</param>
        /// <returns></returns>
        public static DataTable EntityToDataTable<T>(List<T> list) where T : class
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            foreach (var property in properties)
                table.Columns.Add(property.Name, property.PropertyType);

            if (properties.Any())
                foreach (T l in list)
                    table.Rows.Add(properties.Select(p => p.GetValue(l, null)).ToArray());

            return table;
        }

        /// <summary>
        /// Retorna uma Lista da entidade informada, alimentada pelo DataTable.
        /// </summary>
        public static List<T> DataTableToListEntity<T>(DataTable table) where T : class, new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in table.Rows)
            {
                list.Add((T)DataRowToEntity(dr, new T()));
            }

            return list;
        }

        /// <summary>
        /// Retorna o numero de casas decimais de um numero decimal
        /// </summary>
        /// <param name="numero">Numero</param>
        /// <returns>Qtde de casa depois da virgula</returns>
        public static int RetornaNumeroCasasDecimais(decimal numero)
        {
            int numeroCasasDecimais = 0;
            Regex SplitNumberRegex = new Regex(@"[^\d-]+", RegexOptions.Compiled);
            Regex TrailingZerosRegex = new Regex(@"[0]*$", RegexOptions.Compiled);

            string asString = numero.ToString("G", CultureInfo.InvariantCulture);

            string[] parts = SplitNumberRegex.Split(asString);

            if (parts.Length == 2)
            {
                var decimals = parts[1];
                var trimmed = TrailingZerosRegex.Replace(decimals, String.Empty);
                numeroCasasDecimais = trimmed.Length;
            }

            return numeroCasasDecimais;
        }

        /// <summary>
        /// Cria a string de formatacao para decimal com base no numero de caracteres depois da virgula
        /// </summary>
        /// <param name="numeroCasasDecimais">Numero de casas decimais</param>
        /// <param name="formatoMestre">Formato do texto de retorno, não obrigatório (#{0} => valor padrão)</param>
        /// <returns></returns>
        public static string CriaFormatacaoDecimal(int numeroCasasDecimais, string formatoMestre = "#{0}")
        {
            string formato = "0" + (numeroCasasDecimais > 0 ? "." : "");
            for (int i = 0; i < numeroCasasDecimais; i++)
                formato = formato + "0";

            return string.Format(formatoMestre, formato);
        }

        /// <summary>
        /// Carrega a url do modulo filho para a área do aluno que é cadastrada invertida
        /// </summary>
        /// <param name="sis_id">ID do sistema</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="mod_id">ID do módulo pai</param>
        /// <param name="vis_id">ID da visão</param>
        /// <param name="AppMinutosCacheLongoGeral">Quantidade de minutos da configuração de cache longo</param>
        /// <returns>URL do módulo com permissão no Core</returns>
        public static string urlModAreaAluno
        (
            int sis_id
            , int mod_id
            , Guid gru_id
            , int vis_id
            , int AppMinutosCacheLongoGeral
        )
        {
            string urlRetorno = "";
            if (AppMinutosCacheLongoGeral > 0 && HttpContext.Current != null)
            {
                string chave = String.Format("Cache_urlModAreaAluno_{0}_{1}_{2}_{3}_{4}"
                                             , sis_id
                                             , mod_id
                                             , gru_id
                                             , vis_id
                                             , AppMinutosCacheLongoGeral);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    DataTable dtModulo = SYS_ModuloSiteMapBO.GetSelect_by_mod_idPai(sis_id, mod_id, gru_id, vis_id);
                    if (dtModulo.Rows.Count > 0)
                    {
                        SYS_ModuloSiteMap modSiteMap = new SYS_ModuloSiteMap();
                        modSiteMap = (new SYS_ModuloSiteMapDAO()).DataRowToEntity(dtModulo.Rows[0], modSiteMap);
                        if (modSiteMap.mod_id > 0)
                        {
                            urlRetorno = modSiteMap.msm_url.Replace("~/", "/");
                            HttpContext.Current.Cache.Insert(chave, urlRetorno, null, DateTime.Now.AddMinutes(AppMinutosCacheLongoGeral), System.Web.Caching.Cache.NoSlidingExpiration);
                        }
                    }
                }
                else
                {
                    urlRetorno = (string)cache;
                }
            }
            else
            {
                DataTable dtModulo = SYS_ModuloSiteMapBO.GetSelect_by_mod_idPai(sis_id, mod_id, gru_id, vis_id);
                if (dtModulo.Rows.Count > 0)
                {
                    SYS_ModuloSiteMap modSiteMap = new SYS_ModuloSiteMap();
                    modSiteMap = (new SYS_ModuloSiteMapDAO()).DataRowToEntity(dtModulo.Rows[0], modSiteMap);
                    if (modSiteMap.mod_id > 0)
                    {
                        urlRetorno = modSiteMap.msm_url.Replace("~/", "/");
                    }
                }
            }

            return urlRetorno;
        }

        /// <summary>
        /// Retorna o grupo de permissão mediante os parâmetros informados.
        /// </summary>
        /// <param name="sis_id">ID do sistema</param>
        /// <param name="gru_id">ID do grupo</param>
        /// <param name="url">Url</param>
        /// <param name="entModulo">Entidade SYS_Modulo de retorno</param>
        /// <param name="AppMinutosCacheLongoGeral">Quantidade de minutos da configuração de cache longo</param>
        /// <returns>SYS_GrupoPermissao</returns>
        public static SYS_GrupoPermissao GetGrupoPermissao_Grupo_By_Url
        (
            int sis_id
            , Guid gru_id
            , string url
            , out SYS_Modulo entModulo
            , int AppMinutosCacheLongoGeral
        )
        {
            sSYS_Grupo entity = new sSYS_Grupo();

            if (AppMinutosCacheLongoGeral > 0 && HttpContext.Current != null)
            {
                string chave = String.Format("Cache_GetGrupoPermissao_Grupo_By_Url_{0}_{1}_{2}_{3}"
                                             , sis_id
                                             , gru_id
                                             , url
                                             , AppMinutosCacheLongoGeral);
                object cache = HttpContext.Current.Cache[chave];

                if (cache == null)
                {
                    entity.grupoPermissao = SYS_GrupoBO.GetGrupoPermissao_Grupo_By_Url(sis_id
                                                                                       , gru_id
                                                                                       , url
                                                                                       , out entModulo);
                    entity.modulo = entModulo;

                    HttpContext.Current.Cache.Insert(chave, entity, null, DateTime.Now.AddMinutes(AppMinutosCacheLongoGeral), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    entity = (sSYS_Grupo)cache;
                    entModulo = entity.modulo;
                }
            }
            else
                entity.grupoPermissao = SYS_GrupoBO.GetGrupoPermissao_Grupo_By_Url(sis_id
                                                                                   , gru_id
                                                                                   , url
                                                                                   , out entModulo);

            return entity.grupoPermissao;
        }

        #region Dias uteis calendario

        /// <summary>
        /// Calcula o número de dias úteis entre duas datas.
        /// </summary>
        /// <param name="dataInicial">Data inicial</param>
        /// <param name="dataFinal">Data final</param>
        /// <returns>Número de dias úteis</returns>
        public static int NumeroDeDiasUteis(int cal_id, int esc_id, int uni_id, Guid ent_id, bool atividadeDiversificada = false)
        {
            ACA_CalendarioAnual calendario = new ACA_CalendarioAnual { cal_id = cal_id };
            ACA_CalendarioAnualBO.GetEntity(calendario);
            DateTime dataInicial = calendario.cal_dataInicio;
            DateTime dataFinal = calendario.cal_dataFim;

            int totalDiasLetivos = 0;
            int dias = dataFinal.Subtract(dataInicial).Days;
            dias++;

            List<ACA_EventoBO.EventoPeriodoCalendario> eventos = ACA_EventoBO.SelecionaPorCalendarioEscolaTipoEvento(cal_id.ToString(), esc_id, uni_id, -1, null, false);

            // Setar a cidade pelo endereço da Entidade do usuário logado.
            Guid ene_id = SYS_EntidadeEnderecoBO.Select_ene_idBy_ent_id(ent_id);
            SYS_EntidadeEndereco entEndereco = new SYS_EntidadeEndereco
            {
                ent_id = ent_id
                ,
                ene_id = ene_id
            };
            SYS_EntidadeEnderecoBO.GetEntity(entEndereco);

            // Recuperando entidade Endereço do usuário logado.
            END_Endereco endereco = new END_Endereco
            {
                end_id = entEndereco.end_id
            };
            END_EnderecoBO.GetEntity(endereco);
            END_Cidade cidade = END_CidadeBO.GetEntity(new END_Cidade { cid_id = endereco.cid_id });

            List<SYS_DiaNaoUtil> diasNaoUteis = SYS_DiaNaoUtilBO.GetSelect().Where(p => p.dnu_situacao != 3).ToList();

            for (int i = 1; i <= dias; i++)
            {
                bool diaUtil = dataInicial.DayOfWeek != DayOfWeek.Sunday && dataInicial.DayOfWeek != DayOfWeek.Saturday && !DiaDeEvento(dataInicial, eventos) && !DiaNaoUtil(dataInicial, diasNaoUteis, cidade);

                //Caso o dia não seria util mas como tem uma atividade discente cadastrada, ele passa a ser util
                if (atividadeDiversificada && !diaUtil)
                    diaUtil = DiaAtividadeDiversificada(dataInicial, eventos, ent_id);

                //Conta apenas os dias da semana que não são nem dia de evento nem dia não útil
                if (diaUtil)
                {
                    totalDiasLetivos++;
                }

                dataInicial = dataInicial.AddDays(1);
            }
            return totalDiasLetivos;
        }

        /// <summary>
        /// Verifica se o dia pertence ao período de algum evento
        /// </summary>
        /// <param name="dia">Dia do ano</param>
        /// <returns>Verdadeiro se pertence a algum evento.</returns>
        private static bool DiaDeEvento(DateTime dia, List<ACA_EventoBO.EventoPeriodoCalendario> eventos)
        {
            foreach (ACA_EventoBO.EventoPeriodoCalendario evento in eventos)
            {
                if (evento.entityEvento.evt_semAtividadeDiscente)
                {
                    if (evento.entityEvento.evt_dataInicio <= dia && dia <= evento.entityEvento.evt_dataFim)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Verifica se o dia ie marcado como dia não útil.
        /// </summary>
        /// <param name="dia">Dia do ano</param>
        /// <returns>Verdadeiro se é marcado como não útil.</returns>
        private static bool DiaNaoUtil(DateTime dia, List<SYS_DiaNaoUtil> diasNaoUteis, END_Cidade cidade)
        {
            foreach (SYS_DiaNaoUtil li in diasNaoUteis)
            {
                if ((li.dnu_data.Day == dia.Date.Day
                    && li.dnu_data.Month == dia.Date.Month
                    && (li.dnu_data.Year == dia.Date.Year
                        || (li.dnu_recorrencia
                            && li.dnu_vigenciaInicio <= DateTime.Now
                            && (DateTime.Now <= li.dnu_vigenciaFim || li.dnu_vigenciaFim == new DateTime())
                            )
                         )
                    )
                    && (li.unf_id == Guid.Empty || cidade.unf_id == li.unf_id)
                    )
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Verifica se a data esta dentro de um evento de atividade diversificada
        /// </summary>
        /// <param name="dia">Dia do ano</param>
        /// <returns>Verdadeiro se ele pertencer a um evento de atividade diversificada.</returns>
        private static bool DiaAtividadeDiversificada(DateTime dia, List<ACA_EventoBO.EventoPeriodoCalendario> eventos, Guid ent_id)
        {
            int parametroAtivDiversificada = ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_ATIVIDADE_DIVERSIFICADA, ent_id);

            return (
                    from ACA_EventoBO.EventoPeriodoCalendario e in eventos
                    where
                        e.entityEvento.evt_dataInicio.Date <= dia && dia <= e.entityEvento.evt_dataFim.Date
                        //Possui atividade discente
                        && !e.entityEvento.evt_semAtividadeDiscente
                        //For atividade diversificada
                        && e.entityEvento.tev_id == parametroAtivDiversificada
                    select new
                    {
                        evt_dataInicio = e.entityEvento.evt_dataInicio,
                        evt_dataFim = e.entityEvento.evt_dataFim
                    }
                ).Any();
        }

        #endregion Dias uteis calendario


        public static string CarregarMenu(int sis_id, Guid gru_id, int vis_id, int appMinutosCache = 60)
        {

            string menuXML = "";

            Func<string> retorno = delegate ()
            {
                menuXML = SYS_ModuloBO.CarregarMenuXML(gru_id, sis_id, vis_id, 0);

                return menuXML;
            };

            if (appMinutosCache > 0)
            {
                string chave = String.Format(ModelCache.MENU_SISTEMA_GRUPO_VISAO_MODEL_KEY, sis_id, gru_id, vis_id);

                menuXML = CacheManager.Factory.Get
                            (
                                chave,
                                retorno,
                                appMinutosCache
                            );
            }
            else
            {
                menuXML = retorno();
            }

            return menuXML;
        }
    }

    /// <summary>
    /// Classe para implementar String para os itens do enumerador.
    /// Exemplo de utilização:
    ///
    /// public enum ChavesParametroServico
    /// {
    ///       [StringValue("ENTIDADE_RESP_SISTEMA")]
    ///       EntResponsavelSistema = 1
    ///       [StringValue("PAR_GRUPO_PERFIL_CLIENTE")]
    ///       , ParamGrupoPerfilCliente = 2
    /// }
    ///
    /// Para recuperar a Chave:
    /// string chave = StringValueAttribute.GetStringValue(ChavesParametroServico.EntResponsavelSistema);
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        private string _value;
        private string _description;

        /// <summary>
        /// Passar a string com o valor relacionado ao item do enumerador e a descrição
        /// do item do enumerador.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public StringValueAttribute(string value, string description)
        {
            _value = value;
            _description = description;
        }

        /// <summary>
        /// Passar a String com o valor relacionado ao item do enumerador.
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Valor Chave do item do enumerador.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Descrição do item do enumerador.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Retorna o Value setado para o enumerador passado por parâmetro.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(Enum value)
        {
            string output = "";

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs =
               fi.GetCustomAttributes(typeof(StringValueAttribute),
                                       false) as StringValueAttribute[];

            if ((attrs != null) && (attrs.Length > 0))
            {
                output = attrs[0].Value;
            }

            return output;
        }

        /// <summary>
        /// Retorna a descrição setada para o enumerador passado.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringDescription(Enum value)
        {
            string output = "";

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            DescriptionAttribute[] attrs = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attrs != null) && (attrs.Length > 0))
            {
                output = attrs[0].Description;
            }

            return output;
        }

        /// <summary>
        /// Retorna o valor e a descrição setadas para o enumerador passado.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>KeyValuePair: Key = StringValue e Value = StringDecription do enumerador.</returns>
        public static KeyValuePair<string, string> GetStringValueDescription(Enum value)
        {
            KeyValuePair<string, string> output = new KeyValuePair<string, string>();

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs =
               fi.GetCustomAttributes(typeof(StringValueAttribute),
                                       false) as StringValueAttribute[];

            if ((attrs != null) && (attrs.Length > 0))
            {
                output = new KeyValuePair<string, string>(attrs[0].Value, attrs[0].Description);
            }

            return output;
        }
    }

    /// <summary>
    /// Classe para compactar e descompactar conteúdos binários.
    /// </summary>
    public static class Compressor
    {
        /// <summary>
        /// Retorna os dados compactados. Utiliza a compactação GZip.
        /// </summary>
        /// <param name="data">Dados para compactar</param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true);
            zs.Write(data, 0, data.Length);
            zs.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Retorna os dados descompactados. Utiliza a compactação GZip.
        /// </summary>
        /// <param name="data">Dados para descompactar</param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zs = new GZipStream(new MemoryStream(data),
                                           CompressionMode.Decompress, true);
            byte[] buffer = new byte[4096];
            int size;
            while (true)
            {
                size = zs.Read(buffer, 0, buffer.Length);
                if (size > 0)
                    ms.Write(buffer, 0, size);
                else break;
            }
            zs.Close();
            return ms.ToArray();
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        private readonly int order;
        public OrderAttribute([CallerLineNumber]int order = 0)
        {
            this.order = order;
        }

        public int Order { get { return this.order; } }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DBNullValueAttribute : Attribute
    {
        private readonly string nullValue;
        public DBNullValueAttribute(Type type)
        {
            if (type == typeof(DateTime))
            {
                this.nullValue = new DateTime().ToString();
            }

            if (type == typeof(string))
            {
                this.nullValue = string.Empty;
            }

            if (type == typeof(long) || type == typeof(int))
            {
                this.nullValue = "-1";
            }
        }

        public string NullValue { get { return this.nullValue; } }
    }

    public abstract class TipoTabela
    {
        public TipoTabela(Abstract_Entity entity)
        {
            Type tp = GetType();

            var properties = from property in tp.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             let orderAttribute = property.GetCustomAttributes(typeof(OrderAttribute), false).SingleOrDefault() as OrderAttribute
                             where orderAttribute != null
                             orderby orderAttribute.Order
                             select property;

            foreach (PropertyInfo prop in properties)
            {
                PropertyInfo propClone = entity.GetType().GetProperty(prop.Name);

                if (propClone != null && propClone.CanWrite)
                {
                    var value = propClone.GetValue(entity, null);

                    if (value != null)
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            if (value.GetType() == typeof(DateTime))
                            {
                                DateTime date = Convert.ToDateTime(value);
                                prop.SetValue(this, date.ToString(DateUtil.DATA_HORA_SEGUNDOS_MILIS), null);
                            }
                            else
                            {
                                prop.SetValue(this, value.ToString(), null);
                            }
                        }
                        else
                        {
                            prop.SetValue(this, value, null);
                        }
                    }
                }
            }
        }

        public DataRow ToDataRow()
        {
            var properties = GetType().GetProperties();
            var dt = new DataTable();

            foreach (var property in properties)
                dt.Columns.Add(property.Name, property.PropertyType);

            DataRow drRetorno = dt.NewRow();

            DataColumnCollection columnList = dt.Columns;

            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataColumn coluna in columnList)
            {
                PropertyInfo prop = properties.ToList().Find(p => p.Name == coluna.ColumnName);
                var emptyValueAttribute = prop.GetCustomAttributes(typeof(DBNullValueAttribute), false).SingleOrDefault() as DBNullValueAttribute;

                if (prop != null)
                {

                    if (emptyValueAttribute != null && (prop.GetValue(this) ?? string.Empty).ToString() == emptyValueAttribute.NullValue)
                    {
                        drRetorno[coluna] = DBNull.Value;
                    }
                    else
                    {
                        drRetorno[coluna] = prop.GetValue(this);
                    }
                }

                FieldInfo field = fields.ToList().Find(p => p.Name == coluna.ColumnName);
                if (field != null)
                {
                    if (emptyValueAttribute != null && (field.GetValue(this) ?? string.Empty).ToString() == emptyValueAttribute.NullValue)
                    {
                        drRetorno[coluna] = DBNull.Value;
                    }
                    else
                    {
                        drRetorno[coluna] = field.GetValue(this);
                    }
                }
            }

            return drRetorno;
        }
    }

}