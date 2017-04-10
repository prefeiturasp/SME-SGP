
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
        public static void ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync()
        {
            using (DataTable dt = new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync())
            {
                if (dt.Rows.Count > 0)
                {
                    List<sChaveCachePendenciaFechamento> ltChave = (from DataRow dr in dt.Rows
                                                                    select (sChaveCachePendenciaFechamento)GestaoEscolarUtilBO.DataRowToEntity(dr, new sChaveCachePendenciaFechamento())).ToList();

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

        /// <summary>
        /// Faz o pré procesamento de notas e frequeências que estão na fila para o novo fechamento
        /// </summary>
        public static void ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync()
        {
            using (DataTable dt = new GestaoEscolarServicoDAO().ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync())
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
    }
}
