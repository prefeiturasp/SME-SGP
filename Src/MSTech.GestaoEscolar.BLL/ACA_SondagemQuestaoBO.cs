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
    #region Enumeradores

    /// <summary>
    /// Situações da questao da sondagem
    /// </summary>
    public enum ACA_SondagemQuestaoSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    #endregion Enumeradores

    public class ACA_SondagemQuestaoBO : BusinessBase<ACA_SondagemQuestaoDAO, ACA_SondagemQuestao>
    {
        /// <summary>
        /// Seleciona as questões ligadas à sondagem
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="banco">Transação de banco</param>
        /// <returns></returns>
        public static List<ACA_SondagemQuestao> SelectQuestoesBy_Sondagem(int snd_id, TalkDBTransaction banco = null)
        {
            ACA_SondagemQuestaoDAO dao = new ACA_SondagemQuestaoDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectQuestoesBy_Sondagem(snd_id);
        }
    }
}