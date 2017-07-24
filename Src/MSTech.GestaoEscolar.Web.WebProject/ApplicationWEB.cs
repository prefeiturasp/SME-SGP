using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using MSQuartz.Core;
using MSQuartz.Core.SchedulerProviders;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Log;
using MSTech.Web.Mail;


namespace MSTech.GestaoEscolar.Web.WebProject
{
    public class ApplicationWEB : MSTech.Web.WebProject.ApplicationWEB, IRequiresSessionState
    {
        #region Constantes

        public const string AppSistemaID = "appSistemaID";
        public const string AppReportURL = "appReportURL";
        public const string AppReportPath = "appReportPath";
        public const string AppReportUserName = "appReportUserName";
        public const string AppReportPwd = "appReportPwd";
        public const string AppReportDomain = "appReportDomain";
        public const string AppPathLogoRelatorio = "appPathLogoRelatorio";
        public const string AppCalendarioCorInicioAno = "appCalendarioCorInicioAno";
        public const string AppCalendarioCorPeriodosEventos = "appCalendarioCorPeriodosEventos";
        public const string AppCalendarioCorDiasNaoUteis = "appCalendarioCorDiasNaoUteis";
        public const string AppCalendarioCorDiasSemAtividadeDiscente = "appCalendarioCorDiasSemAtividadeDiscente";
        public const string AppJustificativaFaltaAbona = "AppJustificativaFaltaAbona";
        public const string AppJustificativaFaltaNaoAbona = "AppJustificativaFaltaNaoAbona";
        public const string AppMigracaoRioResponsabilidadeSME = "AppMigracaoRioResponsabilidadeSME";
        public const string AppMigracaoRioResponsabilidadeCRE = "AppMigracaoRioResponsabilidadeCRE";
        public const string AppAlunoAusente = "AppAlunoAusente";
        public const string AppAlunoInativo = "AppAlunoInativo";
        public const string AppAlunoExcedente = "AppAlunoExcedente";
        public const string AppAlunoPreMatricula = "AppAlunoPreMatricula";
        public const string AppAlunoIntencaoTransferencia = "AppAlunoIntencaoTransferencia";
        public const string AppCorAlunoNaoAvaliado = "AppCorAlunoNaoAvaliado";
        public const string AppEnturmacao = "AppEnturmacao";
        public const string AppEnturmacaoPendencia = "AppEnturmacaoPendencia";
        public const string AppSchedulerHost = "AppSchedulerHost";
        public const string AppLiberarAlteracaoTotalAdmin = "AppLiberarAlteracaoTotalAdmin";
        public const string AppAreaAlunoSistemaID = "appAreaAlunoSistemaID";
        public const string AppAPI_UserName = "appAPI_UserName";
        public const string AppAPI_Password = "appAPI_Password";
        public const string AppOrientacaoCurricularPlanejada = "AppOrientacaoCurricularPlanejada";
        public const string AppOrientacaoCurricularTrabalhada = "AppOrientacaoCurricularTrabalhada";
        public const string AppAlunoDispensado = "AppAlunoDispensado";
        public const string AppRedirecionaAutomaticoSistema = "appRedirecionaAutomaticoSistema";
        public const string AppLoginProprioDoSistema = "AppLoginProprioDoSistema";
        public const string AppAlunoFrequenciaLimite = "AppAlunoFrequenciaLimite";
        private const string AppHomeDocente = "appHomeDocente";
        private const string AppHomeGestor = "appHomeGestor";
        private const string appMinutosCacheLongoGeral = "appMinutosCacheLongoGeral";
        private const string appMinutosCacheLongo = "appMinutosCacheLongo";
        private const string appMinutosCacheMedio = "appMinutosCacheMedio";
        private const string appMinutosCacheCurto = "appMinutosCacheCurto";
        private const string appMinutosCacheFechamento = "appMinutosCacheFechamento";
        private const string appSincronizarPlanejamentoTablet = "AppSincronizarPlanejamentoTablet";
        private const string appUtilizarIntegracaoADUsuario = "appUtilizarIntegracaoADUsuario";
        private const string PluginNotificacoes = "PluginNotificacoes";
        private const string AppCorNotaPosConselho = "AppCorNotaPosConselho";
        private const string AppCorAlunoForaDaRede = "AppCorAlunoForaDaRede";
        private const string AppCorDocenciaCompartilhada = "AppCorDocenciaCompartilhada";
        private const string AppExibirAnosAnterioresDocente = "AppExibirAnosAnterioresDocente";
        private const string AppExternalUrlReport = "AppExternalUrlReport";
        private const string AppCorAlunoProximoBaixaFrequencia = "AppCorAlunoProximoBaixaFrequencia";
        private const string AppCorPendenciaDisciplina = "AppCorPendenciaDisciplina";
        private const string AppCorBordaFrequenciaAbaixoMinimo = "AppCorBordaFrequenciaAbaixoMinimo";
        private const string wsCarteiraVacinacaoUserName = "wsCarteiraVacinacaoUserName";
        private const string wsCarteiraVacinacaoPassword = "wsCarteiraVacinacaoPassword";
        private const string wsCarteiraVacinacaoMunicipio = "wsCarteiraVacinacaoMunicipio";

        public const string TextoAsteriscoObrigatorio = "<span class=\"asteriscoObrigatorio\">*</span>";

        /// <summary>
        /// Constante com o valor da chave que armazena o valor do tamanho máximo permitido upload de arquivo.
        /// </summary>
        public const string AppTamanhoMaximoArquivo = "AppTamanhoMaximoArquivo";

        /// <summary>
        /// Constante com o valor Default em KB, para caso não esteja configurado no sistema.
        /// </summary>
        private const int TamanhoPadraoArquivo = 1024;

        /// <summary>
        /// Constante com o valor da chave que armazena lista dos arquivos permitidos para upload.
        /// </summary>
        public const string AppTiposArquivosPermitidos = "AppTiposArquivosPermitidos";

        /// <summary>
        /// Constante com o valor Default(Separados por ",") dos arquivos permitidos.
        /// </summary>
        private const string ArquivosPermitidosPadrao = ".doc,.docx,.pdf,.txt,.exp";

        /// <summary>
        /// Quantidade máxima de registro no arquivo de lote
        /// </summary>
        private const string AppQtdMaxRegistroArquivoLote = "AppQtdMaxRegistroArquivoLote";

