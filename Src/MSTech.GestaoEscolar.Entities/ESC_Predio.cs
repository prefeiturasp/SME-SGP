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
	public class ESC_Predio : Abstract_ESC_Predio
	{
        [DataObjectField(true, true, false)]
        public override int prd_id { get; set; }
        [MSValidRange(1000, "Descrição pode conter até 1000 caracteres.")]
        public override string prd_descricao { get; set; }
        [MSDefaultValue(1)]
        public override byte prd_situacao { get; set; }
        public override DateTime prd_dataCriacao { get; set; }
        public override DateTime prd_dataAlteracao { get; set; }
	}
}