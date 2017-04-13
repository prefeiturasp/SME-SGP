/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using Data.Common;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using System.Data;
    #region Enumeradores

    /// <summary>
    /// Situações da questao da sondagem
    /// </summary>
    public enum ACA_SondagemAgendamentoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
        ,

        Cancelado = 4
    }

    #endregion Enumeradores

    public class ACA_SondagemAgendamentoBO : BusinessBase<ACA_SondagemAgendamentoDAO, ACA_SondagemAgendamento>
    {
        /// <summary>
        /// Seleciona os agendamentos ligados à sondagem
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="banco">Transação de banco</param>
        /// <returns></returns>
        public static List<ACA_SondagemAgendamento> SelectAgendamentosBy_Sondagem(int snd_id, TalkDBTransaction banco = null)
        {
            ACA_SondagemAgendamentoDAO dao = new ACA_SondagemAgendamentoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectAgendamentosBy_Sondagem(snd_id);
        }

        /// <summary>
        /// Salva os agendamentos da sondagem
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="lstAgendamento">Lista de agendamentos</param>
        /// <param name="lstAgendamentoPeriodo">Lista de períodos do agendamento</param>
        /// <param name="banco">Transação de banco</param>
        /// <returns></returns>
        public static bool Salvar(int snd_id, List<ACA_SondagemAgendamento> lstAgendamento, List<ACA_SondagemAgendamentoPeriodo> lstAgendamentoPeriodo, TalkDBTransaction banco = null)
        {
            ACA_SondagemDAO dao = new ACA_SondagemDAO();
            if (banco == null)
                dao._Banco.Open(IsolationLevel.ReadCommitted);
            else
                dao._Banco = banco;

            try
            {
                List<ACA_SondagemAgendamento> lstAgendamentoBanco = SelectAgendamentosBy_Sondagem(snd_id, dao._Banco);

                int idAux = 0;
                //Salva agendamentos
                foreach (ACA_SondagemAgendamento sda in lstAgendamento)
                {
                    idAux = sda.sda_id;
                    if (!Save(sda, dao._Banco))
                        return false;
                    
                    //Remove as ligações de períodos dos agendamentos da sondagem para salvar a lista atualizada
                    ACA_SondagemAgendamentoPeriodoBO.DeletePeriodosBy_Agendamento(snd_id, sda.sda_id, dao._Banco);

                    foreach (ACA_SondagemAgendamentoPeriodo sap in lstAgendamentoPeriodo.Where(p => p.sda_id == idAux))
                    {
                        if (!ACA_SondagemAgendamentoPeriodoBO.Save(new ACA_SondagemAgendamentoPeriodo
                                                                        {
                                                                            snd_id = snd_id,
                                                                            sda_id = sda.sda_id,
                                                                            tcp_id = sap.tcp_id,
                                                                            IsNew = true
                                                                        }, dao._Banco))
                            return false;
                    }
                }
                
                //Remove logicamente no banco os agendamentos que foram removidos da sondagem
                foreach (ACA_SondagemAgendamento sdaB in lstAgendamentoBanco)
                    if (!lstAgendamento.Any(a => a.sda_id == sdaB.sda_id) && !lstAgendamento.Any(a => a.sda_id == sdaB.sda_id))
                        Delete(sdaB, dao._Banco);
                
                return true;
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
    }
}