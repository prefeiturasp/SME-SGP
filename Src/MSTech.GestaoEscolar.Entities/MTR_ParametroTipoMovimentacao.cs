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
	public class MTR_ParametroTipoMovimentacao : Abstract_MTR_ParametroTipoMovimentacao
	{        
        [DataObjectField(true, false, false)]
        public override int ptm_id { get; set; }
        [MSDefaultValue(1)]
        public override byte ptm_situacao { get; set; }        
        public override DateTime ptm_dataCriacao { get; set; }        
        public override DateTime ptm_dataAlteracao { get; set; }
	}
}