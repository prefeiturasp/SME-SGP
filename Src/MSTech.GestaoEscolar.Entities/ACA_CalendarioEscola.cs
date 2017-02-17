/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_CalendarioEscola : Abstract_ACA_CalendarioEscola
	{
        [MSDefaultValue(1)]
        public override byte ces_situacao { get; set; }
        public override DateTime ces_dataCriacao { get; set; }
        public override DateTime ces_dataAlteracao { get; set; }
	}
}