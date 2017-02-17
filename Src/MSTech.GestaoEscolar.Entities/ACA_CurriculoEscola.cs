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
	public class ACA_CurriculoEscola : Abstract_ACA_CurriculoEscola
	{
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime ces_vigenciaInicio { get; set; }
        [MSDefaultValue(1)]
        public override byte ces_situacao { get; set; }        
        public override DateTime ces_dataCriacao { get; set; }
        public override DateTime ces_dataAlteracao { get; set; }
        [MSDefaultValue(1)]
        public override int vis_id { get; set; }
	}
}