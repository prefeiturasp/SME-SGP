using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using Newtonsoft.Json.Linq;
using MSTech.GestaoEscolar.CustomResourceProviders;
using System.Data.SqlTypes;
using MSTech.GestaoEscolar.BLL.Caching;
using System.Web;
using GestaoEscolar.Entities;
using GestaoEscolar.BLL;

namespace MSTech.GestaoEscolar.BLL
{
    #region Excessões

    public class EditarAula_ValidationException : ValidationException
    {
        private string _mensagem;

        public EditarAula_ValidationException()
        {
        }

        public EditarAula_ValidationException(string Mensagem)
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

    /// <summary>
    /// Situações da aula da disciplina da turma
    /// </summary>
    public enum CLS_TurmaAulaSituacao : byte
    {
        AulaPrevista = 1
        ,

        Excluido = 3
        ,

        AulaDada = 4
        ,

        AulaCancelada = 6
    }

    /// <summary>
    /// Status do lancamento de frequencia para a aula.
    /// </summary>
    public enum CLS_TurmaAulaStatusFrequencia : byte
    {
        NaoPreenchida = 1,
        Preenchida = 2,
        Efetivada = 3
    }

    /// <summary>
    /// Status do lancamento de ativ. avaliativa para a aula.
    /// </summary>
    public enum CLS_TurmaAulaStatusAtividadeAvaliativa : byte
    {
        NaoPreenchida = 1,
        Preenchida = 2,
        Efetivada = 3
    }

    /// <summary>
    /// Status das anotações da aula
    /// </summary>
    public enum CLS_TurmaAulaStatusAnotacoes : byte
    {
        NaoPreenchida = 1,
        Preenchida = 2
    }

    /// <summary>
    /// Status do plano da aula
    /// </summary>
    public enum CLS_TurmaAulaStatusPlanoAula : byte
    {
        NaoPreenchida = 1,
        Preenchida = 2,
        Incompleto = 3
    }

    #region Estruturas Diario de classe

    /// <summary>
    /// Estrutura utilizada para carregar dados que serão utilizadas na validação da sincronização do diário de classe.
    /// </summary>
    public struct sDadosAulaProtocolo
    {
        public DCL_Protocolo entityProtocolo { get; set; }

        public CLS_TurmaAula entityAula { get; set; }

        public TUR_TurmaDisciplina entityTurmaDisciplina { get; set; }

        public TUR_Turma entityTurma { get; set; }

        public bool turmaIntegral { get; set; }

        public ACA_CalendarioAnual entityCalendarioAnual { get; set; }

        public ACA_FormatoAvaliacao entityFormatoAvaliacao { get; set; }

        public ACA_CurriculoPeriodo entityCurriculoPeriodo { get; set; }

        public ACA_CalendarioPeriodo entityCalendarioPeriodo { get; set; }

        public List<CLS_TurmaAulaRegencia> ltTurmaAulaRegencia { get; set; }

        public List<MTR_MatriculaTurmaDisciplina> ltMatriculaTurmaDisciplina { get; set; }

        public List<CLS_TurmaNota> ltTurmaNota { get; set; }

        public List<CLS_TurmaAulaAluno> ltTurmaAulaAluno { get; set; }

        public List<CLS_TurmaNotaAluno> ltTurmaNotaAluno { get; set; }

        public List<CLS_TurmaAulaRecurso> ltTurmaAulaRecurso { get; set; }

        public List<CLS_TurmaAulaRecursoRegencia> ltTurmaAulaRecursoRegencia { get; set; }

        public List<long> ltTurmaDisciplinaTurma { get; set; }

        public List<CLS_TurmaAula> ltAulasBanco { get; set; }
    }

    /// <summary>
    /// Estrutura que armazena os dados que serão sincronizados e salvos no gestão.
    /// </summary>
    public struct sAulaSincronizacaoDiarioClasse
    {
        public DCL_Protocolo entityProtocolo { get; set; }

        public TUR_TurmaDisciplina entityTurmaDisciplina { get; set; }

        public CLS_TurmaAula entityAula { get; set; }

        public CLS_TurmaAulaDisciplinaRelacionada entityTurmaAulaDisciplinaRelacionada { get; set; }

        public List<CLS_TurmaAulaAluno> ltTurmaAulaAluno { get; set; }

        public List<CLS_TurmaAulaRecurso> ltTurmaAulaRecurso { get; set; }

        public List<CLS_TurmaAulaRegencia> ltTurmaAulaRegencia { get; set; }

        public List<CLS_TurmaAulaRecursoRegencia> ltTurmaAulaRecursoRegencia { get; set; }

        public List<CLS_TurmaNota> ltTurmaNota { get; set; }

        public List<CLS_TurmaNotaRegencia> ltTurmaNotaRegencia { get; set; }

        public List<CLS_TurmaNotaAluno> ltTurmaNotaAluno { get; set; }

        public List<CLS_TurmaAulaPlanoDisciplina> ltTurmaAulaPlanoDisciplina { get; set; }

        public List<CLS_TurmaAulaAlunoTipoAnotacao> ltTurmaAulaAlunoTipoAnotacao { get; set; }

        public List<CLS_TurmaAulaOrientacaoCurricular> ltTurmaAulaOrientacaoCurricular { get; set; }

        public List<LOG_TurmaAula_Alteracao> ltLogAlteracaoAula { get; set; }

        public List<LOG_TurmaNota_Alteracao> ltLogAlteracaoNota { get; set; }

        public List<CLS_TurmaAula> ltAulaTerritorio { get; set; }
    }

    /// <summary>
    /// Estrutura auxiliar para carregar dados para validação da sincronização
    /// </summary>
    public struct sProtocoloDataTable
    {
        public Guid pro_id { get; set; }

        public long tud_id { get; set; }

        public DataTable dados { get; set; }
    }

    #endregion Estruturas Diario de classe

    public class CLS_TurmaAulaBO : BusinessBase<CLS_TurmaAulaDAO, CLS_TurmaAula>
    {
        #region Estrutura

        /// <summary>
        /// Estrutura para controle ao salvar/alterar/excluir uma aula na geração de aulas da agenda.
        /// </summary>
        public struct sTurmaAula
        {
            public CLS_TurmaAula entity { get; set; }

            public bool permiteAlterar { get; set; }

            public bool aulaRetroativa { get; set; }
        }

        #endregion Estrutura

        #region Sincronização com diário de classe

        #region Estrutura

        public struct sProtocoloAulaBusca
        {
            public long tur_id { get; set; }

            public long tud_id { get; set; }

            public int tau_id { get; set; }

            public Guid pro_id { get; set; }

            public DateTime tau_data { get; set; }

            public byte tdt_posicao { get; set; }

            public long pro_protocolo { get; set; }

            public Guid usu_id { get; set; }
        }

        #endregion Estrutura

        /// <summary>
        /// Retorna o valor de um campo texto a ser salvo na sincronização de dados entre o diário de classe e gestão escolar.
        /// </summary>
        /// <param name="valorDiarioClasse">Valor texto vindo do diario de classe.</param>
        /// <param name="dataAlteracaoDiarioClasse">Data de alteração do valor no diário de classe.</param>
        /// <param name="valorGestao">Valor texto salvo no gestão escolar.</param>
        /// <param name="dataAlteracaoGestao">Data de alteração do valor no gestão escolar.</param>
        /// <returns>Valor que deve ser salvo no campo na sincronização.</returns>
        public static string RetornaValorTextoSincronizacao(string valorDiarioClasse, DateTime dataAlteracaoDiarioClasse,
                                                            string valorGestao, DateTime dataAlteracaoGestao)
        {
            if (string.IsNullOrEmpty(valorDiarioClasse) && string.IsNullOrEmpty(valorGestao))
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(valorDiarioClasse) && !string.IsNullOrEmpty(valorGestao))
            {
                return dataAlteracaoGestao > dataAlteracaoDiarioClasse ? valorGestao : valorDiarioClasse;
            }

            return string.IsNullOrEmpty(valorDiarioClasse) ? valorGestao : valorDiarioClasse;
        }

        /// <summary>
        /// Processa os protocolos informado.
        /// </summary>
        /// <param name="ltProtocolo">Lista de protocolos em processamento.</param>
        /// <param name="tentativasProtocolo">Quantidade máxima de tentativas para processar protocolos.</param>
        /// <returns></returns>
        public static bool ProcessaProtocoloAulas(List<DCL_Protocolo> ltProtocolo, int tentativasProtocolo)
        {
            TalkDBTransaction banco = new CLS_TurmaAulaDAO()._Banco.CopyThisInstance();
            banco.Open();
            string logErro = string.Empty;

            try
            {
                // DataTable de protocolos
                DataTable dtProtocolo = DCL_Protocolo.TipoTabela_Protocolo();

                foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa > tentativasProtocolo))
                {
                    protocolo.pro_statusObservacao = String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                                    , tentativasProtocolo, protocolo.pro_statusObservacao);
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErro;
                    protocolo.tur_id = -1;
                    protocolo.tud_id = -1;
                    protocolo.tau_id = -1;
                    protocolo.pro_qtdeAlunos = -1;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                }

                #region Variáveis de validação

                // DataTable de dados dos protocolos utilizados para busca de dados utilizados na validação durante a sincronização.
                DataTable dtAulasProtocolo = CLS_TurmaAula.TipoTabela_TurmaAulaBusca();
                List<string> pro_protocolosCriacaoAulas = new List<string>();

                foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa <= tentativasProtocolo))
                {
                    try
                    {
                        JObject aulaProtocolo = JObject.Parse(protocolo.pro_pacote);

                        sProtocoloAulaBusca protocoloBusca = new sProtocoloAulaBusca
                        {
                            tur_id = (long)aulaProtocolo.GetValue("tur_id", StringComparison.OrdinalIgnoreCase)
                            ,
                            tud_id = (long)aulaProtocolo.GetValue("tud_id", StringComparison.OrdinalIgnoreCase)
                            ,
                            tau_id = (int)aulaProtocolo.GetValue("tau_id", StringComparison.OrdinalIgnoreCase)
                            ,
                            pro_id = protocolo.pro_id
                            ,
                            tau_data = Convert.ToDateTime(aulaProtocolo.GetValue("tau_data", StringComparison.OrdinalIgnoreCase))
                            ,
                            tdt_posicao = byte.Parse((string)aulaProtocolo.GetValue("tdt_posicao", StringComparison.OrdinalIgnoreCase))
                            ,
                            pro_protocolo = (long)(aulaProtocolo.GetValue("pro_protocolo", StringComparison.OrdinalIgnoreCase) ?? "0")
                            ,
                            usu_id = (Guid)aulaProtocolo.GetValue("usu_id", StringComparison.OrdinalIgnoreCase)
                        };

                        DataRow dr = dtAulasProtocolo.NewRow();
                        dr["tud_id"] = protocoloBusca.tud_id;
                        dr["tau_id"] = protocoloBusca.tau_id;
                        dr["pro_id"] = protocoloBusca.pro_id;
                        dr["tau_data"] = protocoloBusca.tau_data;
                        dr["tdt_posicao"] = protocoloBusca.tdt_posicao;
                        dr["pro_protocolo"] = protocoloBusca.pro_protocolo;
                        dr["usu_id"] = protocoloBusca.usu_id;
                        dtAulasProtocolo.Rows.Add(dr);

                        JArray aulasTerritorio = aulaProtocolo.GetValue("AulasTerritorio", StringComparison.OrdinalIgnoreCase) == null ||
                                                    aulaProtocolo.GetValue("AulasTerritorio", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                    ((JArray)aulaProtocolo.GetValue("AulasTerritorio", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                        // Inclui também as aulas replicadas nos territorios
                        foreach (JObject aulaTerritorio in aulasTerritorio)
                        {
                            dr = dtAulasProtocolo.NewRow();
                            dr["tud_id"] = (long)aulaTerritorio.GetValue("tud_id", StringComparison.OrdinalIgnoreCase);
                            dr["tau_id"] = (int)aulaTerritorio.GetValue("tau_id", StringComparison.OrdinalIgnoreCase);
                            dr["pro_id"] = protocoloBusca.pro_id;
                            dr["tau_data"] = protocoloBusca.tau_data;
                            dr["tdt_posicao"] = protocoloBusca.tdt_posicao;
                            dr["pro_protocolo"] = protocoloBusca.pro_protocolo;
                            dr["usu_id"] = protocoloBusca.usu_id;
                            dtAulasProtocolo.Rows.Add(dr);
                        }

                        // Adiciona numa lista os ids dos protocolos de criação das aulas que vieram do tablet.
                        // Propriedade utilizada para buscar as aulas para alteração.
                        pro_protocolosCriacaoAulas.Add(protocoloBusca.pro_protocolo.ToString());
                    }
                    catch
                    {
                        protocolo.pro_statusObservacao = "Protocolo com dados inválidos.";
                        protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                        protocolo.tur_id = -1;
                        protocolo.tud_id = -1;
                        protocolo.tau_id = -1;
                        protocolo.pro_qtdeAlunos = -1;
                        dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    }
                }
                ltProtocolo.RemoveAll(p => p.pro_status == (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao);

                // Lista de estrutura com dados para validação das informações sincronizadas.
                List<sDadosAulaProtocolo> ltAulasProtocolo = SelecionaDadosPorAulas(dtAulasProtocolo, banco);

                // Lista de protocolos de criação das aulas, para buscar aulas para alteração.
                string sProProtocoloCriacao = string.Join(",", pro_protocolosCriacaoAulas.ToArray());
                List<DCL_Protocolo> protocolosCriacaoAulas = !string.IsNullOrEmpty(sProProtocoloCriacao) ?
                                                        DCL_ProtocoloBO.SelectBy_Protocolos(sProProtocoloCriacao) :
                                                        new List<DCL_Protocolo>();

                List<CLS_TurmaAula> ltAulasBanco = new List<CLS_TurmaAula>();
                ltAulasProtocolo.ForEach(p => ltAulasBanco.AddRange(p.ltAulasBanco));
                ltAulasBanco = (from CLS_TurmaAula aula in ltAulasBanco
                                group aula by new { tud_id = aula.tud_id, tau_id = aula.tau_id } into grupo
                                select grupo.First()).ToList();

                #endregion Variáveis de validação

                // ID da atividade para correspondência entre atividades e notas lançadas.
                long idAtividade = 1;
                bool retorno = true;
                TUR_TurmaDisciplina tud = new TUR_TurmaDisciplina();

                //Salva a lista de aulas processadas para não processar no mesmo laço a mesma aula de protocolos diferentes
                List<CLS_TurmaAula> ltAulasProcessadas = new List<CLS_TurmaAula>();

                List<DCL_Protocolo> ltProtocolos = ltProtocolo.Where(pro => pro.pro_tentativa <= tentativasProtocolo).ToList();

                foreach (DCL_Protocolo protocolo in ltProtocolos)
                {
                    // Abre uma transação para cada protocolo dentro do laço.
                    // Assim é possível processar o próximo protocolo, mesmo que o atual esteja com erro.
                    TalkDBTransaction bancoSincronizacao = new CLS_TurmaAulaDAO()._Banco.CopyThisInstance();
                    bancoSincronizacao.Open(IsolationLevel.ReadCommitted);
                    bool processou = false;

                    try
                    {
                        if (protocolo.pro_tentativa <= tentativasProtocolo)
                        {
                            if (string.IsNullOrEmpty(protocolo.pro_versaoAplicativo))
                                throw new ValidationException("É necessário atualizar a versão do sistema.");

                            /**
                             * a partir da versão 1.44 o plano de aula foi alterado.
                             * versões do app anteriores a isto não irão mais salvar o plano de aula
                             * */
                            int minorVersion = int.Parse(protocolo.pro_versaoAplicativo.Split('.')[1]);

                            #region Variáveis

                            bool alteraAnotacao = false;

                            // Lista de aulas para sincronização
                            // Sempre com um único valor (necessário separar para que os protocolos possam ser processados sem a dependência entre os mesmos)
                            List<sAulaSincronizacaoDiarioClasse> listAulaSincronizacao = new List<sAulaSincronizacaoDiarioClasse>();

                            // Estrutura com dados a serem sincronizados.
                            sAulaSincronizacaoDiarioClasse sincronizacao = new sAulaSincronizacaoDiarioClasse
                            {
                                entityProtocolo = new DCL_Protocolo()
                                ,
                                entityAula = new CLS_TurmaAula()
                                ,
                                entityTurmaAulaDisciplinaRelacionada = new CLS_TurmaAulaDisciplinaRelacionada()
                                ,
                                ltTurmaAulaAluno = new List<CLS_TurmaAulaAluno>()
                                ,
                                ltTurmaAulaRecurso = new List<CLS_TurmaAulaRecurso>()
                                ,
                                ltTurmaAulaRecursoRegencia = new List<CLS_TurmaAulaRecursoRegencia>()
                                ,
                                ltTurmaAulaRegencia = new List<CLS_TurmaAulaRegencia>()
                                ,
                                ltTurmaNota = new List<CLS_TurmaNota>()
                                ,
                                ltTurmaNotaRegencia = new List<CLS_TurmaNotaRegencia>()
                                ,
                                ltTurmaNotaAluno = new List<CLS_TurmaNotaAluno>()
                                ,
                                ltTurmaAulaPlanoDisciplina = new List<CLS_TurmaAulaPlanoDisciplina>()
                                ,
                                ltTurmaAulaAlunoTipoAnotacao = new List<CLS_TurmaAulaAlunoTipoAnotacao>()
                                ,
                                ltTurmaAulaOrientacaoCurricular = new List<CLS_TurmaAulaOrientacaoCurricular>()
                                ,
                                ltLogAlteracaoAula = new List<LOG_TurmaAula_Alteracao>()
                                ,
                                ltLogAlteracaoNota = new List<LOG_TurmaNota_Alteracao>()
                                ,
                                ltAulaTerritorio = new List<CLS_TurmaAula>()
                            };

                            #endregion Variáveis

                            #region Informações de Aula

                            // Objeto JSON de entrada.
                            JObject aula = JObject.Parse(protocolo.pro_pacote);

                            // Id local da Aula
                            int tau_id = (int)aula.GetValue("tau_id", StringComparison.OrdinalIgnoreCase);

                            //Id da Turma
                            long tur_id = (long)aula.GetValue("tur_id", StringComparison.OrdinalIgnoreCase);

                            // apenas protocolos de turmas ativas e do ano letivo corrente podem ser processados
                            if (!DCL_ProtocoloBO.PodeProcessarProtocolo(tur_id, 0))
                                throw new ValidationException("O protocolo pertence a uma turma que não esta ativa ou de um ano letivo diferente do corrente, não pode ser processado!");

                            //Id da Disciplina
                            long tud_id = (long)aula.GetValue("tud_id", StringComparison.OrdinalIgnoreCase);

                            //Id do docente que realizou a chamada (pode ser o substituto)
                            long doc_id = (long)aula.GetValue("doc_id", StringComparison.OrdinalIgnoreCase);

                            //Data da Aula
                            DateTime Tau_data = Convert.ToDateTime(aula.GetValue("tau_data", StringComparison.OrdinalIgnoreCase));

                            if (!ACA_DocenteBO.ValidaAulaAtribuicaoEsporadica(doc_id, Tau_data))
                                throw new ValidationException("Não foi possível incluir aula neste dia. Verifique com a gestão da escola as datas de sua atribuição esporádica.");

                            //Data de alteracao
                            DateTime tau_dataAlteracao = Convert.ToDateTime(aula.GetValue("tau_dataAlteracao", StringComparison.OrdinalIgnoreCase));

                            if (tau_dataAlteracao > DateTime.Now.AddMinutes(10))
                                throw new ValidationException("A data de alteração da aula é maior que a data atual.");

                            //Número de créditos que engloba a Aula
                            int tau_numeroaulas = (int)aula.GetValue("tau_numeroAulas", StringComparison.OrdinalIgnoreCase);

                            //Plano de aula
                            string tau_diarioClasse = (string)aula.GetValue("tau_diarioClasse", StringComparison.OrdinalIgnoreCase);
                            string tau_planoAula = (aula.GetValue("tau_planoAula", StringComparison.OrdinalIgnoreCase) ?? "").ToString();
                            string tau_atividadeCasa = (aula.GetValue("tau_atividadeCasa", StringComparison.OrdinalIgnoreCase) ?? "").ToString();
                            string tau_sintese = (aula.GetValue("tau_sintese", StringComparison.OrdinalIgnoreCase) ?? "").ToString();

                            //Id do disciplina que o docente está compartilhando caso seja docência compartilhada.
                            long tud_idRelacionado = 0;
                            long.TryParse((aula.GetValue("tud_idRelacionado", StringComparison.OrdinalIgnoreCase) ?? "0").ToString(), out tud_idRelacionado);

                            Guid usu_id = new Guid((aula.GetValue("usu_id", StringComparison.OrdinalIgnoreCase) ?? Guid.Empty).ToString());

                            if (usu_id == Guid.Empty)
                                throw new ValidationException("É necessário atualizar a versão do sistema.");

                            // todos os registros criados pegam a situação da aula para o caso dela estar excluida.
                            byte tau_situacao;
                            if (Convert.ToInt32(aula.GetValue("tau_situacao", StringComparison.OrdinalIgnoreCase)) > 0)
                            {
                                tau_situacao = Convert.ToByte(aula.GetValue("tau_situacao", StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                tau_situacao = 1;
                            }

                            //posicao do professor que efetuou o registro de frequencia
                            byte tdt_posicao = byte.Parse((string)aula.GetValue("tdt_posicao", StringComparison.OrdinalIgnoreCase));

                            //Se não receber o campo tau_efetivado do protocolo então grava true fixo (como era feito antes)
                            bool tau_efetivado = aula.GetValue("tau_efetivado", StringComparison.OrdinalIgnoreCase) != null &&
                                                 !string.IsNullOrEmpty(aula.GetValue("tau_efetivado", StringComparison.OrdinalIgnoreCase).ToString()) ?
                                                 Convert.ToBoolean(aula.GetValue("tau_efetivado", StringComparison.OrdinalIgnoreCase)) : true;

                            Guid ent_id = TUR_TurmaBO.GetEntidadeByTurma(tur_id);

                            //Se vai processar um protocolo que possui uma aula que já foi processada na lista de protocolos então pula para processar dpeois
                            if (ltAulasProcessadas.Any(p => p.tud_id == tud_id && (p.tau_id == tau_id ||
                                                            (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_INCLUIR_VARIAS_AULAS_POR_DIA, ent_id) &&
                                                            p.tau_data.Date == Tau_data.Date && p.tdt_posicao == tdt_posicao))))
                            {
                                logErro = "Protocolo mesma aula";
                                protocolo.pro_statusObservacao = logErro;
                                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.NaoProcessado;
                                // Nao conta como tentativa quando existe mais de um protocolo para a mesma aula, no mesmo conjunto de processamento.
                                protocolo.pro_tentativa--;
                                dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                                continue;
                            }

                            // Dados de validação para a aula do protocolo.
                            sDadosAulaProtocolo dadosProtocolo = ltAulasProtocolo.Find(p => p.entityProtocolo.pro_id == protocolo.pro_id && p.entityTurmaDisciplina.tud_id == tud_id);

                            //ID do período -> Bimestre/Semestre
                            int tpc_id = dadosProtocolo.entityCalendarioPeriodo.tpc_id;

                            if (tpc_id <= 0)
                                throw new ValidationException("Data da aula está fora do período do calendário.");

                            List<ACA_Evento> lstEventos = ACA_EventoBO.GetEntity_Efetivacao_List(dadosProtocolo.entityCalendarioPeriodo.cal_id, tur_id, Guid.Empty, ent_id, 0, false);

                            //Só nao permite editar o bimestre se o período estiver finalizado
                            bool efetivado = DateTime.Today > dadosProtocolo.entityCalendarioPeriodo.cap_dataFim;

                            //Só permite editar o bimestre se tiver evento ativo
                            efetivado &= !lstEventos.Exists(p => p.tpc_id == tpc_id && p.tev_id == ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_EVENTO_EFETIVACAO_NOTAS, ent_id) &&
                                                                 DateTime.Today >= p.evt_dataInicio && DateTime.Today <= p.evt_dataFim);

                            //if (efetivado)
                            //    throw new ValidationException("O período da data da aula já foi fechado.");

                            DateTime cap_dataInicio = dadosProtocolo.entityCalendarioPeriodo.cap_dataInicio;
                            DateTime cap_dataFim = dadosProtocolo.entityCalendarioPeriodo.cap_dataFim;

                            // Adicionando Aula.
                            CLS_TurmaAula entTurmaAula = null;
                            CLS_TurmaAulaDisciplinaRelacionada entTurmaAulaDisciplinaRelacionada = null;

                            long pro_protocoloCriacao = (long)(aula.GetValue("pro_protocolo", StringComparison.OrdinalIgnoreCase) ?? "0");
                            Guid pro_idCriacao = protocolosCriacaoAulas.Any(p => p.pro_protocolo == pro_protocoloCriacao) ?
                                protocolosCriacaoAulas.Find(p => p.pro_protocolo == pro_protocoloCriacao).pro_id :
                                Guid.Empty;

                            entTurmaAula = dadosProtocolo.entityAula ?? ltAulasBanco.Find(p => p.pro_id == pro_idCriacao && p.tud_id == tud_id && p.mesmaAula);
                            int tau_numeroAulasBanco = tau_numeroaulas;

                            #region Carregar aula mesmo dia
                            //Carrega a aula no mesmo dia se houver e se permite editar essa aula
                            if (entTurmaAula == null)
                            {
                                entTurmaAula = ltAulasBanco.Find(p => p.pro_id == pro_idCriacao && p.tud_id == tud_id && !p.mesmaAula);
                            }
                            #endregion Carregar aula mesmo dia

                            if (entTurmaAula != null && !entTurmaAula.permiteAnotacao && !entTurmaAula.permiteAvaliacao &&
                                !entTurmaAula.permiteEdicaoAula && !entTurmaAula.permiteFrequencia && !entTurmaAula.permitePlanoAula)
                                throw new ValidationException("O docente não possui permissão para editar dados da aula de " + Tau_data.ToString("dd/MM/yyyy") + ".");

                            tud = TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = tud_id }, banco);

                            #region Nova aula
                            //Nova aula
                            if (entTurmaAula == null)
                            {
                                entTurmaAula = new CLS_TurmaAula();
                                entTurmaAula.tud_id = tud_id;
                                entTurmaAula.tau_id = tau_id;
                                entTurmaAula.tau_data = Tau_data;
                                entTurmaAula.tdt_posicao = tdt_posicao;
                                entTurmaAula.tau_reposicao = (Convert.ToInt32(aula.GetValue("tau_reposicao", StringComparison.OrdinalIgnoreCase)) > 0);
                                entTurmaAula.permiteEdicaoAula = entTurmaAula.permiteAnotacao = entTurmaAula.permiteAvaliacao =
                                    entTurmaAula.permiteFrequencia = entTurmaAula.permitePlanoAula = true;

                                // faz validação de quantidade de aulas no dia para a regencia
                                bool DisciplinaPrincipal = tud.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal;
                                List<TUR_TurmaDisciplina> listaDisciplinas = null;
                                if (tud.tud_global)
                                {
                                    listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(entTurmaAula.tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                                    DisciplinaPrincipal = listaDisciplinas.Exists(p => p.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal);
                                }

                                // estas validações são diferentes do gestão pois na web
                                // eles já consideram a aula que esta sendo criada e aqui
                                // a validação é antes de criar a aula.

                                // Se for a disciplina principal não precisa validar.
                                if (!DisciplinaPrincipal && !entTurmaAula.tau_reposicao)
                                {
                                    DateTime dataInicial = new DateTime();
                                    DateTime dataFinal = new DateTime();
                                    switch (entTurmaAula.tau_data.DayOfWeek)
                                    {
                                        case DayOfWeek.Sunday:
                                            dataInicial = entTurmaAula.tau_data;
                                            dataFinal = entTurmaAula.tau_data.AddDays(6);
                                            break;

                                        case DayOfWeek.Monday:
                                            dataInicial = entTurmaAula.tau_data.AddDays(-1);
                                            dataFinal = entTurmaAula.tau_data.AddDays(5);
                                            break;

                                        case DayOfWeek.Tuesday:
                                            dataInicial = entTurmaAula.tau_data.AddDays(-2);
                                            dataFinal = entTurmaAula.tau_data.AddDays(4);
                                            break;

                                        case DayOfWeek.Wednesday:
                                            dataInicial = entTurmaAula.tau_data.AddDays(-3);
                                            dataFinal = entTurmaAula.tau_data.AddDays(3);
                                            break;

                                        case DayOfWeek.Thursday:
                                            dataInicial = entTurmaAula.tau_data.AddDays(-4);
                                            dataFinal = entTurmaAula.tau_data.AddDays(2);
                                            break;

                                        case DayOfWeek.Friday:
                                            dataInicial = entTurmaAula.tau_data.AddDays(-5);
                                            dataFinal = entTurmaAula.tau_data.AddDays(1);
                                            break;

                                        case DayOfWeek.Saturday:
                                            dataInicial = entTurmaAula.tau_data.AddDays(-6);
                                            dataFinal = entTurmaAula.tau_data;
                                            break;
                                    }

                                    int CargaHoraria = tud.tud_cargaHorariaSemanal;

                                    if (tud.tud_global)
                                    {
                                        List<TUR_TurmaDisciplina> lista = listaDisciplinas ??
                                            TUR_TurmaDisciplinaBO.GetSelectBy_Turma(tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                                        var x = from TUR_TurmaDisciplina dis in lista
                                                group dis by
                                                    new { }
                                                    into g
                                                select new { total = g.Sum(dis => dis.tud_cargaHorariaSemanal) };

                                        if (x.Count() > 0)
                                            CargaHoraria = x.First().total;
                                    }

                                    int quantidadeAulas = VerificaSomaNumeroAulasSemana(entTurmaAula.tud_id, dataInicial, dataFinal,
                                                                                        banco, entTurmaAula.tdt_posicao);

                                    EnumTipoDocente enumTipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(entTurmaAula.tdt_posicao, 0);

                                    // verifico qual o tipo do docente para verificar a qtde de aula digitada no dia.
                                    // Regra.: para esses tipos a qtde de aulas por dia deve ser 1 
                                    if (!entTurmaAula.tau_reposicao &&
                                        (tud.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia || tud.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia) &&
                                        (enumTipoDocente == EnumTipoDocente.Titular ||
                                        enumTipoDocente == EnumTipoDocente.SegundoTitular ||
                                        enumTipoDocente == EnumTipoDocente.Substituto)
                                       )
                                    {
                                        // utilizo o mesmo método passado nos campos data inicial e final a data da aula, assim 
                                        // retorna a qtde de aulas que já existem para o dia.
                                        int quantidadeAulasDia = VerificaSomaNumeroAulasSemana(entTurmaAula.tud_id, entTurmaAula.tau_data, entTurmaAula.tau_data,
                                                                                        banco, entTurmaAula.tdt_posicao);

                                        if (enumTipoDocente == EnumTipoDocente.Substituto && dadosProtocolo.turmaIntegral && tud.tud_tipo != (byte)TurmaDisciplinaTipo.Experiencia)
                                        {
                                            if (quantidadeAulasDia > 1)
                                            {
                                                throw new ValidationException("Já existem duas aulas cadastradas para o dia " + entTurmaAula.tau_data.ToString("dd/MM/yyyy") + ".");
                                            }
                                        }
                                        else if (quantidadeAulasDia > 0)
                                        {
                                            throw new ValidationException("Já existe uma aula cadastrada para o dia " + entTurmaAula.tau_data.ToString("dd/MM/yyyy") + ".");
                                        }
                                    }

                                    // objetivo é executar para os demais docentes (menos compartilhado e projeto) e que a disciplina não seja regencia, por isso é negado o if abaixo
                                    if (!entTurmaAula.tau_reposicao &&
                                        !(tud.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia &&
                                         (enumTipoDocente == EnumTipoDocente.Compartilhado ||
                                         enumTipoDocente == EnumTipoDocente.Projeto)))
                                    {
                                        // a quantidade de aulas se refere a quantidade de aulas já criadas.
                                        if (quantidadeAulas >= CargaHoraria)
                                        {
                                            throw new ValidationException("A soma da quantidade de aulas da semana do dia " +
                                                                    dataInicial.ToString("dd/MM/yyyy") + " ao dia " +
                                                                    dataFinal.ToString("dd/MM/yyyy") +
                                                                    " não pode ser maior do que " + CargaHoraria + ".");
                                        }
                                    }
                                }

                                entTurmaAula.pro_id = protocolo.pro_id;
                                entTurmaAula.tau_numeroAulas = tau_numeroaulas;
                                entTurmaAula.tur_id = tur_id;
                                entTurmaAula.tpc_id = tpc_id;
                                entTurmaAula.tau_situacao = tau_situacao;
                                entTurmaAula.usu_id = usu_id;
                                entTurmaAula.tau_efetivado = tau_efetivado;
                                entTurmaAula.tau_statusFrequencia = tau_efetivado ? (byte)CLS_TurmaAulaStatusFrequencia.Efetivada : (byte)CLS_TurmaAulaStatusFrequencia.NaoPreenchida;
                                entTurmaAula.tau_statusAnotacoes = (byte)CLS_TurmaAulaStatusAnotacoes.NaoPreenchida;
                                entTurmaAula.IsNew = true;

                                if (minorVersion >= 44)
                                {
                                    entTurmaAula.tau_diarioClasse = tau_diarioClasse;
                                    entTurmaAula.tau_planoAula = tau_planoAula;
                                    entTurmaAula.tau_atividadeCasa = tau_atividadeCasa;
                                    entTurmaAula.tau_sintese = tau_sintese;
                                    entTurmaAula.tau_checadoAtividadeCasa = !String.IsNullOrEmpty(entTurmaAula.tau_atividadeCasa);
                                }

                                // Caso a aula seja de um docente compartilhado.
                                if (tud_idRelacionado > 0)
                                {
                                    entTurmaAulaDisciplinaRelacionada = new CLS_TurmaAulaDisciplinaRelacionada
                                    {
                                        tud_id = entTurmaAula.tud_id,
                                        tau_id = tau_id > 0 ? tau_id : 1,
                                        tdr_id = 1,
                                        tud_idRelacionada = tud_idRelacionado
                                    };

                                    sincronizacao.entityTurmaAulaDisciplinaRelacionada = entTurmaAulaDisciplinaRelacionada;
                                }
                            }
                            #endregion Nova aula
                            #region Edição aula
                            //Aula já existente, edição
                            else
                            {
                                tau_numeroAulasBanco = entTurmaAula.tau_numeroAulas;

                                if (entTurmaAula.tau_id < 0)
                                {
                                    entTurmaAula.tau_id = SelecionaIdAulaPorProcotolo(entTurmaAula.tud_id, entTurmaAula.pro_id, bancoSincronizacao);
                                }

                                // Os tempos de aula não podem ser alterados se houver conteúdo lançado
                                if (tau_numeroAulasBanco != tau_numeroaulas && dadosProtocolo.ltTurmaAulaAluno.Any())
                                {
                                    throw new ValidationException("Os tempos de aula não poderão ser modificados/sobrescritos.");
                                }

                                entTurmaAula.tau_numeroAulas = tau_numeroaulas;

                                DateTime tau_dataAlteracaoGestao = entTurmaAula.tau_dataAlteracao;

                                entTurmaAula.tur_id = tur_id;

                                //Verifica se enviou a data nova de edição de aula e se é maior que a data do banco
                                if (entTurmaAula.permiteEdicaoAula)
                                {
                                    if (!efetivado && tau_situacao != (byte)CLS_TurmaAulaSituacao.Excluido)
                                        entTurmaAula.tau_situacao = tau_situacao;

                                    entTurmaAula.usu_id = entTurmaAula.usu_id == Guid.Empty ? usu_id : entTurmaAula.usu_id;

                                    entTurmaAula.tau_reposicao = (Convert.ToInt32(aula.GetValue("tau_reposicao", StringComparison.OrdinalIgnoreCase)) > 0);
                                }

                                entTurmaAula.tau_efetivado = tau_efetivado;

                                //Se efetivou a frequencia no tablet e a data de alteração de frequencia é maior então efetiva
                                if (tau_efetivado)
                                {
                                    entTurmaAula.tau_statusFrequencia = (byte)CLS_TurmaAulaStatusFrequencia.Efetivada;
                                }
                                else
                                {
                                    //Se há alteração de frequência
                                    if (entTurmaAula.dataLogAlteracaoFreq != new DateTime())
                                    {
                                        entTurmaAula.tau_statusFrequencia = entTurmaAula.tau_statusFrequencia;
                                    }
                                    else
                                    {
                                        entTurmaAula.tau_statusFrequencia = (byte)CLS_TurmaAulaStatusFrequencia.NaoPreenchida;
                                    }
                                }

                                //Verifica se enviou a data de exclusão e se é maior que a data de alteração de aula do banco
                                if (!efetivado && tau_situacao == (byte)CLS_TurmaAulaSituacao.Excluido && entTurmaAula.permiteEdicaoAula)
                                    entTurmaAula.tau_situacao = tau_situacao;

                                entTurmaAula.IsNew = false;

                                if (minorVersion >= 44)
                                {
                                    //Verifica se enviou a data nova de plano de aula é se maior que a data do banco
                                    if (entTurmaAula.permitePlanoAula)
                                    {
                                        entTurmaAula.tau_planoAula = RetornaValorTextoSincronizacao(tau_planoAula, tau_dataAlteracao, entTurmaAula.tau_planoAula, tau_dataAlteracaoGestao);
                                        entTurmaAula.tau_diarioClasse = RetornaValorTextoSincronizacao(tau_diarioClasse, tau_dataAlteracao, entTurmaAula.tau_diarioClasse, tau_dataAlteracaoGestao);
                                        entTurmaAula.tau_atividadeCasa = RetornaValorTextoSincronizacao(tau_atividadeCasa, tau_dataAlteracao, entTurmaAula.tau_atividadeCasa, tau_dataAlteracaoGestao);
                                        entTurmaAula.tau_sintese = RetornaValorTextoSincronizacao(tau_sintese, tau_dataAlteracao, entTurmaAula.tau_sintese, tau_dataAlteracaoGestao);
                                    }
                                    //Verifica se enviou a data nova de edição de aula e se é maior que a data do banco
                                    if (entTurmaAula.permiteEdicaoAula)
                                        entTurmaAula.tau_checadoAtividadeCasa = !String.IsNullOrEmpty(entTurmaAula.tau_atividadeCasa);
                                }
                            }
                            #endregion Edição aula
                            entTurmaAula.tau_dataAlteracao = entTurmaAula.permiteEdicaoAula ? tau_dataAlteracao : entTurmaAula.tau_dataAlteracao;
                            entTurmaAula.tau_dataUltimaSincronizacao = entTurmaAula.permiteEdicaoAula ? DateTime.Now : entTurmaAula.tau_dataUltimaSincronizacao;
                            tau_situacao = entTurmaAula.tau_situacao;

                            // atualiza o status do plano de aula
                            if (minorVersion >= 44 && entTurmaAula.permitePlanoAula)
                                entTurmaAula.tau_statusPlanoAula = (byte)RetornaStatusPlanoAula(entTurmaAula);

                            sincronizacao.entityAula = entTurmaAula;

                            if (tau_id < 0)
                            {
                                tau_id = entTurmaAula.tau_id;
                            }

                            #endregion Informações de Aula

                            #region Informações de frequência dos alunos

                            // Todas as matrículas de aluno na disciplina.
                            List<MTR_MatriculaTurmaDisciplina> listaMatriculas =
                                (from MTR_MatriculaTurmaDisciplina item in dadosProtocolo.ltMatriculaTurmaDisciplina
                                 where
                                    item.tud_id == tud_id &&
                                    // Busca a matrícula válida no período do calendário.
                                    item.mtd_dataMatricula <= cap_dataFim &&
                                    (
                                        item.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Ativo ||
                                        item.mtd_dataSaida >= cap_dataInicio
                                    ) &&
                                    // Filtra a data da aula.
                                    (
                                        item.mtd_dataMatricula <= Tau_data &&
                                        (
                                            item.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Ativo ||
                                            item.mtd_dataSaida >= Tau_data
                                        )
                                    )
                                 select item).ToList();

                            List<CLS_TurmaAulaAluno> ltFreqLancada = new List<CLS_TurmaAulaAluno>();
                            ltFreqLancada = dadosProtocolo.ltTurmaAulaAluno;

                            JArray alunoLista = aula.GetValue("alunos", StringComparison.OrdinalIgnoreCase) == null ||
                                                aula.GetValue("alunos", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                ((JArray)aula.GetValue("alunos", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                            foreach (JObject aluno in alunoLista)
                            {
                                long alu_id = (long)aluno.GetValue("alu_id", StringComparison.OrdinalIgnoreCase);

                                MTR_MatriculaTurmaDisciplina matricula = listaMatriculas.Where(p => p.alu_id == alu_id).FirstOrDefault();

                                if (matricula != null)
                                {
                                    CLS_TurmaAulaAluno freq = ltFreqLancada.Count > 0 ?
                                        (
                                            ltFreqLancada.Exists(p => p.alu_id == alu_id) ?
                                                ltFreqLancada.Where(p => p.alu_id == alu_id).First() : null
                                        )
                                        : null;

                                    bool freqEncontrada = freq != null;
                                    if (freq == null)
                                        freq = new CLS_TurmaAulaAluno();
                                    var taa_dataAlteracao = Convert.ToDateTime(aluno.GetValue("taa_dataAlteracao", StringComparison.OrdinalIgnoreCase) ?? tau_dataAlteracao);
                                    var freqGestaoMaisRecente = freq.taa_dataAlteracao > taa_dataAlteracao;
                                    freqGestaoMaisRecente |= efetivado;

                                    var taa_frequencia = (int)(aluno.GetValue("taa_frequencia", StringComparison.OrdinalIgnoreCase) ?? 0);
                                    var taa_frequenciaBitMap = (aluno.GetValue("taa_frequenciaBitMap", StringComparison.OrdinalIgnoreCase) ?? "").ToString();
                                    var taa_anotacao = (string)(aluno.GetValue("taa_anotacao", StringComparison.OrdinalIgnoreCase) ?? "");

                                    CLS_TurmaAulaAluno freqAluno = new CLS_TurmaAulaAluno
                                    {
                                        tud_id = tud_id,
                                        tau_id = tau_id > 0 ? tau_id : 1,
                                        alu_id = alu_id,
                                        mtu_id = matricula.mtu_id,
                                        mtd_id = matricula.mtd_id,
                                        //Verifica se enviou a data nova de frequência e se é maior que a data do banco
                                        taa_frequencia = !freqEncontrada || entTurmaAula.permiteFrequencia ?
                                                            (!freqEncontrada || !freqGestaoMaisRecente || tau_numeroAulasBanco != entTurmaAula.tau_numeroAulas ?
                                                                taa_frequencia : freq.taa_frequencia)
                                                            : freq.taa_frequencia,
                                        taa_situacao = tau_situacao,
                                        //Verifica se enviou a data nova de anotação e se é maior que a data do banco
                                        taa_anotacao = !freqEncontrada || entTurmaAula.permiteAnotacao ?
                                                            RetornaValorTextoSincronizacao(taa_anotacao, taa_dataAlteracao, freq.taa_anotacao, freq.taa_dataAlteracao)
                                                            : freq.taa_anotacao,
                                        //Verifica se enviou a data nova de frequência e se é maior que a data do banco
                                        taa_frequenciaBitMap = !freqEncontrada || entTurmaAula.permiteFrequencia ?
                                                                    (!freqEncontrada || !freqGestaoMaisRecente || tau_numeroAulasBanco != entTurmaAula.tau_numeroAulas ?
                                                                        taa_frequenciaBitMap : freq.taa_frequenciaBitMap)
                                                                    : freq.taa_frequenciaBitMap,
                                        //Verifica se enviou a data nova de frequência ou anotação e se é maior que a data do banco
                                        taa_dataAlteracao = !freqEncontrada || entTurmaAula.permiteAnotacao || entTurmaAula.permiteFrequencia ?
                                                                taa_dataAlteracao : freq.taa_dataAlteracao,
                                        IsNew = freq.alu_id <= 0
                                    };

                                    sincronizacao.ltTurmaAulaAluno.Add(freqAluno);

                                    bool permiteAnotacoes = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_ANOTACOES_MAIS_DE_UM_ALUNO, new Guid());

                                    //Verifica se enviou a data nova de anotação e se é maior que a data do banco
                                    if (permiteAnotacoes && entTurmaAula.permiteAnotacao)
                                    {
                                        JArray anotacoesAluno = aluno.GetValue("listaTurmaAulaAlunoTipoAnotacao", StringComparison.OrdinalIgnoreCase) == null ||
                                                                aluno.GetValue("listaTurmaAulaAlunoTipoAnotacao", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                                (JArray)aluno.GetValue("listaTurmaAulaAlunoTipoAnotacao", StringComparison.OrdinalIgnoreCase) ?? new JArray();

                                        sincronizacao.ltTurmaAulaAlunoTipoAnotacao.AddRange(
                                            (from JObject anotacao in anotacoesAluno
                                             select new CLS_TurmaAulaAlunoTipoAnotacao
                                             {
                                                 alu_id = freqAluno.alu_id,
                                                 tud_id = freqAluno.tud_id,
                                                 tau_id = freqAluno.tau_id,
                                                 mtu_id = freqAluno.mtu_id,
                                                 mtd_id = freqAluno.mtd_id,
                                                 tia_id = (int)anotacao.GetValue("tia_id", StringComparison.OrdinalIgnoreCase)
                                             }).ToList());

                                        alteraAnotacao = true;
                                    }
                                }
                            }

                            // atualiza o status das anotações de acordo com a lista de alunos da aula
                            if (entTurmaAula.permiteAnotacao && sincronizacao.ltTurmaAulaAluno.Any())
                                sincronizacao.entityAula.tau_statusAnotacoes = (byte)RetornaStatusAnotacoes(sincronizacao.ltTurmaAulaAluno, sincronizacao.ltTurmaAulaAlunoTipoAnotacao);

                            #endregion Informações de frequência dos alunos

                            //TODO: verificar se precisa comparar datas para gravar isso. se não enviar esses dados não vai dar certo
                            #region Informações de recursos utilizados nas aulas

                            JArray recursosLista = aula.GetValue("recursos", StringComparison.OrdinalIgnoreCase) == null ||
                                                   aula.GetValue("recursos", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                   ((JArray)aula.GetValue("recursos", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                            sincronizacao.ltTurmaAulaRecurso =
                                (from JObject recurso in recursosLista
                                 select new CLS_TurmaAulaRecurso
                                 {
                                     //tud_id = (int)recurso.SelectToken("tud_id"),
                                     tud_id = tud_id,
                                     tau_id = tau_id > 0 ? tau_id : 1,
                                     rsa_id = (int)(recurso.GetValue("rsa_id", StringComparison.OrdinalIgnoreCase) ?? -1),
                                     tar_observacao = recurso.GetValue("tar_observacao", StringComparison.OrdinalIgnoreCase).ToString(),
                                     tar_dataCriacao = DateTime.Now,
                                     tar_dataAlteracao = tau_dataAlteracao
                                 }).ToList();

                            #endregion Informações de recursos utilizados nas aulas

                            //TODO: verificar se precisa comparar datas para gravar isso. se não enviar esses dados não vai dar certo
                            #region Plano de aula das regencias

                            JArray planosAulaRegencias = aula.GetValue("planoAulaRegenciaDisciplinas", StringComparison.OrdinalIgnoreCase) == null ||
                                                         aula.GetValue("planoAulaRegenciaDisciplinas", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                         ((JArray)aula.GetValue("planoAulaRegenciaDisciplinas", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                            foreach (JObject planoAulaRegencia in planosAulaRegencias)
                            {
                                CLS_TurmaAulaPlanoDisciplina planoDisciplina = new CLS_TurmaAulaPlanoDisciplina
                                {
                                    tud_id = Convert.ToInt32(planoAulaRegencia.GetValue("tud_id", StringComparison.OrdinalIgnoreCase)),
                                    tud_idPlano = Convert.ToInt32(planoAulaRegencia.GetValue("tud_idPlano", StringComparison.OrdinalIgnoreCase)),
                                    tau_id = tau_id > 0 ? tau_id : 1
                                };

                                sincronizacao.ltTurmaAulaPlanoDisciplina.Add(planoDisciplina);
                            }

                            #endregion

                            #region Regencias

                            JArray regencias = aula.GetValue("regencias", StringComparison.OrdinalIgnoreCase) == null ||
                                               aula.GetValue("regencias", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                               ((JArray)aula.GetValue("regencias", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                            foreach (JObject regencia in regencias)
                            {
                                CLS_TurmaAulaRegencia aulaRegencia = null;
                                aulaRegencia = dadosProtocolo.ltTurmaAulaRegencia.Find(p => p.tau_id == tau_id && p.tud_id == tud_id && p.tud_idFilho == Convert.ToInt32(regencia["tud_idFilho"]));

                                DateTime tuf_dataAlteracao = Convert.ToDateTime(regencia.GetValue("tuf_dataAlteracao", StringComparison.OrdinalIgnoreCase) ?? tau_dataAlteracao);

                                if (aulaRegencia == null)
                                {
                                    aulaRegencia = new CLS_TurmaAulaRegencia
                                    {
                                        tau_id = tau_id > 0 ? tau_id : 1
                                        ,
                                        tud_id = tud_id
                                        ,
                                        tud_idFilho = Convert.ToInt32(regencia.GetValue("tud_idFilho", StringComparison.OrdinalIgnoreCase))
                                        ,
                                        tuf_dataCriacao = tuf_dataAlteracao
                                    };
                                }

                                DateTime tuf_dataAlteracaoGestao = aulaRegencia.tuf_dataAlteracao;

                                bool planoGestaoMaisRecente = aulaRegencia.tuf_dataAlteracao > tuf_dataAlteracao;
                                string tuf_planoAula = Convert.ToString(regencia.GetValue("tuf_planoAula", StringComparison.OrdinalIgnoreCase));
                                string tuf_atividadeCasa = Convert.ToString(regencia.GetValue("tuf_atividadeCasa", StringComparison.OrdinalIgnoreCase));
                                string tuf_diarioClasse = Convert.ToString(regencia.GetValue("tuf_ata", StringComparison.OrdinalIgnoreCase));
                                string tuf_sintese = Convert.ToString(regencia.GetValue("tuf_sintese", StringComparison.OrdinalIgnoreCase));

                                aulaRegencia.tuf_situacao = tau_situacao;

                                if (entTurmaAula.permitePlanoAula)
                                {
                                    aulaRegencia.tuf_planoAula = RetornaValorTextoSincronizacao(tuf_planoAula, tuf_dataAlteracao, aulaRegencia.tuf_planoAula, tuf_dataAlteracaoGestao);
                                    aulaRegencia.tuf_atividadeCasa = RetornaValorTextoSincronizacao(tuf_atividadeCasa, tuf_dataAlteracao, aulaRegencia.tuf_atividadeCasa, tuf_dataAlteracaoGestao);
                                    aulaRegencia.tuf_diarioClasse = RetornaValorTextoSincronizacao(tuf_diarioClasse, tuf_dataAlteracao, aulaRegencia.tuf_diarioClasse, tuf_dataAlteracaoGestao);
                                    aulaRegencia.tuf_sintese = RetornaValorTextoSincronizacao(tuf_sintese, tuf_dataAlteracao, aulaRegencia.tuf_sintese, tuf_dataAlteracaoGestao);
                                }
                                else
                                {
                                    aulaRegencia.tuf_planoAula = aulaRegencia.tuf_planoAula;
                                    aulaRegencia.tuf_atividadeCasa = aulaRegencia.tuf_atividadeCasa;
                                    aulaRegencia.tuf_diarioClasse = aulaRegencia.tuf_diarioClasse;
                                    aulaRegencia.tuf_sintese = aulaRegencia.tuf_sintese;
                                }

                                aulaRegencia.tuf_numeroAulas = entTurmaAula.tau_numeroAulas;

                                aulaRegencia.tuf_data = entTurmaAula.tau_data;

                                aulaRegencia.tuf_dataAlteracao = planoGestaoMaisRecente ? tuf_dataAlteracaoGestao : tuf_dataAlteracao;

                                aulaRegencia.tuf_checadoAtividadeCasa = !String.IsNullOrEmpty(aulaRegencia.tuf_atividadeCasa);

                                sincronizacao.ltTurmaAulaRegencia.Add(aulaRegencia);

                                JArray recursos = regencia.GetValue("recursos", StringComparison.OrdinalIgnoreCase) == null ||
                                                  regencia.GetValue("recursos", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                  ((JArray)regencia.GetValue("recursos", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                                int trrId = 1;
                                foreach (JObject recurso in recursos)
                                {
                                    CLS_TurmaAulaRecursoRegencia recursoRegencia = new CLS_TurmaAulaRecursoRegencia
                                    {
                                        tau_id = tau_id > 0 ? tau_id : 1
                                        ,
                                        tud_id = tud_id
                                        ,
                                        tud_idFilho = aulaRegencia.tud_idFilho
                                        ,
                                        trr_dataCriacao = DateTime.Now
                                        ,
                                        trr_dataAlteracao = tuf_dataAlteracao
                                        ,
                                        trr_id = trrId
                                        ,
                                        trr_observacao = Convert.ToString(recurso.GetValue("tar_observacao", StringComparison.OrdinalIgnoreCase))
                                        ,
                                        rsa_id = Convert.ToInt32(recurso.GetValue("rsa_id", StringComparison.OrdinalIgnoreCase))
                                    };

                                    sincronizacao.ltTurmaAulaRecursoRegencia.Add(recursoRegencia);
                                    trrId++;
                                }
                            }

                            #endregion Regencias

                            #region Informação da TurmaNota (referentes à atividade e nota dos alunos)

                            //sincronizacao.entityTurmaDisciplina = dadosProtocolo.entityTurmaDisciplina;

                            List<long> disciplinasTurma = dadosProtocolo.ltTurmaDisciplinaTurma;

                            JArray TurmaNotaLista = aula.GetValue("atividades", StringComparison.OrdinalIgnoreCase) == null ||
                                                    aula.GetValue("atividades", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                    (JArray)aula.GetValue("atividades", StringComparison.OrdinalIgnoreCase) ?? new JArray();

                            bool atualizaStatusNota = false;

                            // Lista de atividades cadastradas no banco de dados.
                            List<CLS_TurmaNota> atividades = dadosProtocolo.ltTurmaNota;

                            foreach (JObject turmaNota in TurmaNotaLista)
                            {
                                int tnt_id = (int)(turmaNota.GetValue("tnt_id", StringComparison.OrdinalIgnoreCase) ?? 0);
                                string tnt_nome = (string)turmaNota.GetValue("tnt_nome", StringComparison.OrdinalIgnoreCase);
                                string tnt_descricao = (string)turmaNota.GetValue("tnt_descricao", StringComparison.OrdinalIgnoreCase);
                                int tav_id = (int)turmaNota.GetValue("tav_id", StringComparison.OrdinalIgnoreCase);
                                int tud_idAtividade = (int)turmaNota.GetValue("tud_id", StringComparison.OrdinalIgnoreCase);
                                Boolean tnt_exclusiva = (((int)turmaNota.GetValue("tnt_exclusiva", StringComparison.OrdinalIgnoreCase)) > 0);
                                DateTime tnt_dataAlteracao = Convert.ToDateTime(turmaNota.GetValue("tnt_dataAlteracao", StringComparison.OrdinalIgnoreCase) ?? tau_dataAlteracao);
                                byte tnt_situacao = (byte)(turmaNota.GetValue("tnt_situacao", StringComparison.OrdinalIgnoreCase) ?? 1);
                                int tnt_chaveDiario = (int)(turmaNota.GetValue("tnt_chaveDiario", StringComparison.OrdinalIgnoreCase) ?? 0);

                                if (disciplinasTurma.Any(p => p == tud_idAtividade))
                                {
                                    // Consolidando informações da atividade numa Entidade do tipo TurmaAula.
                                    CLS_TurmaNota entTurmaNota = new CLS_TurmaNota
                                    {
                                        idAtividade = idAtividade,
                                        tud_id = tud_idAtividade,
                                        tpc_id = tpc_id,
                                        tnt_nome = tnt_nome,
                                        tnt_descricao = tnt_descricao,
                                        tav_id = tav_id,
                                        tdt_posicao = tdt_posicao,
                                        tnt_efetivado = true, // vai sempre gravar como efetivado no serviço.
                                        tnt_situacao = tnt_situacao,
                                        tnt_data = Tau_data,
                                        tnt_exclusiva = tnt_exclusiva,
                                        usu_id = usu_id,
                                        tnt_chaveDiario = tnt_chaveDiario,
                                        IsNew = true
                                    };

                                    DateTime dataAlteracaoGestao = entTurmaNota.tnt_dataAlteracao;

                                    if (atividades.Any(p => p.tud_id == tud_idAtividade && p.tnt_id == tnt_id))
                                    {
                                        entTurmaNota = atividades.Find(p => p.tud_id == tud_idAtividade && p.tnt_id == tnt_id);

                                        if (sincronizacao.ltTurmaNota.Any() && entTurmaAula.permiteAvaliacao)
                                            atualizaStatusNota = true;

                                        bool atividadeGestaoMaisRecente = entTurmaNota.tnt_dataAlteracao > tnt_dataAlteracao;
                                        atividadeGestaoMaisRecente |= efetivado;

                                        //Verifica se enviou a data nova de edição de atividade e se é maior que a data do banco
                                        if (entTurmaAula.permiteAvaliacao)
                                        {
                                            if (!efetivado && tnt_situacao != (byte)CLS_TurmaNotaSituacao.Excluido)
                                                entTurmaNota.tnt_situacao = tnt_situacao;

                                            entTurmaNota.tnt_nome = tnt_situacao == (byte)CLS_TurmaNotaSituacao.Excluido ?
                                                                String.Format("TABLET_{0}", entTurmaNota.tnt_nome) :
                                                                RetornaValorTextoSincronizacao(tnt_nome, tnt_dataAlteracao, entTurmaNota.tnt_nome, dataAlteracaoGestao);

                                            entTurmaNota.tnt_descricao = RetornaValorTextoSincronizacao(tnt_descricao, tnt_dataAlteracao, entTurmaNota.tnt_descricao, dataAlteracaoGestao);
                                            entTurmaNota.tnt_efetivado = true; // vai sempre gravar como efetivado no serviço.
                                            entTurmaNota.tnt_exclusiva = atividadeGestaoMaisRecente ? entTurmaNota.tnt_exclusiva : tnt_exclusiva;
                                        }

                                        //Verifica se enviou a data de exclusão e se é maior que a data de alteração de aula do banco
                                        if (!efetivado && tnt_situacao == (byte)CLS_TurmaNotaSituacao.Excluido && entTurmaAula.permiteAvaliacao)
                                            entTurmaNota.tnt_situacao = tnt_situacao;

                                        entTurmaNota.idAtividade = idAtividade;

                                        entTurmaNota.IsNew = false;
                                    }
                                    else if (atividades.Any(p => p.tud_id == tud_idAtividade &&
                                                                 p.tnt_nome.ToUpper() == tnt_nome.ToUpper() &&
                                                                 p.tav_id == tav_id &&
                                                                 p.tdt_posicao == tdt_posicao &&
                                                                 (p.tnt_data == Tau_data || p.tau_id == tau_id)))
                                    {
                                        entTurmaNota = atividades.Find(p => p.tud_id == tud_idAtividade &&
                                                                            p.tnt_nome.ToUpper() == tnt_nome.ToUpper() &&
                                                                            p.tav_id == tav_id &&
                                                                            (p.tnt_data == Tau_data || p.tau_id == tau_id));

                                        bool atividadeGestaoMaisRecente = entTurmaNota.tnt_dataAlteracao > tnt_dataAlteracao;
                                        atividadeGestaoMaisRecente |= efetivado;

                                        //Verifica se enviou a data nova de edição de atividade e se é maior que a data do banco
                                        if (entTurmaAula.permiteAvaliacao)
                                        {
                                            if (!efetivado && tnt_situacao != (byte)CLS_TurmaNotaSituacao.Excluido)
                                                entTurmaNota.tnt_situacao = tnt_situacao;

                                            entTurmaNota.tnt_descricao = RetornaValorTextoSincronizacao(tnt_descricao, tnt_dataAlteracao, entTurmaNota.tnt_descricao, dataAlteracaoGestao);
                                            entTurmaNota.tnt_efetivado = true; // vai sempre gravar como efetivado no serviço.
                                            entTurmaNota.tnt_exclusiva = atividadeGestaoMaisRecente ? entTurmaNota.tnt_exclusiva : tnt_exclusiva;
                                            entTurmaNota.tnt_situacao = tnt_situacao;
                                            entTurmaNota.idAtividade = idAtividade;

                                            entTurmaNota.tnt_nome = tnt_situacao == (byte)CLS_TurmaNotaSituacao.Excluido ?
                                                                    String.Format("TABLET_{0}", entTurmaNota.tnt_nome) :
                                                                    entTurmaNota.tnt_nome;
                                        }

                                        //Verifica se enviou a data de exclusão e se é maior que a data de alteração de aula do banco
                                        if (!efetivado && tnt_situacao == (byte)CLS_TurmaNotaSituacao.Excluido && entTurmaAula.permiteAvaliacao)
                                            entTurmaNota.tnt_situacao = tnt_situacao;

                                        entTurmaNota.IsNew = false;
                                    }

                                    tnt_situacao = entTurmaNota.tnt_situacao;
                                    entTurmaNota.tnt_dataAlteracao = tnt_dataAlteracao;

                                    if (entTurmaNota.IsNew)
                                    {
                                        entTurmaNota.pro_id = protocolo.pro_id; ;
                                    }

                                    // a atividade so vai ser vinculada a aula quando o tipo da disciplina for diferente de 11(regencia).
                                    if (sincronizacao.entityTurmaDisciplina != null && sincronizacao.entityTurmaDisciplina.tud_tipo != (byte)TurmaDisciplinaTipo.Regencia)
                                    {
                                        entTurmaNota.tau_id = tau_id;
                                    }
                                    // Caso a atividade seja de uma aula de regência, salva o vinculo da atividade com a aula na tabela CLS_TurmaNotaRegencia.
                                    else if (entTurmaNota.IsNew)
                                    {
                                        if (tud.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia)
                                        {
                                            CLS_TurmaNotaRegencia entTurmaNotaRegencia = new CLS_TurmaNotaRegencia
                                            {
                                                idAtividade = idAtividade,
                                                tud_id = tud_idAtividade,
                                                tnt_id = entTurmaNota.tnt_id > 0 ? entTurmaNota.tnt_id : 1,
                                                tud_idAula = tud_id,
                                                tau_idAula = tau_id,
                                                IsNew = true
                                            };

                                            sincronizacao.ltTurmaNotaRegencia.Add(entTurmaNotaRegencia);
                                        }
                                    }

                                    sincronizacao.ltTurmaNota.Add(entTurmaNota);

                                    //Nó existente apenas no JSON do Diario de classe
                                    JArray AtividadeNotaAluno = turmaNota.GetValue("atividadeAluno", StringComparison.OrdinalIgnoreCase) == null ||
                                                                turmaNota.GetValue("atividadeAluno", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                                (JArray)turmaNota.GetValue("atividadeAluno", StringComparison.OrdinalIgnoreCase) ?? new JArray();

                                    #region alunos na atividade do diario até versão 1.46
                                    if (AtividadeNotaAluno.Count > 0)
                                    {
                                        List<MTR_MatriculaTurmaDisciplina> lstMatriculasFilhas =
                                                                                     (from MTR_MatriculaTurmaDisciplina item in dadosProtocolo.ltMatriculaTurmaDisciplina
                                                                                      where
                                                                                         item.tud_id == tud_idAtividade &&
                                                                                         // Busca a matrícula válida no período do calendário.
                                                                                         item.mtd_dataMatricula <= cap_dataFim &&
                                                                                         (
                                                                                             item.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Ativo ||
                                                                                             item.mtd_dataSaida >= cap_dataInicio
                                                                                         ) &&
                                                                                         // Filtra a data da aula.
                                                                                         (
                                                                                             item.mtd_dataMatricula <= Tau_data &&
                                                                                             (
                                                                                                 item.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Ativo ||
                                                                                                 item.mtd_dataSaida >= Tau_data
                                                                                             )
                                                                                         )
                                                                                      select item).ToList();

                                        List<CLS_TurmaNotaAluno> ltNotasBanco = dadosProtocolo.ltTurmaNotaAluno.Where(p => p.tud_id == entTurmaNota.tud_id && p.tnt_id == entTurmaNota.tnt_id).ToList();

                                        sincronizacao.ltTurmaNotaAluno.AddRange(
                                        (
                                            from JObject noToken in AtividadeNotaAluno
                                            join MTR_MatriculaTurmaDisciplina entMtd in lstMatriculasFilhas // alunos matriculados nas disciplinas
                                                on Convert.ToInt64(noToken.GetValue("alu_id", StringComparison.OrdinalIgnoreCase)) equals entMtd.alu_id
                                            join CLS_TurmaNotaAluno nota in ltNotasBanco // notas lançandas no gestão
                                                on Convert.ToInt64(noToken.GetValue("alu_id", StringComparison.OrdinalIgnoreCase)) equals nota.alu_id into notasBanco
                                            from nota in notasBanco.DefaultIfEmpty(new CLS_TurmaNotaAluno()) // LFET JOIN entre as notas do diário e do gestão
                                            let alu_id = Convert.ToInt64(noToken.GetValue("alu_id", StringComparison.OrdinalIgnoreCase))
                                            let tna_avaliacao = noToken.GetValue("ata_avaliacao", StringComparison.OrdinalIgnoreCase).ToString()
                                            let tna_relatorio = noToken.GetValue("ata_relatorio", StringComparison.OrdinalIgnoreCase).ToString()
                                            let tna_participante = (((int)noToken.GetValue("ata_participante", StringComparison.OrdinalIgnoreCase)) > 0)
                                            let tna_dataAlteracao = Convert.ToDateTime(noToken.GetValue("tna_dataAlteracao", StringComparison.OrdinalIgnoreCase) ?? tau_dataAlteracao)
                                            let notaGestaoMaisRecente = nota.tna_dataAlteracao > tna_dataAlteracao || efetivado
                                            where entMtd != null && entMtd.alu_id > 0
                                            select new CLS_TurmaNotaAluno
                                            {
                                                idAtividade = idAtividade,
                                                tud_id = tud_idAtividade,
                                                tnt_id = entTurmaNota.tnt_id > 0 ? entTurmaNota.tnt_id : 1,
                                                IsNew = nota.alu_id <= 0,
                                                alu_id = alu_id,
                                                mtu_id = entMtd.mtu_id,
                                                mtd_id = entMtd.mtd_id,
                                                tna_situacao = tnt_situacao,
                                                tna_participante = !entTurmaAula.permiteAvaliacao || notaGestaoMaisRecente ? nota.tna_participante : tna_participante,
                                                tna_avaliacao = !entTurmaAula.permiteAvaliacao || efetivado ?
                                                                    nota.tna_avaliacao
                                                                    : (tna_participante ?
                                                                        RetornaValorTextoSincronizacao(tna_avaliacao, tnt_dataAlteracao, nota.tna_avaliacao, dataAlteracaoGestao).Replace('.', ',')
                                                                        : null),
                                                tna_relatorio = !entTurmaAula.permiteAvaliacao || efetivado ?
                                                                    nota.tna_relatorio
                                                                    : (tna_participante ?
                                                                        RetornaValorTextoSincronizacao(tna_relatorio, tnt_dataAlteracao, nota.tna_relatorio, dataAlteracaoGestao)
                                                                        : null),
                                                tna_dataAlteracao = !entTurmaAula.permiteAvaliacao || notaGestaoMaisRecente ? nota.tna_dataAlteracao : tna_dataAlteracao
                                            }
                                            ).ToList());
                                    }
                                    #endregion
                                }
                                idAtividade++;
                            }

                            // atualiza o status da atividade avaliativa na aula
                            if (sincronizacao.ltTurmaNota.Any() && atualizaStatusNota)
                                sincronizacao.entityAula.tau_statusAtividadeAvaliativa = (byte)RetornaStatusAtividadeAvaliativa(sincronizacao.ltTurmaNota);

                            #endregion Informação da TurmaNota (referentes à atividade e nota dos alunos)

                            //TODO: verificar se precisa comparar datas para gravar isso. se não enviar esses dados não vai dar certo
                            #region Habilidades do Plano de aula

                            JArray habilidadesPlanosAula = aula.GetValue("listaHabilidadesPlanoAula", StringComparison.OrdinalIgnoreCase) == null ||
                                                           aula.GetValue("listaHabilidadesPlanoAula", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                           ((JArray)aula.GetValue("listaHabilidadesPlanoAula", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                            foreach (JObject habilidade in habilidadesPlanosAula)
                            {
                                CLS_TurmaAulaOrientacaoCurricular turmaAulaOrientacaoCurricular = new CLS_TurmaAulaOrientacaoCurricular
                                {
                                    tud_id = Convert.ToInt32(habilidade.GetValue("tud_id", StringComparison.OrdinalIgnoreCase)),
                                    tau_id = Convert.ToInt32(habilidade.GetValue("tau_id", StringComparison.OrdinalIgnoreCase)) > 0 ? tau_id : -1,
                                    ocr_id = Convert.ToInt32(habilidade.GetValue("ocr_id", StringComparison.OrdinalIgnoreCase)),
                                    tao_alcancado = Convert.ToBoolean(habilidade.GetValue("tao_alcancado", StringComparison.OrdinalIgnoreCase)),
                                    tao_pranejado = Convert.ToBoolean(habilidade.GetValue("tao_pranejado", StringComparison.OrdinalIgnoreCase)),
                                    tao_trabalhado = Convert.ToBoolean(habilidade.GetValue("tao_trabalhado", StringComparison.OrdinalIgnoreCase))
                                };

                                sincronizacao.ltTurmaAulaOrientacaoCurricular.Add(turmaAulaOrientacaoCurricular);
                            }

                            #endregion

                            #region Territórios do saber

                            if (minorVersion >= 60)
                            {
                                if (tud.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia)
                                {
                                    List<TUR_TurmaDisciplinaTerritorio> lstTerritorio = TUR_TurmaDisciplinaTerritorioBO.SelecionaVigentesPorExperienciaPeriodo(entTurmaAula.tud_id, cap_dataInicio, cap_dataFim);

                                    bool dataValida = lstTerritorio.Any();
                                    var vigencias = !lstTerritorio.Any() ? null :
                                                    (from territorio in lstTerritorio
                                                     group territorio by territorio.tte_id into gExp
                                                     select new
                                                     {
                                                         tud_id = gExp.First().tud_idTerritorio
                                                         ,
                                                         VigenciaInicio = gExp.First().tte_vigenciaInicio
                                                         ,
                                                         VigenciaFim = gExp.First().tte_vigenciaFim
                                                         ,
                                                         VigenciaValida = (entTurmaAula.tau_data <= (gExp.First().tte_vigenciaFim == new DateTime() ? entTurmaAula.tau_data : gExp.First().tte_vigenciaFim)
                                                                            && entTurmaAula.tau_data >= gExp.First().tte_vigenciaInicio)
                                                     });

                                    if (entTurmaAula.IsNew && (!dataValida || !vigencias.Any(p => p.VigenciaValida)))
                                    {
                                        // Se a data está fora da vigência.
                                        throw new ValidationException("Não foi possível incluir aula neste dia." +
                                                                    (!dataValida ? " Não há vigência para a experiência no período ("
                                                                    + cap_dataInicio.ToString("dd/MM/yyyy") + " - " + cap_dataFim.ToString("dd/MM/yyyy") + ")." :
                                                                    (vigencias.Count() == 1 ? " Vigência" : " Vigências") +
                                                                    " para a experiência: <br>" + vigencias.Select(v => "(" + v.VigenciaInicio.ToString("dd/MM/yyyy") +
                                                                                                                        " - " + v.VigenciaFim.ToString("dd/MM/yyyy") + ")")
                                                                                                           .Aggregate((a, b) => a + "<br>" + b)));
                                    }

                                    JArray aulasTerritorio = aula.GetValue("AulasTerritorio", StringComparison.OrdinalIgnoreCase) == null ||
                                                                aula.GetValue("AulasTerritorio", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                                ((JArray)aula.GetValue("AulasTerritorio", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                                    foreach (JObject aulaTerritorio in aulasTerritorio)
                                    {
                                        // Id local da Aula
                                        int tau_idTerritorio = (int)aulaTerritorio.GetValue("tau_id", StringComparison.OrdinalIgnoreCase);

                                        //Id da Disciplina
                                        long tud_idTerritorio = (long)aulaTerritorio.GetValue("tud_id", StringComparison.OrdinalIgnoreCase);

                                        if (!vigencias.Any(p => p.tud_id == tud_idTerritorio && p.VigenciaValida))
                                        {
                                            // Se a data está fora da vigência do território.
                                            throw new ValidationException("Território não está vigente na data da aula.");
                                        }

                                        // todos os registros criados pegam a situação da aula para o caso dela estar excluida.
                                        byte tau_situacaoTerritorio;
                                        if (Convert.ToInt32(aulaTerritorio.GetValue("tau_situacao", StringComparison.OrdinalIgnoreCase)) > 0)
                                        {
                                            tau_situacaoTerritorio = Convert.ToByte(aulaTerritorio.GetValue("tau_situacao", StringComparison.OrdinalIgnoreCase));
                                        }
                                        else
                                        {
                                            tau_situacaoTerritorio = 1;
                                        }

                                        // Dados de validação para a aula do protocolo.
                                        sDadosAulaProtocolo dadosProtocoloTerritorio = ltAulasProtocolo.Find(p => p.entityProtocolo.pro_id == protocolo.pro_id && p.entityTurmaDisciplina.tud_id == tud_idTerritorio);
                                        CLS_TurmaAula entTurmaAulaTerritorio = dadosProtocoloTerritorio.entityAula ?? ltAulasBanco.Find(p => p.pro_id == pro_idCriacao && p.tud_id == tud_idTerritorio && p.mesmaAula);

                                        #region Carregar aula mesmo dia
                                        //Carrega a aula no mesmo dia se houver e se permite editar essa aula
                                        if (entTurmaAulaTerritorio == null)
                                        {
                                            entTurmaAulaTerritorio = ltAulasBanco.Find(p => p.pro_id == pro_idCriacao && p.tud_id == tud_idTerritorio && !p.mesmaAula);
                                        }
                                        #endregion Carregar aula mesmo dia

                                        #region Nova aula
                                        //Nova aula
                                        if (entTurmaAulaTerritorio == null)
                                        {
                                            entTurmaAulaTerritorio = new CLS_TurmaAula();
                                            entTurmaAulaTerritorio.tud_id = tud_idTerritorio;
                                            entTurmaAulaTerritorio.tau_id = tau_idTerritorio;
                                            entTurmaAulaTerritorio.tau_data = entTurmaAula.tau_data;
                                            entTurmaAulaTerritorio.tdt_posicao = entTurmaAula.tdt_posicao;
                                            entTurmaAulaTerritorio.tau_reposicao = entTurmaAula.tau_reposicao;
                                            entTurmaAulaTerritorio.permiteEdicaoAula = entTurmaAulaTerritorio.permiteFrequencia = true;
                                            entTurmaAulaTerritorio.pro_id = entTurmaAula.pro_id;
                                            entTurmaAulaTerritorio.IsNew = true;
                                            entTurmaAulaTerritorio.tau_numeroAulas = entTurmaAula.tau_numeroAulas;
                                            entTurmaAulaTerritorio.tur_id = entTurmaAula.tur_id;
                                            entTurmaAulaTerritorio.tpc_id = entTurmaAula.tpc_id;
                                            entTurmaAulaTerritorio.tau_situacao = tau_situacaoTerritorio;
                                            entTurmaAulaTerritorio.usu_id = entTurmaAula.usu_id;
                                            entTurmaAulaTerritorio.tau_efetivado = entTurmaAula.tau_efetivado;
                                            entTurmaAulaTerritorio.tau_statusFrequencia = entTurmaAula.tau_efetivado ? (byte)CLS_TurmaAulaStatusFrequencia.Efetivada : (byte)CLS_TurmaAulaStatusFrequencia.NaoPreenchida;
                                        }
                                        #endregion Nova aula
                                        #region Edição aula
                                        //Aula já existente, edição
                                        else
                                        {
                                            if (entTurmaAulaTerritorio.tau_id < 0)
                                            {
                                                entTurmaAulaTerritorio.tau_id = SelecionaIdAulaPorProcotolo(entTurmaAulaTerritorio.tud_id, entTurmaAula.pro_id, bancoSincronizacao);
                                            }
                                            entTurmaAulaTerritorio.tau_numeroAulas = entTurmaAula.tau_numeroAulas;
                                            entTurmaAulaTerritorio.tur_id = entTurmaAula.tur_id;

                                            //Verifica se enviou a data nova de edição de aula e se é maior que a data do banco
                                            if (entTurmaAula.permiteEdicaoAula)
                                            {
                                                if (!efetivado && tau_situacaoTerritorio != (byte)CLS_TurmaAulaSituacao.Excluido)
                                                    entTurmaAulaTerritorio.tau_situacao = tau_situacaoTerritorio;
                                            }

                                            entTurmaAulaTerritorio.usu_id = entTurmaAula.usu_id;
                                            entTurmaAulaTerritorio.tau_reposicao = entTurmaAula.tau_reposicao;
                                            entTurmaAulaTerritorio.tau_efetivado = entTurmaAula.tau_efetivado;

                                            //Se efetivou a frequencia no tablet e a data de alteração de frequencia é maior então efetiva
                                            if (entTurmaAulaTerritorio.tau_efetivado)
                                            {
                                                entTurmaAulaTerritorio.tau_statusFrequencia = (byte)CLS_TurmaAulaStatusFrequencia.Efetivada;
                                            }
                                            else
                                            {
                                                //Se há alteração de frequência 
                                                if (entTurmaAulaTerritorio.dataLogAlteracaoFreq != new DateTime())
                                                {
                                                    entTurmaAulaTerritorio.tau_statusFrequencia = entTurmaAulaTerritorio.tau_statusFrequencia;
                                                }
                                                else
                                                {
                                                    entTurmaAulaTerritorio.tau_statusFrequencia = (byte)CLS_TurmaAulaStatusFrequencia.NaoPreenchida;
                                                }
                                            }

                                            //Verifica se enviou a data de exclusão e se é maior que a data de alteração de aula do banco
                                            if (!efetivado && tau_situacaoTerritorio == (byte)CLS_TurmaAulaSituacao.Excluido && entTurmaAula.permiteEdicaoAula)
                                                entTurmaAulaTerritorio.tau_situacao = tau_situacaoTerritorio;

                                            entTurmaAulaTerritorio.IsNew = false;
                                        }
                                        #endregion Edição aula

                                        entTurmaAulaTerritorio.tau_dataAlteracao = entTurmaAula.tau_dataAlteracao;
                                        entTurmaAulaTerritorio.tau_dataUltimaSincronizacao = entTurmaAula.tau_dataUltimaSincronizacao;
                                        sincronizacao.ltAulaTerritorio.Add(entTurmaAulaTerritorio);

                                        #region Informações de frequência dos alunos

                                        // Todas as matrículas de aluno na disciplina.
                                        List<MTR_MatriculaTurmaDisciplina> listaMatriculasTerritorio =
                                            (from MTR_MatriculaTurmaDisciplina item in dadosProtocoloTerritorio.ltMatriculaTurmaDisciplina
                                             where
                                                item.tud_id == entTurmaAulaTerritorio.tud_id &&
                                                // Busca a matrícula válida no período do calendário.
                                                item.mtd_dataMatricula <= cap_dataFim &&
                                                (
                                                    item.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Ativo ||
                                                    item.mtd_dataSaida >= cap_dataInicio
                                                ) &&
                                                // Filtra a data da aula.
                                                (
                                                    item.mtd_dataMatricula <= Tau_data &&
                                                    (
                                                        item.mtd_situacao == (byte)MTR_MatriculaTurmaDisciplinaSituacao.Ativo ||
                                                        item.mtd_dataSaida >= Tau_data
                                                    )
                                                )
                                             select item).ToList();

                                        List<CLS_TurmaAulaAluno> ltFreqLancadaTerritorio = new List<CLS_TurmaAulaAluno>();
                                        ltFreqLancadaTerritorio = dadosProtocoloTerritorio.ltTurmaAulaAluno;

                                        JArray alunoListaTerritorio = aulaTerritorio.GetValue("alunos", StringComparison.OrdinalIgnoreCase) == null ||
                                                            aulaTerritorio.GetValue("alunos", StringComparison.OrdinalIgnoreCase).Type == JTokenType.Null ? new JArray() :
                                                            ((JArray)aulaTerritorio.GetValue("alunos", StringComparison.OrdinalIgnoreCase) ?? new JArray());

                                        foreach (JObject aluno in alunoListaTerritorio)
                                        {
                                            long alu_id = (long)aluno.GetValue("alu_id", StringComparison.OrdinalIgnoreCase);

                                            MTR_MatriculaTurmaDisciplina matricula = listaMatriculasTerritorio.Where(p => p.alu_id == alu_id).FirstOrDefault();

                                            if (matricula != null)
                                            {
                                                CLS_TurmaAulaAluno freq = ltFreqLancadaTerritorio.Count > 0 ?
                                                    (
                                                        ltFreqLancadaTerritorio.Exists(p => p.alu_id == alu_id) ?
                                                            ltFreqLancadaTerritorio.Where(p => p.alu_id == alu_id).First() : null
                                                    )
                                                    : null;

                                                bool freqEncontrada = freq != null;
                                                if (freq == null)
                                                    freq = new CLS_TurmaAulaAluno();
                                                var taa_dataAlteracao = Convert.ToDateTime(aluno.GetValue("taa_dataAlteracao", StringComparison.OrdinalIgnoreCase) ?? tau_dataAlteracao);
                                                var freqGestaoMaisRecente = freq.taa_dataAlteracao > taa_dataAlteracao;
                                                freqGestaoMaisRecente |= efetivado;

                                                var taa_frequencia = (int)(aluno.GetValue("taa_frequencia", StringComparison.OrdinalIgnoreCase) ?? 0);
                                                var taa_frequenciaBitMap = (aluno.GetValue("taa_frequenciaBitMap", StringComparison.OrdinalIgnoreCase) ?? "").ToString();

                                                CLS_TurmaAulaAluno freqAluno = new CLS_TurmaAulaAluno
                                                {
                                                    tud_id = entTurmaAulaTerritorio.tud_id,
                                                    tau_id = entTurmaAulaTerritorio.tau_id > 0 ? entTurmaAulaTerritorio.tau_id : 1,
                                                    alu_id = alu_id,
                                                    mtu_id = matricula.mtu_id,
                                                    mtd_id = matricula.mtd_id,
                                                    //Verifica se enviou a data nova de frequência e se é maior que a data do banco
                                                    taa_frequencia = !freqEncontrada || entTurmaAula.permiteFrequencia ?
                                                                        (!freqEncontrada || !freqGestaoMaisRecente || tau_numeroAulasBanco != entTurmaAula.tau_numeroAulas ?
                                                                            taa_frequencia : freq.taa_frequencia)
                                                                        : freq.taa_frequencia,
                                                    taa_situacao = tau_situacaoTerritorio,
                                                    //Verifica se enviou a data nova de frequência e se é maior que a data do banco
                                                    taa_frequenciaBitMap = !freqEncontrada || entTurmaAula.permiteFrequencia ?
                                                                                (!freqEncontrada || !freqGestaoMaisRecente || tau_numeroAulasBanco != entTurmaAula.tau_numeroAulas ?
                                                                                    taa_frequenciaBitMap : freq.taa_frequenciaBitMap)
                                                                                : freq.taa_frequenciaBitMap,
                                                    //Verifica se enviou a data nova de frequência ou anotação e se é maior que a data do banco
                                                    taa_dataAlteracao = !freqEncontrada || entTurmaAula.permiteFrequencia ?
                                                                            taa_dataAlteracao : freq.taa_dataAlteracao,
                                                    IsNew = freq.alu_id <= 0
                                                };

                                                sincronizacao.ltTurmaAulaAluno.Add(freqAluno);
                                            }
                                        }

                                        #endregion Informações de frequência dos alunos
                                    }
                                }
                            }

                            #endregion Territórios do saber

                            sincronizacao.entityProtocolo = protocolo;
                            listAulaSincronizacao.Add(sincronizacao);
                            ltAulasProcessadas.Add(sincronizacao.entityAula);

                            processou = SalvarDadosAulaSincronizacaoDiarioClasse
                                (
                                            ltAulasProtocolo,
                                            ltAulasBanco,
                                            listAulaSincronizacao,
                                            alteraAnotacao,
                                            bancoSincronizacao
                                );

                            if (processou)
                            {
                                if (entTurmaAula.IsNew)
                                    ltAulasBanco.Add(entTurmaAula);
                                else if (ltAulasBanco.Any(p => p.tud_id == entTurmaAula.tud_id && p.tau_data == entTurmaAula.tau_data && p.tdt_posicao == entTurmaAula.tdt_posicao))
                                {
                                    ltAulasBanco.RemoveAll(p => p.tud_id == entTurmaAula.tud_id && p.tau_data == entTurmaAula.tau_data && p.tdt_posicao == entTurmaAula.tdt_posicao);
                                    ltAulasBanco.Add(entTurmaAula);
                                }
                            }

                            retorno &= processou;
                        }

                        if (processou)
                        {
                            // Processou com sucesso.
                            protocolo.pro_statusObservacao = String.Format("Protocolo processado com sucesso ({0}).",
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                            protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComSucesso;
                        }
                        else
                        {
                            if (protocolo.pro_tentativa > tentativasProtocolo)
                            {
                                throw new ValidationException(String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                                    , tentativasProtocolo, protocolo.pro_statusObservacao));
                            }

                            // Não processou sem erro - volta o protocolo para não processado.
                            protocolo.pro_statusObservacao = String.Format("Protocolo não processado ({0}).",
                                DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
                            protocolo.tur_id = -1;
                            protocolo.tud_id = -1;
                            protocolo.tau_id = -1;
                            protocolo.pro_qtdeAlunos = -1;
                            logErro = "Não processou";
                            protocolo.pro_statusObservacao = logErro;
                            protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.NaoProcessado;
                        }

                        dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                    }
                    catch (ArgumentException ex)
                    {
                        // Se ocorrer uma excessão de validação, guardar novo status.
                        protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                        protocolo.pro_statusObservacao = ex.Message;
                        protocolo.tur_id = -1;
                        protocolo.tud_id = -1;
                        protocolo.tau_id = -1;
                        protocolo.pro_qtdeAlunos = -1;
                        dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                        bancoSincronizacao.Close(ex);
                    }
                    catch (ValidationException ex)
                    {
                        // Se ocorrer uma excessão de validação, guardar novo status.
                        protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                        protocolo.pro_statusObservacao = ex.Message;
                        protocolo.tur_id = -1;
                        protocolo.tud_id = -1;
                        protocolo.tau_id = -1;
                        protocolo.pro_qtdeAlunos = -1;
                        dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                        bancoSincronizacao.Close(ex);
                    }
                    catch (Exception ex)
                    {
                        // Se ocorrer uma excessão de erro, guardar novo status.
                        protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErro;
                        protocolo.pro_statusObservacao = ex.Message;
                        protocolo.tur_id = -1;
                        protocolo.tud_id = -1;
                        protocolo.tau_id = -1;
                        protocolo.pro_qtdeAlunos = -1;
                        dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
                        bancoSincronizacao.Close(ex);
                    }
                    finally
                    {
                        if (bancoSincronizacao.ConnectionIsOpen)
                            bancoSincronizacao.Close();
                    }
                }

                DCL_ProtocoloBO.AtualizaProtocolos(dtProtocolo, banco);

                return retorno;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                logErro = "Erro: " + ex.StackTrace;
                ltProtocolo.ForEach(p => { p.pro_status = (byte)DCL_ProtocoloBO.eStatus.NaoProcessado; p.pro_statusObservacao = logErro; });
                DCL_ProtocoloBO.AtualizaListaProtocolos(ltProtocolo);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// Retorna as aulas, com informação de atividades e registros ligados no bimestre, para as aulas passado por parâmetro.
        /// </summary>
        /// <param name="dtAulas"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<sDadosAulaProtocolo> SelecionaDadosPorAulas(DataTable dtAulas, TalkDBTransaction banco = null)
        {
            CLS_TurmaAulaDAO dao = banco == null ? new CLS_TurmaAulaDAO() : new CLS_TurmaAulaDAO { _Banco = banco };
            byte tud_tipoRegencia = (byte)TurmaDisciplinaTipo.Regencia;
            byte tud_tipoComponente = (byte)TurmaDisciplinaTipo.ComponenteRegencia;

            #region Variavies

            List<sProtocoloDataTable> aulas = new List<sProtocoloDataTable>();
            List<sProtocoloDataTable> alunos = new List<sProtocoloDataTable>();
            List<sProtocoloDataTable> regencias = new List<sProtocoloDataTable>();
            List<sProtocoloDataTable> atividades = new List<sProtocoloDataTable>();
            List<sProtocoloDataTable> turmaDisciplinas = new List<sProtocoloDataTable>();
            List<sProtocoloDataTable> aulasBanco = new List<sProtocoloDataTable>();

            #endregion Variavies

            #region Insere dados nas variaveis

            using (DataSet ds = dao.SelecionaDadosPorAulas(dtAulas, tud_tipoRegencia, tud_tipoComponente))
            {
                if (ds.Tables.Count > 5)
                {
                    using (DataTable dtAulaInfo = ds.Tables[0],
                                     dtAluno = ds.Tables[1],
                                     dtRegenciaInfo = ds.Tables[2],
                                     dtAtividadeNota = ds.Tables[3],
                                     dtTurmaDisciplinaTurma = ds.Tables[4],
                                     dtAulasDisciplina = ds.Tables[5])
                    {
                        aulas = dtAulaInfo.Rows.Count > 0 ?
                                dtAulaInfo.Rows.Cast<DataRow>()
                                          .GroupBy(p => new { pro_id = p["pro_id"], tud_id = p["tud_id"] })
                                          .Select(p => new sProtocoloDataTable
                                          {
                                              pro_id = new Guid(p.Key.pro_id.ToString()),
                                              tud_id = Convert.ToInt64(p.Key.tud_id),
                                              dados = p.Any() ? p.CopyToDataTable() : new DataTable()
                                          }).ToList() :
                                new List<sProtocoloDataTable>();

                        alunos = dtAluno.Rows.Count > 0 ?
                                 dtAluno.Rows.Cast<DataRow>()
                                        .GroupBy(p => new { pro_id = p["pro_id"] })
                                        .Select(p => new sProtocoloDataTable
                                        {
                                            pro_id = new Guid(p.Key.pro_id.ToString()),
                                            dados = p.Any() ? p.CopyToDataTable() : new DataTable()
                                        }).ToList() :
                                 new List<sProtocoloDataTable>();

                        regencias = dtRegenciaInfo.Rows.Count > 0 ?
                                    dtRegenciaInfo.Rows.Cast<DataRow>()
                                                       .GroupBy(p => new { pro_id = p["pro_id"] })
                                                       .Select(p => new sProtocoloDataTable
                                                       {
                                                           pro_id = new Guid(p.Key.pro_id.ToString()),
                                                           dados = p.Any() ? p.CopyToDataTable() : new DataTable()
                                                       }).ToList() :
                                    new List<sProtocoloDataTable>();

                        atividades = dtAtividadeNota.Rows.Count > 0 ?
                                     dtAtividadeNota.Rows.Cast<DataRow>()
                                                    .GroupBy(p => new { pro_id = p["pro_idProtocolo"] })
                                                    .Select(p => new sProtocoloDataTable
                                                    {
                                                        pro_id = new Guid(p.Key.pro_id.ToString()),
                                                        dados = p.Any() ? p.CopyToDataTable() : new DataTable()
                                                    }).ToList() :
                                     new List<sProtocoloDataTable>();

                        turmaDisciplinas = dtTurmaDisciplinaTurma.Rows.Count > 0 ?
                                           dtTurmaDisciplinaTurma.Rows.Cast<DataRow>()
                                                                 .GroupBy(p => new { pro_id = p["pro_id"] })
                                                                 .Select(p => new sProtocoloDataTable
                                                                 {
                                                                     pro_id = new Guid(p.Key.pro_id.ToString()),
                                                                     dados = p.Any() ? p.CopyToDataTable() : new DataTable()
                                                                 }).ToList() :
                                           new List<sProtocoloDataTable>();

                        aulasBanco = dtAulasDisciplina.Rows.Count > 0 ?
                                     dtAulasDisciplina.Rows.Cast<DataRow>()
                                                           .GroupBy(p => new { pro_id = p["pro_id"], tud_id = p["tud_id"] })
                                                           .Select(p => new sProtocoloDataTable
                                                           {
                                                               pro_id = new Guid(p.Key.pro_id.ToString()),
                                                               tud_id = Convert.ToInt64(p.Key.tud_id),
                                                               dados = p.Any() ? p.CopyToDataTable() : new DataTable()
                                                           }).ToList() :
                                     new List<sProtocoloDataTable>();
                    }
                }
            }

            #endregion Insere dados nas variaveis

            sProtocoloDataTable defaultValue = new sProtocoloDataTable { pro_id = Guid.Empty, dados = new DataTable() };

            return aulas.Any() ?
                (from aula in aulas // aulas separadas por disciplina do protocolo
                 join aluno in alunos // alunos para a disciplina do protocolo
                     on aula.pro_id equals aluno.pro_id into aal
                 join regencia in regencias // dados de regência para a disciplina do protocolo
                     on aula.pro_id equals regencia.pro_id into are
                 join atividade in atividades // atividades e notas para a disciplina do protocolo
                     on aula.pro_id equals atividade.pro_id into aat
                 join disciplina in turmaDisciplinas // disciplinas da turma do protocolo
                    on aula.pro_id equals disciplina.pro_id into adi
                 join aulaBanco in aulasBanco // aulas existentes para a disciplina do  protocolo
                    on new { pro_id = aula.pro_id, tud_id = aula.tud_id } equals new { pro_id = aulaBanco.pro_id, tud_id = aulaBanco.tud_id } into aab
                 // * LEFT JOIN - Carregar valores padrões caso a junçao não trazer dados.
                 from aluno in aal.DefaultIfEmpty(defaultValue)
                 from regencia in are.DefaultIfEmpty(defaultValue)
                 from atividade in aat.DefaultIfEmpty(defaultValue)
                 from disciplina in adi.DefaultIfEmpty(defaultValue)
                 from aulaBanco in aab.DefaultIfEmpty(defaultValue)
                 let tud_tipo = Convert.ToByte(aula.dados.Rows[0]["tud_tipo"])
                 let tau_id = Convert.ToInt32(aula.dados.Rows[0]["tau_id"])
                 let cap_id = Convert.ToInt32(aula.dados.Rows[0]["cap_id"])
                 let dadoAula = aula.dados.Rows[0]
                 select new sDadosAulaProtocolo
                 {
                     entityProtocolo = (DCL_Protocolo)GestaoEscolarUtilBO.DataRowToEntity(dadoAula, new DCL_Protocolo())
                     ,
                     entityAula = tau_id > 0 ?
                                  dao.DataRowToEntity(aula.dados.Rows[0], new CLS_TurmaAula()) :
                                  null
                     ,
                     entityTurmaDisciplina = (TUR_TurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(dadoAula, new TUR_TurmaDisciplina())
                     ,
                     entityTurma = (TUR_Turma)GestaoEscolarUtilBO.DataRowToEntity(dadoAula, new TUR_Turma())
                     ,
                     turmaIntegral = (Convert.ToByte(aula.dados.Rows[0]["ttn_tipo"]) == (byte)ACA_TipoTurnoBO.TipoTurno.Integral)
                     ,
                     entityCalendarioAnual = (ACA_CalendarioAnual)GestaoEscolarUtilBO.DataRowToEntity(dadoAula, new ACA_CalendarioAnual())
                     ,
                     entityFormatoAvaliacao = (ACA_FormatoAvaliacao)GestaoEscolarUtilBO.DataRowToEntity(dadoAula, new ACA_FormatoAvaliacao())
                     ,
                     entityCurriculoPeriodo = (ACA_CurriculoPeriodo)GestaoEscolarUtilBO.DataRowToEntity(dadoAula, new ACA_CurriculoPeriodo())
                     ,
                     entityCalendarioPeriodo = cap_id > 0 ?
                                               (ACA_CalendarioPeriodo)GestaoEscolarUtilBO.DataRowToEntity(dadoAula, new ACA_CalendarioPeriodo()) :
                                               new ACA_CalendarioPeriodo()
                     ,
                     ltTurmaAulaRegencia = tau_id > 0 && regencia.dados.Rows.Count > 0 && tud_tipo == tud_tipoRegencia ?
                                           regencia.dados.Rows.Cast<DataRow>()
                                                         .GroupBy(p => new { tud_id = p["tud_id"], tau_id = p["tau_id"], tud_idFilho = p["tud_idFilho"] })
                                                         .Select(p => (CLS_TurmaAulaRegencia)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaAulaRegencia())).ToList() :
                                           new List<CLS_TurmaAulaRegencia>()
                     ,
                     ltMatriculaTurmaDisciplina = aluno.dados.Rows.Count > 0 ?
                                                  aluno.dados.Rows.Cast<DataRow>()
                                                                  .GroupBy(p => new
                                                                  {
                                                                      tud_id = p["tud_id"],
                                                                      alu_id = p["alu_id"],
                                                                      mtu_id = p["mtu_id"],
                                                                      mtd_id = p["mtd_id"]
                                                                  })
                                                                  .Select(p => (MTR_MatriculaTurmaDisciplina)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new MTR_MatriculaTurmaDisciplina())).ToList() :
                                                  new List<MTR_MatriculaTurmaDisciplina>()
                     ,
                     ltTurmaNota = atividade.dados.Rows.Count > 0 && tau_id > 0 ?
                                   atividade.dados.Rows.Cast<DataRow>()
                                                       .GroupBy(p => new { tud_id = p["tud_id"], tnt_id = p["tnt_id"] })
                                                       .Select(p => (CLS_TurmaNota)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaNota())).ToList() :
                                   new List<CLS_TurmaNota>()
                     ,
                     ltTurmaAulaAluno = tau_id > 0 && aula.dados.Rows.Count > 0 ?
                                        aula.dados.Rows.Cast<DataRow>()
                                                       .Where(p => Convert.ToInt64(p["alu_id"]) > 0)
                                                       .GroupBy(p => new
                                                       {
                                                           tud_id = p["tud_id"],
                                                           tau_id = p["tau_id"],
                                                           alu_id = p["alu_id"],
                                                           mtu_id = p["mtu_id"],
                                                           mtd_id = p["mtd_id"]
                                                       })
                                                       .Select(p => (CLS_TurmaAulaAluno)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaAulaAluno())).ToList() :
                                        new List<CLS_TurmaAulaAluno>()
                     ,
                     ltTurmaNotaAluno = atividade.dados.Rows.Count > 0 && tau_id > 0 ?
                                        atividade.dados.Rows.Cast<DataRow>()
                                                            .Where(p => Convert.ToInt64(p["alu_id"]) > 0)
                                                            .GroupBy(p => new
                                                            {
                                                                tud_id = p["tud_id"],
                                                                tnt_id = p["tnt_id"],
                                                                alu_id = p["alu_id"],
                                                                mtu_id = p["mtu_id"],
                                                                mtd_id = p["mtd_id"]
                                                            })
                                                            .Select(p => (CLS_TurmaNotaAluno)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaNotaAluno())).ToList() :
                                        new List<CLS_TurmaNotaAluno>()
                     ,
                     ltTurmaAulaRecurso = tau_id > 0 && aula.dados.Rows.Count > 0 ?
                                          aula.dados.Rows.Cast<DataRow>()
                                                         .Where(p => Convert.ToInt32(p["tar_id"]) > 0)
                                                         .GroupBy(p => new { tud_id = p["tud_id"], tau_id = p["tau_id"], tar_id = p["tar_id"] })
                                                         .Select(p => (CLS_TurmaAulaRecurso)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaAulaRecurso())).ToList() :
                                          new List<CLS_TurmaAulaRecurso>()
                     ,
                     ltTurmaAulaRecursoRegencia = tud_tipo == tud_tipoRegencia && tau_id > 0 && regencia.dados.Rows.Count > 0 ?
                                                  regencia.dados.Rows.Cast<DataRow>()
                                                                     .Where(p => Convert.ToInt32(p["trr_id"]) > 0)
                                                                     .GroupBy(p => new { tud_id = p["tud_id"], tau_id = p["tau_id"], trr_id = p["trr_id"] })
                                                                     .Select(p => (CLS_TurmaAulaRecursoRegencia)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaAulaRecursoRegencia())).ToList() :
                                                  new List<CLS_TurmaAulaRecursoRegencia>()
                     ,
                     ltTurmaDisciplinaTurma = disciplina.dados.Rows.Count > 0 ?
                                              disciplina.dados.Rows.Cast<DataRow>()
                                                                   .GroupBy(p => Convert.ToInt64(p["tud_id"]))
                                                                   .Select(p => p.Key).ToList() :
                                              new List<long>()
                     ,
                     ltAulasBanco = aulaBanco.dados.Rows.Count > 0 ?
                                    aulaBanco.dados.Rows.Cast<DataRow>()
                                                        .GroupBy(p => new { tud_id = p["tud_id"], tau_id = p["tau_id"] })
                                                        .Select(p => (CLS_TurmaAula)GestaoEscolarUtilBO.DataRowToEntity(p.First(), new CLS_TurmaAula())).ToList() :
                                    new List<CLS_TurmaAula>()
                 }).ToList() :
                 new List<sDadosAulaProtocolo>();
        }

        /// <summary>
        /// Verifica se já existe uma aula da disciplina da turma cadastrada com o mesmo número da aula retornando
        /// a entidade em uma DataTable
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tau_data">Data da aula da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable DiarioVerificaAulaExistentePorData
        (
            long tud_id
            , DateTime tau_data
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            return dao.DiarioSelectBy_Data(tud_id, tau_data);
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma e as anotações dos alunos (Override criado para obter também a informação da frequência)
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="listTurmaAulaAluno">Lista de anotações dos alunos</param>
        /// <param name="listTurmaAulaRecurso">Lista de recursos usados na aula</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool DiarioSalvarAulaAnotacoesRecursos
        (
            CLS_TurmaAula entity
            , List<CLS_TurmaAulaAluno> listTurmaAulaAluno
            , List<CLS_TurmaAulaRecurso> listTurmaAulaRecurso
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (entity.tau_data == new DateTime())
                    throw new ValidationException("Data da aula é obrigatório.");

                TUR_TurmaDisciplina tud = new TUR_TurmaDisciplina { tud_id = entity.tud_id };
                TUR_TurmaDisciplinaBO.GetEntity(tud, dao._Banco);

                if (tud.tud_tipo == 5)
                {
                    entity.tau_numeroAulas = (entity.tau_numeroAulas > 1 ? 1 : entity.tau_numeroAulas);
                }

                // Chama método padrão para salvar a aula
                if (entity.Validate())
                    /*
                     * Faz a validação dos tempos de aula:
                     * Quando se trata de uma disciplina normal, soma-se a quantidade de aulas enviadas na semana e compara-se o total com o campo tud_cargaHorariaSemanal
                     * Quando se trata de uma disciplina global (tud_global = 1), soma-se a quantidade de aulas enviadas e compara-se com o total de carga horaria semanal de todas as disciplinas da turma
                     * Quando se trata de disciplina principal, não é feita validação alguma
                     */
                    Save(entity, dao._Banco);
                //SaveAndValidade(entity, dao._Banco);
                else
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));

                if (origemLogAula > 0)
                {
                    DateTime dataLogAula = DateTime.Now;
                    LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                    {
                        tud_id = entity.tud_id,
                        tau_id = entity.tau_id,
                        usu_id = usu_id,
                        lta_origem = origemLogAula,
                        lta_tipo = tipoLogAula,
                        lta_data = dataLogAula
                    };

                    LOG_TurmaAula_AlteracaoBO.Save(entLogAula, dao._Banco);
                }

                // Salva as anotações dos alunos na aula da disciplina da turma
                foreach (CLS_TurmaAulaAluno aux in listTurmaAulaAluno)
                {
                    // Verifica se já existe uma anotação ou frequência cadastrada
                    CLS_TurmaAulaAluno entityTurmaAulaAluno = new CLS_TurmaAulaAluno
                    {
                        tud_id = aux.tud_id
                        ,
                        tau_id = entity.tau_id
                        ,
                        alu_id = aux.alu_id
                        ,
                        mtu_id = aux.mtu_id
                        ,
                        mtd_id = aux.mtd_id
                    };
                    CLS_TurmaAulaAlunoBO.GetEntity(entityTurmaAulaAluno, dao._Banco);

                    // Atualiza a anotação da entidade para salvar
                    entityTurmaAulaAluno.taa_anotacao = aux.taa_anotacao;
                    entityTurmaAulaAluno.taa_situacao = 1;

                    /*********************************************************************/
                    //Quando for disciplina principal, pode-se atribuir apenas 1 registro de frequencia
                    if (tud.tud_tipo == 5)
                    {
                        aux.taa_frequencia = (aux.taa_frequencia > 1 ? 1 : aux.taa_frequencia);
                    }

                    //Colhendo também a informação sobre frequência dos alunos (Só para o serviço ProcessaDiarioClasse)
                    entityTurmaAulaAluno.taa_frequencia = aux.taa_frequencia;

                    /*********************************************************************/

                    // Salva as anotações dos alunos
                    if (entityTurmaAulaAluno.Validate())
                    {
                        CLS_TurmaAulaAlunoBO.Save(entityTurmaAulaAluno, dao._Banco);
                    }
                    else
                    {
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityTurmaAulaAluno));
                    }
                }

                //Carrega Recursos gravados no banco
                List<CLS_TurmaAulaRecurso> listaBanco = CLS_TurmaAulaRecursoBO.GetSelectBy_Turma_Aula(entity.tud_id
                                                                                                      , entity.tau_id);
                //busca registros que devem ser excluidos
                IEnumerable<Int32> dadosTela =
                (from CLS_TurmaAulaRecurso dr in listTurmaAulaRecurso.AsEnumerable()
                 orderby dr.rsa_id descending
                 select dr.rsa_id).AsEnumerable();

                IEnumerable<Int32> dadosExcluir =
                    (from CLS_TurmaAulaRecurso t in listaBanco.AsEnumerable()
                     orderby t.rsa_id descending
                     select t.rsa_id).Except(dadosTela);

                IList<Int32> dadosDif = dadosExcluir.ToList();
                //deleta registros que foram desmarcados
                for (int i = 0; i < dadosDif.Count; i++)
                {
                    CLS_TurmaAulaRecursoBO.Delete_Byrsa_id(entity.tud_id, entity.tau_id, dadosDif[i], dao._Banco);
                }

                //busca registro que devem ser alterados
                IEnumerable<Int32> dadosBanco =
                    (from CLS_TurmaAulaRecurso t in listaBanco.AsEnumerable()
                     orderby t.rsa_id descending
                     select t.rsa_id).AsEnumerable();

                IEnumerable<Int32> dadosAlterar =
                    (from CLS_TurmaAulaRecurso t in listTurmaAulaRecurso.AsEnumerable()
                     orderby t.rsa_id descending
                     select t.rsa_id).Intersect(dadosBanco);

                IList<Int32> dadosAlte = dadosAlterar.ToList();
                CLS_TurmaAulaRecurso entityAltera;
                for (int i = 0; i < dadosAlte.Count; i++)
                {
                    entityAltera = listTurmaAulaRecurso.Find(p => p.rsa_id == dadosAlte[i]);
                    entityAltera.tar_dataAlteracao = DateTime.Now;
                    CLS_TurmaAulaRecursoBO.Update_Byrsa_id(entityAltera, dao._Banco);
                    listTurmaAulaRecurso.Remove(entityAltera);
                }

                // Salva as recursos utilizados na aula
                foreach (CLS_TurmaAulaRecurso aux in listTurmaAulaRecurso)
                {
                    aux.tau_id = entity.tau_id;
                    if (aux.Validate())
                        CLS_TurmaAulaRecursoBO.Salvar(aux, dao._Banco);
                    else
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(aux));
                }

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

        /// <summary>
        /// retorna os dados de aula por turma e data base, retornando registros ativos e excluidos.
        /// </summary>
        /// <param name="tur_id">id da turma</param>
        /// <param name="dataBase">data base para a consulta</param>
        /// <returns></returns>
        public static DataSet BuscarAulasPorTurmaDataBase(Int64 tur_id, DateTime dataBase)
        {
            return new CLS_TurmaAulaDAO().BuscarAulasPorTurmaDataBase(tur_id, dataBase);
        }

        /// <summary>
        /// retorna os dados de aula por escola e data base, retornando registros ativos e excluidos.
        /// </summary>
        /// <param name="esc_id">id da escola</param>
        /// <param name="dataBase">data base para a consulta</param>
        /// <returns></returns>
        public static DataSet BuscarAulasPorEscolaDataBase(Int32 esc_id, DateTime dataBase)
        {
            return new CLS_TurmaAulaDAO().BuscarAulasPorEscolaDataBase(esc_id, dataBase);
        }

        /// <summary>
        /// returna uma dataset com diversos datatable referente a dados da aula por um determinado periodo
        /// </summary>
        /// <param name="tud_id">id da turma disciplina</param>
        /// <param name="dataInicio">data de inicio do periodo</param>
        /// <param name="dataFim">data fim do periodo</param>
        /// <param name="usu_id">ID do usuário que criou a aula</param>
        /// <returns></returns>
        public static DataSet BuscarAulasPorTurmaDisciplinaPeriodo(Int64 tud_id, byte tdt_posicao, DateTime dataInicio, DateTime dataFim, Guid usu_id)
        {
            return new CLS_TurmaAulaDAO().BuscarAulasPorTurmaDisciplinaPeriodo(tud_id, tdt_posicao, dataInicio, dataFim, usu_id);
        }

        /// <summary>
        /// returna uma dataset com diversos datatable referente a dados de uma aula
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tau_id"></param>       
        /// <returns></returns>
        public static DataSet BuscarAula(Int64 tud_id, Int32 tau_id)
        {
            return new CLS_TurmaAulaDAO().BuscarAula(tud_id, tau_id);
        }

        /// <summary>
        /// Busca as ultimas aulas de acordo com o tud_id.
        /// </summary>
        /// <param name="tud_id">Disciplina</param>
        /// <param name="tur_id">Turma</param>
        /// <param name="diasTras">Quantidade de dias para tras</param>
        /// <param name="diasFrente">Quantidade de dias para frente</param>
        /// <param name="primeiraSincronizacao">Indica se é a primeira sincronização do tablet (caso for, só traz ativos, se não for, traz excluídos também)</param>
        /// <returns></returns>
        public static DataSet BuscaUltimasAulasPorTurmaDisciplina
        (
            Int64 tud_id,
            Int64 tur_id,
            Int32 paraTras,
            Int32 paraFrente,
            bool primeiraSincronizacao
        )
        {
            return new CLS_TurmaAulaDAO().BuscaUltimasAulasPorTurmaDisciplina(tud_id, tur_id, paraTras, paraFrente, primeiraSincronizacao);
        }

        #region Validacao

        /// <summary>
        /// O método valida a aula para sincronização.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listaValidacao"></param>
        /// <param name="ltAulasBanco"></param>
        /// <returns></returns>
        public static bool ValidarAula
        (
            CLS_TurmaAula entity,
            List<sDadosAulaProtocolo> listaValidacao,
            List<CLS_TurmaAula> ltAulasBanco
        )
        {
            if (entity.Validate())
            {
                if (entity.tau_data == new DateTime())
                    throw new ValidationException("Data da aula é obrigatório.");

                // Verifica se a aula foi alterada/excluída por outra pessoa enquanto o usuário tentava alterar a mesma.
                CLS_TurmaAula entityAulaAuxiliar = ltAulasBanco.Any(p => p.tud_id == entity.tud_id && p.tau_id == entity.tau_id) ?
                    ltAulasBanco.Find(p => p.tud_id == entity.tud_id && p.tau_id == entity.tau_id) :
                    new CLS_TurmaAula();

                if (entityAulaAuxiliar != new CLS_TurmaAula() && !entityAulaAuxiliar.IsNew)
                {
                    entity.usu_id = entityAulaAuxiliar.usu_id;
                    entity.tdt_posicao = entityAulaAuxiliar.tdt_posicao;
                }

                //if (entityAulaAuxiliar != new CLS_TurmaAula() && !entityAulaAuxiliar.IsNew && entityAulaAuxiliar.tau_dataAlteracao != entity.tau_dataAlteracao)
                //    throw new EditarAula_ValidationException("Esta aula já foi alterada recentemente.");

                // Verifica se existe a aula cadastrada
                if (ltAulasBanco.Any(p => p.tud_id == entity.tud_id && (entity.IsNew || entity.tau_id != p.tau_id) && p.tau_sequencia == entity.tau_sequencia && p.tau_sequencia > 0))
                    throw new DuplicateNameException("Já existe uma aula cadastrada com este número.");

                // Verifica se a data da aula está dentro do calendário e do período do calendário
                if (entity.tau_data != new DateTime() && entity.tur_id > 0)
                {
                    bool permiteVariasAulas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_INCLUIR_VARIAS_AULAS_POR_DIA, new Guid());

                    if (!permiteVariasAulas
                        &&
                        (
                            (entity.IsNew
                            && ltAulasBanco.Any(p => p.tud_id == entity.tud_id && p.tdt_posicao == entity.tdt_posicao && p.tau_data == entity.tau_data))
                            ||
                            (entity.tau_id > 0
                            && ltAulasBanco.Any(p => p.tud_id == entity.tud_id && p.tdt_posicao == entity.tdt_posicao && p.tau_data == entity.tau_data && p.tau_id != entity.tau_id))
                        ))
                    {
                        throw new ArgumentException(String.Format("Já existe uma aula cadastrada para o dia {0}", entity.tau_data.ToString("dd/MM/yyyy")));
                    }

                    TUR_Turma tur = listaValidacao.Find(p => p.entityTurma.tur_id == entity.tur_id).entityTurma;

                    ACA_CalendarioAnual cal = listaValidacao.Find(p => p.entityCalendarioAnual.cal_id == tur.cal_id).entityCalendarioAnual;

                    if (entity.tau_data.Date > cal.cal_dataFim.Date || entity.tau_data.Date < cal.cal_dataInicio.Date)
                        throw new ArgumentException("A data da aula deve estar dentro do período do calendário escolar (" + cal.cal_dataInicio.ToString("dd/MM/yyyy") + " - " + cal.cal_dataFim.ToString("dd/MM/yyyy") + ").");

                    DataTable dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorTipoPeriodoCalendario(entity.tpc_id, tur.cal_id);

                    ACA_CalendarioPeriodo cap = listaValidacao.Find(p => p.entityCalendarioPeriodo.cal_id == tur.cal_id && p.entityCalendarioPeriodo.tpc_id == entity.tpc_id).entityCalendarioPeriodo;

                    if (cap != null && cap.cap_id > 0)
                    {
                        if (entity.tau_data.Date > Convert.ToDateTime(cap.cap_dataFim).Date || entity.tau_data.Date < Convert.ToDateTime(cap.cap_dataInicio).Date)
                            throw new ArgumentException("A data da aula deve estar dentro do período do calendário (" + cap.cap_dataInicio.Date + " - " + cap.cap_dataFim.Date + ").");
                    }
                }

                // Verifica se a quantidade de aulas semanais não foram ultrapassadas.
                if (entity.tau_data != new DateTime() && entity.tur_id > 0)
                {
                    TUR_TurmaDisciplina tud = listaValidacao.Find(p => p.entityTurmaDisciplina.tud_id == entity.tud_id).entityTurmaDisciplina;

                    bool DisciplinaPrincipal = tud.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal;

                    // Se for a disciplina principal não precisa validar.
                    if (!DisciplinaPrincipal && !entity.tau_reposicao)
                    {
                        DateTime dataInicial = new DateTime();
                        DateTime dataFinal = new DateTime();
                        switch (entity.tau_data.DayOfWeek)
                        {
                            case DayOfWeek.Sunday:
                                dataInicial = entity.tau_data;
                                dataFinal = entity.tau_data.AddDays(6);
                                break;

                            case DayOfWeek.Monday:
                                dataInicial = entity.tau_data.AddDays(-1);
                                dataFinal = entity.tau_data.AddDays(5);
                                break;

                            case DayOfWeek.Tuesday:
                                dataInicial = entity.tau_data.AddDays(-2);
                                dataFinal = entity.tau_data.AddDays(4);
                                break;

                            case DayOfWeek.Wednesday:
                                dataInicial = entity.tau_data.AddDays(-3);
                                dataFinal = entity.tau_data.AddDays(3);
                                break;

                            case DayOfWeek.Thursday:
                                dataInicial = entity.tau_data.AddDays(-4);
                                dataFinal = entity.tau_data.AddDays(2);
                                break;

                            case DayOfWeek.Friday:
                                dataInicial = entity.tau_data.AddDays(-5);
                                dataFinal = entity.tau_data.AddDays(1);
                                break;

                            case DayOfWeek.Saturday:
                                dataInicial = entity.tau_data.AddDays(-6);
                                dataFinal = entity.tau_data;
                                break;
                        }

                        int CargaHoraria = tud.tud_cargaHorariaSemanal;

                        int quantidadeAulas = (from CLS_TurmaAula aula in ltAulasBanco
                                               where aula.tud_id == entity.tud_id &&
                                                     aula.tau_data != entity.tau_data &&
                                                     aula.tau_data >= dataInicial &&
                                                     aula.tau_data <= dataFinal &&
                                                     aula.tdt_posicao == entity.tdt_posicao &&
                                                     !aula.tau_reposicao
                                               select aula.tau_numeroAulas).Sum() + (entity.tau_reposicao ? 0 : entity.tau_numeroAulas);

                        if (quantidadeAulas > CargaHoraria)
                            throw new ArgumentException("A soma da quantidade de aulas da semana do dia " +
                                                        dataInicial.ToString("dd/MM/yyyy") + " ao dia " +
                                                        dataFinal.ToString("dd/MM/yyyy") +
                                                        " não pode ser maior do que " + CargaHoraria + ".");
                    }
                }

                return true;
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        #endregion Validacao

        #endregion Sincronização com diário de classe

        #region Consultas

        /// <summary>
        /// Retorna as aulas criadas pelo cargo especificado (atribuiçao esporádica).
        /// </summary>
        /// <param name="col_id"></param>
        /// <param name="crg_id"></param>
        /// <param name="coc_id"></param>
        public static DataTable PesquisaPor_AtribuicaoEsporadica(long col_id, int crg_id, int coc_id, TalkDBTransaction banco)
        {
            if (banco == null)
            {
                return new CLS_TurmaAulaDAO().PesquisaPor_AtribuicaoEsporadica(col_id, crg_id, coc_id);
            }
            else
            {
                return new CLS_TurmaAulaDAO() { _Banco = banco }.PesquisaPor_AtribuicaoEsporadica(col_id, crg_id, coc_id);
            }
        }

        /// <summary>
        /// Seleciona o id da aula que o protocolo passado por parâmetro gerou.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da aula.</param>
        /// <param name="pro_id">ID do protocolo.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static int SelecionaIdAulaPorProcotolo(long tud_id, Guid pro_id, TalkDBTransaction banco = null)
        {
            return banco == null ?
                new CLS_TurmaAulaDAO().SelecionaIdAulaPorProcotolo(tud_id, pro_id) :
                new CLS_TurmaAulaDAO { _Banco = banco }.SelecionaIdAulaPorProcotolo(tud_id, pro_id);
        }

        /// <summary>
        /// Retorna as aulas, com informação de atividades e registros ligados
        /// no bimestre, para as disciplinas e a posição do docente naquela disciplina.
        /// </summary>
        /// <param name="tud_id">IDs das disciplinas</param>
        /// <param name="tpc_id">ID do bimestre</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static DataTable SelecionaAulasAtividadesPor_DisicplinasDocentePeriodo
        (
            string tud_id
            , int tpc_id
            , long doc_id
            , TalkDBTransaction banco = null
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            if (banco != null) dao._Banco = banco;

            return dao.SelectBy_DisicplinasDocentePeriodo(tud_id, tpc_id, doc_id);
        }

        /// <summary>
        /// Retorna as aulas, com informação de atividades e registros ligados
        /// no bimestre, para as disciplinas e a posição do docente naquela disciplina.
        /// </summary>
        /// <param name="tud_id">IDs das disciplinas</param>
        /// <param name="tpc_id">ID do bimestre</param>
        /// <param name="doc_id">ID do docente</param>
        /// <param name="banco">Transação com banco</param>
        /// <returns></returns>
        public static List<CLS_TurmaAula> SelecionaListaAulasAtividadesPor_DisicplinasDocentePeriodo
        (
            string tud_id
            , int tpc_id
            , long doc_id
            , TalkDBTransaction banco
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO { _Banco = banco };
            return dao.SelectBy_DisicplinasDocentePeriodo(tud_id, tpc_id, doc_id).Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new CLS_TurmaAula())).ToList();
        }

        /// <summary>
        ///	Retorna as aulas da disciplina da turma e do período do calendário
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        ///<param name="tdt_posicao">Posição do docente</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaDisciplinaPeriodoCalendario
        (
            long tud_id
            , int tpc_id
            , Guid ent_id
            , Guid usu_id
            , byte tdt_posicao = 0
            , bool usuario_superior = false
            , long tud_idRelacionada = -1
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            return dao.SelectBy_tud_id_tpc_id(tud_id, tpc_id, ent_id, usu_id, tdt_posicao, usuario_superior, tud_idRelacionada);
        }

        /// <summary>
        ///	Retorna as aulas da disciplina da turma e do período do calendário
        ///	mais as atividades que não estejam ligada as aulas
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do Período do calendário</param>
        /// <param name="tud_idsComponentes">ID das disciplinas componentes da regência, se houver</param>
        /// <param name="ent_id">Entidade do usuário logado</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAulaAtividadeAvaliativa
        (
            long tud_id
            , int tpc_id
            , string tud_idsComponentes
            , Guid ent_id
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            return dao.SelectBy_AulaAtividadeAvaliativa(tud_id, tpc_id, tud_idsComponentes, ent_id);
        }

        /// <summary>
        ///	Retorna todas as anotações por aluno e periodo do calendário
        /// </summary>
        /// <param name="alu_id">ID do aluno</param>
        /// <param name="tpc_ids">String de ids do Período do calendário</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaAnotacaoPorAlunoPeriodoCalendario
        (
            long alu_id
            , string tpc_ids
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            return dao.SelectBy_AlunoPeriodoCalendario(alu_id, tpc_ids);
        }

        /// <summary>
        /// Verifica se já existe uma aula da disciplina da turma cadastrada com o mesmo número da aula
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="banco"></param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaAulaExistente
        (
            CLS_TurmaAula entity
            , TalkDBTransaction banco
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO { _Banco = banco };
            return dao.SelectBy_Nome(entity.tud_id, entity.tau_id, entity.tau_sequencia);
        }

        /// <summary>
        /// Verifica se já existe uma aula da disciplina da turma cadastrada com o mesmo número da aula
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="tau_data">Data da aula da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaAulaExistentePorDataPosicaoDocente
        (
            long tud_id
            , byte tdt_posicao
            , DateTime tau_data
            , int tau_id = -1
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            DataTable dt = new DataTable();
            if (tau_data > SqlDateTime.MinValue)
            {
                dt = dao.SelectBy_DataPosicaoDocente(tud_id, tdt_posicao, tau_data, tau_id);
            }

            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// Verifica se já existe uma aula da disciplina da turma cadastrada com o mesmo número da aula
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="tau_data">Data da aula da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static bool VerificaAulaExistentePorDataPosicaoDocente
        (
            long tud_id
            , byte tdt_posicao
            , DateTime tau_data
            , out int tau_numeroAulas
            , out int tau_id
            , TalkDBTransaction banco = null
        )
        {
            CLS_TurmaAulaDAO dao = banco == null ?
                new CLS_TurmaAulaDAO() :
                new CLS_TurmaAulaDAO { _Banco = banco };
            DataTable dt = dao.SelectBy_DataPosicaoDocente(tud_id, tdt_posicao, tau_data);

            tau_numeroAulas = Convert.ToInt32(dt.Rows.Count > 0 ? dt.Rows[0]["tau_numeroAulas"] : 0);
            tau_id = Convert.ToInt32(dt.Rows.Count > 0 ? dt.Rows[0]["tau_id"] : 0);
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// Retorna aula por data e posição do docente
        /// </summary>
        /// <param name="tud_id">Disciplina</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="tau_data">Data da aula</param>
        /// <returns>Registro de CLS_TurmaAula</returns>
        public static CLS_TurmaAula BuscarAulaPorDataPosicaoDocente(
            long tud_id
            , byte tdt_posicao
            , DateTime tau_data)
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            DataTable dt = dao.SelectBy_DataPosicaoDocente(tud_id, tdt_posicao, tau_data);

            if (dt.Rows.Count > 0)
            {
                CLS_TurmaAula turmaAula = new CLS_TurmaAula
                {
                    tud_id = Convert.ToInt32(dt.Rows[0]["tud_id"]),
                    tau_id = Convert.ToInt16(dt.Rows[0]["tau_id"])
                };

                return GetEntity(turmaAula);
            }

            return null;
        }

        /// <summary>
        /// Verifica e retorna a última sequência de aula cadastrada
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="dataInicial">Data do início da semana</param>
        /// <param name="dataFinal">Data do fim da semana</param>
        /// <param name="banco">Transação</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int VerificaSomaNumeroAulasSemana
        (
            long tud_id
            , DateTime dataInicial
            , DateTime dataFinal
            , TalkDBTransaction banco
            , byte? tdt_posicao = null
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            if (banco != null)
                dao._Banco = banco;

            return dao.SelectBy_Semana(tud_id, tdt_posicao ?? 0, dataInicial, dataFinal);
        }

        /// <summary>
        /// Verifica e retorna a última sequência de aula cadastrada
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static int VerificaUltimaAulaCadastrada
        (
            long tud_id
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            return dao.SelectBy_tud_id_top_one(tud_id);
        }

        /// <summary>
        /// Seleciona as aulas por período do calendário, turma disciplina e dia da semana.
        /// </summary>
        /// <param name="tud_id">ID da turma disciplina.</param>
        /// <param name="tpc_id">ID do tipo de período do calendário.</param>
        /// <param name="dataInicial">Data inicial.</param>
        /// <param name="dataFinal">Data final.</param>
        /// <param name="dayOfWeek">Dia da semana.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<CLS_TurmaAula> SelecionaPorPeriodoCalendarioDisciplinaDiaSemana
        (
            long tud_id,
            int tpc_id,
            DateTime dataInicial,
            DateTime dataFinal,
            DayOfWeek dayOfWeek,
            byte tdt_posicao,
            TalkDBTransaction banco = null
        )
        {
            byte diaSemana = 0;

            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    diaSemana = 1;
                    break;

                case DayOfWeek.Monday:
                    diaSemana = 2;
                    break;

                case DayOfWeek.Tuesday:
                    diaSemana = 3;
                    break;

                case DayOfWeek.Wednesday:
                    diaSemana = 4;
                    break;

                case DayOfWeek.Thursday:
                    diaSemana = 5;
                    break;

                case DayOfWeek.Friday:
                    diaSemana = 6;
                    break;

                case DayOfWeek.Saturday:
                    diaSemana = 7;
                    break;

                default:
                    break;
            }

            if (diaSemana == 0)
                throw new ValidationException("Dia da semana inválido.");

            CLS_TurmaAulaDAO dao = banco == null ?
                new CLS_TurmaAulaDAO() : new CLS_TurmaAulaDAO { _Banco = banco };

            return dao.SelecionaPorPeriodoCalendarioDisciplinaDiaSemana(tud_id, tpc_id, dataInicial, dataFinal, diaSemana, tdt_posicao);
        }

        /// <summary>
        /// Retorna a quantidade de aulas dadas no bimestre e a quantidade de aulas de reposicação.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="tdc_ids"></param>
        /// <param name="qtdeAulasReposicao"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static int SelecionaQuantidadeAulasDadas(long tud_id, int tpc_id, string tdc_ids, out int qtdeAulasReposicao, out int qtdeAulasDadasPeriodo, out int qtdeAulasDadasReposicaoPeriodo, TalkDBTransaction banco = null, int appMinutosCacheLongo = 0)
        {
            DataTable dados = null;

            Func<DataTable> retorno = delegate ()
            {
                dados = banco == null ?
                new CLS_TurmaAulaDAO().SelecionaQuantidadeAulasDadas(tud_id, tpc_id, tdc_ids) :
                new CLS_TurmaAulaDAO { _Banco = banco }.SelecionaQuantidadeAulasDadas(tud_id, tpc_id, tdc_ids);
                return dados;
            };

            //Não usar cache para essa busca
            appMinutosCacheLongo = 0;
            if (appMinutosCacheLongo > 0)
            {
                string chave = String.Format(ModelCache.TURMA_AULA_QTDE_AULASDADAS_MODEL_KEY, tud_id, tpc_id, tdc_ids);

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

            int qtdeAulasDadas = qtdeAulasReposicao = qtdeAulasDadasPeriodo = qtdeAulasDadasReposicaoPeriodo = 0;

            if (dados.Rows.Count > 0)
            {
                Int32.TryParse(dados.Rows[0]["qtdeAulasDadas"].ToString(), out qtdeAulasDadas);
                Int32.TryParse(dados.Rows[0]["qtdeAulasReposicao"].ToString(), out qtdeAulasReposicao);
                Int32.TryParse(dados.Rows[0]["qtdeAulasDadasPeriodo"].ToString(), out qtdeAulasDadasPeriodo);
                Int32.TryParse(dados.Rows[0]["qtdeAulasReposicaoPeriodo"].ToString(), out qtdeAulasDadasReposicaoPeriodo);
            }

            return qtdeAulasDadas;
        }

        /// <summary>
        /// Retorna um datarow com dados da entidade da aula.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataRow TurmaAulaToDataRow(CLS_TurmaAula entity, DataRow dr, DateTime tau_dataAlteracao = new DateTime(), long idAula = -1)
        {
            if (idAula > 0)
            {
                dr["idAula"] = idAula;
            }

            dr["tud_id"] = entity.tud_id;
            dr["tau_id"] = entity.tau_id;
            dr["tpc_id"] = entity.tpc_id;

            if (entity.tau_sequencia > 0)
                dr["tau_sequencia"] = entity.tau_sequencia;
            else
                dr["tau_sequencia"] = DBNull.Value;

            if (entity.tau_data != new DateTime())
                dr["tau_data"] = entity.tau_data;
            else
                dr["tau_data"] = DBNull.Value;

            if (entity.tau_numeroAulas > 0)
                dr["tau_numeroAulas"] = entity.tau_numeroAulas;
            else
                dr["tau_numeroAulas"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tau_planoAula))
                dr["tau_planoAula"] = entity.tau_planoAula;
            else
                dr["tau_planoAula"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tau_diarioClasse))
                dr["tau_diarioClasse"] = entity.tau_diarioClasse;
            else
                dr["tau_diarioClasse"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tau_conteudo))
                dr["tau_conteudo"] = entity.tau_conteudo;
            else
                dr["tau_conteudo"] = DBNull.Value;

            dr["tau_efetivado"] = entity.tau_efetivado;

            if (!string.IsNullOrEmpty(entity.tau_atividadeCasa))
                dr["tau_atividadeCasa"] = entity.tau_atividadeCasa;
            else
                dr["tau_atividadeCasa"] = DBNull.Value;

            dr["tau_situacao"] = entity.tau_situacao;
            dr["tdt_posicao"] = entity.tdt_posicao;

            if (entity.pro_id != Guid.Empty)
                dr["pro_id"] = entity.pro_id;
            else
                dr["pro_id"] = DBNull.Value;

            if (!string.IsNullOrEmpty(entity.tau_sintese))
                dr["tau_sintese"] = entity.tau_sintese;
            else
                dr["tau_sintese"] = DBNull.Value;

            dr["tau_reposicao"] = entity.tau_reposicao;

            if (entity.usu_id != Guid.Empty)
                dr["usu_id"] = entity.usu_id;
            else
                dr["usu_id"] = DBNull.Value;

            if (tau_dataAlteracao != new DateTime())
                dr["tau_dataAlteracao"] = tau_dataAlteracao;
            else
                dr["tau_dataAlteracao"] = DBNull.Value;

            if (entity.usu_idDocenteAlteracao != Guid.Empty)
                dr["usu_idDocenteAlteracao"] = entity.usu_idDocenteAlteracao;
            else
                dr["usu_idDocenteAlteracao"] = DBNull.Value;

            dr["tau_statusFrequencia"] = entity.tau_statusFrequencia;
            dr["tau_statusAtividadeAvaliativa"] = entity.tau_statusAtividadeAvaliativa;
            dr["tau_statusAnotacoes"] = entity.tau_statusAnotacoes;
            dr["tau_statusPlanoAula"] = entity.tau_statusPlanoAula;
            dr["tau_checadoAtividadeCasa"] = entity.tau_checadoAtividadeCasa;
            if (entity.tau_dataUltimaSincronizacao != new DateTime())
                dr["tau_dataUltimaSincronizacao"] = entity.tau_dataUltimaSincronizacao;
            else
                dr["tau_dataUltimaSincronizacao"] = DBNull.Value;

            return dr;
        }

        /// <summary>
        /// Retorna as informações de planos de aula para o listão
        /// </summary>
        /// <param name="tud_id">ID da disciplina</param>
        /// <param name="tpc_id">ID do periodo do calendario</param>
        /// <returns></returns>
        public static DataTable SelecionaPlanosAulaPor_Disciplina
        (
            Int64 tud_id
            , int tpc_id
            , Guid usu_id
            , byte tdt_posicao
            , bool usuario_superior
            , long tud_idRelacionada
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            return dao.SelectBy_Disciplina(tud_id, tpc_id, usu_id, tdt_posicao, usuario_superior, tud_idRelacionada);
        }

        /// <summary>
        /// Seleciona a lista com as aulas de acordo com os tau_ids informados.
        /// </summary>
        /// <param name="tud_id">ID da disciplina da aula.</param>
        /// <param name="tau_ids">IDs das aulas.</param>
        /// <returns>Lista com as aulas</returns>
        public static List<CLS_TurmaAula> SelecionarListaAulasPorIds(long tud_id, string tau_ids)
        {
            return new CLS_TurmaAulaDAO().SelecionarListaAulasPorIds(tud_id, tau_ids);
        }

        /// <summary>
        /// Verifica se existe aula criada para Experiência (Território do Saber) de acordo com a vigência da Experiência
        /// </summary>
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do período do calendário (bimestre)</param>
        /// <returns>False: Não tem pendência | True: Tem pendência</returns>
        public static bool VerificaPendenciaCadastroAulaExperiencia(long tud_id, int tpc_id)
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            return dao.VerificaPendenciaCadastroAulaExperiencia(tud_id, tpc_id);
        }

        #endregion Consultas

        #region Métodos

        /// <summary>CLS_TurmaAulaStatusAnotacoes
        /// Retorna o Status da Atividade Avaliativa
        /// </summary>
        /// <param name="listTurmaNota">Lista de TurmaNota</param>
        /// <returns></returns>
        public static CLS_TurmaAulaStatusAtividadeAvaliativa RetornaStatusAtividadeAvaliativa(List<CLS_TurmaNota> listTurmaNota)
        {
            CLS_TurmaAulaStatusAtividadeAvaliativa retorno;

            int numEfetivados = listTurmaNota.Count(p => p.tnt_efetivado && p.tnt_situacao != 3);
            int numTotal = listTurmaNota.Count(p => p.tnt_situacao != 3);

            if (numTotal > 0)
            {
                if (numEfetivados == numTotal)
                    retorno = CLS_TurmaAulaStatusAtividadeAvaliativa.Efetivada;
                else
                    retorno = CLS_TurmaAulaStatusAtividadeAvaliativa.Preenchida;
            }
            else
                retorno = CLS_TurmaAulaStatusAtividadeAvaliativa.NaoPreenchida;

            return retorno;
        }

        /// <summary>
        /// Retorna o status da anotacao da aula
        /// </summary>
        /// <param name="listTurmaAulaAluno">Lista de TurmaAulaAluno</param>
        /// <returns></returns>
        public static CLS_TurmaAulaStatusAnotacoes RetornaStatusAnotacoes(List<CLS_TurmaAulaAluno> listTurmaAulaAluno, List<CLS_TurmaAulaAlunoTipoAnotacao> ltTurmaAulaAlunoTipoAnotacao)
        {
            CLS_TurmaAulaStatusAnotacoes retorno;

            if (listTurmaAulaAluno.Exists(p => !string.IsNullOrEmpty(p.taa_anotacao)) || ltTurmaAulaAlunoTipoAnotacao.Any())
                retorno = CLS_TurmaAulaStatusAnotacoes.Preenchida;
            else
                retorno = CLS_TurmaAulaStatusAnotacoes.NaoPreenchida;

            return retorno;
        }

        /// <summary>
        /// Retorna o status do planejamento de aula para uma disciplina que não regencia
        /// </summary>
        /// <param name="entity">Turma Aula</param>
        /// <returns></returns>
        public static CLS_TurmaAulaStatusPlanoAula RetornaStatusPlanoAula(CLS_TurmaAula entity)
        {
            return RetornaStatusPlanoAula(entity, ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_SINTESE_REGENCIA_AULA_TURMA, Guid.Empty),
                                          !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, Guid.Empty));
        }

        /// <summary>
        /// Retorna o status do planejamento de aula para uma disciplina que não regencia
        /// </summary>
        /// <param name="entity">Turma Aula</param>
        /// <returns></returns>
        public static CLS_TurmaAulaStatusPlanoAula RetornaStatusPlanoAula(CLS_TurmaAula entity, bool mostraSinteseAula, bool mostraRegistroAula)
        {
            CLS_TurmaAulaStatusPlanoAula retorno;

            if (mostraSinteseAula)
            {
                retorno = CLS_TurmaAulaStatusPlanoAula.NaoPreenchida;
                if (!string.IsNullOrEmpty(entity.tau_sintese))
                    retorno = CLS_TurmaAulaStatusPlanoAula.Preenchida;
                else
                    if (!string.IsNullOrEmpty(entity.tau_planoAula) ||
                        (!string.IsNullOrEmpty(entity.tau_diarioClasse) && mostraRegistroAula) ||
                        !string.IsNullOrEmpty(entity.tau_conteudo) ||
                        !string.IsNullOrEmpty(entity.tau_atividadeCasa))
                    retorno = CLS_TurmaAulaStatusPlanoAula.Incompleto;
            }
            else
            {
                if (!string.IsNullOrEmpty(entity.tau_planoAula) ||
                        (!string.IsNullOrEmpty(entity.tau_diarioClasse) && mostraRegistroAula) ||
                        !string.IsNullOrEmpty(entity.tau_conteudo) ||
                        !string.IsNullOrEmpty(entity.tau_atividadeCasa))
                    retorno = CLS_TurmaAulaStatusPlanoAula.Preenchida;
                else
                    retorno = CLS_TurmaAulaStatusPlanoAula.NaoPreenchida;
            }

            return retorno;
        }

        /// <summary>
        /// Retorna o status do planejamento de aula para uma disciplina de regencia
        /// </summary>
        /// <param name="entity">Turma Aula</param>
        /// <returns></returns>
        public static CLS_TurmaAulaStatusPlanoAula RetornaStatusPlanoAulaRegencia(List<CLS_TurmaAulaRegencia> listTurmaAulaRegencia)
        {
            CLS_TurmaAulaStatusPlanoAula retorno;

            retorno = CLS_TurmaAulaStatusPlanoAula.NaoPreenchida;
            if (listTurmaAulaRegencia.Any(p => !string.IsNullOrEmpty(p.tuf_sintese)))
                retorno = CLS_TurmaAulaStatusPlanoAula.Preenchida;
            else
                if (listTurmaAulaRegencia.Any(p => string.IsNullOrEmpty(p.tuf_sintese) && (
                        !string.IsNullOrEmpty(p.tuf_planoAula) || !string.IsNullOrEmpty(p.tuf_diarioClasse) ||
                        !string.IsNullOrEmpty(p.tuf_conteudo) || !string.IsNullOrEmpty(p.tuf_atividadeCasa))))
                retorno = CLS_TurmaAulaStatusPlanoAula.Incompleto;

            return retorno;
        }

        #endregion Métodos

        #region Saves

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="dataInicial">Data inicial para gerar aulas</param>
        /// <param name="dataFinal">Data final para gerar aulas</param>
        /// <param name="diasGerarAulas">Dias da semana/Quantidade de aulas por dia para gerar aulas</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool GerarAulas
        (
            CLS_TurmaAula entity
            , DateTime dataInicial
            , DateTime dataFinal
            , SortedDictionary<DayOfWeek, int> diasGerarAulas
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                // Verifica se todos os dados obrigatórios foram preenchidos
                if (dataInicial == new DateTime())
                    throw new ArgumentException("Data inicial é obrigatório.");

                if (dataFinal == new DateTime())
                    throw new ArgumentException("Data final é obrigatório.");

                if (dataInicial.Date > dataFinal.Date)
                    throw new ArgumentException("Data inicial não pode ser maior que a data final.");

                long somaNumeroAulas = 0;
                foreach (KeyValuePair<DayOfWeek, int> kvp in diasGerarAulas)
                    somaNumeroAulas = somaNumeroAulas + kvp.Value;

                if (somaNumeroAulas <= 0)
                    throw new ArgumentException("Selecione pelo menos um dia da semana para gerar aulas.");

                // Verifica se as datas inicial e final estão dentro do calendário
                TUR_Turma tur = new TUR_Turma { tur_id = entity.tur_id };
                TUR_TurmaBO.GetEntity(tur, dao._Banco);

                ACA_CalendarioAnual cal = new ACA_CalendarioAnual { cal_id = tur.cal_id };
                ACA_CalendarioAnualBO.GetEntity(cal, dao._Banco);

                if (dataInicial.Date > cal.cal_dataFim.Date || dataInicial.Date < cal.cal_dataInicio.Date)
                    throw new ArgumentException("A data inicial deve estar dentro do período do calendário escolar (" + cal.cal_dataInicio.ToString("dd/MM/yyyy") + " - " + cal.cal_dataFim.ToString("dd/MM/yyyy") + ").");

                if (dataFinal.Date > cal.cal_dataFim.Date || dataFinal.Date < cal.cal_dataInicio.Date)
                    throw new ArgumentException("A data final deve estar dentro do período do calendário escolar (" + cal.cal_dataInicio.ToString("dd/MM/yyyy") + " - " + cal.cal_dataFim.ToString("dd/MM/yyyy") + ").");

                // Verifica se as datas inicial e final estão dentro do período do calendário
                DataTable dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorTipoPeriodoCalendario(entity.tpc_id, tur.cal_id);

                if (dt.Rows.Count > 0)
                {
                    string dataIni = Convert.ToDateTime(dt.Rows[0]["cap_dataInicio"].ToString()).ToString("dd/MM/yyyy");
                    string dataFim = Convert.ToDateTime(dt.Rows[0]["cap_dataFim"].ToString()).ToString("dd/MM/yyyy");

                    if (dataInicial.Date > Convert.ToDateTime(dataFim).Date || dataInicial.Date < Convert.ToDateTime(dataIni).Date)
                        throw new ArgumentException("A data inicial deve estar dentro do período do calendário (" + dataIni + " - " + dataFim + ").");

                    if (dataFinal.Date > Convert.ToDateTime(dataFim).Date || dataFinal.Date < Convert.ToDateTime(dataIni).Date)
                        throw new ArgumentException("A data final deve estar dentro do período do calendário (" + dataIni + " - " + dataFim + ").");
                }

                // Pega a sequência da última aula cadastrada e soma 1
                int tau_sequencia = VerificaUltimaAulaCadastrada(entity.tud_id) + 1;

                //Insere as aulas automaticamente nos dias da semana que possuem aula
                bool salvouAula = false;
                while (dataInicial <= dataFinal)
                {
                    int tau_numeroAulas;
                    diasGerarAulas.TryGetValue(dataInicial.DayOfWeek, out tau_numeroAulas);

                    if (tau_numeroAulas > 0 && !VerificaAulaExistentePorDataPosicaoDocente(entity.tud_id, entity.tdt_posicao, dataInicial))
                    {
                        entity.tau_id = -1;
                        entity.tau_sequencia = tau_sequencia;
                        entity.tau_data = dataInicial;
                        entity.tau_numeroAulas = tau_numeroAulas;

                        Save(entity, dao._Banco);

                        salvouAula = true;
                        tau_sequencia++;
                    }

                    dataInicial = dataInicial.AddDays(1);
                }

                if (!salvouAula)
                    throw new ValidationException("Não foi possível gerar aula(s), pois ela(s) já existe(m), em qualquer período do calendário, ou o dia da semana informado não encontra-se no intervalo de datas escolhido.");

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

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="cal_id">ID do calendário.</param>
        /// <param name="diasGerarAulas">Dias da semana/Quantidade de aulas por dia para gerar aulas</param>
        /// <param name="calendarioPeriodo">Estrtura com dados do período do calendário e os dias não úteis.</param>
        /// <param name="banco">Transação.</param>
        /// <param name="gerouTodasAulas">Flag que indica se todas as aulas futuras foram salvas.</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool GerarAulasPlanejamentoDiario
        (
            CLS_TurmaAula entity
            , int cal_id
            , Dictionary<DayOfWeek, int> diasGerarAulas
            , sCalendarioPeriodoEscolaDiasNaoUteis calendarioPeriodo
            , TalkDBTransaction banco
            , ref bool gerouTodasAulas
            , DataTable dtAulasBanco
            , int tud_cargaHorariaSemanal
            , byte tud_tipo
            , ref Dictionary<string, string> ultrapassouCargaHorariaSemanal
            , Dictionary<string, string> dicTurmasDisciplinas
            , sTurmaDisciplinaRelacionada entityDisRelacionada
            , DateTime dataInicial
            , DateTime dataFinal
        )
        {
            List<DateTime> listaDiasNaoUteis = new List<DateTime>();

            // Lista de aulas para salvar/alterar/excluir (utilizadas por semana).
            List<sTurmaAula> ltTurmaAulaSalvar = new List<sTurmaAula>();
            List<sTurmaAula> ltTurmaAulaExcluir = new List<sTurmaAula>();

            if (dataInicial != new DateTime() &&
                (dataInicial < calendarioPeriodo.cap_dataInicio || dataInicial > calendarioPeriodo.cap_dataFim))
                throw new ArgumentException(String.Format(CustomResource.GetGlobalResourceObject("BLL", "TurmaAula.ValidaDataInicioBimestre"),
                                                          calendarioPeriodo.cap_dataInicio.ToShortDateString(),
                                                          calendarioPeriodo.cap_dataFim.ToShortDateString()));

            if (dataFinal != new DateTime() &&
                (dataFinal < calendarioPeriodo.cap_dataInicio || dataFinal > calendarioPeriodo.cap_dataFim))
                throw new ArgumentException(String.Format(CustomResource.GetGlobalResourceObject("BLL", "TurmaAula.ValidaDataFimBimestre"),
                                                          calendarioPeriodo.cap_dataInicio.ToShortDateString(),
                                                          calendarioPeriodo.cap_dataFim.ToShortDateString()));

            if (dataInicial == new DateTime() && calendarioPeriodo.cap_dataInicio != null)
                dataInicial = calendarioPeriodo.cap_dataInicio;

            if (dataFinal == new DateTime() && calendarioPeriodo.cap_dataFim != null)
                dataFinal = calendarioPeriodo.cap_dataFim;

            if (calendarioPeriodo.listaDiaNaoUtil != null)
                listaDiasNaoUteis = calendarioPeriodo.listaDiaNaoUtil;

            // Verifica se todos os dados obrigatórios foram preenchidos
            if (dataInicial == new DateTime())
                throw new ArgumentException("Data inicial é obrigatório.");

            if (dataFinal == new DateTime())
                throw new ArgumentException("Data final é obrigatório.");

            if (dataInicial.Date > dataFinal.Date)
                throw new ArgumentException("Data inicial não pode ser maior que a data final.");

            long somaNumeroAulas = 0;
            foreach (KeyValuePair<DayOfWeek, int> kvp in diasGerarAulas)
                somaNumeroAulas = somaNumeroAulas + kvp.Value;

            if (somaNumeroAulas > 0)
            {
                // Percorre o período do calendário.
                while (dataInicial <= dataFinal)
                {
                    if (!listaDiasNaoUteis.Any(p => p.Date == dataInicial.Date))
                    {

                        int tau_numeroAulas;
                        diasGerarAulas.TryGetValue(dataInicial.DayOfWeek, out tau_numeroAulas);

                        var dadoAulaBanco =
                                (
                                    from DataRow dr in dtAulasBanco.Rows
                                    where Convert.ToInt64(dr["tud_id"]) == entity.tud_id
                                        && Convert.ToInt16(dr["tdt_posicao"]) == entity.tdt_posicao
                                        && Convert.ToDateTime(dr["tau_data"]) == dataInicial
                                        && (entityDisRelacionada.tud_id <= 0 || Convert.ToInt64(dr["tud_idRelacionada"]) == entityDisRelacionada.tud_id)
                                    select new
                                    {
                                        DiaSemana = (DayOfWeek)Convert.ToByte(dr["DiaSemana"])
                                        ,
                                        PermiteAlterar = Convert.ToBoolean(dr["PermiteAlterar"])
                                        ,
                                        entAula = new CLS_TurmaAulaDAO().DataRowToEntity(dr, new CLS_TurmaAula())
                                    }
                                );

                        // Verifica se já existe uma aula cadastrada para aquele dia.
                        bool existeAula = dadoAulaBanco.Count() > 0;

                        // Aulas retroativas
                        if (dataInicial.Date <= DateTime.Now.Date)
                        {
                            // Apenas salva aulas novas, sem alterar/excluir as aulas existes
                            if (!existeAula && tau_numeroAulas > 0)
                            {
                                ltTurmaAulaSalvar.Add
                                (
                                    new sTurmaAula
                                    {
                                        entity = new CLS_TurmaAula
                                        {
                                            tud_id = entity.tud_id
                                                    ,
                                            tau_id = -1
                                                    ,
                                            tur_id = -1
                                                    ,
                                            tpc_id = entity.tpc_id
                                                    ,
                                            tau_data = dataInicial
                                                    ,
                                            tau_sequencia = -1
                                                    ,
                                            tau_numeroAulas = tau_numeroAulas
                                                    ,
                                            tau_situacao = entity.tau_situacao
                                                    ,
                                            tdt_posicao = entity.tdt_posicao
                                                    ,
                                            usu_id = entity.usu_id
                                                    ,
                                            IsNew = true
                                        }
                                        ,
                                        permiteAlterar = true
                                        ,
                                        aulaRetroativa = true
                                    }
                                );
                            }
                            // Se a aula já existir
                            else if (existeAula)
                            {
                                // Caso ela exista, verifica se existe algum registro ligado a essa aula.
                                CLS_TurmaAula turmaAulaBanco = dadoAulaBanco.FirstOrDefault().entAula;
                                bool permiteAlterar = dadoAulaBanco.FirstOrDefault().PermiteAlterar;

                                // Deleta.
                                if (tau_numeroAulas <= 0)
                                {
                                    turmaAulaBanco.tur_id = entity.tur_id;

                                    ltTurmaAulaExcluir.Add
                                    (
                                        new sTurmaAula
                                        {
                                            entity = turmaAulaBanco
                                            ,
                                            permiteAlterar = permiteAlterar
                                            ,
                                            aulaRetroativa = true
                                        }
                                    );
                                }
                                // Altera.
                                else
                                {
                                    turmaAulaBanco.tau_numeroAulas = tau_numeroAulas;
                                    turmaAulaBanco.tau_sequencia = -1;
                                    turmaAulaBanco.tur_id = entity.tur_id;
                                    turmaAulaBanco.IsNew = false;

                                    ltTurmaAulaSalvar.Add
                                    (
                                        new sTurmaAula
                                        {
                                            entity = turmaAulaBanco
                                            ,
                                            permiteAlterar = permiteAlterar
                                            ,
                                            aulaRetroativa = true
                                        }
                                    );
                                }

                                gerouTodasAulas &= permiteAlterar;
                            }
                        }

                        // Aulas futuras
                        else
                        {
                            // Cria a aula caso ela for nova.
                            if (!existeAula && tau_numeroAulas > 0)
                            {
                                ltTurmaAulaSalvar.Add
                                (
                                    new sTurmaAula
                                    {
                                        entity = new CLS_TurmaAula
                                        {
                                            tud_id = entity.tud_id
                                                    ,
                                            tau_id = -1
                                                    ,
                                            tur_id = entity.tur_id
                                                    ,
                                            tpc_id = entity.tpc_id
                                                    ,
                                            tau_data = dataInicial
                                                    ,
                                            tau_sequencia = -1
                                                    ,
                                            tau_numeroAulas = tau_numeroAulas
                                                    ,
                                            tau_situacao = entity.tau_situacao
                                                    ,
                                            tdt_posicao = entity.tdt_posicao
                                                    ,
                                            usu_id = entity.usu_id
                                                    ,
                                            IsNew = true
                                        }
                                        ,
                                        permiteAlterar = true
                                        ,
                                        aulaRetroativa = false
                                    }
                                );
                            }
                            // Se a aula já existir
                            else if (existeAula)
                            {
                                // Caso ela exista, verifica se existe algum registro ligado a essa aula.
                                CLS_TurmaAula turmaAulaBanco = dadoAulaBanco.FirstOrDefault().entAula;
                                bool permiteAlterar = dadoAulaBanco.FirstOrDefault().PermiteAlterar;

                                // Deleta.
                                if (tau_numeroAulas <= 0)
                                {
                                    turmaAulaBanco.tur_id = entity.tur_id;

                                    ltTurmaAulaExcluir.Add
                                    (
                                        new sTurmaAula
                                        {
                                            entity = turmaAulaBanco
                                            ,
                                            permiteAlterar = permiteAlterar
                                            ,
                                            aulaRetroativa = false
                                        }
                                    );
                                }
                                // Altera.
                                else
                                {
                                    turmaAulaBanco.tau_numeroAulas = tau_numeroAulas;
                                    turmaAulaBanco.tau_sequencia = -1;
                                    turmaAulaBanco.tur_id = entity.tur_id;
                                    turmaAulaBanco.IsNew = false;

                                    ltTurmaAulaSalvar.Add
                                    (
                                        new sTurmaAula
                                        {
                                            entity = turmaAulaBanco
                                            ,
                                            permiteAlterar = permiteAlterar
                                            ,
                                            aulaRetroativa = false
                                        }
                                    );
                                }

                                gerouTodasAulas &= permiteAlterar;
                            }
                        }
                    }

                    if (dataInicial.DayOfWeek == DayOfWeek.Sunday || dataInicial == dataFinal)
                    {
                        DateTime dataIniSemana;
                        DateTime dataFimSemana;

                        // Datas iniciais e finais da semana da aula.
                        if (dataInicial.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dataIniSemana = dataInicial.AddDays(-7);
                            dataFimSemana = dataInicial.AddDays(-1);
                        }
                        // o bimestre terminou antes do domingo
                        else
                        {
                            dataIniSemana = new DateTime();
                            switch (dataInicial.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    dataIniSemana = dataInicial.AddDays(-1);
                                    break;

                                case DayOfWeek.Tuesday:
                                    dataIniSemana = dataInicial.AddDays(-2);
                                    break;

                                case DayOfWeek.Wednesday:
                                    dataIniSemana = dataInicial.AddDays(-3);
                                    break;

                                case DayOfWeek.Thursday:
                                    dataIniSemana = dataInicial.AddDays(-4);
                                    break;

                                case DayOfWeek.Friday:
                                    dataIniSemana = dataInicial.AddDays(-5);
                                    break;

                                case DayOfWeek.Saturday:
                                    dataIniSemana = dataInicial.AddDays(-6);
                                    break;
                            }
                            dataFimSemana = dataFinal;
                        }

                        // Dados das aulas persistentes no banco da dados.
                        var aulasBanco = from DataRow dr in dtAulasBanco.Rows
                                         let tau_data = Convert.ToDateTime(dr["tau_data"])
                                         where tau_data >= dataIniSemana
                                               && tau_data <= dataFimSemana
                                               && Convert.ToInt64(dr["tud_id"]) == entity.tud_id
                                         select new
                                         {
                                             tud_id = Convert.ToInt64(dr["tud_id"])
                                             ,
                                             tau_id = Convert.ToInt32(dr["tau_id"])
                                             ,
                                             tau_numeroAulas = Convert.ToInt32(dr["tau_numeroAulas"])
                                         };

                        // Dados das aulas carregadas do banco e que foram alteradas na agenda.
                        var aulasAlteradas = from sTurmaAula tau in ltTurmaAulaSalvar
                                             let aula = tau.entity
                                             where tau.permiteAlterar
                                                   && !aula.IsNew
                                                   && aula.tau_data >= dataIniSemana
                                                   && aula.tau_data <= dataFimSemana
                                             select new
                                             {
                                                 aula.tud_id
                                                 ,
                                                 aula.tau_id
                                                 ,
                                                 aula.tau_numeroAulas
                                             };

                        // Quantidade de aulas criadas para a semana.
                        int qtdeAulasNovas = (from sTurmaAula tau in ltTurmaAulaSalvar
                                              let aula = tau.entity
                                              where tau.permiteAlterar
                                                    && aula.IsNew
                                                    && aula.tau_data >= dataIniSemana
                                                    && aula.tau_data <= dataFimSemana
                                              select aula.tau_numeroAulas).Sum();

                        // Quantidade de aulas excluídas da semana.
                        int qtdeAulasExcluidas = (from sTurmaAula tau in ltTurmaAulaExcluir
                                                  let aula = tau.entity
                                                  where tau.permiteAlterar
                                                        && aula.tau_data >= dataIniSemana
                                                        && aula.tau_data <= dataFimSemana
                                                  select aula.tau_numeroAulas).Sum();

                        // Quantidade de aulas que foram alteradas.
                        int qtdeAulasAlteradas = (from itemBanco in aulasBanco
                                                  join itemAlterado in aulasAlteradas
                                                  on new { itemBanco.tud_id, itemBanco.tau_id }
                                                  equals new { itemAlterado.tud_id, itemAlterado.tau_id } into juncao
                                                  from itemAlterado in juncao.DefaultIfEmpty() // "left join" entre dados do banco com dados alterados
                                                  select itemAlterado != null && itemAlterado.tau_numeroAulas > 0 ?
                                                         itemAlterado.tau_numeroAulas : itemBanco.tau_numeroAulas).Sum();

                        // Total de aulas na semana.
                        int qtdeAulasSemana = (qtdeAulasNovas + qtdeAulasAlteradas) - qtdeAulasExcluidas;

                        // flags que indicam se há aulas retroativas ou não dentro da semana.
                        bool anteriorDataAtual = ltTurmaAulaSalvar.Any(p => p.aulaRetroativa) || ltTurmaAulaExcluir.Any(p => p.aulaRetroativa);
                        bool posteriorDataAtual = ltTurmaAulaSalvar.Any(p => !p.aulaRetroativa) || ltTurmaAulaExcluir.Any(p => !p.aulaRetroativa);

                        // flag que indica se é possível realizar alteração/exclusão nas aulas da semana.
                        bool permiteAlterar = (ltTurmaAulaSalvar.Where(p => !p.entity.IsNew).Aggregate(false, (permite, turmaAula) => permite | turmaAula.permiteAlterar) ||
                                               ltTurmaAulaExcluir.Aggregate(false, (permite, turmaAula) => permite | turmaAula.permiteAlterar));

                        // flag que indica se a agenda está apenas inserindo novas aulas
                        bool apenasInclusao = ltTurmaAulaSalvar.Aggregate(true, (novo, turmaAula) => novo & turmaAula.entity.IsNew)
                                              && !ltTurmaAulaExcluir.Any();

                        // Salva/exclui as aulas se permitido.
                        if (qtdeAulasSemana > tud_cargaHorariaSemanal)
                        {
                            // se ultrapassou o numero de aulas permitidas na semana, e havia aulas para serem salvas
                            // atualizo a variavel para alertar o usuario no final.
                            if ((ltTurmaAulaExcluir.Any(p => p.permiteAlterar) || ltTurmaAulaSalvar.Any(p => p.permiteAlterar)))
                            {
                                string chaveTud = entityDisRelacionada.tud_id > 0 ? entityDisRelacionada.tud_id.ToString() : entity.tud_id.ToString();
                                if (ultrapassouCargaHorariaSemanal.ContainsKey(chaveTud))
                                {
                                    ultrapassouCargaHorariaSemanal[chaveTud] = String.Format("{0}, {1}-{2}", ultrapassouCargaHorariaSemanal[chaveTud], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy"));
                                }
                                else
                                {
                                    ultrapassouCargaHorariaSemanal.Add(chaveTud, String.Format("{0}: {1}-{2}", dicTurmasDisciplinas[chaveTud], dataIniSemana.ToString("dd/MM/yyyy"), dataFimSemana.ToString("dd/MM/yyyy")));
                                }
                            }
                        }
                        else if (anteriorDataAtual || (posteriorDataAtual && (permiteAlterar || apenasInclusao)))
                        {
                            ltTurmaAulaExcluir.Where(p => p.permiteAlterar).Aggregate(true, (excluiu, turmaAula) => excluiu & Delete(turmaAula.entity, banco));
                            ltTurmaAulaSalvar.Where(p => p.permiteAlterar).Aggregate(true, (salvou, turmaAula) => salvou & Save(turmaAula.entity, banco, entityDisRelacionada));
                        }

                        // Limpa as listas para serem utilizadas para a próxima semana.
                        ltTurmaAulaExcluir = new List<sTurmaAula>();
                        ltTurmaAulaSalvar = new List<sTurmaAula>();
                    }

                    // Próximo dia.
                    dataInicial = dataInicial.AddDays(1);
                }

                // Reordena sequência das aulas.
                AtualizarSequenciaAulasPorTurmaDisciplina(entity.tud_id, banco);
            }

            return true;
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma e as anotações dos alunos
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="listTurmaAulaAluno">Lista de anotações dos alunos</param>
        /// <param name="listTurmaAulaRecurso">Lista de recursos usados na aula</param>
        /// <param name="salvarEntityTurmaAula">indica se deve ser salvo a entity CLS_TurmaAula</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool SalvarAulaAnotacoesRecursos
        (
            CLS_TurmaAula entity
            , List<CLS_TurmaAulaAluno> listTurmaAulaAluno
            , List<CLS_TurmaAulaRecurso> listTurmaAulaRecurso
            , List<CLS_TurmaAulaAlunoTipoAnotacao> listTurmaAulaAnotacao
            , List<Int64> lstComponentesRegencia
            , bool salvarEntityTurmaAula = true
            , List<CLS_TurmaAulaOrientacaoCurricular> listOriCurTurAula = null
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
            , List<CLS_ObjetoAprendizagemTurmaAula> listObjTudTau = null
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                if (listObjTudTau != null)
                    CLS_ObjetoAprendizagemTurmaAulaBO.SalvarLista(listObjTudTau, dao._Banco);

                if (listOriCurTurAula != null)
                    CLS_TurmaAulaOrientacaoCurricularBO.Salvar(listOriCurTurAula, dao._Banco);

                return SalvarAulaAnotacoesRecursos(entity, listTurmaAulaAluno, listTurmaAulaRecurso, listTurmaAulaAnotacao, dao._Banco, lstComponentesRegencia, false, salvarEntityTurmaAula, usu_id, origemLogAula, tipoLogAula);
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



        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma e as anotações dos alunos
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="listTurmaAulaAluno">Lista de anotações dos alunos</param>
        /// <param name="listTurmaAulaRecurso">Lista de recursos usados na aula</param>
        /// <param name="banco">Transação com banco aberta</param>
        /// <param name="salvarFaltasAlunos">Indica se será salvo o campo taa_frequencia para os alunos - se passar false salva só a anotação</param>
        /// <param name="salvarEntityTurmaAula">indica se deve ser salvo a entity CLS_TurmaAula</param>
        private static bool SalvarAulaAnotacoesRecursos
        (
            CLS_TurmaAula entity
            , List<CLS_TurmaAulaAluno> listTurmaAulaAluno
            , List<CLS_TurmaAulaRecurso> listTurmaAulaRecurso
            , List<CLS_TurmaAulaAlunoTipoAnotacao> listTurmaAulaAnotacao
            , TalkDBTransaction banco
            , List<Int64> lstComponentesRegencia
            , bool salvarFaltasAlunos = false
            , bool salvarEntityTurmaAula = true
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
        )
        {
            if (entity.tau_data == new DateTime())
            {
                throw new ValidationException("Data da aula é obrigatório.");
            }

            // Caso o usuário logado não seja um docente, grava como posição 1.
            if (entity.tdt_posicao < 1)
            {
                entity.tdt_posicao = 1;
            }

            if (salvarEntityTurmaAula)
            {
                // Chama método padrão para salvar a aula
                if (entity.Validate())
                {
                    Save(entity, banco);
                    CLS_TurmaAulaPendenciaBO.AtualizarPendencia(entity.tud_id, entity.tau_id, string.IsNullOrEmpty(entity.tau_planoAula));
                }
                else
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
            }

            if (origemLogAula > 0)
            {
                DateTime dataLogAula = DateTime.Now;
                LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                {
                    tud_id = entity.tud_id,
                    tau_id = entity.tau_id,
                    usu_id = usu_id,
                    lta_origem = origemLogAula,
                    lta_tipo = tipoLogAula,
                    lta_data = dataLogAula
                };

                LOG_TurmaAula_AlteracaoBO.Save(entLogAula, banco);
            }

            // Salva as anotações dos alunos na aula da disciplina da turma
            foreach (CLS_TurmaAulaAluno aux in listTurmaAulaAluno)
            {
                // Verifica se já existe uma anotação ou frequência cadastrada
                CLS_TurmaAulaAluno entityTurmaAulaAluno = new CLS_TurmaAulaAluno
                {
                    tud_id = aux.tud_id
                    ,
                    tau_id = entity.tau_id
                    ,
                    alu_id = aux.alu_id
                    ,
                    mtu_id = aux.mtu_id
                    ,
                    mtd_id = aux.mtd_id
                };
                CLS_TurmaAulaAlunoBO.GetEntity(entityTurmaAulaAluno, banco);

                // Atualiza a anotação da entidade para salvar
                entityTurmaAulaAluno.taa_anotacao = aux.taa_anotacao;
                entityTurmaAulaAluno.taa_situacao = entity.tau_situacao;

                // gravo o usuário que fez a última atualização nos dados
                entityTurmaAulaAluno.usu_idDocenteAlteracao = aux.usu_idDocenteAlteracao;

                if (salvarFaltasAlunos)
                {
                    // Se foi passado o parâmetro para salvar as faltas dos alunos, é pra sobrescrever.
                    entityTurmaAulaAluno.taa_frequencia = aux.taa_frequencia;
                    entityTurmaAulaAluno.taa_frequenciaBitMap = aux.taa_frequenciaBitMap;
                }

                // Salva as anotações dos alunos
                if (entityTurmaAulaAluno.Validate())
                {
                    CLS_TurmaAulaAlunoBO.Save(entityTurmaAulaAluno, banco);
                }
                else
                {
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityTurmaAulaAluno));
                }
            }


            if (listTurmaAulaAnotacao != null)
            {
                //Carrega as anotacoes tipo gravados no banco
                List<CLS_TurmaAulaAlunoTipoAnotacao> listaAnotacoesBanco = CLS_TurmaAulaAlunoTipoAnotacaoBO.SelecionaPorTurmaAula(entity.tud_id
                                                                                                      , entity.tau_id);

                List<CLS_TurmaAulaAlunoTipoAnotacao> dadosAnotacoesIguais =
                    (from CLS_TurmaAulaAlunoTipoAnotacao t in listaAnotacoesBanco
                     join CLS_TurmaAulaAlunoTipoAnotacao x in listTurmaAulaAnotacao
                     on new
                     {
                         t.alu_id,
                         t.mtu_id,
                         t.mtd_id,
                         t.tud_id,
                         t.tau_id,
                         t.tia_id
                     }
                      equals
                     new
                     {
                         x.alu_id,
                         x.mtu_id,
                         x.mtd_id,
                         x.tud_id,
                         x.tau_id,
                         x.tia_id
                     }
                     select t
                     ).ToList();

                //Retiro da lista do banco as entidades que são iguais (nao foram modificadas, nao eh pra remover)
                List<CLS_TurmaAulaAlunoTipoAnotacao> listDeletarBanco = (from CLS_TurmaAulaAlunoTipoAnotacao item in listaAnotacoesBanco select item).ToList();
                foreach (CLS_TurmaAulaAlunoTipoAnotacao ent in dadosAnotacoesIguais)
                {
                    listDeletarBanco.RemoveAll(p => p.alu_id == ent.alu_id && p.mtd_id == ent.mtd_id && p.mtu_id == ent.mtu_id && p.tau_id == ent.tau_id && p.tud_id == ent.tud_id && p.tia_id == ent.tia_id);
                }


                //deleta registros que sobraram na lista do banco
                foreach (CLS_TurmaAulaAlunoTipoAnotacao ent in listDeletarBanco)
                {
                    CLS_TurmaAulaAlunoTipoAnotacaoBO.Delete(ent, banco);
                }

                //Dados que nao foram modificados
                List<CLS_TurmaAulaAlunoTipoAnotacao> dadosIguais =
                    (from CLS_TurmaAulaAlunoTipoAnotacao t in listTurmaAulaAnotacao
                     join CLS_TurmaAulaAlunoTipoAnotacao x in listaAnotacoesBanco
                     on new
                     {
                         t.alu_id,
                         t.mtu_id,
                         t.mtd_id,
                         t.tud_id,
                         t.tau_id,
                         t.tia_id
                     }
                      equals
                     new
                     {
                         x.alu_id,
                         x.mtu_id,
                         x.mtd_id,
                         x.tud_id,
                         x.tau_id,
                         x.tia_id
                     }
                     select t
                     ).ToList();


                foreach (CLS_TurmaAulaAlunoTipoAnotacao ent in dadosIguais)
                {
                    listTurmaAulaAnotacao.Remove(ent);
                }

                //Salva as anotacoes novas
                foreach (CLS_TurmaAulaAlunoTipoAnotacao anotacao in listTurmaAulaAnotacao)
                {
                    CLS_TurmaAulaAlunoTipoAnotacaoBO.Save(anotacao, banco);
                }
            }

            //Carrega Recursos gravados no banco
            List<CLS_TurmaAulaRecurso> listaBanco = CLS_TurmaAulaRecursoBO.GetSelectBy_Turma_Aula(entity.tud_id
                                                                                                  , entity.tau_id);
            //busca registros que devem ser excluidos
            IEnumerable<Int32> dadosTela =
            (from CLS_TurmaAulaRecurso dr in listTurmaAulaRecurso.AsEnumerable()
             orderby dr.rsa_id descending
             select dr.rsa_id).AsEnumerable();

            IEnumerable<Int32> dadosExcluir =
                (from CLS_TurmaAulaRecurso t in listaBanco.AsEnumerable()
                 orderby t.rsa_id descending
                 select t.rsa_id).Except(dadosTela);

            IList<Int32> dadosDif = dadosExcluir.ToList();
            //deleta registros que foram desmarcados
            for (int i = 0; i < dadosDif.Count; i++)
            {
                CLS_TurmaAulaRecursoBO.Delete_Byrsa_id(entity.tud_id, entity.tau_id, dadosDif[i], banco);
            }

            //busca registro que devem ser alterados
            IEnumerable<Int32> dadosBanco =
                (from CLS_TurmaAulaRecurso t in listaBanco.AsEnumerable()
                 orderby t.rsa_id descending
                 select t.rsa_id).AsEnumerable();

            IEnumerable<Int32> dadosAlterar =
                (from CLS_TurmaAulaRecurso t in listTurmaAulaRecurso.AsEnumerable()
                 orderby t.rsa_id descending
                 select t.rsa_id).Intersect(dadosBanco);

            IList<Int32> dadosAlte = dadosAlterar.ToList();
            CLS_TurmaAulaRecurso entityAltera;
            for (int i = 0; i < dadosAlte.Count; i++)
            {
                entityAltera = listTurmaAulaRecurso.Find(p => p.rsa_id == dadosAlte[i]);
                entityAltera.tar_dataAlteracao = DateTime.Now;
                CLS_TurmaAulaRecursoBO.Update_Byrsa_id(entityAltera, banco);
                listTurmaAulaRecurso.Remove(entityAltera);
            }

            // Salva as recursos utilizados na aula
            foreach (CLS_TurmaAulaRecurso aux in listTurmaAulaRecurso)
            {
                aux.tau_id = entity.tau_id;
                if (aux.Validate())
                    CLS_TurmaAulaRecursoBO.Salvar(aux, banco);
                else
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(aux));
            }

            if (lstComponentesRegencia != null)
            {
                CLS_TurmaAulaPlanoDisciplinaBO.DeleteBy_aulaDisciplina(entity.tud_id, entity.tau_id, banco);
                foreach (Int64 componenteRegencia in lstComponentesRegencia)
                {
                    CLS_TurmaAulaPlanoDisciplina ent = new CLS_TurmaAulaPlanoDisciplina { tud_id = entity.tud_id, tau_id = entity.tau_id, tud_idPlano = componenteRegencia };

                    if (!CLS_TurmaAulaPlanoDisciplinaBO.VerificaExisteRegistro(ent, banco))
                        CLS_TurmaAulaPlanoDisciplinaBO.Save(ent, banco);
                }
            }

            return true;
        }


        /// <summary>
        /// Salva as anotacoes para mais de um aluno
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listTurmaAulaAluno"></param>
        /// <param name="listTurmaAulaAnotacao"></param>
        /// <returns></returns>
        public static bool SalvarAulaAnotacoes
        (
            CLS_TurmaAula entity
            , List<CLS_TurmaAulaAluno> listTurmaAulaAluno
            , List<CLS_TurmaAulaAlunoTipoAnotacao> listTurmaAulaAnotacao
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
        )
        {

            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);
            try
            {
                if (entity.tau_data == new DateTime())
                {
                    throw new ValidationException("Data da aula é obrigatório.");
                }

                // Caso o usuário logado não seja um docente, grava como posição 1.
                if (entity.tdt_posicao < 1)
                {
                    entity.tdt_posicao = 1;
                }

                foreach (CLS_TurmaAulaAluno aux in listTurmaAulaAluno)
                {
                    // Verifica se já existe uma anotação ou frequência cadastrada
                    CLS_TurmaAulaAluno entityTurmaAulaAluno = new CLS_TurmaAulaAluno
                    {
                        tud_id = aux.tud_id
                        ,
                        tau_id = entity.tau_id
                        ,
                        alu_id = aux.alu_id
                        ,
                        mtu_id = aux.mtu_id
                        ,
                        mtd_id = aux.mtd_id
                    };
                    CLS_TurmaAulaAlunoBO.GetEntity(entityTurmaAulaAluno, dao._Banco);

                    // Atualiza a anotação da entidade para salvar
                    entityTurmaAulaAluno.taa_anotacao = String.IsNullOrEmpty(entityTurmaAulaAluno.taa_anotacao) ? aux.taa_anotacao : entityTurmaAulaAluno.taa_anotacao + "; " + aux.taa_anotacao;
                    entityTurmaAulaAluno.taa_situacao = entity.tau_situacao;

                    // gravo o usuário que fez a última atualização nos dados
                    entityTurmaAulaAluno.usu_idDocenteAlteracao = aux.usu_idDocenteAlteracao;

                    // Salva as anotações dos alunos
                    if (entityTurmaAulaAluno.Validate())
                    {
                        CLS_TurmaAulaAlunoBO.Save(entityTurmaAulaAluno, dao._Banco);
                    }
                    else
                    {
                        throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityTurmaAulaAluno));
                    }

                    if (origemLogAula > 0)
                    {
                        DateTime dataLogAula = DateTime.Now;
                        LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                        {
                            tud_id = entity.tud_id,
                            tau_id = entity.tau_id,
                            usu_id = usu_id,
                            lta_origem = origemLogAula,
                            lta_tipo = tipoLogAula,
                            lta_data = dataLogAula
                        };

                        LOG_TurmaAula_AlteracaoBO.Save(entLogAula, dao._Banco);
                    }

                    //Carrega as anotacoes tipo gravados no banco
                    List<CLS_TurmaAulaAlunoTipoAnotacao> listaAnotacoesBanco = CLS_TurmaAulaAlunoTipoAnotacaoBO.SelecionaPorTurmaAulaAluno(entityTurmaAulaAluno.tud_id
                                                                                                          , entityTurmaAulaAluno.tau_id, entityTurmaAulaAluno.alu_id, entityTurmaAulaAluno.mtu_id
                                                                                                          , entityTurmaAulaAluno.mtd_id);

                    //Dados que nao foram modificados
                    List<CLS_TurmaAulaAlunoTipoAnotacao> dadosIguais =
                        (from CLS_TurmaAulaAlunoTipoAnotacao t in listTurmaAulaAnotacao
                         join CLS_TurmaAulaAlunoTipoAnotacao x in listaAnotacoesBanco
                         on new
                         {
                             t.alu_id,
                             t.mtu_id,
                             t.mtd_id,
                             t.tud_id,
                             t.tau_id,
                             t.tia_id
                         }
                          equals
                         new
                         {
                             x.alu_id,
                             x.mtu_id,
                             x.mtd_id,
                             x.tud_id,
                             x.tau_id,
                             x.tia_id
                         }
                         select t
                         ).ToList();

                    foreach (CLS_TurmaAulaAlunoTipoAnotacao ent in dadosIguais)
                    {
                        listTurmaAulaAnotacao.Remove(ent);
                    }
                }
                //Salva as anotacoes novas
                foreach (CLS_TurmaAulaAlunoTipoAnotacao anotacao in listTurmaAulaAnotacao)
                {
                    CLS_TurmaAulaAlunoTipoAnotacaoBO.Save(anotacao, dao._Banco);
                }

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

            return true;

        }



        private static void SetaPosicao(sAulaSincronizacaoDiarioClasse sincronizacao)
        {
            byte tdt_posicao = sincronizacao.entityAula.tdt_posicao;
            sincronizacao.entityAula.tdt_posicao = tdt_posicao < 1 ? (byte)1 : tdt_posicao;
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma e as anotações dos alunos
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="listTurmaAulaAluno">Lista de anotações dos alunos</param>
        /// <param name="listTurmaAulaRecurso">Lista de recursos usados na aula</param>
        /// <param name="banco">Transação com banco aberta</param>
        /// <param name="salvarFaltasAlunos">Indica se será salvo o campo taa_frequencia para os alunos - se passar false salva só a anotação</param>
        private static bool SalvarDadosAulaSincronizacaoDiarioClasse
        (
            List<sDadosAulaProtocolo> ltDadosAulasValidacao
            , List<CLS_TurmaAula> ltAulasBanco
            , List<sAulaSincronizacaoDiarioClasse> ltAulaSincronizacaoDiarioClasse
            , bool alteraAnotacao
            , TalkDBTransaction bancoSincronizacao
        )
        {
            // Caso o usuário logado não seja um docente, grava como posição 1.
            ltAulaSincronizacaoDiarioClasse.ForEach(SetaPosicao);

            // Recupera a lista de entidades CLS_TurmaAulaAluno para verificar se ela já existe.
            List<CLS_TurmaAulaAluno> listaTurmaAulaAluno = new List<CLS_TurmaAulaAluno>();

            ltDadosAulasValidacao.ForEach(p =>
            {
                listaTurmaAulaAluno.AddRange(p.ltTurmaAulaAluno);
            });

            foreach (sAulaSincronizacaoDiarioClasse sincronizacao in ltAulaSincronizacaoDiarioClasse)
            {
                long idAula = -1;
                DCL_Protocolo protocolo = sincronizacao.entityProtocolo;

                if (ValidarAula(sincronizacao.entityAula, ltDadosAulasValidacao, ltAulasBanco))
                {
                    using (DataTable dtProtocolo = DCL_Protocolo.TipoTabela_Protocolo(),
                                     dtTurmaAula = CLS_TurmaAula.TipoTabela_TurmaAula(),
                                     dtTurmaAulaDisciplinaRelacionada = CLS_TurmaAulaDisciplinaRelacionada.TipoTabela_TurmaAulaDisciplinaRelacionada(),
                                     dtTurmaAulaAluno = CLS_TurmaAulaAluno.TipoTabela_TurmaAulaAluno(),
                                     dtTurmaAulaRecurso = CLS_TurmaAulaRecurso.TipoTabela_TurmaAulaRecurso(),
                                     dtTurmaAulaRegencia = CLS_TurmaAulaRegencia.TipoTabela_TurmaAulaRegencia(),
                                     dtTurmaAulaRecursoRegencia = CLS_TurmaAulaRecursoRegencia.TipoTabela_TurmaAulaRecursoRegencia(),
                                     dtTurmaNota = CLS_TurmaNota.TipoTabela_TurmaNota(),
                                     dtTurmaNotaRegencia = CLS_TurmaNotaRegencia.TipoTabela_TurmaNotaRegencia(),
                                     dtTurmaNotaAluno = CLS_TurmaNotaAluno.TipoTabela_TurmaNotaAluno(),
                                     dtTurmaAulaPlanoDisciplina = CLS_TurmaAulaPlanoDisciplina.TipoTabela_TurmaAulaPlanoDisciplina(),
                                     dtTurmaAulaAlunoTipoAnotacao = CLS_TurmaAulaAlunoTipoAnotacao.TipoTabela_TurmaAulaAlunoTipoAnotacao(),
                                     dtTurmaAulaOrientacaoCurricular = CLS_TurmaAulaOrientacaoCurricular.TipoTabela_TurmaAulaOrientacaoCurricular(),
                                     dtLogAula = LOG_TurmaAula_Alteracao.TipoTabela_LOG_TurmaAula_Alteracao(),
                                     dtLogNota = LOG_TurmaNota_Alteracao.TipoTabela_LOG_TurmaNota_Alteracao(),
                                     dtTurmaAulaTerritorio = CLS_TurmaAula.TipoTabela_TurmaAula(false))
                    {
                        DataRow drAula = CLS_TurmaAulaBO.TurmaAulaToDataRow(sincronizacao.entityAula, dtTurmaAula.NewRow(), sincronizacao.entityAula.tau_dataAlteracao);
                        dtTurmaAula.Rows.Add(drAula);
                        idAula = Convert.ToInt32(dtTurmaAula.Rows[dtTurmaAula.Rows.IndexOf(drAula)]["idAula"]);

                        foreach (CLS_TurmaAula aulaTerritorio in sincronizacao.ltAulaTerritorio)
                        {
                            dtTurmaAulaTerritorio.Rows.Add(CLS_TurmaAulaBO.TurmaAulaToDataRow(aulaTerritorio, dtTurmaAulaTerritorio.NewRow(), aulaTerritorio.tau_dataAlteracao, idAula));
                        }

                        if (sincronizacao.entityTurmaAulaDisciplinaRelacionada.Validate())
                        {
                            sincronizacao.entityTurmaAulaDisciplinaRelacionada.idAula = idAula;
                            dtTurmaAulaDisciplinaRelacionada.Rows.Add(CLS_TurmaAulaDisciplinaRelacionada.TurmaAulaDisciplinaRelacionadaToDataRow(sincronizacao.entityTurmaAulaDisciplinaRelacionada, dtTurmaAulaDisciplinaRelacionada.NewRow()));
                        }

                        sincronizacao.ltLogAlteracaoAula.ForEach(entityLogAula =>
                        {
                            if (entityLogAula.Validate())
                            {
                                entityLogAula.idAula = idAula;
                                dtLogAula.Rows.Add(LOG_TurmaAula_AlteracaoBO.LOGTurmaAulaAlteracaoToDataRow(entityLogAula, dtLogAula.NewRow()));
                            }
                        });

                        sincronizacao.ltTurmaAulaAluno.ForEach(entityTurmaAulaAluno =>
                        {
                            if (!sincronizacao.ltTurmaAulaAluno.TrueForAll(freq => freq.taa_frequencia <= sincronizacao.entityAula.tau_numeroAulas))
                                throw new ValidationException("A frequência deve estar entre 0 e " + sincronizacao.entityAula.tau_numeroAulas);

                            if (!sincronizacao.ltTurmaAulaAluno.TrueForAll(freq => !string.IsNullOrEmpty(freq.taa_frequenciaBitMap) &&
                                                                                  freq.taa_frequenciaBitMap.ToList().TrueForAll(s => s == '0' || s == '1')))
                                throw new ValidationException("É necessário atualizar a versão do sistema para a mais atual.");

                            if (entityTurmaAulaAluno.Validate())
                            {
                                entityTurmaAulaAluno.idAula = idAula;
                                dtTurmaAulaAluno.Rows.Add(CLS_TurmaAulaAlunoBO.TurmaAulaAlunoToDataRow(entityTurmaAulaAluno, dtTurmaAulaAluno.NewRow(), entityTurmaAulaAluno.taa_dataAlteracao));
                            }
                            else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entityTurmaAulaAluno));
                        });

                        sincronizacao.ltTurmaAulaAlunoTipoAnotacao.ForEach(anotacao =>
                        {
                            if (anotacao.Validate())
                            {
                                anotacao.idAula = idAula;
                                dtTurmaAulaAlunoTipoAnotacao.Rows.Add(CLS_TurmaAulaAlunoTipoAnotacaoBO.TurmaAulaAlunoTipoAnotacaoToDataRow(anotacao, dtTurmaAulaAlunoTipoAnotacao.NewRow()));
                            }
                        });

                        sincronizacao.ltTurmaAulaRecurso.ForEach(recurso =>
                        {
                            if (recurso.Validate())
                            {
                                recurso.idAula = idAula;
                                dtTurmaAulaRecurso.Rows.Add(CLS_TurmaAulaRecursoBO.TurmaAulaRecursoToDataRow(recurso, dtTurmaAulaRecurso.NewRow(), recurso.tar_dataAlteracao));
                            }
                            else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(recurso));
                        });

                        sincronizacao.ltTurmaAulaRegencia.ForEach(turmaAulaRegencia =>
                        {
                            if (turmaAulaRegencia.Validate())
                            {
                                turmaAulaRegencia.idAula = idAula;
                                dtTurmaAulaRegencia.Rows.Add(CLS_TurmaAulaRegenciaBO.TurmaAulaRegenciaToDataRow(turmaAulaRegencia, dtTurmaAulaRegencia.NewRow(), turmaAulaRegencia.tuf_dataAlteracao));
                            }
                            else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(turmaAulaRegencia));
                        });

                        sincronizacao.ltTurmaAulaPlanoDisciplina.ForEach(turmaAulaPlanoDisciplina =>
                        {
                            if (turmaAulaPlanoDisciplina.Validate())
                            {
                                turmaAulaPlanoDisciplina.idAula = idAula;
                                dtTurmaAulaPlanoDisciplina.Rows.Add(CLS_TurmaAulaPlanoDisciplinaBO.TurmaAulaPlanoDiciplinaToDataRow(turmaAulaPlanoDisciplina, dtTurmaAulaPlanoDisciplina.NewRow()));
                            }
                            else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(turmaAulaPlanoDisciplina));
                        });

                        sincronizacao.ltTurmaAulaRecursoRegencia.ForEach(recursoRegencia =>
                        {
                            if (recursoRegencia.Validate())
                            {
                                recursoRegencia.idAula = idAula;
                                dtTurmaAulaRecursoRegencia.Rows.Add(CLS_TurmaAulaRecursoRegenciaBO.TurmaAulaRecursoRegenciaToDataRow(recursoRegencia, dtTurmaAulaRecursoRegencia.NewRow(), recursoRegencia.trr_dataAlteracao));
                            }
                            else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(recursoRegencia));
                        });

                        string errorMSG = string.Empty;
                        if (!CLS_TurmaNotaAlunoBO.ValidaParticipantesAvaliacaoDiarioClasse(sincronizacao.ltTurmaNota.Where(p => p.tnt_situacao != (byte)CLS_TurmaNotaSituacao.Excluido).ToList(),
                                                                                           sincronizacao.ltTurmaNotaAluno.Where(p => p.tna_situacao != 3).ToList(),
                                                                                           out errorMSG))
                            throw new ValidationException(errorMSG);

                        var atividadesNotas = from CLS_TurmaNota atividade in sincronizacao.ltTurmaNota
                                              join CLS_TurmaNotaAluno nota in sincronizacao.ltTurmaNotaAluno
                                              on atividade.idAtividade equals nota.idAtividade into notas
                                              join CLS_TurmaNotaRegencia atividadeRegencia in sincronizacao.ltTurmaNotaRegencia
                                              on atividade.idAtividade equals atividadeRegencia.idAtividade into atividadeRegencia
                                              join LOG_TurmaNota_Alteracao log in sincronizacao.ltLogAlteracaoNota
                                              on atividade.idAtividade equals log.idAtividade into logs
                                              select new
                                              {
                                                  atividade
                                                      ,
                                                  notas = notas.ToList()
                                                      ,
                                                  atividadeRegencia = atividadeRegencia.ToList()
                                                      ,
                                                  logs = logs.ToList()
                                              };
                        atividadesNotas.ToList().ForEach(atividadeNota =>
                        {
                            CLS_TurmaNota atividade = atividadeNota.atividade;
                            if (atividade.Validate() || atividade.tnt_situacao == (byte)CLS_TurmaNotaSituacao.Excluido)
                            {
                                atividade.idAula = idAula;
                                DataRow drAtividade = CLS_TurmaNotaBO.TurmaNotaToDataRow(atividade, dtTurmaNota.NewRow(), atividade.tnt_dataAlteracao);
                                dtTurmaNota.Rows.Add(drAtividade);
                                long idAtividade = Convert.ToInt32(dtTurmaNota.Rows[dtTurmaNota.Rows.IndexOf(drAtividade)]["idAtividade"]);

                                atividadeNota.logs.ForEach(log =>
                                {
                                    if (log.Validate())
                                    {
                                        log.idAtividade = idAtividade;
                                        dtLogNota.Rows.Add(LOG_TurmaNota_AlteracaoBO.LOGTurmaNotaAlteracaoToDataRow(log, dtLogNota.NewRow()));
                                    }
                                });

                                atividadeNota.notas.ForEach(nota =>
                                {
                                    if (nota.Validate() || nota.tna_situacao == (byte)CLS_TurmaNotaSituacao.Excluido)
                                    {
                                        nota.idAtividade = idAtividade;
                                        dtTurmaNotaAluno.Rows.Add(CLS_TurmaNotaAlunoBO.TurmaNotaAlunoToDataRow(nota, dtTurmaNotaAluno.NewRow(), nota.tna_dataAlteracao));
                                    }
                                    else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(nota));
                                });

                                atividadeNota.atividadeRegencia.ForEach(atividadeRegencia =>
                                {
                                    if (atividadeRegencia.Validate())
                                    {
                                        atividadeRegencia.idAtividade = idAtividade;
                                        dtTurmaNotaRegencia.Rows.Add(CLS_TurmaNotaRegenciaBO.TurmaNotaRegenciaToDataRow(atividadeRegencia, dtTurmaNotaRegencia.NewRow()));
                                    }
                                    else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(atividadeRegencia));
                                });

                            }
                            else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(atividade));
                        });

                        sincronizacao.ltTurmaAulaOrientacaoCurricular.ForEach(orientacaoCurricular =>
                        {
                            if (orientacaoCurricular.Validate())
                            {
                                orientacaoCurricular.idAula = idAula;
                                dtTurmaAulaOrientacaoCurricular.Rows.Add(CLS_TurmaAulaOrientacaoCurricularBO.TurmaAulaOrientacaoCurricularToDataRow(orientacaoCurricular, dtTurmaAulaOrientacaoCurricular.NewRow()));
                            }
                            else throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(orientacaoCurricular));
                        });

                        protocolo.tud_id = sincronizacao.entityAula.tud_id;
                        protocolo.tau_id = sincronizacao.entityAula.tau_id;
                        protocolo.tur_id = sincronizacao.entityAula.tur_id;
                        protocolo.pro_qtdeAlunos = sincronizacao.ltTurmaAulaAluno.Count;
                        protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComSucesso;
                        protocolo.pro_statusObservacao = String.Format("Protocolo processado com sucesso ({0}).",
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        protocolo.idAula = idAula;
                        dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));

                        SalvaAulaFrequenciaAtividadeNota
                        (
                            dtProtocolo,
                            dtTurmaAula,
                            dtTurmaAulaDisciplinaRelacionada,
                            dtTurmaAulaAluno,
                            dtTurmaAulaRecurso,
                            dtTurmaAulaRegencia,
                            dtTurmaAulaRecursoRegencia,
                            dtTurmaNota,
                            dtTurmaNotaRegencia,
                            dtTurmaNotaAluno,
                            dtTurmaAulaPlanoDisciplina,
                            dtTurmaAulaAlunoTipoAnotacao,
                            dtTurmaAulaOrientacaoCurricular,
                            dtLogAula,
                            dtLogNota,
                            dtTurmaAulaTerritorio,
                            alteraAnotacao,
                            bancoSincronizacao
                        );
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Atualiza a sequência das aulas da disciplina, ordenando-as por data.
        /// </summary>
        /// <param name="tud_id">ID da turma disicplina.</param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool AtualizarSequenciaAulasPorTurmaDisciplina(long tud_id, TalkDBTransaction banco = null)
        {
            CLS_TurmaAulaDAO dao = banco == null ? new CLS_TurmaAulaDAO() : new CLS_TurmaAulaDAO { _Banco = banco };
            return dao.AtualizarSequenciaAulasPorTurmaDisciplina(tud_id);
        }

        /// <summary>
        /// Altera/Insere as aulas passadas por parâmetro na tabela.
        /// </summary>
        /// <param name="tbTurmaAula">Tabela de aulas</param>
        /// <returns></returns>
        public static bool SalvarAulas(List<CLS_TurmaAula> ltTurmaAula, List<sDadosAulaProtocolo> ltDadosAulasValidacao, List<CLS_TurmaAula> ltAulasBanco, TalkDBTransaction banco = null)
        {
            DataTable dtTurmaAula = CLS_TurmaAula.TipoTabela_TurmaAula();

            ltTurmaAula.ForEach(p =>
                                     {
                                         if (ValidarAula(p, ltDadosAulasValidacao, ltAulasBanco))
                                             dtTurmaAula.Rows.Add(CLS_TurmaAulaBO.TurmaAulaToDataRow(p, dtTurmaAula.NewRow()));
                                     });

            return banco == null ?
                new CLS_TurmaAulaDAO().SalvarAulas(dtTurmaAula) :
                new CLS_TurmaAulaDAO { _Banco = banco }.SalvarAulas(dtTurmaAula);
        }

        /// <summary>
        /// O método salva todos os dados das aulas passadas por parâmetro.
        /// </summary>
        /// <param name="dtProtocolo"></param>
        /// <param name="dtTurmaAula"></param>
        /// <param name="dtTurmaAulaDisciplinaRelacionada"></param>
        /// <param name="dtTurmaAulaAluno"></param>
        /// <param name="dtTurmaAulaRecurso"></param>
        /// <param name="dtTurmaAulaRegencia"></param>
        /// <param name="dtTurmaAulaRecursoRegencia"></param>
        /// <param name="dtTurmaNota"></param>
        /// <param name="dtTurmaNotaAluno"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static bool SalvaAulaFrequenciaAtividadeNota
        (
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
            bool alteraAnotacao,
            TalkDBTransaction banco = null
        )
        {
            byte tud_tipoRegencia = (byte)TurmaDisciplinaTipo.Regencia;
            return banco == null ?
                new CLS_TurmaAulaDAO().SalvaAulaFrequenciaAtividadeNota
                                       (
                                           tud_tipoRegencia,
                                           dtProtocolo,
                                           dtTurmaAula,
                                           dtTurmaAulaDisciplinaRelacionada,
                                           dtTurmaAulaAluno,
                                           dtTurmaAulaRecurso,
                                           dtTurmaAulaRegencia,
                                           dtTurmaAulaRecursoRegencia,
                                           dtTurmaNota,
                                           dtTurmaNotaRegencia,
                                           dtTurmaNotaAluno,
                                           dtTurmaAulaPlanoDisciplina,
                                           dtTurmaAulaAlunoTipoAnotacao,
                                           dtTurmaAulaOrientacaoCurricular,
                                           dtLogAula,
                                           dtLogNota,
                                           dtTurmaAulaTerritorio,
                                           alteraAnotacao
                                       ) :
                new CLS_TurmaAulaDAO { _Banco = banco }.SalvaAulaFrequenciaAtividadeNota
                                                        (
                                                            tud_tipoRegencia,
                                                            dtProtocolo,
                                                            dtTurmaAula,
                                                            dtTurmaAulaDisciplinaRelacionada,
                                                            dtTurmaAulaAluno,
                                                            dtTurmaAulaRecurso,
                                                            dtTurmaAulaRegencia,
                                                            dtTurmaAulaRecursoRegencia,
                                                            dtTurmaNota,
                                                            dtTurmaNotaRegencia,
                                                            dtTurmaNotaAluno,
                                                            dtTurmaAulaPlanoDisciplina,
                                                            dtTurmaAulaAlunoTipoAnotacao,
                                                            dtTurmaAulaOrientacaoCurricular,
                                                            dtLogAula,
                                                            dtLogNota,
                                                            dtTurmaAulaTerritorio,
                                                            alteraAnotacao
                                                        );
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco - opcional</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="turmaIntegral">Indica se a turma é integral.</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// <param name="entTurma">Entidade da turma onde será salva a aula</param>
        /// <param name="entTurmaDisciplina">Entidade da disciplina onde será salva a aula</param>
        /// <param name="entCalendario">Entidade do calendário da turma onde será salva a aula</param>
        /// <param name="cap_dataInicio">Data de início do bimestre</param>
        /// <param name="cap_dataFim">Data de fim do bimestre</param>
        /// <param name="turmaDisciplinaRelacionada">Disciplina relacionada</param>
        /// <returns></returns>
        public static bool Save
        (
            CLS_TurmaAula entity
            , TalkDBTransaction banco
            , out string mensagemInfo
            , Guid ent_id
            , bool turmaIntegral
            , bool fechamentoAutomatico
            , List<VigenciaCriacaoAulas> vigenciasCriacaoAulas
            , TUR_Turma entTurma = null
            , TUR_TurmaDisciplina entTurmaDisciplina = null
            , ACA_CalendarioAnual entCalendario = null
            , DateTime? cap_dataInicio = null
            , DateTime? cap_dataFim = null
            , sTurmaDisciplinaRelacionada turmaDisciplinaRelacionada = new sTurmaDisciplinaRelacionada()
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
            , byte fav_tipoApuracaoFrequencia = 0
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();

            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            mensagemInfo = "";

            List<TUR_TurmaDisciplinaTerritorio> lstTerritorio = new List<TUR_TurmaDisciplinaTerritorio>();

            try
            {
                if (entity.Validate())
                {
                    // Verifica se a aula foi alterada/excluída por outra pessoa enquanto o usuário tentava alterar a mesma.
                    CLS_TurmaAula entityAulaAuxiliar = new CLS_TurmaAula
                    {
                        tud_id = entity.tud_id
                        ,
                        tau_id = entity.tau_id
                    };
                    GetEntity(entityAulaAuxiliar, dao._Banco);

                    if (!entityAulaAuxiliar.IsNew)
                    {
                        entity.usu_id = entityAulaAuxiliar.usu_id;
                        entity.tdt_posicao = entityAulaAuxiliar.tdt_posicao;

                        // Os tempos de aula não podem ser alterados se houver conteúdo lançado
                        if (entityAulaAuxiliar.tau_numeroAulas != entity.tau_numeroAulas
                            && CLS_TurmaAulaAlunoBO.VerificaLancamentoFrequencia(entity.tud_id, entity.tpc_id, entity.tau_id))
                        {
                            throw new ValidationException("A aula não pode ser alterada pois já existem dados lançados para a mesma.");
                        }
                    }

                    // Verifica se existe a aula cadastrada
                    if (VerificaAulaExistente(entity, dao._Banco))
                        throw new DuplicateNameException("Já existe uma aula cadastrada com este número.");


                    // Verifica se a data da aula está dentro do calendário e do período do calendário
                    if (entity.tau_data != new DateTime() && entity.tur_id > 0)
                    {
                        bool permiteVariasAulas = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_INCLUIR_VARIAS_AULAS_POR_DIA, ent_id);

                        bool aulasCriadasDia = VerificaAulaExistentePorDataPosicaoDocente(entity.tud_id, entity.tdt_posicao, entity.tau_data, entity.IsNew ? -1 : entity.tau_id);

                        if (!permiteVariasAulas && aulasCriadasDia)
                        {
                            throw new ArgumentException(String.Format("Aula salva com sucesso. Já existe uma aula cadastrada para o dia {0}.", entity.tau_data.ToString("dd/MM/yyyy")));
                        }

                        if (aulasCriadasDia && entity.tau_reposicao)
                        {
                            mensagemInfo = String.Format("Aula salva com sucesso. Já existe uma aula cadastrada para o dia {0}.", entity.tau_data.ToString("dd/MM/yyyy"));
                        }

                        // Verifica se a entidade recebida por parâmetro foi alimentada, se não foi, dá o GetEntity.
                        TUR_Turma tur = entTurma ??
                            TUR_TurmaBO.GetEntity(new TUR_Turma { tur_id = entity.tur_id }, dao._Banco);

                        // Verifica se a entidade recebida por parâmetro foi alimentada, se não foi, dá o GetEntity.
                        ACA_CalendarioAnual cal = entCalendario ??
                            ACA_CalendarioAnualBO.GetEntity(new ACA_CalendarioAnual { cal_id = tur.cal_id }, dao._Banco);

                        if (entity.tau_data.Date > cal.cal_dataFim.Date || entity.tau_data.Date < cal.cal_dataInicio.Date)
                            throw new ArgumentException("A data da aula deve estar dentro do período do calendário escolar (" + cal.cal_dataInicio.ToString("dd/MM/yyyy") + " - " + cal.cal_dataFim.ToString("dd/MM/yyyy") + ").");

                        DateTime dataIni = new DateTime();
                        DateTime dataFim = new DateTime();

                        if (cap_dataInicio != null && cap_dataFim != null)
                        {
                            // Verifica se as variáveis de data do bimestre foram passadas por parâmetro, para não fazer
                            // consulta no banco.
                            dataIni = Convert.ToDateTime(cap_dataInicio);
                            dataFim = Convert.ToDateTime(cap_dataFim);
                        }
                        else
                        {
                            DataTable dt = ACA_TipoPeriodoCalendarioBO.SelecionaTipoPeriodoCalendarioPorTipoPeriodoCalendario
                                (entity.tpc_id, tur.cal_id);

                            if (dt.Rows.Count > 0)
                            {
                                dataIni = Convert.ToDateTime(dt.Rows[0]["cap_dataInicio"].ToString());
                                dataFim = Convert.ToDateTime(dt.Rows[0]["cap_dataFim"].ToString());
                            }
                        }

                        if (dataIni != new DateTime() && dataFim != new DateTime())
                        {
                            if (entity.tau_data.Date > dataFim.Date || entity.tau_data.Date < dataIni.Date)
                                throw new ArgumentException("A data da aula deve estar dentro do período do calendário ("
                                    + dataIni.ToString("dd/MM/yyyy") + " - " + dataFim.ToString("dd/MM/yyyy") + ").");

                            if (entity.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia)
                            {
                                lstTerritorio = TUR_TurmaDisciplinaTerritorioBO.SelecionaVigentesPorExperienciaPeriodo(entity.tud_id, dataIni, dataFim, dao._Banco);

                                bool dataValida = lstTerritorio.Any();
                                bool vigenciaValida = false;

                                var vigencias = !lstTerritorio.Any() ? null :
                                                (from territorio in lstTerritorio
                                                 group territorio by new
                                                 {
                                                     tud_id = territorio.tud_idExperiencia
                                                     ,
                                                     VigenciaInicio = territorio.tte_vigenciaInicio
                                                     ,
                                                     VigenciaFim = territorio.tte_vigenciaFim
                                                 } into gExp
                                                 select new
                                                 {
                                                     tud_id = gExp.Key
                                                     ,
                                                     VigenciaInicio = gExp.Key.VigenciaInicio
                                                     ,
                                                     VigenciaFim = gExp.Key.VigenciaFim
                                                 });

                                if (lstTerritorio.Any())
                                {
                                    foreach (var vigencia in vigencias)
                                        if (entity.tau_data <= (vigencia.VigenciaFim == new DateTime() ? entity.tau_data : vigencia.VigenciaFim)
                                            && entity.tau_data >= vigencia.VigenciaInicio)
                                            vigenciaValida = true;
                                }

                                if (!dataValida || !vigenciaValida)
                                {
                                    // Se a data está fora da vigência.
                                    throw new ValidationException("Não foi possível incluir aula neste dia." +
                                                                (!lstTerritorio.Any() ? " Não há vigência para a experiência no período ("
                                                                + dataIni.ToString("dd/MM/yyyy") + " - " + dataFim.ToString("dd/MM/yyyy") + ")." :
                                                                (vigencias.Count() == 1 ? " Vigência" : " Vigências") +
                                                                " para a experiência: <br>" + vigencias.Select(v => "(" + v.VigenciaInicio.ToString("dd/MM/yyyy") +
                                                                                                                    " - " + v.VigenciaFim.ToString("dd/MM/yyyy") + ")")
                                                                                                       .Aggregate((a, b) => a + "<br>" + b)));
                                }
                            }
                        }
                    }

                    // Valida se a aula está dentro da vigência que deve ser verificada.
                    if (vigenciasCriacaoAulas != null)
                    {
                        // Busca se existe alguma atribuição para essa disciplina que deve verificar a vigência.
                        VigenciaCriacaoAulas vig = vigenciasCriacaoAulas.Find(p => p.tud_id == entity.tud_id);
                        if (vig.tud_id > 0)
                        {
                            if (!(entity.tau_data <= (vig.tdt_vigenciaFim == new DateTime() ? entity.tau_data : vig.tdt_vigenciaFim)
                            && entity.tau_data >= vig.tdt_vigenciaInicio))
                            {
                                // Se a data está fora da vigência.
                                throw new ArgumentException(CustomResource.GetGlobalResourceObject("BLL", "CLS_TurmaAula.ValidacaoDataAtribuicaoAula").ToString());
                            }
                        }
                    }

                    DateTime dataInicioSemana, dataFimSemana;
                    RetornaPeriodoSemana(entity.tau_data, out dataInicioSemana, out dataFimSemana);

                    bool entityIsNew = entity.IsNew;

                    // Salva a aula da disciplina da turma
                    dao.Salvar(entity);

                    // Salva a informação de pendência do plano de aula.
                    CLS_TurmaAulaPendenciaBO.AtualizarPendencia(entity.tud_id, entity.tau_id, string.IsNullOrEmpty(entity.tau_planoAula));

                    if (entity.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia)
                    {
                        // Territórios vigentes dentro da semana.
                        var territoriosVigentes = lstTerritorio.Where
                            (t => t.tte_vigenciaInicio.Date <= dataFimSemana.Date
                            && (t.tte_vigenciaFim == new DateTime() || t.tte_vigenciaFim.Date >= dataInicioSemana.Date)).ToList();

                        // Ligação da experiência com territórios nas aulas da semana.
                        List<TurmaAulaTerritorioDados> aulasTerritorios = new List<TurmaAulaTerritorioDados>();
                        Dictionary<long, int> dicTerritorios = new Dictionary<long, int>();

                        aulasTerritorios = CLS_TurmaAulaTerritorioBO.SelecionaAulasTerritorioPorExperiencia
                            (entity.tud_id, dataInicioSemana, dataFimSemana, dao._Banco);

                        dicTerritorios = (from TUR_TurmaDisciplinaTerritorio item in
                                          territoriosVigentes.Where(t => !aulasTerritorios.Any(a => a.tud_idTerritorio == t.tud_idTerritorio &&
                                                                                                    a.tau_idExperiencia != entity.tau_id))
                                          select new
                                          {
                                              tud_id = item.tud_idTerritorio
                                              ,
                                              tud_nomeTerritorio = item.tud_nomeTerritorio
                                              ,
                                              qtAulas = (from TurmaAulaTerritorioDados iAula in aulasTerritorios
                                                         where
                                                         iAula.tud_idExperiencia == item.tud_idExperiencia
                                                         && iAula.tud_idTerritorio == item.tud_idTerritorio
                                                         select iAula.tud_idTerritorio).Count()
                                          }).OrderBy(p => p.tud_nomeTerritorio).ToDictionary(p => p.tud_id, p => p.qtAulas);

                        //Se for uma edição de aula então pega apenas as aulas ligadas à ela
                        aulasTerritorios = aulasTerritorios.Where(a => a.tud_idExperiencia == entity.tud_id &&
                                                                       a.tau_idExperiencia == entity.tau_id).ToList();

                        //Valida a carga horária da experiência
                        if (!dicTerritorios.Any() && !aulasTerritorios.Any())
                            throw new ArgumentException("A soma da quantidade de aulas da semana do dia " +
                                                        dataInicioSemana.ToString("dd/MM/yyyy") + " ao dia " +
                                                        dataFimSemana.ToString("dd/MM/yyyy") +
                                                        " não pode ser maior do que " + territoriosVigentes.Count.ToString() + ".");

                        // Verifica ligações com territórios quando a aula é de experiência.
                        CriaLigacoesTerritorios(usu_id, origemLogAula, entity, dao._Banco, aulasTerritorios, dicTerritorios);

                        //SalvarAulaTerritorio(entity, dao._Banco);
                    }

                    byte tud_tipo = 0;

                    // Verifica se a quantidade de aulas semanais não foram ultrapassadas.
                    if (entity.tau_data != new DateTime() && entity.tur_id > 0)
                    {
                        // Verifica se a entidade recebida por parâmetro foi alimentada, se não foi, dá o GetEntity.
                        TUR_TurmaDisciplina tud = entTurmaDisciplina ??
                            TUR_TurmaDisciplinaBO.GetEntity(new TUR_TurmaDisciplina { tud_id = entity.tud_id }, dao._Banco);

                        tud_tipo = tud.tud_tipo;

                        bool DisciplinaPrincipal = tud.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal;
                        List<TUR_TurmaDisciplina> listaDisciplinas = null;
                        if (tud.tud_global)
                        {
                            listaDisciplinas = TUR_TurmaDisciplinaBO.GetSelectBy_Turma(entity.tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                            DisciplinaPrincipal = listaDisciplinas.Exists(p => p.tud_tipo == (byte)TurmaDisciplinaTipo.DisciplinaPrincipal);
                        }

                        // Se for a disciplina principal não precisa validar.
                        if (!DisciplinaPrincipal && !entity.tau_reposicao)
                        {
                            int CargaHoraria = tud.tud_cargaHorariaSemanal;

                            if (tud.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia)
                            {
                                // Territórios vigentes dentro da semana.
                                var territoriosVigentes = lstTerritorio.Where
                                    (t => t.tte_vigenciaInicio.Date <= dataFimSemana.Date
                                    && (t.tte_vigenciaFim == new DateTime() || t.tte_vigenciaFim.Date >= dataInicioSemana.Date)).ToList();

                                CargaHoraria = territoriosVigentes.Count();
                            }

                            if (tud.tud_global)
                            {
                                List<TUR_TurmaDisciplina> lista = listaDisciplinas ??
                                    TUR_TurmaDisciplinaBO.GetSelectBy_Turma(entity.tur_id, null, GestaoEscolarUtilBO.MinutosCacheLongo);
                                var x = from TUR_TurmaDisciplina dis in lista
                                        group dis by
                                            new { }
                                            into g
                                        select new { total = g.Sum(dis => dis.tud_cargaHorariaSemanal) };

                                if (x.Count() > 0)
                                    CargaHoraria = x.First().total;
                            }

                            int quantidadeAulas = VerificaSomaNumeroAulasSemana(entity.tud_id, dataInicioSemana, dataFimSemana,
                                                                                dao._Banco, entity.tdt_posicao);

                            EnumTipoDocente enumTipoDocente = ACA_TipoDocenteBO.SelecionaTipoDocentePorPosicao(entity.tdt_posicao, 0);

                            // verifico qual o tipo do docente para verificar a qtde de aula digitada no dia.
                            // Regra.: para esses tipos a qtde de aulas por dia deve ser 1 
                            if (!entity.tau_reposicao &&
                                ((turmaIntegral || tud.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia) &&
                                 (enumTipoDocente == EnumTipoDocente.Titular ||
                                  enumTipoDocente == EnumTipoDocente.SegundoTitular ||
                                  enumTipoDocente == EnumTipoDocente.Substituto))
                               )
                            {
                                // utilizo o mesmo método passado nos campos data inicial e final a data da aula, assim 
                                // retorna a qtde de aulas do dia.
                                int quantidadeAulasDia = VerificaSomaNumeroAulasSemana(entity.tud_id, entity.tau_data, entity.tau_data,
                                                                                dao._Banco, entity.tdt_posicao);

                                if (enumTipoDocente == EnumTipoDocente.Substituto && turmaIntegral && tud.tud_tipo != (byte)TurmaDisciplinaTipo.Experiencia)
                                {
                                    if (quantidadeAulasDia > 2)
                                    {
                                        throw new ArgumentException("Já existem duas aulas cadastradas para o dia " + entity.tau_data.ToString("dd/MM/yyyy") + ".");
                                    }
                                }
                                else if (fav_tipoApuracaoFrequencia == (byte)ACA_FormatoAvaliacaoTipoApuracaoFrequencia.TemposAula)
                                {
                                    if (quantidadeAulas > tud.tud_cargaHorariaSemanal)
                                    {
                                        throw new ArgumentException("A quantidade de aulas da semana foi excedida.");
                                    }

                                }
                                else if (quantidadeAulasDia > 1)
                                {
                                    throw new ArgumentException("Já existe uma aula cadastrada para o dia " + entity.tau_data.ToString("dd/MM/yyyy") + ".");
                                }
                            }

                            // objetivo é executar para os demais docentes (menos compartilhado e projeto) e que a disciplina não seja regencia, por isso é negado o if abaixo
                            if (!entity.tau_reposicao &&
                                !(tud.tud_tipo == (byte)TurmaDisciplinaTipo.Regencia &&
                                 (enumTipoDocente == EnumTipoDocente.Compartilhado ||
                                 enumTipoDocente == EnumTipoDocente.Projeto)))
                            {
                                if (quantidadeAulas > CargaHoraria)
                                {
                                    throw new ArgumentException("A soma da quantidade de aulas da semana do dia " +
                                                            dataInicioSemana.ToString("dd/MM/yyyy") + " ao dia " +
                                                            dataFimSemana.ToString("dd/MM/yyyy") +
                                                            " não pode ser maior do que " + CargaHoraria + ".");
                                }
                            }
                        }

                        // Relaciono a aula com a disciplina compartilhada
                        if (tud.tud_tipo == (byte)TurmaDisciplinaTipo.DocenciaCompartilhada
                            && turmaDisciplinaRelacionada.tud_id > 0
                            && entity.IsNew)
                        {
                            CLS_TurmaAulaDisciplinaRelacionada entTurmaAulaDisciplinaRelacionada = new CLS_TurmaAulaDisciplinaRelacionada
                            {
                                tud_id = entity.tud_id
                                ,
                                tau_id = entity.tau_id
                                ,
                                tdr_id = turmaDisciplinaRelacionada.tdr_id
                                ,
                                tud_idRelacionada = turmaDisciplinaRelacionada.tud_id
                                ,
                                IsNew = true
                            };
                            CLS_TurmaAulaDisciplinaRelacionadaBO.Save(entTurmaAulaDisciplinaRelacionada, dao._Banco);
                        }
                    }

                    // Se for alteração atualiza a data das atividades relacionadas com a aula.
                    if (!entity.IsNew)
                        CLS_TurmaNotaBO.AtualizaDataAtividade(entity.tud_id, entity.tau_id, entity.tau_data, dao._Banco);

                    if (origemLogAula > 0)
                    {
                        DateTime dataLogAula = DateTime.Now;
                        LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                        {
                            tud_id = entity.tud_id,
                            tau_id = entity.tau_id,
                            usu_id = usu_id,
                            lta_origem = origemLogAula,
                            lta_tipo = tipoLogAula,
                            lta_data = dataLogAula
                        };

                        LOG_TurmaAula_AlteracaoBO.Save(entLogAula, dao._Banco);
                    }

                    // Caso o fechamento seja automático, grava na fila de processamento.
                    if (fechamentoAutomatico && tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia && entity.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                    {
                        CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(entity.tud_id, entity.tpc_id, dao._Banco);
                    }

                    return true;
                }

                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
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
        /// Retorna o início e fim da semana da data informada.
        /// </summary>
        private static void RetornaPeriodoSemana(DateTime data, out DateTime dataInicioSemana, out DateTime dataFimSemana)
        {
            dataInicioSemana = new DateTime();
            dataFimSemana = new DateTime();
            switch (data.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dataInicioSemana = data;
                    dataFimSemana = data.AddDays(6);
                    break;

                case DayOfWeek.Monday:
                    dataInicioSemana = data.AddDays(-1);
                    dataFimSemana = data.AddDays(5);
                    break;

                case DayOfWeek.Tuesday:
                    dataInicioSemana = data.AddDays(-2);
                    dataFimSemana = data.AddDays(4);
                    break;

                case DayOfWeek.Wednesday:
                    dataInicioSemana = data.AddDays(-3);
                    dataFimSemana = data.AddDays(3);
                    break;

                case DayOfWeek.Thursday:
                    dataInicioSemana = data.AddDays(-4);
                    dataFimSemana = data.AddDays(2);
                    break;

                case DayOfWeek.Friday:
                    dataInicioSemana = data.AddDays(-5);
                    dataFimSemana = data.AddDays(1);
                    break;

                case DayOfWeek.Saturday:
                    dataInicioSemana = data.AddDays(-6);
                    dataFimSemana = data;
                    break;
            }
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="banco">Conexão aberta com o banco de dados</param>
        /// <param name="ent_id">Id da entidade do usuário logado.</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Save
        (
            CLS_TurmaAula entity
            , TalkDBTransaction banco
            , Guid ent_id
            , bool fechamentoAutomatico
        )
        {
            string mensagemInfo;

            bool turmaIntegral = ACA_TipoTurnoBO.SelecionaEntTipoTurnoPorTurma(entity.tur_id).ttn_tipo == (byte)ACA_TipoTurnoBO.TipoTurno.Integral;
            return Save(entity, banco, out mensagemInfo, ent_id, turmaIntegral, fechamentoAutomatico, null, null, null, null, null);
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco - opcional</param>
        /// <returns></returns>
        public static bool Delete
        (
            CLS_TurmaAula entity
            , TalkDBTransaction banco
            , byte lta_origem
            , byte lta_tipo
            , Guid usu_id
        )
        {
            bool retorno = Delete(entity, banco);

            if (lta_origem > 0 && lta_tipo > 0 && !usu_id.Equals(new Guid()))
            {
                DateTime dataLogAula = DateTime.Now;
                LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                {
                    tud_id = entity.tud_id,
                    tau_id = entity.tau_id,
                    usu_id = usu_id,
                    lta_origem = lta_origem,
                    lta_tipo = lta_tipo,
                    lta_data = dataLogAula
                };

                LOG_TurmaAula_AlteracaoBO.Save(entLogAula, banco);
            }

            return retorno;
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco - opcional</param>
        /// <returns></returns>
        public static bool Save
        (
            CLS_TurmaAula entity
            , TalkDBTransaction banco
            , byte lta_origem
            , byte lta_tipo
            , Guid usu_id
        )
        {
            bool retorno = Save(entity, banco);
            CLS_TurmaAulaPendenciaBO.AtualizarPendencia(entity.tud_id, entity.tau_id, string.IsNullOrEmpty(entity.tau_planoAula));

            if (lta_origem > 0 && lta_tipo > 0 && !usu_id.Equals(new Guid()))
            {
                DateTime dataLogAula = DateTime.Now;
                LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                {
                    tud_id = entity.tud_id,
                    tau_id = entity.tau_id,
                    usu_id = usu_id,
                    lta_origem = lta_origem,
                    lta_tipo = lta_tipo,
                    lta_data = dataLogAula
                };

                LOG_TurmaAula_AlteracaoBO.Save(entLogAula, banco);
            }

            return retorno;
        }

        /// <summary>
        /// Inclui ou altera a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade a ser salva</param>
        /// <param name="banco">Transação com banco - opcional</param>
        /// <param name="turmaDisciplinaRelacionada">Disciplina relacionada</param>
        /// <returns></returns>
        public static bool Save
        (
            CLS_TurmaAula entity
            , TalkDBTransaction banco
            , sTurmaDisciplinaRelacionada turmaDisciplinaRelacionada
            , byte lta_origem = 0
            , byte lta_tipo = 0
            , Guid usu_id = new Guid()
        )
        {
            bool retorno = Save(entity, banco, lta_origem, lta_tipo, usu_id);

            // Relaciono a aula com a disciplina compartilhada
            if (turmaDisciplinaRelacionada.tud_id > 0
                && entity.IsNew)
            {
                CLS_TurmaAulaDisciplinaRelacionada entTurmaAulaDisciplinaRelacionada = new CLS_TurmaAulaDisciplinaRelacionada
                {
                    tud_id = entity.tud_id
                        ,
                    tau_id = entity.tau_id
                        ,
                    tdr_id = turmaDisciplinaRelacionada.tdr_id
                        ,
                    tud_idRelacionada = turmaDisciplinaRelacionada.tud_id
                        ,
                    IsNew = true
                };
                retorno &= CLS_TurmaAulaDisciplinaRelacionadaBO.Save(entTurmaAulaDisciplinaRelacionada, banco);
            }

            return retorno;
        }

        /// <summary>
        /// Deleta logicamente a aula da disciplina da turma
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaAula</param>
        /// <param name="fechamentoAutomatico">Indica se o fechamento é automático.</param>
        /// <returns>True = deletado/alterado | False = não deletado/alterado</returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static bool Delete
        (
            CLS_TurmaAula entity
            , bool fechamentoAutomatico
            , bool permissaoExcluirDiretor
            , int tje_id
            , string lte_observacao
            , Guid usu_id = new Guid()
            , byte origemLogAula = 0
            , byte tipoLogAula = 0
            , byte tud_tipo = 0
            , int fav_id = 0
            , long tur_id = 0
            , Guid ent_id = new Guid()
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                //Deleta logicamente a aula
                Delete(entity, dao._Banco);

                // Limpa o cache do fechamento
                try
                {
                    string chave = string.Empty;
                    List<ACA_Avaliacao> avaliacao = ACA_AvaliacaoBO.GetSelectBy_FormatoAvaliacaoPeriodo(fav_id, entity.tpc_id);

                    if (avaliacao.Any())
                    {
                        int ava_id = avaliacao.First().ava_id;
                        if (entity.tud_id > 0)
                        {
                            chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodo(entity.tud_id, fav_id, ava_id, string.Empty);
                            CacheManager.Factory.RemoveByPattern(chave);

                            chave = MTR_MatriculaTurmaDisciplinaBO.RetornaChaveCache_GetSelectBy_TurmaDisciplinaPeriodoFiltroDeficiencia(entity.tud_id, fav_id, ava_id, string.Empty);
                            CacheManager.Factory.RemoveByPattern(chave);
                        }
                        else
                        {
                            chave = MTR_MatriculaTurmaBO.RetornaChaveCache_GetSelectBy_Turma_Periodo(entity.tur_id, fav_id, ava_id);
                            HttpContext.Current.Cache.Remove(chave);
                        }
                    }
                }
                catch
                { }

                // Caso o fechamento seja automático, grava na fila de processamento.
                if (fechamentoAutomatico && tud_tipo != (byte)TurmaDisciplinaTipo.DocenteEspecificoComplementacaoRegencia && entity.tpc_id != ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.TIPO_PERIODO_CALENDARIO_RECESSO, ent_id))
                {
                    CLS_AlunoFechamentoPendenciaBO.SalvarFilaFrequencia(entity.tud_id, entity.tpc_id, dao._Banco);
                }

                if (origemLogAula > 0)
                {
                    DateTime dataLogAula = DateTime.Now;
                    LOG_TurmaAula_Alteracao entLogAula = new LOG_TurmaAula_Alteracao
                    {
                        tud_id = entity.tud_id,
                        tau_id = entity.tau_id,
                        usu_id = usu_id,
                        lta_origem = origemLogAula,
                        lta_tipo = tipoLogAula,
                        lta_data = dataLogAula
                    };

                    LOG_TurmaAula_AlteracaoBO.Save(entLogAula, dao._Banco);
                }

                if (permissaoExcluirDiretor && tje_id > 0)
                {
                    LOG_TurmaAula_Exclusao entLogExclusaoAula = new LOG_TurmaAula_Exclusao
                    {
                        lte_id = Guid.NewGuid(),
                        tud_id = entity.tud_id,
                        tau_id = entity.tau_id,
                        tje_id = tje_id,
                        lte_observacao = lte_observacao,
                        usu_id = usu_id,
                        lte_data = DateTime.Now
                    };

                    LOG_TurmaAula_ExclusaoBO.Save(entLogExclusaoAula, dao._Banco);
                }

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

        /// <summary>
        /// Atualiza o campo tau_efetivado das aulas.
        /// </summary>
        /// <param name="listaTurmaAula">Lista das aulas.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool AtualizarEfetivado(List<CLS_TurmaAula> listaTurmaAula, TalkDBTransaction banco = null)
        {
            DataTable dtTurmaAula = CLS_TurmaAula.TipoTabela_TurmaAula();
            if (listaTurmaAula.Any())
            {
                object lockObject = new object();

                Parallel.ForEach
                (
                    listaTurmaAula,
                    turmaAula =>
                    {
                        lock (lockObject)
                        {
                            dtTurmaAula.Rows.Add(TurmaAulaToDataRow(turmaAula, dtTurmaAula.NewRow()));
                        }
                    }
                );

                return banco == null ?
                       new CLS_TurmaAulaDAO().AtualizarEfetivado(dtTurmaAula) :
                       new CLS_TurmaAulaDAO { _Banco = banco }.AtualizarEfetivado(dtTurmaAula);
            }

            return true;
        }

        /// <summary>
        /// Salva o plano de aulas de aulas normais e de regência na mesma transação
        /// </summary>
        /// <param name="lstTurmaAula">Lista de aulas</param>
        /// <param name="lstTurmaAulaPlanoDisc">Lista dos componentes da regência vinculados ao plano de aula</param>
        /// <param name="lstTurmaAulaPlanoDiscDeletar">Lista de aulas para limpar o vinculo com os componentes da regência</param>
        public static void SalvarAulasReg(List<CLS_TurmaAula> lstTurmaAula
                                          , List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDisc
                                          , List<CLS_TurmaAulaPlanoDisciplina> lstTurmaAulaPlanoDiscDeletar)
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            dao._Banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                SalvarAulas(lstTurmaAula, new List<sDadosAulaProtocolo>(), new List<CLS_TurmaAula>(), dao._Banco);
                if (lstTurmaAulaPlanoDisc != null)
                {
                    CLS_TurmaAulaPlanoDisciplinaBO.DeletarAulasPlanos(lstTurmaAulaPlanoDiscDeletar, dao._Banco);
                    CLS_TurmaAulaPlanoDisciplinaBO.SalvarAulasPlanos(lstTurmaAulaPlanoDisc, dao._Banco);
                }
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

        /// <summary>
        /// Atualiza o campo tau_statusAtividadeAvaliativa das aulas.
        /// </summary>
        /// <param name="dtTurmaAula">DataTable das aulas.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool AtualizarStatusAtividadeAvaliativa(List<CLS_TurmaAula> listaTurmaAula, TalkDBTransaction banco = null)
        {
            DataTable dtTurmaAula = CLS_TurmaAula.TipoTabela_TurmaAula();
            if (listaTurmaAula.Any())
            {
                object lockObject = new object();
                Parallel.ForEach
                (
                    listaTurmaAula,
                    turmaAula =>
                    {
                        lock (lockObject)
                        {
                            DataRow dr = dtTurmaAula.NewRow();
                            dtTurmaAula.Rows.Add(TurmaAulaToDataRow(turmaAula, dr));
                        }
                    }
                );

                return banco == null ?
                       new CLS_TurmaAulaDAO().AtualizarStatusAtividadeAvaliativa(dtTurmaAula) :
                       new CLS_TurmaAulaDAO { _Banco = banco }.AtualizarStatusAtividadeAvaliativa(dtTurmaAula);
            }

            return true;
        }

        /// <summary>
        /// Método save sobrescrito para limpar o cache.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Save
        (
            CLS_TurmaAula entity,
            TalkDBTransaction banco = null
        )
        {
            if (banco != null)
                return new CLS_TurmaAulaDAO { _Banco = banco }.Salvar(entity);
            else
                return new CLS_TurmaAulaDAO().Salvar(entity);
        }

        /// <summary>
        /// Método delete sobrescrito para limpar o cache.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public new static bool Delete
        (
            CLS_TurmaAula entity,
            TalkDBTransaction banco = null
        )
        {
            CLS_TurmaAulaDAO dao = new CLS_TurmaAulaDAO();
            if (banco != null)
                dao._Banco = banco;

            //Verifica se possui atividade e se for a última atividade e algum aluno da turma possuir nota/parecer final então não permite excluir.
            if (entity.tud_id > 0 && entity.tpc_id > 0 && entity.tau_id > 0)
            {
                DataTable dtAtividade = CLS_TurmaNotaBO.SelecionaPorTurmaDisciplinaPeriodoCalendario(entity.tud_id, entity.tpc_id, 0, Guid.Empty, dao._Banco);
                if (dtAtividade.AsEnumerable().Any(p => Convert.ToInt32(p["tau_id"]) == entity.tau_id) &&
                    !dtAtividade.AsEnumerable().Any(p => Convert.ToInt32(p["tau_id"]) != entity.tau_id) &&
                    CLS_AlunoAvaliacaoTurmaDisciplinaMediaBO.BuscaNotasFinaisTud(entity.tud_id, entity.tpc_id, dao._Banco).Any(p => !string.IsNullOrEmpty(p.atm_media)))
                    throw new ValidationException(String.Format(CustomResource.GetGlobalResourceObject("BLL", "CLS_TurmaAula.ValidacaoExclusaoUltimaAvaliacao").ToString(),
                                                                entity.tau_data != new DateTime() ? " " + entity.tau_data.ToShortDateString() : ""));
            }
            return dao.Delete(entity);
        }

        /// <summary>
        /// Verifica se a aula está sendo criada em uma disciplina "Experiência do saber" e cria/altera as ligações necessárias 
        /// com territórios do saber.
        /// </summary>
        internal static void CriaLigacoesTerritorios
            (Guid usu_id, byte origemLogAula, CLS_TurmaAula tauExperiencia, TalkDBTransaction banco
            , List<TurmaAulaTerritorioDados> aulasTerritorios, Dictionary<long, int> dicTerritorios)
        {
            if (tauExperiencia.tud_tipo == (byte)TurmaDisciplinaTipo.Experiencia)
            {
                // Gravar os dados da ligação da aula com território.
                int qtLigacoesCriar = tauExperiencia.tau_numeroAulas -
                        (from TurmaAulaTerritorioDados iAula in aulasTerritorios
                         where iAula.tud_idExperiencia == tauExperiencia.tud_id
                         && iAula.tau_idExperiencia == tauExperiencia.tau_id
                         select iAula).Count();

                //Atualiza a data da aula do território
                foreach (TurmaAulaTerritorioDados tat in aulasTerritorios)
                {
                    CLS_TurmaAula tauTerr = new CLS_TurmaAula
                    {
                        tud_id = tat.tud_idTerritorio,
                        tau_id = tat.tau_idTerritorio
                    };
                    CLS_TurmaAulaBO.GetEntity(tauTerr, banco);

                    if (!tauTerr.IsNew)
                    {
                        tauTerr.tau_data = tauExperiencia.tau_data;

                        Save(tauTerr, banco);
                    }
                }

                if (qtLigacoesCriar < 0)
                {
                    // Precisa excluir ligações com territórios, pois a quantidade de aulas diminuiu.
                    while (qtLigacoesCriar < 0)
                    {
                        // Ordenar pelo nome do território.
                        TurmaAulaTerritorioDados ligacaoExcluir =
                            (from TurmaAulaTerritorioDados iAula in aulasTerritorios
                             where iAula.tud_idExperiencia == tauExperiencia.tud_id
                             && iAula.tau_idExperiencia == tauExperiencia.tau_id
                             && dicTerritorios.ContainsKey(iAula.tud_idTerritorio)
                             && dicTerritorios[iAula.tud_idTerritorio] > 0
                             select iAula).OrderByDescending(p => p.tud_nomeTerritorio).FirstOrDefault();

                        if (ligacaoExcluir != null)
                        {
                            CLS_TurmaAula tauExcluir = new CLS_TurmaAula
                            {
                                tud_id = ligacaoExcluir.tud_idTerritorio
                                ,
                                tau_id = ligacaoExcluir.tau_idTerritorio
                            };
                            Delete(tauExcluir, banco);

                            CLS_TurmaAulaTerritorio tatExcluir = new CLS_TurmaAulaTerritorio
                            {
                                tud_idExperiencia = ligacaoExcluir.tud_idExperiencia,
                                tau_idExperiencia = ligacaoExcluir.tau_idExperiencia,
                                tud_idTerritorio = ligacaoExcluir.tud_idTerritorio,
                                tau_idTerritorio = ligacaoExcluir.tau_idTerritorio
                            };
                            CLS_TurmaAulaTerritorioBO.Delete(tatExcluir, banco);

                            // Decrementa 1 no contador de aulas do território.
                            dicTerritorios[ligacaoExcluir.tud_idTerritorio]--;
                            qtLigacoesCriar++;
                        }
                    }
                }
                else if (qtLigacoesCriar > 0)
                {
                    while (qtLigacoesCriar > 0)
                    {
                        // Ordenar pelos itens que tem menos aulas.
                        KeyValuePair<long, int> terr = dicTerritorios.OrderBy(p => p.Value).First();

                        // Criar as ligações que faltam com territórios.
                        CLS_TurmaAula tauTerr = new CLS_TurmaAula
                        {
                            tud_id = terr.Key,
                            tau_numeroAulas = 1, // Fixo 1 pois cada território equivale a 1 tempo.
                            tdt_posicao = tauExperiencia.tdt_posicao,
                            tau_data = tauExperiencia.tau_data,
                            tpc_id = tauExperiencia.tpc_id,
                            tau_reposicao = tauExperiencia.tau_reposicao,
                            usu_id = usu_id,
                            IsNew = true
                        };
                        Save(tauTerr, banco, origemLogAula, (byte)LOG_TurmaAula_Alteracao_Tipo.AlteracaoAula, usu_id);

                        CLS_TurmaAulaTerritorio aulaTer = new CLS_TurmaAulaTerritorio
                        {
                            tud_idExperiencia = tauExperiencia.tud_id,
                            tau_idExperiencia = tauExperiencia.tau_id,
                            tud_idTerritorio = terr.Key,
                            tau_idTerritorio = tauTerr.tau_id,
                            IsNew = true
                        };
                        CLS_TurmaAulaTerritorioBO.Save(aulaTer, banco);

                        // Incrementa 1 no ID do território utilizado.
                        dicTerritorios[terr.Key] = terr.Value + 1;
                        qtLigacoesCriar--;
                    }
                }
            }
        }

        #endregion Saves
    }
}