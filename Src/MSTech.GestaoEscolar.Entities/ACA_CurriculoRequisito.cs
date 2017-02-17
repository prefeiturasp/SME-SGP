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
	public class ACA_CurriculoRequisito : Abstract_ACA_CurriculoRequisito
	{
        [MSDefaultValue(1)]
        public override byte crq_situacao { get; set; }
        public override DateTime crq_dataCriacao { get; set; }
        public override DateTime crq_dataAlteracao { get; set; }
	}
}