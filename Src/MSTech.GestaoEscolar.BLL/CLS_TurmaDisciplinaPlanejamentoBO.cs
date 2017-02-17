/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.Business.Common;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.DAL;
using System.ComponentModel;
using System;
using System.Data;
using System.Collections.Generic;
using MSTech.Data.Common;
using MSTech.Validation.Exceptions;
using MSTech.CoreSSO.BLL;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    #region Enumerador

    /// <summary>
    /// Enumerador com os valores dos formatos de calculo de média dos alunos em atividades avaliativas.
    /// </summary>
    public enum CLS_TurmaDisciplinaPlanejamentoFormatoCalculoMedia : byte
    {
        MediaAritmetica = 1
        ,
        Soma = 2
    }

    #endregion

    #region Estrutura

    public struct CLS_TurmaDisciplinaPlanejamento_Cadastro
    {
        public CLS_TurmaDisciplinaPlanejamento entity;
        public SYS_Arquivo entArquivoPlanejamento;
        public SYS_Arquivo entArquivoDiagnostico;
    }

    #endregion

    /// <summary>
    /// CLS_TurmaDisciplinaPlanejamento Business Object 
    /// </summary>
    public class CLS_TurmaDisciplinaPlanejamentoBO : BusinessBase<CLS_TurmaDisciplinaPlanejamentoDAO, CLS_TurmaDisciplinaPlanejamento>
    {
        #region Sincronização com diário de classe

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
        /// Processa os protocolos informados.
        /// </summary>
        /// <param name="ltProtocolo">Lista de protocolos em processamento.</param>
        /// <param name="tentativasProtocolo">Quantidade máxima de tentativas para processar protocolos.</param>
        /// <returns></returns>
        public static bool ProcessaProtocoloPlanejamentoAnual(List<DCL_Protocolo> ltProtocolo, int tentativasProtocolo)
        {
            // DataTable de protocolos
            DataTable dtProtocolo = DCL_Protocolo.TipoTabela_Protocolo();

            foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa > tentativasProtocolo))
            {
                protocolo.pro_statusObservacao = String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                                , tentativasProtocolo, protocolo.pro_statusObservacao);
                protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErrosValidacao;
                protocolo.tur_id = -1;
                protocolo.tud_id = -1;
                protocolo.tau_id = -1;
                protocolo.pro_qtdeAlunos = -1;
                dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));
            }

            foreach (DCL_Protocolo protocolo in ltProtocolo.Where(pro => pro.pro_tentativa <= tentativasProtocolo))
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

                        ///*
                        // * na versão 44 o planejamento mudou e não mais podera mais processar
                        // * protocolos de planejamento antigos
                        // * */
                        //int minorVersion = int.Parse(protocolo.pro_versaoAplicativo.Split('.')[1]);
                        //if (minorVersion < 44)
                        //    throw new ValidationException("É necessário atualizar a versão do sistema.");

                        #region Varável

                        List<CLS_TurmaDisciplinaPlanejamento> listaPlanejamento = new List<CLS_TurmaDisciplinaPlanejamento>();

                        #endregion

                        #region Informações do planejamento anual

                        // Objeto JSON de entrada.
                        JObject planejamento = JObject.Parse(protocolo.pro_pacote);

                        long tud_id = (long)planejamento.SelectToken("tud_id");
                        protocolo.tud_id = tud_id;

                        // apenas protocolos de turmas ativas e do ano letivo corrente podem ser processados
                        if (!DCL_ProtocoloBO.PodeProcessarProtocolo(0, tud_id))
                            throw new ValidationException("O protocolo pertence a uma turma que não esta ativa ou de um ano letivo diferente do corrente, não pode ser processado!");

                        ACA_CurriculoPeriodo entCrp = ACA_CurriculoPeriodoBO.SelecionaPorTurmaDisciplina(tud_id, bancoSincronizacao).FirstOrDefault();

                        #endregion

                        // Todas as matrículas de aluno na disciplina.
                        List<MTR_MatriculaTurmaDisciplina> listaMatriculas = MTR_MatriculaTurmaDisciplinaBO.
                            SelecionaMatriculasPorTurmaDisciplina(tud_id.ToString(), bancoSincronizacao);

                        List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> listaAlunoTurmaDisciplinaOrientacao = new List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular>();
                        List<CLS_PlanejamentoOrientacaoCurricular> listaPlanejamentoOrientacao = new List<CLS_PlanejamentoOrientacaoCurricular>();
                        List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> listaPlanejamentoDiagnostico = new List<CLS_PlanejamentoOrientacaoCurricularDiagnostico>();

                        if (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, Guid.Empty))
                        {

                            #region Informações sobre alcance dos alunos nas orientações curriculares

                            JArray alunoLista = ((JArray)planejamento.SelectToken("AlunoTurmaDisciplinaOrientacaoCurricular") ?? new JArray());

                            List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular> listaAlunoTurmaDisciplinaOrientacaoCadastrados =
                                CLS_AlunoTurmaDisciplinaOrientacaoCurricularBO.SelecionaAlunosPorTurmaDisciplina(tud_id, bancoSincronizacao);

                            listaAlunoTurmaDisciplinaOrientacao =
                                alunoLista.Count > 0 ?
                                (from JObject aluno in alunoLista
                                 let alu_id = (long)aluno.SelectToken("alu_id")
                                 let ocr_id = (long)aluno.SelectToken("ocr_id")
                                 let tpc_id = (int)aluno.SelectToken("tpc_id")
                                 let aha_alcancada = (bool)aluno.SelectToken("aha_alcancada")
                                 let aha_efetivada = (bool)aluno.SelectToken("aha_efetivada")
                                 let dataAlteracao = Convert.ToDateTime(aluno.SelectToken("DataAlteracao") ?? new DateTime())
                                 let entMtd = listaMatriculas.Find(p => p.alu_id == alu_id)
                                 where entMtd != null && !entMtd.IsNew
                                 let entAlunoTurmaOrientacao = listaAlunoTurmaDisciplinaOrientacaoCadastrados.Any(p => p.alu_id == entMtd.alu_id &&
                                                                                                                        p.mtu_id == entMtd.mtu_id &&
                                                                                                                        p.mtd_id == entMtd.mtd_id &&
                                                                                                                        p.ocr_id == ocr_id &&
                                                                                                                        p.tpc_id == tpc_id) ?
                                 listaAlunoTurmaDisciplinaOrientacaoCadastrados.Find(p => p.alu_id == entMtd.alu_id &&
                                                                                                                        p.mtu_id == entMtd.mtu_id &&
                                                                                                                        p.mtd_id == entMtd.mtd_id &&
                                                                                                                        p.ocr_id == ocr_id &&
                                                                                                                        p.tpc_id == tpc_id) :
                                                                                                                        new CLS_AlunoTurmaDisciplinaOrientacaoCurricular()
                                 let aha_id = entAlunoTurmaOrientacao != null && entAlunoTurmaOrientacao.aha_id > 0 ?
                                              entAlunoTurmaOrientacao.aha_id :
                                              -1
                                 select new CLS_AlunoTurmaDisciplinaOrientacaoCurricular
                                 {
                                     tud_id = tud_id
                                     ,
                                     alu_id = alu_id
                                     ,
                                     mtu_id = entMtd.mtu_id
                                     ,
                                     mtd_id = entMtd.mtd_id
                                     ,
                                     ocr_id = ocr_id
                                     ,
                                     tpc_id = tpc_id
                                     ,
                                     aha_id = aha_id
                                     ,
                                     aha_alcancada = entAlunoTurmaOrientacao.aha_dataAlteracao > dataAlteracao ? entAlunoTurmaOrientacao.aha_alcancada : aha_alcancada
                                     ,
                                     aha_efetivada = entAlunoTurmaOrientacao.aha_dataAlteracao > dataAlteracao ? entAlunoTurmaOrientacao.aha_efetivada : aha_efetivada
                                     ,
                                     aha_situacao = 1
                                     ,
                                     aha_dataAlteracao = dataAlteracao
                                     ,
                                     IsNew = aha_id <= 0
                                 }).ToList() :
                                 new List<CLS_AlunoTurmaDisciplinaOrientacaoCurricular>();

                            #endregion

                            #region Informações sobre planejamento das orientações curriculares nos bimestre

                            JArray planejamentoOrientacaoLista = ((JArray)planejamento.SelectToken("PlanejamentoOrientacaoCurricular") ?? new JArray());

                            listaPlanejamentoOrientacao =
                                planejamentoOrientacaoLista.Count > 0 ?
                                (from JObject planejamentoOrientacao in planejamentoOrientacaoLista
                                 select new CLS_PlanejamentoOrientacaoCurricular
                                 {
                                     tud_id = tud_id
                                     ,
                                     ocr_id = (long)planejamentoOrientacao.SelectToken("ocr_id")
                                     ,
                                     tpc_id = (int)planejamentoOrientacao.SelectToken("tpc_id")
                                     ,
                                     poc_planejado = (bool)planejamentoOrientacao.SelectToken("poc_planejado")
                                     ,
                                     poc_trabalhado = (bool)planejamentoOrientacao.SelectToken("poc_trabalhado")
                                     ,
                                     poc_alcancado = (bool)planejamentoOrientacao.SelectToken("poc_alcancado")
                                     ,
                                     tdt_posicao = (byte)(planejamentoOrientacao.SelectToken("tdt_posicao") ?? 1)
                                 }).ToList() :
                                 new List<CLS_PlanejamentoOrientacaoCurricular>();

                            #endregion

                            #region Informações sobre o planejamento das orientações curriculares no ano anterior

                            JArray planejamentoOrientacaoDiagLista = ((JArray)planejamento.SelectToken("PlanejamentoOrientacaoCurricularDiagnostico") ?? new JArray());

                            listaPlanejamentoDiagnostico =
                                planejamentoOrientacaoDiagLista.Count > 0 ?
                                (from JObject diagnostico in planejamentoOrientacaoDiagLista
                                 select new CLS_PlanejamentoOrientacaoCurricularDiagnostico
                                 {
                                     tud_id = tud_id
                                     ,
                                     ocr_id = (long)diagnostico.SelectToken("ocr_id")
                                     ,
                                     pod_alcancado = (bool)diagnostico.SelectToken("pod_alcancado")
                                     ,
                                     tdt_posicao = (byte)(diagnostico.SelectToken("tdt_posicao") ?? 1)
                                 }).ToList() :
                                 new List<CLS_PlanejamentoOrientacaoCurricularDiagnostico>();

                            #endregion

                        }

                        #region Informações sobre os planejamentos anuais e/ou bimestrais

                        JArray TudPlanejamentoLista = ((JArray)planejamento.SelectToken("TurmaDisciplinaPlanejamento") ?? new JArray());

                        List<CLS_TurmaDisciplinaPlanejamento> ltPlanejamentoBanco = SelecionaListaPorTurmaDisciplina(tud_id, bancoSincronizacao);

                        foreach (JObject turmaPlanejamento in TudPlanejamentoLista)
                        {
                            DateTime tdp_dataAlteracao = Convert.ToDateTime(turmaPlanejamento.SelectToken("tdp_dataAlteracao").ToString());

                            if (tdp_dataAlteracao > DateTime.Now.AddMinutes(10))
                                throw new ValidationException("A data de alteração do planejamento é maior que a data atual.");

                            int tpc_id = (int)(turmaPlanejamento.SelectToken("tpc_id") ?? -1);
                            int tdp_id = (int)turmaPlanejamento.SelectToken("tdp_id");
                            byte tdt_posicao = (byte)(turmaPlanejamento.SelectToken("tdt_posicao") ?? 1);

                            CLS_TurmaDisciplinaPlanejamento planejamentoBanco = ltPlanejamentoBanco.Any(p => p.tud_id == tud_id && p.tdp_id == tdp_id && p.tdt_posicao == tdt_posicao) ?
                               ltPlanejamentoBanco.Find(p => p.tud_id == tud_id && p.tdp_id == tdp_id && p.tdt_posicao == tdt_posicao) :
                               new CLS_TurmaDisciplinaPlanejamento();

                            planejamentoBanco.IsNew = planejamentoBanco.tdp_id <= 0;

                            string tdp_planejamento = (turmaPlanejamento.SelectToken("tdp_planejamento") ?? string.Empty).ToString();
                            string tdp_diagnostico = (turmaPlanejamento.SelectToken("tdp_diagnostico") ?? string.Empty).ToString();
                            string tdp_avaliacaoTrabalho = (turmaPlanejamento.SelectToken("tdp_avaliacaoTrabalho") ?? string.Empty).ToString();
                            string tdp_recursos = (turmaPlanejamento.SelectToken("tdp_recursos") ?? string.Empty).ToString();
                            string tdp_intervencoesPedagogicas = (turmaPlanejamento.SelectToken("tdp_intervencoesPedagogicas") ?? string.Empty).ToString();
                            string tdp_registroIntervencoes = (turmaPlanejamento.SelectToken("tdp_registroIntervencoes") ?? string.Empty).ToString();

                            CLS_TurmaDisciplinaPlanejamento entPlanejamento = new CLS_TurmaDisciplinaPlanejamento();

                            if (!planejamentoBanco.IsNew)
                            {
                                entPlanejamento = planejamentoBanco;
                            }
                            else
                            {
                                entPlanejamento = new CLS_TurmaDisciplinaPlanejamento
                                {
                                    tud_id = tud_id
                                    ,
                                    tdp_id = -1
                                    ,
                                    tpc_id = tpc_id
                                    ,
                                    tdt_posicao = (byte)(turmaPlanejamento.SelectToken("tdt_posicao") ?? 1)
                                    ,
                                    tdp_situacao = 1
                                    ,
                                    pro_id = protocolo.pro_id
                                };
                            }

                            entPlanejamento.tdp_planejamento = RetornaValorTextoSincronizacao(tdp_planejamento, tdp_dataAlteracao, planejamentoBanco.tdp_planejamento, planejamentoBanco.tdp_dataAlteracao);

                            entPlanejamento.tdp_diagnostico = RetornaValorTextoSincronizacao(tdp_diagnostico, tdp_dataAlteracao, planejamentoBanco.tdp_diagnostico, planejamentoBanco.tdp_dataAlteracao);

                            entPlanejamento.tdp_avaliacaoTrabalho = RetornaValorTextoSincronizacao(tdp_avaliacaoTrabalho, tdp_dataAlteracao, planejamentoBanco.tdp_avaliacaoTrabalho, planejamentoBanco.tdp_dataAlteracao);

                            entPlanejamento.tdp_recursos = RetornaValorTextoSincronizacao(tdp_recursos, tdp_dataAlteracao, planejamentoBanco.tdp_recursos, planejamentoBanco.tdp_dataAlteracao);

                            entPlanejamento.tdp_intervencoesPedagogicas = RetornaValorTextoSincronizacao(tdp_intervencoesPedagogicas, tdp_dataAlteracao, planejamentoBanco.tdp_intervencoesPedagogicas, planejamentoBanco.tdp_dataAlteracao);

                            entPlanejamento.tdp_registroIntervencoes = RetornaValorTextoSincronizacao(tdp_registroIntervencoes, tdp_dataAlteracao, planejamentoBanco.tdp_registroIntervencoes, planejamentoBanco.tdp_dataAlteracao);

                            entPlanejamento.cur_id = entCrp.cur_id;
                            entPlanejamento.crr_id = entCrp.crr_id;
                            entPlanejamento.crp_id = entCrp.crp_id;

                            entPlanejamento.tdp_dataAlteracao = tdp_dataAlteracao;

                            listaPlanejamento.Add(entPlanejamento);
                        }

                        #endregion

                        if (!ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PLANEJAMENTO_ANUAL_CICLO, Guid.Empty))
                        {

                            #region Salvar alcance dos alunos nas orientações curriculares

                            processou = CLS_AlunoTurmaDisciplinaOrientacaoCurricularBO.Salvar(listaAlunoTurmaDisciplinaOrientacao, bancoSincronizacao, true);

                            #endregion
                        }

                        #region Salvar os planejamentos anuais e bimestrais

                        processou |= CLS_PlanejamentoOrientacaoCurricularBO.SalvaPlanejamentoTurmaDisciplina(listaPlanejamento, listaPlanejamentoOrientacao, listaPlanejamentoDiagnostico, bancoSincronizacao, true);

                        #endregion
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

            DCL_ProtocoloBO.AtualizaProtocolos(dtProtocolo);

            return true;
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Retorna os periodos da turma e os planejamentos cadastrados
        /// </summary>                
        /// <param name="tud_id">Id da disciplina da turma</param> 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaDisciplina
        (
            long tud_id
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.SelectBy_tud_id(tud_id);
        }


        public static List<CLS_TurmaDisciplinaPlanejamento> SelecionaPorTurmaCalendario
        (
           long tur_id
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.SelecionaPorTurmaCalendario(tur_id);
        }

        /// <summary>
        /// Seleciona os planejamentos anuais e bimestrais de uma disciplina.
        /// </summary>
        /// <param name="tud_id"></param>
        /// <param name="banco"></param>
        /// <returns></returns>
        public static List<CLS_TurmaDisciplinaPlanejamento> SelecionaListaPorTurmaDisciplina
        (
            long tud_id,
            TalkDBTransaction banco = null
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = banco == null ?
                new CLS_TurmaDisciplinaPlanejamentoDAO() :
                new CLS_TurmaDisciplinaPlanejamentoDAO { _Banco = banco };

            return dao.SelecionaPorTurmaDisciplina(tud_id).Rows.Cast<DataRow>().Select(p => dao.DataRowToEntity(p, new CLS_TurmaDisciplinaPlanejamento())).ToList();
        }

        /// <summary>
        /// Busca o planejamento da turma .
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static List<CLS_TurmaDisciplinaPlanejamento> BuscaPlanejamentoTurmaDisciplina
        (
            long tur_id
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.BuscaPlanejamentoTurmaDisciplina(tur_id);
        }

        /// <summary>
        /// Busca o planejamento da turma .
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static DataTable BuscaPlanejamentoTurmaDisciplinaDT
        (
            string esc_id, Int64 tur_id, DateTime syncDate
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.BuscaPlanejamentoTurmaDisciplinaDT(esc_id, tur_id, syncDate);
        }

        /// <summary>
        /// Busca o planejamento da turma .
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static DataTable BuscaPlanejamentoOrientacaoCurricularDT
        (
            string esc_id, Int64 tur_id
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.BuscaPlanejamentoOrientacaoCurricularDT(esc_id, tur_id);
        }

        /// <summary>
        /// Retorna os periodos da turma e os planejamentos cadastrados
        /// </summary>                
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tud_id">ID da disciplina da turma</param> 
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        public static DataTable SelecionaPorDisciplinaCalendarioPermissaoDocente
        (
            int cal_id
            , long tud_id
            , byte tdt_posicao
        )
        {
            return new CLS_TurmaDisciplinaPlanejamentoDAO().SelecionaPorDisciplinaCalendarioPermissaoDocente(cal_id, tud_id, tdt_posicao);
        }

        /// <summary>
        /// Retorna os periodos da turma e os planejamentos cadastrados
        /// </summary>                
        /// <param name="cal_id">ID do calendário escolar</param>
        /// <param name="tud_id">ID da disciplina da turma</param> 
        /// <param name="tdt_posicao">Posição do docente responsável</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaDisciplinaCalendario
        (
            int cal_id
            , long tud_id
            , byte tdt_posicao
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.SelectBy_tud_id_cal_id(cal_id, tud_id, tdt_posicao);
        }

        /// <summary>
        /// Retorna o planejamento da disciplina da turma que tenha
        /// o tipo de período calendário nulo.
        /// </summary>
        /// <param name="tud_id">Id da disciplina da turma.</param>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DataTable SelecionaPorTurmaDisciplinaPeriodoCalendarioNulo
        (
            long tud_id
            , byte tdt_posicao
        )
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.SelectBy_tud_id_tpc_id_null(tud_id, tdt_posicao);
        }

        /// <summary>
        /// Retorna o planejamento da disciplina da turma que tenha
        /// o tipo de período calendário nulo.
        /// </summary>
        public static DataTable SelecionaPorDisciplinaPermissaoDocente
        (
            long tud_id
            , byte tdt_posicao
        )
        {
            return new CLS_TurmaDisciplinaPlanejamentoDAO().SelecionaPorDisciplinaPermissaoDocente(tud_id, tdt_posicao);
        }

        /// <summary>
        /// Seleciona as turmas em que o docente leciona no mesmo curso, curriculo,
        /// periodo, disciplina e posição da atribuição que está sendo salva.
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cur_id">ID do curso</param>
        /// <param name="crr_id">ID do curriculo</param>
        /// <param name="crp_id">ID do periodo</param>
        /// <param name="tud_id">ID da turmadisciplina</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <returns></returns>
        public static DataTable SelecionaOutrasTurmasDocente(Int64 tur_id, int cal_id, int cur_id, int crr_id, int crp_id, long tud_id, byte tdt_posicao)
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO();
            return dao.SelecionaOutrasTurmasDocente(tur_id, cal_id, cur_id, crr_id, crp_id, tud_id, tdt_posicao);
        }

        #endregion

        #region Validação

        /// <summary>
        /// Verifica se o planejamento já foi lançado anteriormente
        /// </summary>                        
        /// <param name="tud_id">ID da disciplina da turma</param>
        /// <param name="tpc_id">ID do período do calendário</param>
        /// <param name="tdt_posicao">Posição do docente</param>
        /// <param name="banco">Transacao</param>
        /// <returns></returns>
        public static int VerificaPlanejamentoExistente(long tud_id, int tpc_id, byte tdt_posicao, TalkDBTransaction banco)
        {
            CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO {_Banco = banco};
            return dao.VerificaPlanejamentoExistente(tud_id, tpc_id, tdt_posicao);
        }

        #endregion

        #region Saves

        /// <summary>
        /// Salva os dados do planejamento da disciplina
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaDisciplinaPlanejamento</param>        
        public new static bool Save(CLS_TurmaDisciplinaPlanejamento entity)
        {
            TalkDBTransaction banco = new CLS_TurmaDisciplinaPlanejamentoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                Save(entity, banco);

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        /// <summary>
        /// Salva os dados do planejamento da disciplina
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaDisciplinaPlanejamento</param>    
        /// <param name="banco">Transação</param>        
        public new static bool Save(CLS_TurmaDisciplinaPlanejamento entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                // Se for uma inclusão de planejamento.
                if (entity.IsNew)
                {
                    // Verifica se já existe um planejamento cadastrado para a mesma disciplina e período do calendário.                  
                    int tdp_id = VerificaPlanejamentoExistente(entity.tud_id, entity.tpc_id, entity.tdt_posicao, banco);                   
 
                    // Se existir apenas atualiza o registro já existente.
                    if (tdp_id > 0)
                    {
                        entity.tdp_id = tdp_id;
                        entity.IsNew = false;
                    }
                }

                CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO { _Banco = banco };
                return dao.Salvar(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Salva os dados do planejamento da disciplina. Considera a data de alteração do tablet.
        /// </summary>
        /// <param name="entity">Entidade CLS_TurmaDisciplinaPlanejamento</param>    
        /// <param name="banco">Transação</param>        
        public static bool SaveSincronizacaoDiarioClasse(CLS_TurmaDisciplinaPlanejamento entity, TalkDBTransaction banco)
        {
            if (entity.Validate())
            {
                // Se for uma inclusão de planejamento.
                if (entity.IsNew)
                {
                    // Verifica se já existe um planejamento cadastrado para a mesma disciplina e período do calendário.                  
                    int tdp_id = VerificaPlanejamentoExistente(entity.tud_id, entity.tpc_id, entity.tdt_posicao, banco);

                    // Se existir apenas atualiza o registro já existente.
                    if (tdp_id > 0)
                    {
                        entity.tdp_id = tdp_id;
                        entity.IsNew = false;
                    }
                }

                CLS_TurmaDisciplinaPlanejamentoDAO dao = new CLS_TurmaDisciplinaPlanejamentoDAO { _Banco = banco };
                return dao.SalvarSincronizacaoDiarioClasse(entity);
            }

            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Salva a lista de planejamento
        /// </summary>
        /// <param name="lstPlanejamento">Lista com o planejamento da turma e dos bimestres</param>
        /// <returns></returns>
        public static bool SalvaPlanejamentoTurmaDisciplina(List<CLS_TurmaDisciplinaPlanejamento> lstPlanejamento)
        {
            TalkDBTransaction banco = new CLS_TurmaDisciplinaPlanejamentoDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                foreach (CLS_TurmaDisciplinaPlanejamento planejamento in lstPlanejamento)
                    Save(planejamento, banco);

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                banco.Close();
            }
        }

        #endregion
    }
}