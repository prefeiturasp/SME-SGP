/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
    using MSTech.GestaoEscolar.Entities.Abstracts;
    using Validation;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Description: .
    /// </summary>
    [Serializable]
	public class REL_GraficoAtendimento_FiltrosFixos : Abstract_REL_GraficoAtendimento_FiltrosFixos
	{
        public string gff_tituloFiltro { get; set; }
        public string gff_valorDetalhado { get; set; }
    }
}