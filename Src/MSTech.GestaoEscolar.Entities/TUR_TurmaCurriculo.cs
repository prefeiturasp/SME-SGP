/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class TUR_TurmaCurriculo : Abstract_TUR_TurmaCurriculo
	{
        [MSNotNullOrEmpty("Prioridade é obrigatório.")]
        public override int tcr_prioridade { get; set; }        
        [MSDefaultValue(1)]
        public override byte tcr_situacao { get; set; }
        public override DateTime tcr_dataCriacao { get; set; }
        public override DateTime tcr_dataAlteracao { get; set; }
	}
}