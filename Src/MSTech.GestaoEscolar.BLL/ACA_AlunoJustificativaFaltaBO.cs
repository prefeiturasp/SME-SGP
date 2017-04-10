using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MSTech.GestaoEscolar.BLL
{
    public class ACA_AlunoJustificativaFaltaBO : BusinessBase<ACA_AlunoJustificativaFaltaDAO, ACA_AlunoJustificativaFalta>
    {
        #region Enumerador

        public enum eSituacao : byte
        {
            Ativo = 1
            , Excluido = 3
        }

        /// <summary>
        /// Utilizado nos cadastros para indicar se o aluno
        /// possui justificativa de falta, caso possui se abona
        /// ou não a falta
        /// </summary>
        public enum eJustificativaFalta : byte
        {
            NaoPossui = 0
            ,

            PossuiAbona
                , PossuiNaoAbona
        }

        #endregion Enumerador

        /// <summary>
        /// Retorna um datatable contendo todos as justificativas
        /// de falta do aluno ativas, filtrado por alu_id
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <param name="paginado">Indica se será paginado</param>
        /// <param name="currentPage">Página atual</param>
        /// <param name="pageSize">Quantidade de registros por página</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<ACA_AlunoJustificativaFalta> SelecionaPorAluno
            (
                Int64 alu_id
                , bool paginado
                , int currentPage
                , int pageSize
            )
        {
            ACA_AlunoJustificativaFaltaDAO dao = new ACA_AlunoJustificativaFaltaDAO();
            return dao.SelectBy_alu_id(alu_id, paginado, currentPage, pageSize, out totalRecords);
        }

        /// <summary>
        /// Retorna um datatable contendo todos as justificativas
        /// de falta do aluno ativas, filtrado por alu_id
        /// </summary>
        /// <param name="alu_id">Id do aluno</param>
        /// <returns></returns>
        public static List<ACA_AlunoJustificativaFalta> SelecionaPorAluno
            (
                Int64 alu_id
            )
        {
            ACA_AlunoJustificativaFaltaDAO dao = new ACA_AlunoJustificativaFaltaDAO();
            return dao.SelectBy_alu_id(alu_id, false, 0, 0, out totalRecords);
        }

        /// <summary>
        /// Seleciona as justificativas de falta dos alunos por mes e ano.
        /// </summary>
        /// <param name="mes">Mes de referência</param>
        /// <param name="ano">Ano de referência</param>
        /// <returns></returns>
        public static DataTable SelecionaPorMesEAno(int mes, int ano)
        {
            ACA_AlunoJustificativaFaltaDAO dao = new ACA_AlunoJustificativaFaltaDAO();
            return dao.SelecionaPorMesEAno(mes, ano);
        }

        /// <summary>
        ///  Salva (inclusão ou alteração) uma justificativa de falta.
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoJustificativaFalta</param>
        /// <returns></returns>
        public static bool Salvar(ACA_AlunoJustificativaFalta entity)
        {
            ACA_AlunoJustificativaFaltaDAO dao = new ACA_AlunoJustificativaFaltaDAO();
            if (entity.Validate())
            {
                if (!VerificaIntervaloPeriodo(entity))
                {
                    return dao.Salvar(entity);
                }

                throw new ValidationException("Já existe uma justificativa de falta no mesmo intervalo de data para o aluno.");
            }

            throw new ValidationException(CoreSSO.BLL.UtilBO.ErrosValidacao(entity));
        }

        /// <summary>
        /// Verifica se já existe uma justifica para o intervalo de data da justificativa que vai ser salva
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoJustificativaFalta</param>
        /// <returns>true se exister, false se não</returns>
        private static bool VerificaIntervaloPeriodo(ACA_AlunoJustificativaFalta entity)
        {
            ACA_AlunoJustificativaFaltaDAO dao = new ACA_AlunoJustificativaFaltaDAO();
            return dao.VerificaIntervaloPeriodo(entity);
        }

        /// <summary>
        /// Verifica se já esxiste uma avaliacao efetivada para o aluno no intervalo de data da justificativa que vai ser salva
        /// </summary>
        /// <param name="entity">Entidade ACA_AlunoJustificativaFalta</param>
        /// <param name="avaliacao">Nome da avaliação.</param>
        /// <returns>true se exister, false se não</returns>
        public static bool VerificaAlunoAvaliacao(ACA_AlunoJustificativaFalta entity, out string avaliacao)
        {
            ACA_AlunoJustificativaFaltaDAO dao = new ACA_AlunoJustificativaFaltaDAO();
            return dao.VerificaAlunoAvaliacao(entity, out avaliacao);
        }


        /// <summary>
        /// Processa os protocolos informados.
        /// </summary>
        /// <param name="ltProtocolo">Lista de protocolos em processamento.</param>
        /// <param name="tentativasProtocolo">Quantidade máxima de tentativas para processar protocolos.</param>
        /// <returns></returns>
        public static bool ProcessaProtocoloJustificativaFalta(List<DCL_Protocolo> ltProtocolo, int tentativasProtocolo)
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
                        JObject falta = JObject.Parse(protocolo.pro_pacote);
                        JArray arrayFalta = ((JArray)falta.SelectToken("Justificativas") ?? new JArray());

                        foreach (JToken token in arrayFalta.Children())
                        {
                            ACA_AlunoJustificativaFalta entidade = new ACA_AlunoJustificativaFalta
                            {
                                alu_id = (Int64)(token.SelectToken("alu_id") ?? 0),
                                afj_id = (int)(token.SelectToken("afj_idGestao") ?? 0)
                            };

                            GetEntity(entidade, bancoSincronizacao);

                            entidade.tjf_id = (int)(token.SelectToken("tjf_id") ?? 0);
                            entidade.afj_dataInicio = (DateTime)(token.SelectToken("afj_dataInicio") ?? new DateTime());
                            entidade.afj_dataFim = (DateTime)(token.SelectToken("afj_dataFim") ?? new DateTime());
                            entidade.afj_situacao = (Byte)(token.SelectToken("afj_situacao") ?? Convert.ToByte(1));
                            entidade.afj_dataAlteracao = (DateTime)(token.SelectToken("afj_dataAlteracao") ?? new DateTime());
                            entidade.pro_id = protocolo.pro_id;

                            if (!entidade.Validate())
                                throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(entidade));

                            processou = Save(entidade, bancoSincronizacao);
                        }
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

    }
}