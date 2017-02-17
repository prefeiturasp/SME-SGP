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
	public class MTR_ParametroTipoMovimentacaoCurriculoPeriodo : Abstract_MTR_ParametroTipoMovimentacaoCurriculoPeriodo
	{
        [DataObjectField(true, false, false)]
        public override int pmp_id { get; set; }
        [MSDefaultValue(1)]
        public override byte pmp_situacao { get; set; }
        public override DateTime pmp_dataCriacao { get; set; }
        public override DateTime pmp_dataAlteracao { get; set; }
	}
}