/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ESC_UnidadeEscolaPredio : Abstract_ESC_UnidadeEscolaPredio
	{
        [DataObjectField(true, false, false)]
        public override int esc_id { get; set; }
        [DataObjectField(true, false, false)]
        public override int uni_id { get; set; }
        [DataObjectField(true, false, false)]
        public override int prd_id { get; set; }
        [DataObjectField(true, false, false)]
        public override int uep_id { get; set; }
        [MSDefaultValue(true)]
        public override bool uep_principal { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime uep_vigenciaInicio { get; set; }
        [MSDefaultValue(1)]
        public override byte uep_situacao { get; set; }
        public override DateTime uep_dataCriacao { get; set; }
        public override DateTime uep_dataAlteracao { get; set; }        
	}
}