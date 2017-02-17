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
	public class ACA_TipoModalidadeEnsino : Abstract_ACA_TipoModalidadeEnsino
	{
        [MSValidRange(100, "Tipo de modalidade de ensino pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Tipo de modalidade de ensino obrigatório.")]
        public override string tme_nome { get; set; }
        [MSDefaultValue(1)]
        public override byte tme_situacao { get; set; }
        public override DateTime tme_dataCriacao { get; set; }
        public override DateTime tme_dataAlteracao { get; set; }
	}
}