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
	public class ACA_TipoMovimentacao : Abstract_ACA_TipoMovimentacao
	{        
        [DataObjectField(true, true, false)]
        public override int tmv_id { get; set; }
        [MSValidRange(100)]
        [MSNotNullOrEmpty("Tipo de movimentação é obrigatório.")]
        public override string tmv_nome { get; set; }
        [MSValidRange(10,"Código pode conter até 10 caracteres.")]
        public override string tmv_codigo { get; set; }
        [MSNotNullOrEmpty("Motivo é obrigatório.")]
        public override byte tmv_motivo { get; set; }
        [MSDefaultValue(1)]
        public override byte tmv_situacao { get; set; }        
        public override DateTime tmv_dataCriacao { get; set; }        
        public override DateTime tmv_dataAlteracao { get; set; }
	}
}