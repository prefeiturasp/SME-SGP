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
	public class ACA_AvaliacaoRelacionada : Abstract_ACA_AvaliacaoRelacionada
	{
        [MSNotNullOrEmpty("Avaliação relacionada é obrigatório.")]
        public override int ava_idRelacionada { get; set; }
        [MSNotNullOrEmpty("Substitui nota é obrigatório.")]
        public override bool avr_substituiNota { get; set; }
        [MSNotNullOrEmpty("Mantem maior nota é obrigatório.")]
        public override bool avr_mantemMaiorNota { get; set; }
        [MSNotNullOrEmpty("Obrigatório nota mínima é obrigatório.")]
        public override bool avr_obrigatorioNotaMinima { get; set; }
        [MSDefaultValue(1)]
        public override byte avr_situacao { get; set; }
	}
}