        /// <summary>
        /// Constante com o valor Default(Separados por ",") das imagens permitidos.
        /// </summary>
        private const string ImagensPermitidasPadrao = ".png,.jpg,.jpeg";

        /// <summary>
        /// Constante com o valor da chave que armazena lista dos tipos de imagens permitidos para upload.
        /// </summary>
        public const string AppTiposImagensPermitidas = "AppTiposImagensPermitidas";

        /// <summary>
        /// Constante com o valor da chave que armazena o caminho/nome da imagem utilizada como logo nos relatórios do SSRS.
        /// </summary>
        public const string AppLogoRelatorioSSRS = "AppLogoRelatorioSSRS";

        /// <summary>
        /// Constante com o valor da chave que armazena o id da imagem utilizada como logo nos relatórios web.(arq_id).
        /// </summary>
        public const string AppLogoRelatorioDB = "AppLogoRelatorioDB";

        #endregion Constantes

        #region Propriedades

        protected new SessionWEB __SessionWEB
        {
            get { return ((SessionWEB)Session[SessSessionWEB]); }
            set { Session[SessSessionWEB] = value; }
        }

        /// <summary>
        /// Retorna o id da primeira entidade vinculada com o sistema.
        /// Caso não tenha encontrado nada, retorna um Guid.Empty
        /// </summary>
        public static Guid _EntidadeID
        {
            get
            {
                IList<SYS_SistemaEntidade> listaSistemaEntidade = SYS_SistemaEntidadeBO.GetSelect();

                if (listaSistemaEntidade.Count > 0)
                    return listaSistemaEntidade[0].ent_id;

                return Guid.Empty;
            }
        }

        /// <summary>
        /// Configuração do sistema corrente
        /// </summary>
        public static eConfig Config
        {
            get
            {
                return (eConfig)HttpContext.Current.Application["eConfig"];
            }
            set
            {
                try
                {
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application["eConfig"] = value;
                }
                finally
                {
                    HttpContext.Current.Application.UnLock();
                }
            }
        }

        /// <summary>
        /// Retorna as configurações do sistema
        /// </summary>
        public static IDictionary<string, ICFG_Configuracao> Configuracoes
        {
            get
            {
                IDictionary<string, ICFG_Configuracao> configuracoes = (IDictionary<string, ICFG_Configuracao>)HttpContext.Current.Application["Config"];

                if ((configuracoes == null) || (configuracoes.Count == 0))
                {
                    BLL.CFG_ConfiguracaoBO.Consultar(Config, out configuracoes);
                    try
                    {
                        HttpContext.Current.Application.Lock();
                        HttpContext.Current.Application["Config"] = configuracoes;
                    }
                    finally
                    {
                        HttpContext.Current.Application.UnLock();
                    }
                }
                return configuracoes;
            }
        }

        /// <summary>
        ///  ID do Sistema
        /// </summary>
        public static int SistemaID
        {
            get { return Convert.ToInt32(Configuracoes[AppSistemaID].cfg_valor); }
        }

        /// <summary>
        ///  ID do Sistema de área do aluno
        /// </summary>
        public static int AreaAlunoSistemaID
        {
            get
            {
                if (Configuracoes.ContainsKey(AppAreaAlunoSistemaID) &&
                    !string.IsNullOrEmpty(Configuracoes[AppAreaAlunoSistemaID].cfg_valor))
                    return Convert.ToInt32(Configuracoes[AppAreaAlunoSistemaID].cfg_valor);
                return -1;
            }
        }

        /// <summary>
        /// Título das Páginas
        /// </summary>
        public new static string _TituloDasPaginas
        {
            get { return Configuracoes[AppTitle].cfg_valor; }
        }

        /// <summary>
        /// Host servidor de email a ser utilizado pelo sistema
        /// </summary>
        public new static string _EmailHost
        {
            get { return Configuracoes[AppEmailHost].cfg_valor; }
        }

        /// <summary>
        ///  Endereço de Email para suporte
        /// </summary>
        public new static string _EmailSuporte
        {
            get { return Configuracoes[AppEmailSuporte].cfg_valor; }
        }

        /// <summary>
        /// Paginação Padrão
        /// </summary>
        public new static int _Paginacao
        {
            get { return Convert.ToInt32(Configuracoes[AppPaginacao].cfg_valor); }
        }

        /// <summary>
        /// Página para se redirecionar quando houver erros no sistema
        /// </summary>
        public new static string _PaginaErro
        {
            get { return Configuracoes[AppPaginaErro].cfg_valor; }
        }

        /// <summary>
        /// Página para se redirecionar quando expirar a sessão do usuário
        /// </summary>
        public new static string _PaginaExpira
        {
            get { return Configuracoes[AppPaginaExpira].cfg_valor; }
        }

        /// <summary>
        /// Página de entrada do sistema
        /// </summary>
        public new static string _PaginaInicial
        {
            get { return Configuracoes[AppPaginaInicial].cfg_valor; }
        }

        /// <summary>
        /// Página para se redirecionar quando expirar a sessão do usuário
        /// </summary>
        public new static string _PaginaLogoff
        {
            get { return Configuracoes[AppPaginaLogOff].cfg_valor; }
        }

        /// <summary>
        ///
        /// </summary>
        public static string ReportURL
        {
            get { return Configuracoes[AppReportURL].cfg_valor; }
        }

        /// <summary>
        ///
        /// </summary>
        public static string ReportPath
        {
            get { return Configuracoes[AppReportPath].cfg_valor; }
        }

        /// <summary>
        /// Gets the name of the report user.
        /// </summary>
        /// <value>
        /// The name of the report user.
        /// </value>
        public static string ReportUserName
        {
            get { return Configuracoes[AppReportUserName].cfg_valor; }
        }

        /// <summary>
        ///
        /// </summary>
        public static string ReportPwd
        {
            get { return Configuracoes[AppReportPwd].cfg_valor; }
        }

        /// <summary>
        ///
        /// </summary>
        public static string ReportDomain
        {
            get { return Configuracoes[AppReportDomain].cfg_valor; }
        }

        /// <summary>
        /// Retorna o tamanho máximo permitido do arquivo em KB
        /// </summary>
        public static int TamanhoMaximoArquivo
        {
            get
            {
                int tamanho = -1;
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(AppTamanhoMaximoArquivo, out cfg);
                if (cfg != null)
                {
                    int.TryParse(cfg.cfg_valor, out tamanho);
                }
                if (tamanho < 1)
                {
                    tamanho = TamanhoPadraoArquivo;
                }

                return tamanho;
            }
        }

