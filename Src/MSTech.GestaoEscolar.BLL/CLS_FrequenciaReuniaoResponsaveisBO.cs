/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSTech.Business.Common;
using MSTech.Data.Common;
using MSTech.GestaoEscolar.DAL;
using MSTech.GestaoEscolar.Entities;
using MSTech.Validation.Exceptions;

namespace MSTech.GestaoEscolar.BLL
{
    /// <summary>
    /// CLS_FrequenciaReuniaoResponsaveis Business Object
    /// </summary>
    public class CLS_FrequenciaReuniaoResponsaveisBO : BusinessBase<CLS_FrequenciaReuniaoResponsaveisDAO, CLS_FrequenciaReuniaoResponsaveis>
    {
        /// <summary>
        /// Verifica se existe frequências cadastradas para o calendário.
        /// </summary>
        /// <param name="cal_id">Id do calendário.</param>
        /// <param name="cap_id">Id do período do calendário. </param>
        /// <param name="cur_id">Id do curso.</param>
        /// <param name="crr_id">Id do currículo.</param>
        /// <param name="cadastroPorPeriodo">Flag que indica se as reuniões são cadastradas por período.</param>
        /// <returns>Verdadeiro se existe algum registro ligado ao calendário.</returns>
        public static bool VerificaFrequenciaPorCalendario(int cal_id, int cap_id, int cur_id, int crr_id, bool cadastroPorPeriodo)
        {
            CLS_FrequenciaReuniaoResponsaveisDAO dao = new CLS_FrequenciaReuniaoResponsaveisDAO();
            return dao.VerificaFrequenciaPorCalendario(cal_id, cap_id, cur_id, crr_id, cadastroPorPeriodo);
        }

        #region Saves

        public static bool Salvar(List<CLS_FrequenciaReuniaoResponsaveis> list, List<CLS_FrequenciaReuniao> listReunioes)
        {
            TalkDBTransaction banco = new CLS_FrequenciaReuniaoResponsaveisDAO()._Banco.CopyThisInstance();
            banco.Open(IsolationLevel.ReadCommitted);

            try
            {
                #region Frequencia Reuniao

                if (listReunioes.Any(p => !p.Validate()))
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(listReunioes.First(p => !p.Validate())));

                CLS_FrequenciaReuniaoBO.SalvarEmLote(listReunioes, banco);

                #endregion Frequencia Reuniao

                #region Frequencia Reuniao Responsaveis

                if (list.Any(p => !p.Validate()))
                    throw new ValidationException(GestaoEscolarUtilBO.ErrosValidacao(list.First(p => !p.Validate())));

                SalvarEmLote(list, banco);

                #endregion Frequencia Reuniao Responsaveis

                return true;
            }
            catch (Exception ex)
            {
                banco.Close(ex);
                throw;
            }
            finally
            {
                if (banco.ConnectionIsOpen)
                    banco.Close();
            }
        }

        /// <summary>
        /// Salva os dados em lote
        /// </summary>
        /// <param name="list">Lista de dados.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarEmLote(List<CLS_FrequenciaReuniaoResponsaveis> list, TalkDBTransaction banco = null)
        {
            if (list.Any())
            {
                List<CLS_FrequenciaReuniaoResponsaveis_SalvarEmLote> listFrequenciaReuniaoResponsaveis =
                    list.Select(p => new CLS_FrequenciaReuniaoResponsaveis_SalvarEmLote
                    {
                        alu_id = p.alu_id,
                        cal_id = p.cal_id,
                        cap_id = p.cap_id,
                        frp_id = p.frp_id,
                        frp_frequencia = p.frp_frequencia
                    }).ToList();

                DataTable dtFrequenciaReuniaoResponsaveis =
                    GestaoEscolarUtilBO.EntityToDataTable<CLS_FrequenciaReuniaoResponsaveis_SalvarEmLote>(listFrequenciaReuniaoResponsaveis);

                return banco == null ?
                       new CLS_FrequenciaReuniaoResponsaveisDAO().SalvarEmLote(dtFrequenciaReuniaoResponsaveis) :
                       new CLS_FrequenciaReuniaoResponsaveisDAO { _Banco = banco }.SalvarEmLote(dtFrequenciaReuniaoResponsaveis);
            }

            return true;
        }

        #endregion Saves
    }
}