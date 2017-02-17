/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

using System;
using MSTech.GestaoEscolar.Entities.Abstracts;
using MSTech.Validation;

namespace MSTech.GestaoEscolar.Entities
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MTR_ConfiguracaoVagas : Abstract_MTR_ConfiguracaoVagas
	{        
        public override int cvg_qtdeMaxAluno { get; set; }
        [MSDefaultValue(1)]
        public override byte cvg_situacao { get; set; }        
        public override DateTime cvg_dataCriacao { get; set; }        
        public override DateTime cvg_dataAlteracao { get; set; }
	}
}