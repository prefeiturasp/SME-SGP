/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System;

    #region Estruturas

    /// <summary>
    /// Estrutura para controle das ações realizadas.
    /// </summary>
    [Serializable]
    public struct sAcoesRealizadas
    {
        public long rpa_id { get; set; }
        public string rpa_data { get; set; }
        public bool rpa_impressao { get; set; }
        public string rpa_acao { get; set; }
        public int idTemp { get; set; }
        public bool excluido { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: CLS_RelatorioPreenchimentoAcoesRealizadas Business Object. 
    /// </summary>
    public class CLS_RelatorioPreenchimentoAcoesRealizadasBO : BusinessBase<CLS_RelatorioPreenchimentoAcoesRealizadasDAO, CLS_RelatorioPreenchimentoAcoesRealizadas>
	{
				
	}
}