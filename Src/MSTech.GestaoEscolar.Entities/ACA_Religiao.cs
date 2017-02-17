/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;
using System;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_Religiao : Abstract_ACA_Religiao
	{
        [MSValidRange(100, "Tipo de religião pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de religião é obrigatório.")]
        public override string rlg_nome { get; set; }
        [MSDefaultValue(1)]
        public override byte rlg_situacao { get; set; }
	}
}