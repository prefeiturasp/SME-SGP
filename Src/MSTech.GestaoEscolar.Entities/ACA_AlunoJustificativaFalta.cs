/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using System.ComponentModel;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ACA_AlunoJustificativaFalta : Abstract_ACA_AlunoJustificativaFalta
	{
        [MSNotNullOrEmpty("Aluno é obrigatório.")]
        [DataObjectField(true, false, false)]
        public override Int64 alu_id { get; set; }

        [DataObjectField(true, false, false)]
        public override int afj_id { get; set; }

        [MSNotNullOrEmpty("Tipo de justificativa de falta é obrigatório.")]
        public override int tjf_id { get; set; }

        [MSNotNullOrEmpty("Data início é obrigatório.")]
        public override DateTime afj_dataInicio { get; set; }

        [MSDefaultValue(1)]
        public override byte afj_situacao { get; set; }

        public override DateTime afj_dataCriacao { get; set; }
        
        public override DateTime afj_dataAlteracao { get; set; }

        public string tjf_nome { get; set; }

        public string afj_observacao { get; set; }
    }
}