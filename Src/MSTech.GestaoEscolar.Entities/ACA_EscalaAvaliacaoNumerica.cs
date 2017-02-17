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
	public class ACA_EscalaAvaliacaoNumerica : Abstract_ACA_EscalaAvaliacaoNumerica
	{
        public override decimal ean_menorValor { get; set; }
        [MSNotNullOrEmpty("Maior valor é obrigatório.")]
        public override decimal ean_maiorValor { get; set; }
        [MSNotNullOrEmpty("Variação é obrigatório.")]
        public override decimal ean_variacao { get; set; }
        [MSDefaultValue(1)]
        public override byte ean_situacao { get; set; }
	}
}