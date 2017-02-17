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
	public class ACA_TipoEvento : Abstract_ACA_TipoEvento
	{        
        [DataObjectField(true, true, false)]
        public override int tev_id { get; set; }
        [MSValidRange(200, "Tipo de evento pode conter até 200 caracteres.")]
        [MSNotNullOrEmpty("Tipo de evento é obrigatório.")]
        public override string tev_nome { get; set; }
        [MSDefaultValue(1)]
        public override byte tev_situacao { get; set; }
        public override DateTime tev_dataCriacao { get; set; }
        public override DateTime tev_dataAlteracao { get; set; }
        public override bool tev_periodoCalendario { get; set; }
        [MSDefaultValue(1)]
        public override byte tev_liberacao { get; set; }
	}
}