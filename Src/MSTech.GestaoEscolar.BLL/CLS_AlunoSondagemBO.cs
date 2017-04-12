/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using Data.Common;
    using System.Collections.Generic;    /// <summary>
                                         /// Description: CLS_AlunoSondagem Business Object. 
                                         /// </summary>
    public class CLS_AlunoSondagemBO : BusinessBase<CLS_AlunoSondagemDAO, CLS_AlunoSondagem>
    {
        /// <summary>
        /// Seleciona os alunos ligados à sondagem/agendamento.
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="sda_id">ID do agendamento</param>
        /// <param name="banco">Transação do banco</param>
        /// <returns></returns>
        public static List<CLS_AlunoSondagem> SelectAgendamentosBy_Sondagem(int snd_id, int sda_id, TalkDBTransaction banco = null)
        {
            CLS_AlunoSondagemDAO dao = new CLS_AlunoSondagemDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectAgendamentosBy_Sondagem(snd_id, sda_id);
        }
    }
}