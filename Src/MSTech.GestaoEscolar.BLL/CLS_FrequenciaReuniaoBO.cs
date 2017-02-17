/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using MSTech.Business.Common;
    using MSTech.Data.Common;
    using MSTech.GestaoEscolar.DAL;
    using MSTech.GestaoEscolar.Entities;

    /// <summary>
    /// Description: CLS_FrequenciaReuniao Business Object.
    /// </summary>
    public class CLS_FrequenciaReuniaoBO : BusinessBase<CLS_FrequenciaReuniaoDAO, CLS_FrequenciaReuniao>
    {
        /// <summary>
        /// SelecionaEfetivacaoDeReuniaoDeResponsaveis
        /// </summary>
        /// <param name="tur_id">ID da turma</param>
        /// <param name="cal_id">ID do calendário</param>
        /// <param name="cap_id">ID do período do calendário</param>
        /// <returns></returns>
        public static DataTable SelecionaEfetivacaoDeReuniaoDeResponsaveis(long tur_id, int cal_id, int cap_id)
        {
            CLS_FrequenciaReuniaoDAO dao = new CLS_FrequenciaReuniaoDAO();
            return dao.SelectBy_tur_id_cal_id_cap_id(tur_id, cal_id, cap_id);
        }

        #region Saves

        /// <summary>
        /// Salva os dados em lote
        /// </summary>
        /// <param name="list">Lista de dados.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarEmLote(List<CLS_FrequenciaReuniao> list, TalkDBTransaction banco = null)
        {
            if (list.Any())
            {
                List<CLS_FrequenciaReuniao_SalvarEmLote> listFrequenciaReuniao =
                    list.Select(p => new CLS_FrequenciaReuniao_SalvarEmLote
                    {
                        tur_id = p.tur_id,
                        cal_id = p.cal_id,
                        cap_id = p.cap_id,
                        frp_id = p.frp_id,
                        frr_efetivado = p.frr_efetivado
                    }).ToList();

                DataTable dtFrequenciaReuniao = GestaoEscolarUtilBO.EntityToDataTable<CLS_FrequenciaReuniao_SalvarEmLote>(listFrequenciaReuniao);

                return banco == null ?
                       new CLS_FrequenciaReuniaoDAO().SalvarEmLote(dtFrequenciaReuniao) :
                       new CLS_FrequenciaReuniaoDAO { _Banco = banco }.SalvarEmLote(dtFrequenciaReuniao);
            }

            return true;
        }

        #endregion Saves
    }
}