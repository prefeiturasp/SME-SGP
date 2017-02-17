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
	public class ACA_AlunoCurriculoDocumento : Abstract_ACA_AlunoCurriculoDocumento
	{
        [MSDefaultValue(1)]
        public override byte acd_situacao { get; set; }
        public override DateTime acd_dataCriacao { get; set; }
        public override DateTime acd_dataAlteracao { get; set; }
	}
}