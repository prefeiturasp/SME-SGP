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
    /// Situações da resposta da sondagem
    /// </summary>
    public enum ACA_SondagemRespostaSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    #endregion Enumeradores

    public class ACA_SondagemRespostaBO : BusinessBase<ACA_SondagemRespostaDAO, ACA_SondagemResposta>
    {
        /// <summary>
        /// Seleciona as respostas ligadas à sondagem
        /// </summary>
        /// <param name="snd_id">ID da sondagem</param>
        /// <param name="banco">Transação de banco</param>
        /// <returns></returns>
        public static List<ACA_SondagemResposta> SelectRespostasBy_Sondagem(int snd_id, TalkDBTransaction banco = null)
        {
            ACA_SondagemRespostaDAO dao = new ACA_SondagemRespostaDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectRespostasBy_Sondagem(snd_id);
        }
    }
}