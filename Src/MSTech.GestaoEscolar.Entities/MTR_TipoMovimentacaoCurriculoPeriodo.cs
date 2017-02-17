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
	public class MTR_TipoMovimentacaoCurriculoPeriodo : Abstract_MTR_TipoMovimentacaoCurriculoPeriodo
	{        
        [DataObjectField(true, false, false)]
        public override int tmp_id { get; set; }
        [MSDefaultValue(1)]
        public override byte tmp_situacao { get; set; }
        public override DateTime tmp_dataCriacao { get; set; }
        public override DateTime tmp_dataAlteracao { get; set; }
	}
}