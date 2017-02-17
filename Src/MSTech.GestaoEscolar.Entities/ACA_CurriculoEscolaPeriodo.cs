/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_CurriculoEscolaPeriodo : Abstract_ACA_CurriculoEscolaPeriodo
	{
        [MSDefaultValue(1)]
        public override byte cep_situacao { get; set; }
	}
}