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
	public class ESC_UnidadeEscolaEnsino : Abstract_ESC_UnidadeEscolaEnsino
	{
        [MSNotNullOrEmpty("Tipo de ensino é obrigatório.")]
        public override int tne_id { get; set; }
        [MSNotNullOrEmpty("Modalidade de ensino é obrigatório.")]
        public override int tme_id { get; set; }        
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime uee_vigenciaInicio { get; set; }        
        [MSDefaultValue(1)]
        public override byte uee_situacao { get; set; }        
        public override DateTime uee_dataCriacao { get; set; }
        public override DateTime uee_dataAlteracao { get; set; }
	}
}