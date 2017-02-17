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
	public class MTR_MomentoCalendarioPeriodo : Abstract_MTR_MomentoCalendarioPeriodo
	{
        [MSDefaultValue(1)]
        public override byte mcp_situacao { get; set; }

        public override DateTime mcp_dataCriacao { get; set; }

        public override DateTime mcp_dataAlteracao { get; set; }
	}
}