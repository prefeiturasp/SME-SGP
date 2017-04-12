/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;
    using System.Collections.Generic;
    using Data.Common;
    /// <summary>
    /// Description: ACA_SondagemAgendamentoPeriodo Business Object. 
    /// </summary>
    public class ACA_SondagemAgendamentoPeriodoBO : BusinessBase<ACA_SondagemAgendamentoPeriodoDAO, ACA_SondagemAgendamentoPeriodo>
    {
        /// <summary>
        /// Seleciona os períodos selecionados da sondagem ou do agendamento 
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="sda_id">ID do agendamento</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        public static List<ACA_SondagemAgendamentoPeriodo> SelectPeriodosBy_Agendamento(int snd_id, int sda_id, TalkDBTransaction banco = null)
        {
            ACA_SondagemAgendamentoPeriodoDAO dao = new ACA_SondagemAgendamentoPeriodoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectPeriodosBy_Agendamento(snd_id, sda_id);
        }

        /// <summary>
        /// Remove as ligações de períodos do agendamento informado
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="sda_id">ID do agendamento</param>
        /// <param name="banco">Transação do banco</param>
        public static void DeletePeriodosBy_Agendamento(int snd_id, int sda_id, TalkDBTransaction banco)
        {
            ACA_SondagemAgendamentoPeriodoDAO dao = new ACA_SondagemAgendamentoPeriodoDAO();
            if (banco != null)
                dao._Banco = banco;
            dao.DeletePeriodosBy_Agendamento(snd_id, sda_id);
        }
    }
}