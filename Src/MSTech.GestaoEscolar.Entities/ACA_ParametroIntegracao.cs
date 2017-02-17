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
	public class ACA_ParametroIntegracao : Abstract_ACA_ParametroIntegracao
    {
        [DataObjectField(true, true, false)]
        public override int pri_id { get; set; }

        [MSValidRange(100, "Chave pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Chave é obrigatório.")]
        public override string pri_chave { get; set; }

        [MSValidRange(1000, "Valor pode conter até 1000 caracteres.")]
        [MSNotNullOrEmpty("Valor é obrigatório.")]
        public override string pri_valor { get; set; }

        [MSValidRange(200, "Descrição pode conter até 200 caracteres.")]
        public override string pri_descricao { get; set; }

        [MSDefaultValue(1)]
        public override byte pri_situacao { get; set; }

        public override DateTime pri_dataCriacao { get; set; }
        public override DateTime pri_dataAlteracao { get; set; }
	}
}