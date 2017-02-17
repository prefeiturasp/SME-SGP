/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ORC_Objetivo : Abstract_ORC_Objetivo
	{
        [DataObjectField(true, true, false)]
        public override int obj_id { get; set; }
        
        [MSNotNullOrEmpty("Curso é obrigatório.")]
        public override int cur_id { get; set; }

        [MSNotNullOrEmpty("Curso é obrigatório.")]
        public override int crr_id { get; set; }

        [MSNotNullOrEmpty("Curso é obrigatório.")]
        public override int crp_id { get; set; }

        [MSNotNullOrEmpty("Tipo de [MSG_DISCIPLINA] é obrigatório.")]
        public override int tds_id { get; set; }

        [MSNotNullOrEmpty("Descrição do objetivo é obrigatório.")]
        public override string obj_descricao { get; set; }
        
        [MSDefaultValue(1)]
        public override byte obj_situacao { get; set; }

        public override DateTime obj_dataCriacao { get; set; }
        public override DateTime obj_dataAlteracao { get; set; }

	}
}