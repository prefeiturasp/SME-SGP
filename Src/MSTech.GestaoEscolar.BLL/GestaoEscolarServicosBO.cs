
using MSTech.CoreSSO.BLL;
using System;
using System.Text;
using System.Net.Mail;
using System.Collections.Generic;
using MSTech.GestaoEscolar.Entities;
using System.Data;
using System.Linq;
using MSTech.CoreSSO.DAL;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using System.Net;
using MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Saida;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MSTech.GestaoEscolar.BLL
{
    public class GestaoEscolarServicosBO
    {
        public struct sChavesCacheFechamento
        {
            public string tur_id { get; set; }
            public string tud_id { get; set; }
            public string fav_id { get; set; }
            public string ava_id { get; set; }
            public string tpc_id { get; set; }
        }

        public struct sChaveCachePendenciaFechamento
        {
            public string esc_id { get; set; }
            public string uni_id { get; set; }
            public string cal_id { get; set; }
            public string tud_id { get; set; }
        }

        #region Propriedades
        private static int TENTATIVAS_PROCESSAMENTO_PROTOCOLO = 5;
        #endregion

        #region Enumeradores

        /// <summary>
        /// Enumerador das frequencias utilizadas para o serviço
        /// </summary>
        public enum Frequencias : byte
        {
            Diario = 1
            ,
            Semanal = 2
            ,
            Mensal = 3
            ,
            SegundaSexta = 4
            ,
            SabadoDomingo = 5
            ,
            VariasVezesDia = 6
            ,
            HoraEmHora = 7
            ,
            Personalizado = 8
            ,
            IntervaloMinutos = 9
            ,
            IntervaloSegundos = 10
        }

        public enum DiasSemana : byte
        {
            Domingo = 1,
            Segunda = 2,
            Terca = 3,
            Quarta = 4,
            Quinta = 5,
            Sexta = 6,
            Sabado = 7
        }

        public enum eServicoAtivo : byte
        {
            Desabilitado = 0
            ,
            Ativo = 1
        }

        #endregion

        public static void ExecJobArquivoExclusao()
        {
            GestaoEscolarServicoDAO dao = new GestaoEscolarServicoDAO();
            dao.ExecJobArquivoExclusaoAsync();
        }

        /// <summary>
        /// Retorna a expressão de configuração de acordo com o nome do trigger.
        /// </summary>
        /// <param name="trigger">Nome do trigger.</param>
        /// <param name="expressao">Expressão de configuração.</param>
        /// <returns>Verdadeiro se encontrou uma expressão para o trigger.</returns>
        public static bool SelecionaExpressaoPorTrigger(string trigger, out string expressao)
        {
            expressao = string.Empty;

            GestaoEscolarServicoDAO dao = new GestaoEscolarServicoDAO();

            DataTable dt = dao.SelecionaExpressaoPorTrigger(trigger);

            if (dt.Rows.Count > 0)
            {
                expressao = dt.Rows[0]["cron_expression"].ToString();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Executa a sincronização dos dados alterados no diário de classe. (Aulas)
        /// </summary>
        public static void ExecJobAtualizaAulasDiarioClasse()
        {
            List<DCL_Protocolo> ltProtocolo = DCL_ProtocoloBO.SelecionaNaoProcessadosPorTipo
                (DCL_ProtocoloBO.eTipo.Aula,
                 ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_AULA, new Guid()));

            ltProtocolo.ForEach(protocolo =>
                {
                    // Marca o protocolo como "Em processamento".
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.EmProcessamento;
                    protocolo.pro_tentativa++;
                });

            DCL_ProtocoloBO.AtualizaListaProtocolos(ltProtocolo);

            CLS_TurmaAulaBO.ProcessaProtocoloAulas(ltProtocolo, TENTATIVAS_PROCESSAMENTO_PROTOCOLO);
        }

        /// <summary>
        /// Executa a sincronização dos dados alterados no diário de classe. (Planejamento anual e bimestral)
        /// </summary>
        public static void ExecJobAtualizaPlanejamentoDiarioClasse()
        {
            List<DCL_Protocolo> ltProtocolo = DCL_ProtocoloBO.SelecionaNaoProcessadosPorTipo
                (DCL_ProtocoloBO.eTipo.PlanejamentoAnual,
                 ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_PLANEJAMENTO, new Guid()));

            ltProtocolo.ForEach(protocolo =>
            {
                // Marca o protocolo como "Em processamento".
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.EmProcessamento;
                protocolo.pro_tentativa++;
            });

            DCL_ProtocoloBO.AtualizaListaProtocolos(ltProtocolo);

            CLS_TurmaDisciplinaPlanejamentoBO.ProcessaProtocoloPlanejamentoAnual(ltProtocolo, TENTATIVAS_PROCESSAMENTO_PROTOCOLO);
        }

        /// <summary>
        /// Executa a sincronização dos dados alterados no diário de classe. (Logs)
        /// </summary>
        public static void ExecJobAtualizaLogsDiarioClasse()
        {
            List<DCL_Protocolo> ltProtocolo = DCL_ProtocoloBO.SelecionaNaoProcessadosPorTipo
                (DCL_ProtocoloBO.eTipo.Logs,
                 ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_LOGS, new Guid()));

            ltProtocolo.ForEach(protocolo =>
            {
                // Marca o protocolo como "Em processamento".
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.EmProcessamento;
                protocolo.pro_tentativa++;
            });

            DCL_ProtocoloBO.AtualizaListaProtocolos(ltProtocolo);

            DCL_LogBO.ProcessaProtocoloLog(ltProtocolo, TENTATIVAS_PROCESSAMENTO_PROTOCOLO);
        }

        /// <summary>
        /// Executa a sincronização dos dados alterados no diário de classe. (Justificativa de faltas)
        /// </summary>
        public static void ExecJobAtualizaJustificativaDiarioClasse()
        {
            List<DCL_Protocolo> ltProtocolo = DCL_ProtocoloBO.SelecionaNaoProcessadosPorTipo
                (DCL_ProtocoloBO.eTipo.JustificativaFaltaAluno,
                 ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_JUSTIFICATIVA, new Guid()));

            ltProtocolo.ForEach(protocolo =>
            {
                // Marca o protocolo como "Em processamento".
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.EmProcessamento;
                protocolo.pro_tentativa++;
            });

            DCL_ProtocoloBO.AtualizaListaProtocolos(ltProtocolo);

            ACA_AlunoJustificativaFaltaBO.ProcessaProtocoloJustificativaFalta(ltProtocolo, TENTATIVAS_PROCESSAMENTO_PROTOCOLO);
        }

        /// <summary>
        /// Executa a sincronização dos dados alterados no diário de classe. (Fotos)
        /// </summary>
        public static void ExecJobAtualizaFotoDiarioClasse()
        {
            List<DCL_Protocolo> ltProtocolo = DCL_ProtocoloBO.SelecionaNaoProcessadosPorTipo
                (DCL_ProtocoloBO.eTipo.Foto,
                 ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_FOTO, new Guid()));

            ltProtocolo.ForEach(protocolo =>
            {
                // Marca o protocolo como "Em processamento".
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.EmProcessamento;
                protocolo.pro_tentativa++;
            });

            DCL_ProtocoloBO.AtualizaListaProtocolos(ltProtocolo);

            ACA_AlunoBO.ProcessarProtocoloFoto(ltProtocolo, TENTATIVAS_PROCESSAMENTO_PROTOCOLO);
        }

        /// <summary>
        /// Executa a sincronização dos dados alterados no diário de classe. (Compensacao de ausência)
        /// </summary>
        public static void ExecJobAtualizaCompensacaoDiarioClasse()
        {
            List<DCL_Protocolo> ltProtocolo = DCL_ProtocoloBO.SelecionaNaoProcessadosPorTipo
                (DCL_ProtocoloBO.eTipo.CompensacaoDeAula,
                 ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_MAXIMA_BUSCA_PROTOCOLO_COMPENSACAO, new Guid()));

            ltProtocolo.ForEach(protocolo =>
            {
                // Marca o protocolo como "Em processamento".
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.EmProcessamento;
                protocolo.pro_tentativa++;
            });

            DCL_ProtocoloBO.AtualizaListaProtocolos(ltProtocolo);

            CLS_CompensacaoAusenciaBO.ProcessarProtocoloCompensacao(ltProtocolo, TENTATIVAS_PROCESSAMENTO_PROTOCOLO);
        }

        /// <summary>
        /// Atualiza as situaçoes das atribuiçoes esporádicas e das TurmaDocente geradas de acordo com a vigencia.
        /// </summary>
        public static void ExecJOB_AtualizaAtribuicoesEsporadicas()
        {
            GestaoEscolarServicoDAO dao = new GestaoEscolarServicoDAO();
            dao.ExecJobAtualizaAtribuicoesEsporadicasAsync();
        }

        /// <summary>
        /// Atualiza as informações pré-calculadas da tabela CLS_IndicadorFrequencia.
        /// </summary>
        public static void ExecJOB_AtualizaIndicadorFrequencia()
        {
            GestaoEscolarServicoDAO dao = new GestaoEscolarServicoDAO();
            dao.ExecJobAtualizaIndicadorFrequenciaAsync();
        }

        /// <summary>
        /// Recálculo da frequência no fechamento baseado em aulas previstas.
        /// </summary>
        public static void ExecJobFechamentoRecalcularFrequenciaAulasPrevistas()
        {
            new GestaoEscolarServicoDAO().ExecJobFechamentoRecalcularFrequenciaAulasPrevistasAsync();
        }

        public static void ExecJobGeracaoHistoricoPedagogico()
        {
            new GestaoEscolarServicoDAO().ExecJobGeracaoHistoricoPedagogicoAsync();
        }

        public static void ExecJobMatriculasBoletimAtualizar()
        {
            new GestaoEscolarServicoDAO().ExecJobJobMatriculasBoletimAtualizarAsync();
        }

        /// <summary>
        /// Atualiza a frequência ajustada no fechamento do último bimestre, dos registros afetados.
        /// </summary>
        public static void ExecJobAtualizaFrequenciaAjustadaFinal()
        {
            new GestaoEscolarServicoDAO().ExecJobJobAtualizaFrequenciaAjustadaFinalAsync();
        }

        /// <summary>
        /// Faz o pré procesamento do relatório pendências por disciplinas e alunos
        /// </summary>
        public static void ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync(bool limpacache = true)
        {
            using (DataTable dt = new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync())
            {
                if (dt.Rows.Count > 0)
                {
                    List<sChaveCachePendenciaFechamento> ltChave = (from DataRow dr in dt.Rows
                                                                    select (sChaveCachePendenciaFechamento)GestaoEscolarUtilBO.DataRowToEntity(dr, new sChaveCachePendenciaFechamento())).ToList();

                    if (limpacache)
                    {
                        // Informações do e-mail.
                        IDictionary<string, ICFG_Configuracao> configuracao;
                        CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
                        string ips = configuracao["AppEnderecoIPRaizHandlerLimpaCache"].cfg_valor;

                        string[] listaIps = ips.Split('|');

                        foreach (string ip in listaIps)
                        {
                            string handler = ip + "/Configuracao/Conteudo/LimpaCache.ashx?tipoCache=2";
                            handler += "&esc_ids=" + string.Join(";", ltChave.Select(p => p.esc_id));
                            handler += "&uni_ids=" + string.Join(";", ltChave.Select(p => p.uni_id));
                            handler += "&cal_ids=" + string.Join(";", ltChave.Select(p => p.cal_id));
                            handler += "&tud_ids=" + string.Join(";", ltChave.Select(p => p.tud_id));

                            try
                            {
                                HttpWebRequest request = WebRequest.Create(handler) as HttpWebRequest;
                                request.GetResponseAsync();
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Faz o pré procesamento de notas e frequeências que estão na fila para o novo fechamento
        /// </summary>
        public static void ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync(bool limpacache = true)
        {
            using (DataTable dt = new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync())
            {
                if (limpacache)
                {
                    if (dt.Rows.Count > 0)
                    {
                        List<sChavesCacheFechamento> ltChave = (from DataRow dr in dt.Rows
                                                                select (sChavesCacheFechamento)GestaoEscolarUtilBO.DataRowToEntity(dr, new sChavesCacheFechamento())).ToList();

                        // Informações da configuracao.
                        IDictionary<string, ICFG_Configuracao> configuracao;
                        CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
                        string ips = configuracao["AppEnderecoIPRaizHandlerLimpaCache"].cfg_valor;

                        string[] listaIps = ips.Split('|');

                        foreach (string ip in listaIps)
                        {
                            string handler = ip + "/Configuracao/Conteudo/LimpaCache.ashx?tipoCache=1";
                            handler += "&tur_ids=" + string.Join(";", ltChave.Select(p => p.tur_id));
                            handler += "&tud_ids=" + string.Join(";", ltChave.Select(p => p.tud_id));
                            handler += "&fav_ids=" + string.Join(";", ltChave.Select(p => p.fav_id));
                            handler += "&ava_ids=" + string.Join(";", ltChave.Select(p => p.ava_id));
                            handler += "&tpc_ids=" + string.Join(";", ltChave.Select(p => p.tpc_id));

                            HttpWebRequest request = WebRequest.Create(handler) as HttpWebRequest;
                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Joga os registros da tabela pré-processada para a tabela de fechamento no dia da abertura do evento de fechamento
        /// </summary>
        public static void ExecJOB_AtualizaFechamento_AberturaEventoAsync()
        {
            new GestaoEscolarServicoDAO().ExecJOB_AtualizaFechamento_AberturaEventoAsync();
        }

        /// <summary>
        /// Executa o job que processa as pendências da escola no bimestre de acordo com a abertura do evento.
        /// </summary>
        public static void ExecJobProcessamentoPendenciasAberturaEvento()
        {
            new GestaoEscolarServicoDAO().ExecJobProcessamentoPendenciasAberturaEventoAsync();
        }

        /// <summary>
        /// Retorna o parametro academico.
        /// </summary>
        public static int GetQuantidadeMaximaFilaFechamento()
        {
            return ACA_ParametroAcademicoBO.ParametroValorInt32PorEntidade(eChaveAcademico.QUANTIDADE_THREADS_FILA_FECHAMENTO, new Guid());
        }

        /// <summary>
        /// Processa as pendencias de aulas sem plano.
        /// </summary>
        public static void ExecJOB_ProcessamentoPendenciaAulas()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoPendenciaAulas();
        }

        /// <summary>
        /// Processa a remoção das faltas com justificativa de abono.
        /// </summary>
        public static void ExecJOB_ProcessamentoAbonoFalta()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoAbonoFalta();
        }

        /// <summary>
        /// Processa a abertura/fechamento das turmas dos anos anteriores.
        /// </summary>
        public static void ExecJOB_ProcessamentoAberturaTurmaAnosAnteriores()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoAberturaTurmaAnosAnteriores();

            IDictionary<string, ICFG_Configuracao> configuracao;
            CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
            string ips = configuracao["AppEnderecoIPRaizHandlerLimpaCache"].cfg_valor;

            string[] listaIps = ips.Split('|');

            foreach (string ip in listaIps)
            {
                string handler = ip + "/Configuracao/Conteudo/LimpaCache.ashx?tipoCache=3";

                try
                {
                    HttpWebRequest request = WebRequest.Create(handler) as HttpWebRequest;
                    request.GetResponseAsync();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Processa as divergências nas rematrículas conforme o resultado do ano anterior
        /// </summary>
        public static void ExecJOB_ProcessamentoDivergenciasRematriculas(Guid sle_id)
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoDivergenciasRematriculas(sle_id);
        }

        /// <summary>
        /// Processa os dados para a sugestão das aulas previstas.
        /// </summary>
        public static void ExecJOB_ProcessamentoSugestaoAulasPrevistas()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoSugestaoAulasPrevistas(false);
        }

        /// <summary>
        /// Processa os dados para a sugestão das aulas previstas,
        /// para eventos cadastrados para toda a rede.
        /// </summary>
        public static void ExecJOB_ProcessamentoSugestaoAulasPrevistas_TodaRede()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoSugestaoAulasPrevistas(true);
        }

        /// <summary>
        /// Processa as divergências entre registros de aulas dadas e aulas previstas.
        /// </summary>
        public static void MS_JOB_ProcessamentoDivergenciasAulasPrevistas()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoDivergenciasAulasPrevistas();
        }

        /// <summary>
        /// Processa o alerta de preenchimento de frequência.
        /// </summary>
        public static void ExecJOB_AlertaPreenchimentoFrequencias()
        {
            CFG_Alerta alerta = CFG_AlertaBO.GetEntity(new CFG_Alerta { cfa_id = (byte)CFG_AlertaBO.eChaveAlertas.AlertaPreenchimentoFrequencia });
            if (alerta.cfa_periodoAnalise > 0 && !string.IsNullOrEmpty(alerta.cfa_assunto))
            {
                // Busca os usuários para envio da notificação
                DataTable dt = new GestaoEscolarServicoDAO().ExecJOB_AlertaPreenchimentoFrequencias();
                List<sAlertaPreenchimentoFrequencia> lstUsuarios = (from DataRow dr in dt.Rows
                                                                    select (sAlertaPreenchimentoFrequencia)GestaoEscolarUtilBO.DataRowToEntity(dr, new sAlertaPreenchimentoFrequencia())).ToList();
                if (lstUsuarios.Any())
                {
                    DateTime dataAtual = DateTime.UtcNow;
                    NotificacaoDTO notificacao = new NotificacaoDTO();
                    notificacao.SenderName = "SGP";
                    notificacao.Recipient = new DestinatarioNotificacao();
                    notificacao.Recipient.UserRecipient = new List<string>();
                    notificacao.MessageType = 3;
                    notificacao.DateStartNotification = string.Format("{0:yyyy-MM-ddTHH:mm:ss.0000000-00:00}", dataAtual);
                    notificacao.DateEndNotification = alerta.cfa_periodoValidade > 0 ? string.Format("{0:yyyy-MM-ddTHH:mm:ss.0000000-00:00}", dataAtual.AddHours(alerta.cfa_periodoValidade)) : null;
                    notificacao.Title = alerta.cfa_nome;
                    notificacao.Message = alerta.cfa_assunto;
                    lstUsuarios.ForEach(ue => notificacao.Recipient.UserRecipient.Add(ue.usu_id.ToString()));
                    if (EnviarNotificacao(notificacao))
                    {
                        List<LOG_AlertaPreenchimentoFrequencia> lstLog = new List<LOG_AlertaPreenchimentoFrequencia>();
                        notificacao.Recipient.UserRecipient.ForEach(ur => lstLog.Add(new LOG_AlertaPreenchimentoFrequencia { usu_id = new Guid(ur), lpf_dataEnvio = DateTime.Now }));
                        LOG_AlertaPreenchimentoFrequenciaBO.SalvarEmLote(lstLog);
                    }
                }
            }
        }

        /// <summary>
        /// Processa o alerta de aviso de início de fechamento.
        /// </summary>
        public static void ExecJOB_AlertaInicioFechamento()
        {
            CFG_Alerta alerta = CFG_AlertaBO.GetEntity(new CFG_Alerta { cfa_id = (byte)CFG_AlertaBO.eChaveAlertas.AlertaInicioFechamento });
            if (alerta.cfa_periodoAnalise > 0 && !string.IsNullOrEmpty(alerta.cfa_assunto))
            {
                // Busca os usuários para envio da notificação
                DataTable dt = new GestaoEscolarServicoDAO().ExecJOB_AlertaInicioFechamento(alerta.cfa_periodoAnalise);
                List<sAlertaInicioFechamento> lstUsuarios = (from DataRow dr in dt.Rows
                                                            select (sAlertaInicioFechamento)GestaoEscolarUtilBO.DataRowToEntity(dr, new sAlertaInicioFechamento())).ToList();
                List<long> lstEventos = lstUsuarios.Select(p => p.evt_id).Distinct().ToList();
                DateTime dataAtual = DateTime.UtcNow;
                lstEventos.ForEach(e =>
                    {
                        NotificacaoDTO notificacao = new NotificacaoDTO();
                        notificacao.SenderName = "SGP";
                        notificacao.Recipient = new DestinatarioNotificacao();
                        notificacao.Recipient.UserRecipient = new List<string>();
                        notificacao.MessageType = 3;
                        notificacao.DateStartNotification = string.Format("{0:yyyy-MM-ddTHH:mm:ss.0000000-00:00}", dataAtual);
                        notificacao.DateEndNotification = alerta.cfa_periodoValidade > 0 ? string.Format("{0:yyyy-MM-ddTHH:mm:ss.0000000-00:00}", dataAtual.AddHours(alerta.cfa_periodoValidade)) : null;
                        notificacao.Title = alerta.cfa_nome;
                        List<sAlertaInicioFechamento> lstUsuariosEvento = lstUsuarios.FindAll(u => u.evt_id == e);
                        notificacao.Message = alerta.cfa_assunto
                                                .Replace("[Dias]", lstUsuariosEvento.First().dias.ToString())
                                                .Replace("[NomeEvento]", lstUsuariosEvento.First().evt_nome.ToString());
                        lstUsuariosEvento.ForEach(ue => notificacao.Recipient.UserRecipient.Add(ue.usu_id.ToString()));
                        if (EnviarNotificacao(notificacao))
                        {
                            List<LOG_AlertaInicioFechamento> lstLog = new List<LOG_AlertaInicioFechamento>();
                            notificacao.Recipient.UserRecipient.ForEach(ur => lstLog.Add(new LOG_AlertaInicioFechamento { usu_id = new Guid(ur), evt_id = e, lif_dataEnvio = DateTime.Now }));
                            LOG_AlertaInicioFechamentoBO.SalvarEmLote(lstLog);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Processa o alerta de aviso de final de fechamento.
        /// </summary>
        public static void ExecJOB_AlertaFimFechamento()
        {
            CFG_Alerta alerta = CFG_AlertaBO.GetEntity(new CFG_Alerta { cfa_id = (byte)CFG_AlertaBO.eChaveAlertas.AlertaFimFechamento });
            if (alerta.cfa_periodoAnalise > 0 && !string.IsNullOrEmpty(alerta.cfa_assunto))
            {
                // Busca os usuários para envio da notificação
                DataTable dt = new GestaoEscolarServicoDAO().ExecJOB_AlertaFimFechamento(alerta.cfa_periodoAnalise);
                List<sAlertaFimFechamento> lstUsuarios = (from DataRow dr in dt.Rows
                                                            select (sAlertaFimFechamento)GestaoEscolarUtilBO.DataRowToEntity(dr, new sAlertaFimFechamento())).ToList();
                List<long> lstEventos = lstUsuarios.Select(p => p.evt_id).Distinct().ToList();
                DateTime dataAtual = DateTime.UtcNow;
                lstEventos.ForEach(e =>
                    {
                        NotificacaoDTO notificacao = new NotificacaoDTO();
                        notificacao.SenderName = "SGP";
                        notificacao.Recipient = new DestinatarioNotificacao();
                        notificacao.Recipient.UserRecipient = new List<string>();
                        notificacao.MessageType = 3;
                        notificacao.DateStartNotification = string.Format("{0:yyyy-MM-ddTHH:mm:ss.0000000-00:00}", dataAtual);
                        notificacao.DateEndNotification = alerta.cfa_periodoValidade > 0 ? string.Format("{0:yyyy-MM-ddTHH:mm:ss.0000000-00:00}", dataAtual.AddHours(alerta.cfa_periodoValidade)) : null;
                        notificacao.Title = alerta.cfa_nome;
                        List<sAlertaFimFechamento> lstUsuariosEvento = lstUsuarios.FindAll(u => u.evt_id == e);
                        notificacao.Message = alerta.cfa_assunto
                                                .Replace("[Dias]", lstUsuariosEvento.First().dias.ToString())
                                                .Replace("[NomeEvento]", lstUsuariosEvento.First().evt_nome.ToString());
                        lstUsuariosEvento.ForEach(ue => notificacao.Recipient.UserRecipient.Add(ue.usu_id.ToString()));
                        if (EnviarNotificacao(notificacao))
                        {
                            List<LOG_AlertaFimFechamento> lstLog = new List<LOG_AlertaFimFechamento>();
                            notificacao.Recipient.UserRecipient.ForEach(ur => lstLog.Add(new LOG_AlertaFimFechamento { usu_id = new Guid(ur), evt_id = e, lff_dataEnvio = DateTime.Now }));
                            LOG_AlertaFimFechamentoBO.SalvarEmLote(lstLog);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Processa o alerta de alunos com baixa frequência.
        /// </summary>
        public static void ExecJOB_AlertaAlunosBaixaFrequencia()
        {
            new GestaoEscolarServicoDAO().ExecJOB_AlertaAlunosBaixaFrequencia();
        }

        /// <summary>
        /// Processa o alerta de alunos com faltas consecutivas.
        /// </summary>
        public static void ExecJOB_AlertaAlunosFaltasConsecutivas()
        {
            new GestaoEscolarServicoDAO().ExecJOB_AlertaAlunosFaltasConsecutivas();
        }

        /// <summary>
        /// Processa o preenchimento de frequência, conta as aulas sem a flag efetivado.
        /// Utiliza a fila do fechamento.
        /// </summary>
        public static void ExecJOB_ProcessamentoPreenchimentoFrequencia()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoPreenchimentoFrequencia();
        }

        /// <summary>
        /// Processa os alunos com baixa frequência e com faltas consecutivas.
        /// Utiliza a fila do fechamento.
        /// </summary>
        public static void ExecJOB_ProcessamentoAlunosFrequencia()
        {
            new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoAlunosFrequencia();
        }

        /// <summary>
        /// Envia o alerta por API para o sistema Notificações.
        /// </summary>
        /// <param name="notificacao"></param>
        private static bool EnviarNotificacao(NotificacaoDTO notificacao)
        {
            SYS_RecursoAPI recurso = new SYS_RecursoAPI { rap_id = (int)eRecursoAPI.Notificacoes };
            SYS_RecursoAPIBO.GetEntity(recurso);

            if (recurso.IsNew || string.IsNullOrEmpty(recurso.rap_url) || recurso.rap_situacao == (byte)RecursoAPISituacao.Excluido)
                return true;

            HttpClient client = new HttpClient();
            List<SYS_UsuarioAPI> lstUsuario = SYS_RecursoUsuarioAPIBO.SelecionaUsuarioPorRecurso(eRecursoAPI.Notificacoes);
            if (lstUsuario.Any())
            {
                var auth = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", lstUsuario.First().uap_usuario, lstUsuario.First().uap_senha));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(auth));
            }

            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(notificacao), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(recurso.rap_url, contentPost).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
