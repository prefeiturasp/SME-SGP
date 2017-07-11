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
    /// Estrutura para controle das sugestões nos capítulos do currículo.
    /// </summary>
    [Serializable]
    public struct sAlertaInicioFechamento
    {
        public Guid usu_id { get; set; }
        public long evt_id { get; set; }
        public string evt_nome { get; set; }
        public int dias { get; set; }
    }

    #endregion

    /// <summary>
    /// Description: LOG_AlertaInicioFechamento Business Object. 
    /// </summary>
    public class LOG_AlertaInicioFechamentoBO : BusinessBase<LOG_AlertaInicioFechamentoDAO, LOG_AlertaInicioFechamento>
	{
				
	}
}