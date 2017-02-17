/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
	using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;

    #region ENUMERADORES

    public enum LOG_AvaliacaoMedia_Alteracao_Tipo : byte
    {
        AlteracaoMedia = 1
    }

    public enum LOG_AvaliacaoMedia_Alteracao_Origem : byte
    {
        Web = 1,
        Sincronizacao = 2
    }

    #endregion

    /// <summary>
	/// Description: LOG_AvaliacaoMedia_Alteracao Business Object. 
	/// </summary>
	public class LOG_AvaliacaoMedia_AlteracaoBO : BusinessBase<LOG_AvaliacaoMedia_AlteracaoDAO, LOG_AvaliacaoMedia_Alteracao>
	{
				
	}
}