        /// <summary>
        /// Retorna a quantidade máxima de registro no arquivo de lote
        /// </summary>
        public static int QtdMaxRegistroArquivoLote
        {
            get
            {
                int tamanho = -1;
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(AppQtdMaxRegistroArquivoLote, out cfg);

                if (cfg != null)
                {
                    int.TryParse(cfg.cfg_valor, out tamanho);
                }
                if (tamanho <= 0)
                {
                    tamanho = 5000;
                }

                return tamanho;
            }
        }

        /// <summary>
        /// Retorna array com as extensões permitidas para upload de arquivos
        /// </summary>
        public static string[] TiposArquivosPermitidos
        {
            get
            {
                string arquivos = "";
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(AppTiposArquivosPermitidos, out cfg);
                if (cfg != null)
                {
                    arquivos = cfg.cfg_valor;
                }
                if (String.IsNullOrEmpty(arquivos))
                {
                    arquivos = ArquivosPermitidosPadrao;
                }
                return arquivos.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// Retorna array com as extensões permitidas para upload de imagens
        /// </summary>
        public static string[] TipoImagensPermitidas
        {
            get
            {
                string imagens = "";
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(AppTiposImagensPermitidas, out cfg);
                if (cfg != null)
                {
                    imagens = cfg.cfg_valor;
                }
                if (String.IsNullOrEmpty(imagens))
                {
                    imagens = ImagensPermitidasPadrao;
                }
                return imagens.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string PathLogoRelatorio
        {
            get { return Configuracoes[AppPathLogoRelatorio].cfg_valor; }
        }

        /// <summary>
        /// Cor padrão do calendário para indicar início e fim do ano letivo.
        /// </summary>
        public static string CalendarioCorInicioAnoLetivo
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCalendarioCorInicioAno))
                    return Configuracoes[AppCalendarioCorInicioAno].cfg_valor;

                return "#ff0000";
            }
        }

