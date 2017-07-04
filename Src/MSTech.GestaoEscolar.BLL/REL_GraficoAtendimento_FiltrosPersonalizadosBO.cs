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

    #region Enumeradores

    /// <summary>
    /// Situações do filtro personalizado do gráfico
    /// </summary>
    public enum REL_GraficoAtendimento_FiltrosPersonalizadosSituacao : byte
    {
        Ativo = 1
        ,

        Excluido = 3
    }

    #endregion Enumeradores


    /// <summary>
    /// Description: REL_GraficoAtendimento_FiltrosPersonalizados Business Object. 
    /// </summary>
    public class REL_GraficoAtendimento_FiltrosPersonalizadosBO : BusinessBase<REL_GraficoAtendimento_FiltrosPersonalizadosDAO, REL_GraficoAtendimento_FiltrosPersonalizados>
	{
        public static List<REL_GraficoAtendimento_FiltrosPersonalizados> SelectBy_gra_id(int gra_id, TalkDBTransaction banco = null)
        {
            REL_GraficoAtendimento_FiltrosPersonalizadosDAO dao = new REL_GraficoAtendimento_FiltrosPersonalizadosDAO();
            if (banco != null)
                dao._Banco = banco;
            return dao.SelectBy_gra_id(gra_id);
        }
    }
}