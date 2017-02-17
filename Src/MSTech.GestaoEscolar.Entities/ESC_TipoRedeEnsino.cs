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
	public class ESC_TipoRedeEnsino : Abstract_ESC_TipoRedeEnsino
	{
        [MSValidRange(100, "Tipo de rede de ensino pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de rede de ensino é obrigatório.")]
        public override string tre_nome { get; set; }
        [MSDefaultValue(1)]
        public override byte tre_situacao { get; set; }
        public override DateTime tre_dataCriacao { get; set; }
        public override DateTime tre_dataAlteracao { get; set; }
	}
}