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
	public class ACA_CurriculoTurno : Abstract_ACA_CurriculoTurno
	{        
        [DataObjectField(true, false, false)]
        public override int crt_id { get; set; }
        [MSNotNullOrEmpty("Tipo de turno é obrigatório.")]
        public override int ttn_id { get; set; }
        [MSNotNullOrEmpty("Vigência inicial é obrigatório.")]
        public override DateTime crt_vigenciaInicio { get; set; }
        [MSDefaultValue(1)]
        public override byte crt_situacao { get; set; }                
        public override DateTime crt_dataCriacao { get; set; }        
        public override DateTime crt_dataAlteracao { get; set; }
	}
}