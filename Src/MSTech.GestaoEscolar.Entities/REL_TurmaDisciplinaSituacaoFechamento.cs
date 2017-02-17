/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.Entities
{
	using MSTech.GestaoEscolar.Entities.Abstracts;
    using System;
		
	/// <summary>
	/// Description: .
	/// </summary>
    [Serializable]
	public class REL_TurmaDisciplinaSituacaoFechamento : Abstract_REL_TurmaDisciplinaSituacaoFechamento
	{
        public byte tud_tipo { get; set; }
        public long tud_idRegencia { get; set; }
	}
}