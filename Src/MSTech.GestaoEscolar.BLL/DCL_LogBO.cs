/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.Data.Common;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using System;
    using MSTech.CoreSSO.BLL;
    using MSTech.Validation.Exceptions;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Description: DCL_Log Business Object. 
    /// </summary>
    public class DCL_LogBO : BusinessBase<DCL_LogDAO, DCL_Log>
    {

        #region Sincronizacao com diario de classe
        /// <summary>
        /// Processa o protocolo informado.
        /// </summary>
        /// <param name="protocolo"></param>
        /// <returns></returns>
        public static bool ProcessaProtocoloLog(List<DCL_Protocolo> ltProtocolo, int tentativasProtocolo)
        {
            // DataTable de protocolos
            DataTable dtProtocolo = DCL_Protocolo.TipoTabela_Protocolo();

            DCL_LogDAO dao = new DCL_LogDAO();

            DataTable dtLogs = new DataTable();
            dtLogs.Columns.Add("log_id", typeof(Guid));
            dtLogs.Columns.Add("log_dataHora", typeof(DateTime));
            dtLogs.Columns.Add("usu_id", typeof(Guid));
            dtLogs.Columns.Add("usu_login", typeof(String));
            dtLogs.Columns.Add("log_acao", typeof(String));
            dtLogs.Columns.Add("log_descricao", typeof(String));
            dtLogs.Columns.Add("log_macAddress", typeof(String));
            dtLogs.Columns.Add("log_ipAddress", typeof(String));
            dtLogs.Columns.Add("log_appVersion", typeof(String));
            dtLogs.Columns.Add("log_serialNumber", typeof(String));
            dtLogs.Columns.Add("esc_id", typeof(Int32));
            dtLogs.Columns.Add("tur_id", typeof(Int64));
            dtLogs.Columns.Add("tud_id", typeof(Int64));
            dtLogs.Columns.Add("sis_id", typeof(Int32));

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
                TalkDBTransaction banco = new DCL_LogDAO()._Banco.CopyThisInstance();

                try
                {
                    banco.Open();

                    if (protocolo.pro_tentativa > tentativasProtocolo)
                    {
                        throw new ValidationException(String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                            , tentativasProtocolo, protocolo.pro_statusObservacao));
                    }

                    #region Variável

                    List<DCL_Log> listaEntLogs = new List<DCL_Log>();

                    #endregion

                    #region Informações do log de sistema

                    JObject logs = JObject.Parse(protocolo.pro_pacote);

                    int sis_id = (int)logs.SelectToken("sis_id");
                    int esc_id = (int)logs.SelectToken("esc_id");

                    long turmaId = -1;
                    long turmaDisciplinaId = -1;

                    JArray logLista = ((JArray)logs.SelectToken("Logs") ?? new JArray());

                    #endregion

                    #region Informações de cada log do protocolo

                    foreach (JObject jsonLog in logLista)
                    {
                        string log_descricao = jsonLog.SelectToken("log_descricao").ToString();
                        string usu_login = jsonLog.SelectToken("usu_login").ToString();
                        string log_acao;

                        try
                        {
                            log_acao = Enum.GetName(typeof(LOG_SistemaTipo), jsonLog.SelectToken("log_acao"));
                        }
                        catch
                        {
                            log_acao = jsonLog.SelectToken("log_acao").ToString();
                        }

                        Guid usu_id = new Guid(string.IsNullOrEmpty(jsonLog.SelectToken("usu_id").ToString()) ? Guid.Empty.ToString() : jsonLog.SelectToken("usu_id").ToString());

                        DateTime log_dataHora = (DateTime)(jsonLog.SelectToken("log_dataHora") ?? DateTime.Now);

                        turmaId = Convert.ToInt64(jsonLog.SelectToken("tur_id"));
                        turmaDisciplinaId = Convert.ToInt64(jsonLog.SelectToken("tud_id"));

                        string ipAddress = Convert.ToString(jsonLog.SelectToken("log_ipAddress"));
                        string macAddress = Convert.ToString(jsonLog.SelectToken("log_macAddress"));
                        string appVersion = Convert.ToString(jsonLog.SelectToken("log_appVersion"));
                        string serialNumber = Convert.ToString(jsonLog.SelectToken("log_serialNumber"));

                        DCL_Log log = new DCL_Log
                        {
                            log_id = Guid.NewGuid(),
                            sis_id = sis_id,
                            log_descricao = log_descricao,
                            usu_login = usu_login,
                            usu_id = usu_id,
                            log_acao = log_acao,
                            log_dataHora = log_dataHora,
                            esc_id = esc_id,
                            log_ipAddress = ipAddress,
                            tud_id = turmaDisciplinaId,
                            tur_id = turmaId,
                            log_macAddress = macAddress,
                            log_appVersion = appVersion,
                            log_serialNumber = serialNumber
                        };

                        // validações do log.
                        if (!log.Validate())
                        {
                            throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(log));
                        }

                        DataRow dr = dtLogs.NewRow();
                        dr["log_id"] = Guid.NewGuid();
                        dr["log_dataHora"] = log_dataHora;

                        if (usu_id != Guid.Empty)
                            dr["usu_id"] = usu_id;
                        else
                            dr["usu_id"] = DBNull.Value;

                        if (!string.IsNullOrEmpty(usu_login))
                            dr["usu_login"] = usu_login;
                        else
                            dr["usu_login"] = DBNull.Value;

                        if (!string.IsNullOrEmpty(log_acao))
                            dr["log_acao"] = log_acao;
                        else
                            dr["log_acao"] = DBNull.Value;

                        if (!string.IsNullOrEmpty(log_descricao))
                            dr["log_descricao"] = log_descricao;
                        else
                            dr["log_descricao"] = DBNull.Value;

                        if (!string.IsNullOrEmpty(macAddress))
                            dr["log_macAddress"] = macAddress;
                        else
                            dr["log_macAddress"] = DBNull.Value;

                        if (!string.IsNullOrEmpty(ipAddress))
                            dr["log_ipAddress"] = ipAddress;
                        else
                            dr["log_ipAddress"] = DBNull.Value;

                        if (!string.IsNullOrEmpty(appVersion))
                            dr["log_appVersion"] = appVersion;
                        else
                            dr["log_appVersion"] = DBNull.Value;

                        if (!string.IsNullOrEmpty(serialNumber))
                            dr["log_serialNumber"] = serialNumber;
                        else
                            dr["log_serialNumber"] = DBNull.Value;

                        if (esc_id > 0)
                            dr["esc_id"] = esc_id;
                        else
                            dr["esc_id"] = DBNull.Value;

                        if (turmaId > 0)
                            dr["tur_id"] = turmaId;
                        else
                            dr["tur_id"] = DBNull.Value;

                        if (turmaDisciplinaId > 0)
                            dr["tud_id"] = turmaDisciplinaId;
                        else
                            dr["tud_id"] = DBNull.Value;

                        if (sis_id > 0)
                            dr["sis_id"] = sis_id;
                        else
                            dr["sis_id"] = DBNull.Value;

                        dtLogs.Rows.Add(dr);

                    }

                    protocolo.tur_id = turmaId;
                    protocolo.tud_id = turmaDisciplinaId;
                    protocolo.pro_statusObservacao = String.Format("Protocolo processado com sucesso ({0}).",
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    protocolo.pro_status = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComSucesso;
                    dtProtocolo.Rows.Add(DCL_ProtocoloBO.ProtocoloToDataRow(protocolo, dtProtocolo.NewRow()));

                    #endregion
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
                    banco.Close(ex);
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
                    banco.Close(ex);
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
                    banco.Close(ex);
                }
                finally
                {
                    if (banco.ConnectionIsOpen)
                        banco.Close();
                }
            }

            try
            {
                if (dao.SalvarLogs(dtLogs))
                {
                    DCL_ProtocoloBO.AtualizaProtocolos(dtProtocolo);
                    return true;
                }
                else
                {
                    foreach (DataRow dr in dtProtocolo.Rows)
                    {
                        if (Convert.ToInt32(dr["pro_tentativa"]) > tentativasProtocolo)
                        {
                            throw new ValidationException(String.Format("Número máximo ({0}) de tentativas de processamento deste protocolo foram excedidas. Erro: {1}"
                                , tentativasProtocolo, dr["pro_statusObservacao"].ToString()));
                        }

                        dr["pro_statusObservacao"] = String.Format("Protocolo não processado ({0}).",
                                DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
                        dr["pro_status"] = (byte)DCL_ProtocoloBO.eStatus.NaoProcessado;
                    }

                    DCL_ProtocoloBO.AtualizaProtocolos(dtProtocolo);
                    return false;
                }
            }
            catch (Exception ex)
            {
                foreach (DataRow dr in dtProtocolo.Rows)
                {
                    dr["pro_status"] = (byte)DCL_ProtocoloBO.eStatus.ProcessadoComErro;
                    dr["pro_statusObservacao"] = ex.Message;
                    dr["tud_id"] = DBNull.Value;
                    dr["tur_id"] = DBNull.Value;
                }

                DCL_ProtocoloBO.AtualizaProtocolos(dtProtocolo);
                return false;
            }
        }
        #endregion

    }
}