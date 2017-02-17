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
	public class ESC_UnidadeEscola : Abstract_ESC_UnidadeEscola
	{
        [MSValidRange(20, "Código pode conter até 20 caracteres.")]
        public override string uni_codigo { get; set; }
        [MSValidRange(1000, "Descrição pode conter até 1000 caracteres.")]
        public override string uni_descricao { get; set; }
        [MSDefaultValue(true)]
        public override bool uni_principal { get; set; }
        [MSDefaultValue(1)]
        public override byte uni_situacao { get; set; }
        public override DateTime uni_dataCriacao { get; set; }
        public override DateTime uni_dataAlteracao { get; set; }
	}
}