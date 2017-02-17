/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
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
	public class ACA_TipoEquipamentoDeficiente : Abstract_ACA_TipoEquipamentoDeficiente
	{
        [MSValidRange(100, "Nome do tipo de equipamento para deficiente pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome do tipo de equipamento para deficiente é obrigatório.")]
        public override string ted_nome { get; set; }

        [MSDefaultValue(1)]
        public override byte ted_situacao { get; set; }
        public override DateTime ted_dataCriacao { get; set; }
        public override DateTime ted_dataAlteracao { get; set; }
	}
}