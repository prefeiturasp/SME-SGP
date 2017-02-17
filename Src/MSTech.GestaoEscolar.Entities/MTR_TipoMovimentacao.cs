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
	public class MTR_TipoMovimentacao : Abstract_MTR_TipoMovimentacao
	{        
        [DataObjectField(true, true, false)]
        public override int tmo_id { get; set; }
        [MSValidRange(100,"Nome do parâmetro de movimentação pode conter até 100 caracteres.")]
        [MSNotNullOrEmpty("Nome do parâmetro de movimentação é obrigatório.")]
        public override string tmo_nome { get; set; }
        [MSNotNullOrEmpty("Tipo de movimento é obrigatório.")]
        public override byte tmo_tipoMovimento { get; set; }
        [MSDefaultValue(1)]
        public override byte tmo_situacao { get; set; }
        public override DateTime tmo_dataCriacao { get; set; }
        public override DateTime tmo_dataAlteracao { get; set; }        
	}
}