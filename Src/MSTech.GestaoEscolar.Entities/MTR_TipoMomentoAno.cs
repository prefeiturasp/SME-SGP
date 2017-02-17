/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;
using System.ComponentModel;

namespace MSTech.GestaoEscolar.Entities
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MTR_TipoMomentoAno : Abstract_MTR_TipoMomentoAno
	{
        [MSDefaultValue(1)]
        public override byte tma_situacao { get; set; }

        public override DateTime tma_dataCriacao { get; set; }

        public override DateTime tma_dataAlteracao { get; set; }
	}
}