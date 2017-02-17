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
	public class ESC_UnidadeEscolaContato : Abstract_ESC_UnidadeEscolaContato
	{
        [MSNotNullOrEmpty("Tipo de contato é obrigatório.")]
        public override Guid tmc_id { get; set; }
        [MSValidRange(200, "Contato pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Contato é obrigatório.")]
        public override string uec_contato { get; set; }
        [MSDefaultValue(1)]
        public override byte uec_situacao { get; set; }
        public override DateTime uec_dataCriacao { get; set; }
        public override DateTime uec_dataAlteracao { get; set; }
	}
}