/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.ComponentModel;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_CoordenadorDisciplina : Abstract_ACA_CoordenadorDisciplina
	{        
        [DataObjectField(true, true, false)]
        public override int cdd_id { get; set; }
        [MSNotNullOrEmpty("Escola é obrigatório.")]
        public override int esc_id { get; set; }
        [MSNotNullOrEmpty("Docente é obrigatório.")]
        public override Int64 doc_id { get; set; }
        [MSNotNullOrEmpty("[MSG_DISCIPLINA] é obrigatório.")]
        public override int tds_id { get; set; }
        [MSDefaultValue(1)]
        public override byte cdd_situacao { get; set; }
        public override DateTime cdd_dataCriacao { get; set; }
        public override DateTime cdd_dataAlteracao { get; set; }
	}
}