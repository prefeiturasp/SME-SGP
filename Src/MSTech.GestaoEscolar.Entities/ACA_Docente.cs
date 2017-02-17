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
	public class ACA_Docente : Abstract_ACA_Docente
	{
        [DataObjectField(true, true, false)]
        public override Int64 doc_id { get; set; }

        [MSNotNullOrEmpty("Colaborador é obrigatório.")]
        public override Int64 col_id { get; set; }        
        [MSValidRange(20, "Código INEP pode conter até 20 caracteres.")]
        public override string doc_codigoInep { get; set; }
        [MSDefaultValue(1)]
        public override byte doc_situacao { get; set; }        
        public override DateTime doc_dataCriacao { get; set; }
        public override DateTime doc_dataAlteracao { get; set; }        
	}
}