        /// <summary>
        /// Cor padrão do calendário para indicar períodos e eventos.
        /// </summary>
        public static string CalendarioCorPeriodosEventos
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCalendarioCorPeriodosEventos))
                    return Configuracoes[AppCalendarioCorPeriodosEventos].cfg_valor;

                return "#0000ff";
            }
        }

        /// <summary>
        /// Cor padrão do calendário para indicar dias não úteis.
        /// </summary>
        public static string CalendarioCorDiasNaoUteis
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCalendarioCorDiasNaoUteis))
                    return Configuracoes[AppCalendarioCorDiasNaoUteis].cfg_valor;

                return "#ff8000";
            }
        }

        /// <summary>
        /// Cor padrão do calendário para indicar dias sem atividade discente
        /// </summary>
        public static string CalendarioCorDiasSemAtividadeDiscente
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCalendarioCorDiasSemAtividadeDiscente))
                    return Configuracoes[AppCalendarioCorDiasSemAtividadeDiscente].cfg_valor;

                return "#D0D0D0";
            }
        }

        /// <summary>
        /// Cor padrão da frequência para indicar justificativa de falta que abona falta
        /// </summary>
        public static string JustificativaFaltaAbona
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppJustificativaFaltaAbona))
                    return Configuracoes[AppJustificativaFaltaAbona].cfg_valor;

                return "#EE3B3B";
            }
        }

        /// <summary>
        /// Cor padrão da frequência para indicar justificativa de falta que não abona falta
        /// </summary>
        public static string JustificativaFaltaNaoAbona
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppJustificativaFaltaNaoAbona))
                    return Configuracoes[AppJustificativaFaltaNaoAbona].cfg_valor;

                return "#FFFF00";
            }
        }

        /// <summary>
        /// Cor padrão para indicar responsabilidade de nível SME para correção de duplicidade
        /// </summary>
        public static string MigracaoRioResponsabilidadeSME
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppMigracaoRioResponsabilidadeSME))
                    return Configuracoes[AppMigracaoRioResponsabilidadeSME].cfg_valor;

                return "#F0BCB9";
            }
        }

        /// <summary>
        /// Cor padrão para indicar responsabilidade de nível CRE para correção de duplicidade
        /// </summary>
        public static string MigracaoRioResponsabilidadeCRE
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppMigracaoRioResponsabilidadeCRE))
                    return Configuracoes[AppMigracaoRioResponsabilidadeCRE].cfg_valor;

                return "#CEFF95";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o registro processado com sucesso
        /// </summary>
        public static string ProcessadoComSucesso
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoAusente))
                    return Configuracoes[AppAlunoAusente].cfg_valor;

                return "#CCFFCC";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o registro processado com erro
        /// </summary>
        public static string ProcessadoComErro
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoInativo))
                    return Configuracoes[AppAlunoInativo].cfg_valor;

                return "#FFE1CD";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno ausente
        /// </summary>
        public static string AlunoAusente
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoAusente))
                    return Configuracoes[AppAlunoAusente].cfg_valor;

                return "#FF7F24";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno ausente
        /// </summary>
        public static string AlunoDispensado
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoDispensado))
                    return Configuracoes[AppAlunoDispensado].cfg_valor;

                return "#FF7F24";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno inativo
        /// </summary>
        public static string AlunoInativo
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoInativo))
                    return Configuracoes[AppAlunoInativo].cfg_valor;

                return "#FFE1CD";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno excedente
        /// </summary>
        public static string AlunoExcedente
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoExcedente))
                    return Configuracoes[AppAlunoExcedente].cfg_valor;

                return "#FFE1AC";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno em pré-matrícula
        /// </summary>
        public static string AlunoEmPreMatricula
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoPreMatricula))
                    return Configuracoes[AppAlunoPreMatricula].cfg_valor;

                return "#D1E0FF";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno com intenção de transferência
        /// </summary>
        public static string AlunoIntencaoTransferencia
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoIntencaoTransferencia))
                    return Configuracoes[AppAlunoIntencaoTransferencia].cfg_valor;

                return "#FFE143";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno excedente
        /// </summary>
        public static string CorAlunoNaoAvaliado
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCorAlunoNaoAvaliado))
                    return Configuracoes[AppCorAlunoNaoAvaliado].cfg_valor;

                return "#E5E05F";
            }
        }

        /// <summary>
        /// Cor padrão para indicar o aluno com frequencia final abaixo do limite
        /// </summary>
        public static string AlunoFrequenciaLimite
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAlunoFrequenciaLimite))
                    return Configuracoes[AppAlunoFrequenciaLimite].cfg_valor;

                return "#fd6559"; //anterior vermelho "#FA3440";
            }
        }

        /// <summary>
        /// Tela inicial para a visão do docente.
        /// </summary>
        public static string HomeDocente
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppHomeDocente))
                    return Configuracoes[AppHomeDocente].cfg_valor;

                return "";
            }
        }

        /// <summary>
        /// Tela inicial para a visão do gestor.
        /// </summary>
        public static string HomeGestor
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppHomeGestor))
                    return Configuracoes[AppHomeGestor].cfg_valor;

                return "";
            }
        }

        /// <summary>
        /// Cor padrão para indicar que o aluno não se enquadrou em nenhuma turma
        /// </summary>
        public static string Enturmacao
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppEnturmacao))
                    return Configuracoes[AppEnturmacao].cfg_valor;

                return "#FFE1CD";
            }
        }

        /// <summary>
        /// Cor padrão para indicar que o aluno possui pendências no sistema
        /// </summary>
        public static string EnturmacaoPendencias
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppEnturmacaoPendencia))
                    return Configuracoes[AppEnturmacaoPendencia].cfg_valor;

                return "#E6DFFF";
            }
        }

        /// <summary>
        /// Host para conectar no SchedulerProvider.
        /// </summary>
        public static string SchedulerHost
        {
            get
            {
                string url = "";
                if (Configuracoes.Keys.Contains(AppSchedulerHost))
                    url = Configuracoes[AppSchedulerHost].cfg_valor;
                return url;
            }
        }

        /// <summary>
        /// Indica se o usuário admin pode alterar qualquer coisa no cadastro de curso
        /// </summary>
        public static bool LiberarAlteracaoTotalAdmin
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppLiberarAlteracaoTotalAdmin))
                {
                    return string.IsNullOrEmpty(Configuracoes[AppLiberarAlteracaoTotalAdmin].cfg_valor) ? false : Convert.ToBoolean(Configuracoes[AppLiberarAlteracaoTotalAdmin].cfg_valor);
                }

                return false;
            }
        }

        /// <summary>
        /// Interface para acesso aos dados e operações do scheduler do Quartz
        /// </summary>
        public static ISchedulerDataProvider SchedulerDataProvider
        {
            get
            {
                return new DefaultSchedulerDataProvider(SchedulerProvider);
            }
        }

        /// <summary>
        /// Interface para acesso direto ao scheduler do Quartz utilizado na inicializado do serviço.
        /// </summary>
        private static ISchedulerProvider _schedulerProvider;

        public static ISchedulerProvider SchedulerProvider
        {
            get
            {
                if (_schedulerProvider == null)
                {
                    RemoteSchedulerProvider rm = new RemoteSchedulerProvider(SchedulerHost);
                    _schedulerProvider = rm;
                }

                return _schedulerProvider;
            }
        }

        /// <summary>
        /// Retorna o usuário configurado para autenticação da WebApi.
        /// </summary>
        public static string Api_UserName
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAPI_UserName))
                {
                    return Configuracoes[AppAPI_UserName].cfg_valor;
                }

                return "";
            }
        }

        /// <summary>
        /// Retorna a senha configurada para autenticação da WebApi.
        /// </summary>
        public static string Api_Password
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppAPI_Password))
                {
                    return Configuracoes[AppAPI_Password].cfg_valor;
                }

                return "";
            }
        }

        /// <summary>
        /// Cor padrão para indicar a orientação curricular planejada em bimestres anteriores.
        /// </summary>
        public static string OrientacaoCurricularPlanejada
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppOrientacaoCurricularPlanejada))
                    return Configuracoes[AppOrientacaoCurricularPlanejada].cfg_valor;

                return "#7ccced";
            }
        }

        /// <summary>
        /// Cor padrão para indicar a orientação curricular trabalhada em bimestres anteriores.
        /// </summary>
        public static string OrientacaoCurricularTrabalhada
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppOrientacaoCurricularTrabalhada))
                    return Configuracoes[AppOrientacaoCurricularTrabalhada].cfg_valor;

                return "#8dc63f";
            }
        }

        /// <summary>
        /// Retorna os parâmetros de mensagem
        /// </summary>
        public static List<CFG_ParametroMensagem> ParametrosMSG
        {
            get
            {
                List<CFG_ParametroMensagem> paramMSG = (List<CFG_ParametroMensagem>)HttpContext.Current.Application["ParamMSG"];
                if ((paramMSG == null) || (paramMSG.Count == 0))
                {
                    paramMSG = (List<CFG_ParametroMensagem>)BLL.CFG_ParametroMensagemBO.GetSelect();
                    try
                    {
                        HttpContext.Current.Application.Lock();
                        HttpContext.Current.Application["ParamMSG"] = paramMSG;
                    }
                    finally
                    {
                        HttpContext.Current.Application.UnLock();
                    }
                }
                return paramMSG;
            }
        }

        /// <summary>
        /// Url de acesso externo do boletim online (área do aluno).
        /// </summary>
        public static string UrlAcessoExternoBoletimOnline
        {
            get
            {
                if (Configuracoes.Keys.Contains("Url_ExternaBoletimOnline"))
                    return Configuracoes["Url_ExternaBoletimOnline"].cfg_valor;

                return "";
            }
        }

        /// <summary>
        /// Url de acesso externo às APIs do Gestão Escolar
        /// </summary>
        public static string UrlGestaoAcademicaWebApi
        {
            get
            {
                if (Configuracoes.Keys.Contains("UrlGestaoAcademicaWebApi"))
                    return Configuracoes["UrlGestaoAcademicaWebApi"].cfg_valor;

                return string.Empty;
            }
        }

        /// <summary>
        /// Url de acesso externo ao sistema administrativo (CoreSSO)
        /// </summary>
        public static string UrlCoreSSO
        {
            get
            {
                return SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.URL_ADMINISTRATIVO);
            }
        }

        /// <summary>
        /// Guarda em uma string as ordens dos currículos períodos nos quais os alunos terão
        /// acesso ao link externo do boletim online (padrão 7º, 8º, 9º anos).
        /// Guarda os valores separados por ",". Ex: "7,8,9".
        /// </summary>
        public static string[] Crp_ordem_AcessoExternoBoletimOnline
        {
            get
            {
                if (Configuracoes.Keys.Contains("Crp_ordem_AcessoExternoBoletimOnline"))
                {
                    string[] chaves = Configuracoes["Crp_ordem_AcessoExternoBoletimOnline"].cfg_valor.Split(',');
                    return chaves;
                }

                return new[] { "7", "8", "9" };
            }
        }

        /// <summary>
        /// Retorna a configuração que informa se é pra
        /// redirecionar automaticamente para o sistema ao logar.
        /// Caso esteja configurado como True, ao acessar a url do gestão,
        /// é passado por parâmetro o sis_id para o Core, e quando o login for feito,
        /// cairá automaticamente no Gestão, sem passar pela tela de sistema.
        /// </summary>
        public static bool RedirecionarAutomaticoSistema
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppRedirecionaAutomaticoSistema))
                    return Convert.ToBoolean(Configuracoes[AppRedirecionaAutomaticoSistema].cfg_valor);

                return false;
            }
        }

        /// <summary>
        /// Retorna a configuração que informa se vai realizar a sincronização das orientações curriculares na
        /// sincronização inicial do tablet (diário de classe).
        /// Valor padrão = true;
        /// </summary>
        public static bool SincronizarPlanejamentoTablet
        {
            get
            {
                if (Configuracoes.Keys.Contains(appSincronizarPlanejamentoTablet))
                    return Convert.ToBoolean(Configuracoes[appSincronizarPlanejamentoTablet].cfg_valor);

                return true;
            }
        }

        /// <summary>
        /// Retorna a configuração que informa se vai integrar os usuários com AD (significa que os usuários
        /// terão seus dados "replicados" para o AD).
        /// Valor padrão = false;
        /// </summary>
        public static bool UtilizarIntegracaoADUsuario
        {
            get
            {
                if (Configuracoes.Keys.Contains(appUtilizarIntegracaoADUsuario))
                    return Convert.ToBoolean(Configuracoes[appUtilizarIntegracaoADUsuario].cfg_valor);

                return false;
            }
        }

        /// <summary>
        /// Retorna a configuração que informa se vai exibir o plugin do notificações
        /// Valor padrão = false;
        /// </summary>
        public static bool LigarPluginNotificacoes
        {
            get
            {
                if (Configuracoes.Keys.Contains(PluginNotificacoes))
                    return Convert.ToBoolean(Configuracoes[PluginNotificacoes].cfg_valor);

                return false;
            }
        }

        /// <summary>
        /// Retorna a configuração que informa se o usuário logou do próprio sistema,
        /// redirecionando assim para a página de login do próprio sistema.
        /// </summary>
        public static bool LoginProprioDoSistema
        {
            get
            {
                bool loginProprioDoSistema;
                if (bool.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get(AppLoginProprioDoSistema), out loginProprioDoSistema))
                {
                    return loginProprioDoSistema;
                }

                return false;
            }
        }

        /// <summary>
        /// Retorna a quantidade de minutos da configuração de cache longo.
        /// Utilizado em metodos que podem ser armazenados em cache em todos os clientes.
        /// </summary>
        public static int AppMinutosCacheLongoGeral
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;

                int retorno = -1;
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(appMinutosCacheLongoGeral, out cfg);

                if (cfg != null)
                {
                    int.TryParse(cfg.cfg_valor, out retorno);
                }
                if (retorno <= 0)
                {
                    retorno = 0;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a quantidade de minutos da configuração de cache longo.
        /// </summary>
        public static int AppMinutosCacheLongo
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;

                int retorno = -1;
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(appMinutosCacheLongo, out cfg);

                if (cfg != null)
                {
                    int.TryParse(cfg.cfg_valor, out retorno);
                }
                if (retorno <= 0)
                {
                    retorno = 0;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a quantidade de minutos da configuração de cache médio.
        /// </summary>
        public static int AppMinutosCacheMedio
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;

                int retorno = -1;
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(appMinutosCacheMedio, out cfg);

                if (cfg != null)
                {
                    int.TryParse(cfg.cfg_valor, out retorno);
                }
                if (retorno <= 0)
                {
                    retorno = 0;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a quantidade de minutos da configuração de cache curto.
        /// </summary>
        public static int AppMinutosCacheCurto
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;

                int retorno = -1;
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(appMinutosCacheCurto, out cfg);

                if (cfg != null)
                {
                    int.TryParse(cfg.cfg_valor, out retorno);
                }
                if (retorno <= 0)
                {
                    retorno = 0;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Retorna a quantidade de minutos da configuração de cache para o fechamento.
        /// </summary>
        public static int AppMinutosCacheFechamento
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;

                int retorno = -1;
                ICFG_Configuracao cfg;
                Configuracoes.TryGetValue(appMinutosCacheFechamento, out cfg);

                if (cfg != null)
                {
                    int.TryParse(cfg.cfg_valor, out retorno);
                }

                return retorno <= 0 ? 0 : retorno;
            }
        }

        /// <summary>
        /// Retorna o nome do arquivo de resources global configurado para o cliente.
        /// </summary>
        public static string Nome_GlobalResourcesCliente
        {
            get
            {
                return "Mensagens";
            }
        }

        /// <summary>
        /// Cor para a nota/conceito atribuída/o pelo conselho de classe
        /// </summary>
        public static string CorNotaPosConselho
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCorNotaPosConselho))
                    return Configuracoes[AppCorNotaPosConselho].cfg_valor;

                return "#99D9EA";
            }
        }

        /// <summary>
        /// Cor para aluno com percentual próximo de atingir frequência mínima exigida.
        /// </summary>
        public static string CorAlunoProximoBaixaFrequencia
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCorAlunoProximoBaixaFrequencia))
                    return Configuracoes[AppCorAlunoProximoBaixaFrequencia].cfg_valor;

                return "#FFF200";
            }
        }

        /// <summary>
        /// Cor para aluno fora da rede
        /// </summary>
        public static string CorAlunoForaDaRede
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCorAlunoForaDaRede))
                    return Configuracoes[AppCorAlunoForaDaRede].cfg_valor;

                return "#C0C0C0";
            }
        }

        /// <summary>
        /// Cor para indicar pendência do aluno na disciplina.
        /// </summary>
        public static string CorPendenciaDisciplina
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCorPendenciaDisciplina))
                    return Configuracoes[AppCorPendenciaDisciplina].cfg_valor;

                return "#FFC90E";
            }
        }

        /// <summary>
        /// Cor da borda para frequência abaixo do limite.
        /// </summary>
        public static string CorBordaFrequenciaAbaixoMinimo
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCorBordaFrequenciaAbaixoMinimo))
                    return Configuracoes[AppCorBordaFrequenciaAbaixoMinimo].cfg_valor;

                return "#813B93";
            }
        }

        /// <summary>
        /// caminho/nome da imagem utilizada como logo nos relatórios do SSRS
        /// </summary>
        public static string LogoRelatorioSSRS
        {
            get { return Configuracoes[AppLogoRelatorioSSRS].cfg_valor; }
        }

        /// <summary>
        /// id da imagem utilizada como logo nos relatórios web.
        /// </summary>
        public static int LogoRelatorioDB
        {
            get { return Convert.ToInt32(Configuracoes[AppLogoRelatorioDB].cfg_valor); }
        }

        /// <summary>
        /// Cor para a nota/conceito atribuída/o pelo conselho de classe
        /// </summary>
        public static string CorDocenciaCompartilhada
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppCorDocenciaCompartilhada))
                    return Configuracoes[AppCorDocenciaCompartilhada].cfg_valor;

                return "#00A100";
            }
        }

        /// <summary>
        /// Retorna a configuração que informa se exibirá turmas de anos anteriores para o docente na tela do minhas turmas.
        /// </summary>
        public static bool ExibirTurmasAnosAnterioresDocente
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppExibirAnosAnterioresDocente))
                    return Convert.ToBoolean(Configuracoes[AppExibirAnosAnterioresDocente].cfg_valor);

                return false;
            }
        }

        /// <summary>
        /// Retorna a Url do site de relatório do Gestão que é utilizado no IFrame nas páginas:
        /// "~\Documentos\Relatorios.aspx" e "~\Relatorios\Relatorios.aspx".
        /// </summary>
        public static string ExternalUrlReport
        {
            get
            {
                if (Configuracoes.Keys.Contains(AppExternalUrlReport))
                {
                    var valorChave = Configuracoes[AppExternalUrlReport].cfg_valor;
                    if (!String.IsNullOrEmpty(valorChave))
                        return valorChave;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Login para o WebService da carteira de vacinação.
        /// </summary>
        public static string WsCarteiraVacinacaoUserName
        {
            get
            {
                if (Configuracoes.Keys.Contains(wsCarteiraVacinacaoUserName))
                    return Configuracoes[wsCarteiraVacinacaoUserName].cfg_valor;

                return "";
            }
        }

        /// <summary>
        /// Senha para o WebService da carteira de vacinação.
        /// </summary>
        public static string WsCarteiraVacinacaoPassword
        {
            get
            {
                if (Configuracoes.Keys.Contains(wsCarteiraVacinacaoPassword))
                    return Configuracoes[wsCarteiraVacinacaoPassword].cfg_valor;

                return "";
            }
        }

        /// <summary>
        /// Id do município para o WebService da carteira de vacinação.
        /// </summary>
        public static long WsCarteiraVacinacaoMunicipio
        {
            get
            {
                if (Configuracoes.Keys.Contains(wsCarteiraVacinacaoMunicipio))
                    return Convert.ToInt64(Configuracoes[wsCarteiraVacinacaoMunicipio].cfg_valor);

                return 0;
            }
        }

        #endregion Propriedades

        #region Propriedades - métodos assíncronos

        // Variáveis usadas para gravar erro
        private delegate void InvokerGravaErro(LOG_Erros entity, SYS_Sistema sistema);

        private static InvokerGravaErro methodErr;
        private static IAsyncResult resErr;

        // Variáveis usadas para gravar log de sistema
        private delegate void InvokerGravaLogSistema(LOG_Sistema entity, SYS_Sistema sistema);

        private static InvokerGravaLogSistema methodLog;
        private static IAsyncResult resLog;

        #endregion Propriedades - métodos assíncronos

        #region Metodos

        /// <summary>
        /// Expira a Sessão do usuário e redireciona para a pagina passada
        /// </summary>
        /// <param name="url">Página para redirecionar</param>
        public static void Expira(string url)
        {
            HttpContext.Current.Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();
            HttpContext.Current.Response.Redirect(url, true);
        }

        protected override void Session_Start(object sender, EventArgs e)
        {
            __SessionWEB = new SessionWEB();
        }


        /// <summary>
        /// Retorna a coleção em uma string única
        /// </summary>
        /// <param name="Colecao">Coleção de dados</param>
        /// <param name="nomeColecao">Nome da coleção</param>
        /// <param name="listaNaoGravar">Lista com os itens que não devem ser retornados na string</param>
        /// <returns>String única com os itens da coleção</returns>
        private static string retornaListaColecao(System.Collections.Specialized.NameValueCollection Colecao, string nomeColecao, List<string> listaNaoGravar)
        {
            string infoRequest = "\r\n*********** " + nomeColecao + " ***********";
            for (int i = 0; i < Colecao.Count; i++)
            {
                if (!(listaNaoGravar.Exists(p => p == Colecao.AllKeys[i])))
                {
                    infoRequest += "\r\n | ";
                    infoRequest += Colecao.AllKeys[i] + ": ";
                    infoRequest += Colecao[i];
                }
            }

            return infoRequest;
        }

        /// <summary>
        /// Salva log de erro no banco de dados.
        /// Em caso de exceção salva em arquivo teste
        /// na pasta Log da raiz do site.
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void _GravaErroAsync(LOG_Erros entity, SYS_Sistema sistema)
        {
            try
            {
                if (sistema != null)
                {

                    if (string.IsNullOrEmpty(sistema.sis_nome) && (sistema.sis_id > 0))
                    {
                        SYS_SistemaBO.GetEntity(sistema);
                    }

                    entity.sis_id = sistema.sis_id;
                    entity.sis_decricao = sistema.sis_nome;
                }

                //Salva o log no banco de dados
                LOG_ErrosBO.Save(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Salva log de erro no banco de dados.
        /// Em caso de exceção salva em arquivo teste
        /// na pasta Log da raiz do site.
        /// </summary>
        /// <param name="ex">Exception</param>
        public new static void _GravaErro(Exception ex)
        {
            try
            {
                string path = String.Concat(_DiretorioFisico, "Log");
                LogError logError = new LogError(path);
                //Liga o método no delegate para salvar log no banco de dados.
                logError.SaveLogBD = delegate (string message)
                {
                    LOG_Erros entity = new LOG_Erros();
                    try
                    {
                        //Preenche a entidade com os dados necessário
                        entity.err_descricao = message;
                        entity.err_erroBase = ex.GetBaseException().Message;
                        entity.err_tipoErro = ex.GetBaseException().GetType().FullName;
                        entity.err_dataHora = DateTime.Now;
                        if (HttpContext.Current != null && HttpContext.Current.Request != null)
                        {
                            string infoRequest = "";
                            try
                            {
                                string naoGravar = SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.LOG_ERROS_CHAVES_NAO_GRAVAR);
                                List<string> listaNaoGravar = new List<string>(naoGravar.Split(';'));

                                bool gravarQueryString;
                                Boolean.TryParse(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.LOG_ERROS_GRAVAR_QUERYSTRING), out gravarQueryString);
                                if (gravarQueryString)
                                {
                                    infoRequest += retornaListaColecao(HttpContext.Current.Request.QueryString, "QueryString", listaNaoGravar);
                                }

                                bool gravarServerVariables;
                                Boolean.TryParse(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.LOG_ERROS_GRAVAR_SERVERVARIABLES), out gravarServerVariables);
                                if (gravarServerVariables)
                                {
                                    infoRequest += retornaListaColecao(HttpContext.Current.Request.ServerVariables, "ServerVariables", listaNaoGravar);
                                }

                                bool gravarParams;
                                Boolean.TryParse(SYS_ParametroBO.ParametroValor(SYS_ParametroBO.eChave.LOG_ERROS_GRAVAR_PARAMS), out gravarParams);
                                if (gravarParams)
                                {
                                    infoRequest += retornaListaColecao(HttpContext.Current.Request.Params, "Params", listaNaoGravar);
                                }
                            }
                            catch
                            {
                            }

                            entity.err_descricao = entity.err_descricao + infoRequest;

                            entity.err_ip = HttpContext.Current.Request.UserHostAddress;
                            entity.err_machineName = HttpContext.Current.Server.MachineName;
                            entity.err_caminhoArq = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
                            try
                            {
                                entity.err_browser = String.Concat(new[] { HttpContext.Current.Request.Browser.Browser, HttpContext.Current.Request.Browser.MajorVersion.ToString(), HttpContext.Current.Request.Browser.MinorVersionString });
                            }
                            catch
                            {
                                entity.err_browser = string.Empty;
                            }
                            if (HttpContext.Current.Session != null)
                            {
                                SessionWEB session = (SessionWEB)HttpContext.Current.Session[SessSessionWEB];
                                if (session != null)
                                {
                                    if (session.__UsuarioWEB.Usuario != null)
                                    {
                                        entity.usu_id = session.__UsuarioWEB.Usuario.usu_id;
                                        entity.usu_login = session.__UsuarioWEB.Usuario.usu_login;
                                    }
                                    if (session.__UsuarioWEB.Grupo != null)
                                    {
                                        SYS_Sistema sistema = new SYS_Sistema
                                        {
                                            sis_id = session.__UsuarioWEB.Grupo.sis_id
                                            ,
                                            sis_nome = session.TituloSistema
                                        };

                                        methodErr = _GravaErroAsync;
                                        resErr = methodErr.BeginInvoke(entity, sistema, null, null);
                                    }
                                    else
                                    {
                                        methodErr = _GravaErroAsync;
                                        resErr = methodErr.BeginInvoke(entity, null, null, null);
                                    }
                                }
                                else
                                {
                                    methodErr = _GravaErroAsync;
                                    resErr = methodErr.BeginInvoke(entity, null, null, null);
                                }
                            }
                            else
                            {
                                methodErr = _GravaErroAsync;
                                resErr = methodErr.BeginInvoke(entity, null, null, null);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
                logError.Log(ex, true);
            }
            catch { }
        }

        /// <summary>
        /// Grava log de sistema no banco de dados.
        /// </summary>
        /// <param name="acao">Ação executada pelo usuário</param>
        /// <param name="descricao">Descrição do log</param>
        /// <returns>Informa se o log de sistema foi salvo com sucesso.</returns>
        public static void _GravaLogSistemaAsync(LOG_Sistema entity, SYS_Sistema sistema)
        {
            try
            {
                if (string.IsNullOrEmpty(sistema.sis_nome) && sistema.sis_id > 0)
                {
                    // [Carla] Alterações para melhoria de performance.
                    // Se o título do sistema na sessão estiver vazio, busca do banco.
                    SYS_SistemaBO.GetEntity(sistema);
                }

                entity.sis_id = sistema.sis_id;
                entity.sis_nome = sistema.sis_nome;

                if (!LOG_SistemaBO.Save(entity))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                _GravaErro(ex);
                throw;
            }
        }

        /// <summary>
        /// Grava log de sistema no banco de dados.
        /// </summary>
        /// <param name="acao">Ação executada pelo usuário</param>
        /// <param name="descricao">Descrição do log</param>
        /// <returns>Informa se o log de sistema foi salvo com sucesso.</returns>
        public static void _GravaLogSistema(LOG_SistemaTipo acao, string descricao)
        {
            try
            {
                SYS_Sistema sistema = new SYS_Sistema();
                LOG_Sistema entity = new LOG_Sistema();
                entity.log_acao = Enum.GetName(typeof(LOG_SistemaTipo), acao);
                entity.log_dataHora = DateTime.Now;
                entity.log_descricao = descricao;
                if (HttpContext.Current != null)
                {
                    //Preenche dados do host do site
                    LOG_SistemaBO.GenerateLogID();
                    entity.log_id = new Guid(HttpContext.Current.Session[LOG_Sistema.SessionName].ToString());
                    entity.log_ip = HttpContext.Current.Request.UserHostAddress;
                    entity.log_machineName = HttpContext.Current.Server.MachineName;
                    if (HttpContext.Current.Session != null)
                    {
                        SessionWEB session = (SessionWEB)HttpContext.Current.Session[SessSessionWEB];
                        if (session != null)
                        {
                            //Preenche dados referente ao usuário
                            if (session.__UsuarioWEB.Usuario != null)
                            {
                                entity.usu_id = session.__UsuarioWEB.Usuario.usu_id;
                                entity.usu_login = session.__UsuarioWEB.Usuario.usu_login;
                            }
                            //Preenche dados referente ao grupo do usuário
                            if (session.__UsuarioWEB.Grupo != null)
                            {
                                //Preenche os dados do grupo
                                entity.gru_id = session.__UsuarioWEB.Grupo.gru_id;
                                entity.gru_nome = session.__UsuarioWEB.Grupo.gru_nome;
                                //Preenche os dados do sistema
                                sistema = new SYS_Sistema
                                {
                                    sis_id = session.__UsuarioWEB.Grupo.sis_id
                                    ,
                                    sis_nome = session.TituloSistema
                                };

                                //Preenche os dados do módulo
                                SYS_Modulo modulo = (SYS_Modulo)HttpContext.Current.Session[SYS_Modulo.SessionName];
                                if (modulo != null)
                                {
                                    entity.mod_id = modulo.mod_id;
                                    entity.mod_nome = modulo.mod_nome;
                                }
                                //Preenche as entidades e unidades administrativa do grupo
                                if (session.__UsuarioWEB.GrupoUA != null)
                                {
                                    //Formata a entidade no padrão JSON
                                    JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                                    entity.log_grupoUA = oSerializer.Serialize(session.__UsuarioWEB.GrupoUA);
                                }
                            }
                        }
                    }
                }

                methodLog = _GravaLogSistemaAsync;
                resLog = methodLog.BeginInvoke(entity, sistema, null, null);

                if (HttpContext.Current != null)
                    HttpContext.Current.Session[LOG_Sistema.SessionName] = null;

                //return entity.log_id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Envia e-mail
        /// </summary>
        /// <param name="email">Endereço de email do destinatário</param>
        /// <param name="corpo">Corpo do e-mail</param>
        /// <param name="assunto">assunto do e-mail</param>
        /// <param name="remetente">nome do remetente</param>
        public static void EnviaEmail(string email, string corpo, string assunto, string remetente)
        {
            try
            {
                string host = _EmailHost;

                Mail mail = new Mail(host, true, System.Net.Mail.MailPriority.Normal)
                {
                    _From = "\"" + remetente + "\"<" + _EmailSuporte + ">",
                    _Subject = assunto,
                    _To = email,
                    _Body = corpo
                };
                mail.SendMail();
            }
            catch (Exception)
            {
                throw new ArgumentException("Erro ao tentar enviar o e-mail.");
            }
        }

        /// <summary>
        /// Recarrega as configurações do sistema
        /// </summary>
        public static void RecarregarConfiguracoes()
        {
            IDictionary<string, ICFG_Configuracao> configuracoes;
            BLL.CFG_ConfiguracaoBO.Consultar(Config, out configuracoes);
            try
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["Config"] = configuracoes;
            }
            finally
            {
                HttpContext.Current.Application.UnLock();
            }
        }

        /// <summary>
        /// Seta para as colunas que aceitam ordenação o Tooltip adequado e configura as
        /// classes de css para a coluna ordenada.
        /// </summary>
        /// <param name="grid">GridView que será ordenado</param>
        internal static void ConfiguraColunasOrdenacao(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    TableCell tcell = grid.HeaderRow.Cells[i];
                    DataControlField coluna = grid.Columns[i];

                    if ((tcell.Controls.Count > 0) && (tcell.Controls[0] is LinkButton) &&
                        (!string.IsNullOrEmpty(coluna.SortExpression)))
                    {
                        // Setar tooltip no link das colunas do grid.
                        LinkButton link = (LinkButton)tcell.Controls[0];
                        link.ToolTip = "Ordernar resultados";

                        if (!string.IsNullOrEmpty(coluna.HeaderText))
                        {
                            link.ToolTip += " por " + coluna.HeaderText.ToLower();
                        }

                        // Limpar classes das colunas.
                        coluna.HeaderStyle.CssClass = coluna.HeaderStyle.CssClass.Replace(" headerSortDown", "").Replace(" headerSortUp", "");

                        // Verifica se a coluna foi usada para ordenar.
                        if (coluna.SortExpression.Equals(grid.SortExpression))
                        {
                            // Seta a classe de css de acordo com a ordenação.
                            coluna.HeaderStyle.CssClass +=
                                (grid.SortDirection == SortDirection.Ascending
                                     ? " headerSortDown"
                                     : " headerSortUp");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Seta para as colunas que aceitam ordenação o Tooltip adequado e configura as
        /// classes de css para a coluna ordenada.
        /// </summary>
        /// <param name="grid">GridView que será ordenado</param>
        /// <param name="SortDirect"></param>
        /// <param name="SortExp"></param>
        internal static void ConfiguraColunasOrdenacao(GridView grid, string SortExp, SortDirection SortDirect)
        {
            if (grid.Rows.Count > 0)
            {
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    TableCell tcell = grid.HeaderRow.Cells[i];
                    DataControlField coluna = grid.Columns[i];

                    if ((tcell.Controls.Count > 0) && (tcell.Controls[0] is LinkButton) &&
                        (!string.IsNullOrEmpty(coluna.SortExpression)))
                    {
                        // Setar tooltip no link das colunas do grid.
                        LinkButton link = (LinkButton)tcell.Controls[0];
                        link.ToolTip = "Ordernar resultados";

                        if (!string.IsNullOrEmpty(coluna.HeaderText))
                        {
                            link.ToolTip += " por " + coluna.HeaderText.ToLower();
                        }

                        // Limpar classes das colunas.
                        coluna.HeaderStyle.CssClass = coluna.HeaderStyle.CssClass.Replace(" headerSortDown", "").Replace(" headerSortUp", "");

                        // Verifica se a coluna foi usada para ordenar.
                        if (coluna.SortExpression.Equals(SortExp))
                        {
                            // Seta a classe de css de acordo com a ordenação.
                            coluna.HeaderStyle.CssClass +=
                                (SortDirect == SortDirection.Ascending
                                     ? " headerSortDown"
                                     : " headerSortUp");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Busca o valor do parâmetro filtrado pela chave caso exista.
        /// </summary>
        /// <param name="pms_chave">Enum que representa a chave a ser pesquisada.</param>
        /// <returns>O valor do parâmetro (pms_valor) e a chave.</returns>
        public static List<CFG_ParametroMensagem> BuscaValorByChave
        (
            string pms_chave
        )
        {
            List<CFG_ParametroMensagem> param = new List<CFG_ParametroMensagem>();
            param = ParametrosMSG.Where(p => pms_chave.Contains(p.pms_chave.ToLower())).ToList<CFG_ParametroMensagem>();

            return param;
        }

        #endregion Metodos
    }
}