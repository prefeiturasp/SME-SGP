/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using MSTech.Business.Common;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.DAL;
    using System.ComponentModel;
    /// <summary>
    /// Situações da movimentação dos tipos de movimentação
    /// </summary>
    public enum CLS_RelatorioAtendimentoTipo : byte
    {
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.AEE")]
        AEE = 1
        ,
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.NAAPA")]
        NAAPA = 2
        ,
        [Description("CLS_RelatorioAtendimentoBO.CLS_RelatorioAtendimentoTipo.RP")]
        RP = 3
    }

    /// <summary>
    /// Description: CLS_RelatorioAtendimento Business Object. 
    /// </summary>
    public class CLS_RelatorioAtendimentoBO : BusinessBase<CLS_RelatorioAtendimentoDAO, CLS_RelatorioAtendimento>
	{
				
	}
}